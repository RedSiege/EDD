using System;
using EDD.Models;

using System.Collections.Generic;
using System.IO;

namespace EDD.Functions
{
    public class GetReadableDomainShares : EDDFunction
    {
        public override string FunctionName => "GetReadableDomainShares";

        public override string FunctionDesc => "Get a list of all readable domain shares";

        public override string FunctionUsage => "EDD.exe -f GetReadableDomainShares -t <threads>";

        public override string[] Execute(ParsedArgs args)
        {
            List<string> readableShares = new List<string>();
            List<string> domainSystems = new List<string>();
            try
            {
                if (string.IsNullOrEmpty(args.FileData))
                {
                    LDAP computerQuery = new LDAP();
                    domainSystems = computerQuery.CaptureComputers();
                }
                else
                {
                    if (File.Exists(args.FileData))
                    {
                        foreach (string line in File.ReadLines(args.FileData))
                        {
                            domainSystems.Add(line);
                        }
                    }
                    else
                    {
                        return new string[] { "[X] You did not provide a valid file path containing the computers to target!" };
                    }
                }
                Amass shareMe = new Amass();
                string[] allShares = shareMe.GetShares(domainSystems, args.Threads);

                foreach (string shareDir in allShares)
                {
                    try
                    {
                        string[] subdirectoryEntries = Directory.GetDirectories(shareDir);
                        readableShares.Add(shareDir);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // do nothing
                    }
                    catch (IOException)
                    {
                        // do nothing either
                    }

                }

                return readableShares.ToArray();
            }
            catch (Exception e)
            {
                foreach (string path in readableShares)
                {
                    Console.WriteLine(path);
                }

                Console.WriteLine("[X] ERROR State Occurred - Paths above are current status prior to error!");
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
