using System;
using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetDomainShares : EDDFunction
    {
        public override string FunctionName => "GetDomainShares";

        public override string FunctionDesc => "Get a list of all domain shares";

        public override string FunctionUsage => "EDD.exe -f GetDomainShares -t <threads>";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();
                Amass shareMe = new Amass();
                string[] allShares = shareMe.GetShares(domainSystems, args.Threads);

                return allShares;
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}