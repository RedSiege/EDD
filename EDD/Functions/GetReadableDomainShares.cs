using System;
using EDD.Models;

using System.Collections.Generic;
using System.IO;

namespace EDD.Functions
{
    public class GetReadableDomainShares : EDDFunction
    {
        public override string FunctionName => "GetReadableDomainShares";

        public override string[] Execute(ParsedArgs args)
        {
            List<string> readableShares = new List<string>();
            try
            {
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();
                Amass shareMe = new Amass();
                string[] allShares = shareMe.GetShares(domainSystems, args.Threads);

                foreach (string shareDir in allShares)
                {
                    try
                    {
                        string[] subdirectoryEntries = Directory.GetDirectories(shareDir);
                        readableShares.Add(shareDir);
                    }
                    catch(UnauthorizedAccessException)
                    {
                        // do nothing
                    }
                    
                }

                return readableShares.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
