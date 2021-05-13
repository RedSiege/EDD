using System;
using EDD.Models;

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EDD.Functions
{
    public class FindInterestingDomainShareFile : EDDFunction
    {
        public override string FunctionName => "FindInterestingDomainShareFile";

        public override string[] Execute(ParsedArgs args)
        {
            LDAP computerQuery = new LDAP();
            List<string> interestingFiles = new List<string>();

            List<string> domainSystems = computerQuery.CaptureComputers();
            Amass shareMe = new Amass();
            List<string> allShares = shareMe.GetShares(domainSystems);

            foreach (var share in allShares.ToArray())
            {
                try
                {
                    foreach (var fileSystemEntry in Directory.EnumerateFileSystemEntries(share, "*", SearchOption.AllDirectories))
                    {
                        Console.WriteLine(args.SearchTerms);
                        Console.WriteLine(fileSystemEntry);
                    }
                }
                catch (UnauthorizedAccessException e)
                {
                    Console.WriteLine("Directory not accessable: ");
                    
                }
                
                //foreach (var file in Directory.GetFiles(share)) 
                //{
                //    interestingFiles.Add(file);
                //    //Console.WriteLine(file);
                //}
            }

            return interestingFiles.ToArray();
        }
    }
}