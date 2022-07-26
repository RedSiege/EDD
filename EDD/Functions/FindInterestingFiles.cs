using System;
using EDD.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace EDD.Functions
{
    public class FindInterestingDomainShareFile : EDDFunction
    {
        public override string FunctionName => "FindInterestingDomainShareFile";

        public override string FunctionDesc => "Searches the domain environment for all accessible shares. Once found, it parses all filenames for \"interesting\" strings";

        public override string FunctionUsage => "EDD.exe -f FindInterestingDomainShareFile --sharepath=[share path] -t <threads> -s <search terms>";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                LDAP computerQuery = new LDAP();
                List<string> interestingFiles = new List<string>();
                List<Regex> regexList = new List<Regex>();
                string[] allShares = new string[1];

                if (string.IsNullOrEmpty(args.SharePath))
                {
                    List<string> domainSystems = computerQuery.CaptureComputers();
                    Amass shareMe = new Amass();
                    allShares = shareMe.GetShares(domainSystems, args.Threads);
                }
                else
                {
                    allShares[0] = args.SharePath;
                }


                // We need to convert the given wildcard string to regex and account for multiple strings
                foreach (var term in args.SearchTerms)
                {
                    if (term.Contains("*"))
                    {
                        string regexText = WildcardToRegex(term);
                        Regex regex = new Regex(regexText, RegexOptions.IgnoreCase);
                        regexList.Add(regex);
                    }
                    else
                    {
                        Regex regex = new Regex(term, RegexOptions.IgnoreCase);
                        regexList.Add(regex);
                    }
                }

                // Pipe multiple search strings together into one regex string
                var regexString = regexList.Count() > 1 ? string.Join("|", regexList) : regexList[0].ToString();

                if (allShares != null)
                    foreach (var share in allShares)
                    {
                        try
                        {
                            Parallel.ForEach(GetFiles(share), file =>
                            {
                                if (Regex.IsMatch(file, regexString, RegexOptions.IgnoreCase))
                                    interestingFiles.Add(file);
                            });
                        }
                        catch (UnauthorizedAccessException)
                        {
                            //Do nothing
                        }
                    }

                return interestingFiles.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }

        }

        // Borrowed from SO - https://stackoverflow.com/a/929418
        static IEnumerable<string> GetFiles(string path)
        {
            Queue<string> queue = new Queue<string>();
            queue.Enqueue(path);
            while (queue.Count > 0)
            {
                path = queue.Dequeue();
                try
                {
                    foreach (string subDir in Directory.GetDirectories(path))
                    {
                        queue.Enqueue(subDir);
                    }
                }
                catch (Exception)
                {
                }
                string[] files = null;
                try
                {
                    files = Directory.GetFiles(path);
                }
                catch (Exception)
                {
                }
                if (files != null)
                {
                    foreach (var t in files)
                    {
                        yield return t;
                    }
                }
            }
        }

        // Borrowed from SO - https://stackoverflow.com/a/31490838
        public static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern)
                              .Replace(@"\*", ".*")
                              .Replace(@"\?", ".")
                       + "$";
        }
    }
}