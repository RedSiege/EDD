using System;
using EDDLib.Models;

using System.Collections.Generic;

namespace EDDLib.Functions
{
    public class GetDomainComputers : EDDFunction
    {
        public override string FunctionName => "GetDomainComputers";

        public override string FunctionDesc => "Get a list of all computers in the domain";

        public override string FunctionUsage => "EDD.exe -f GetDomainComputers";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                List<string> domainComputers = new List<string>();
                LDAP domainQuery = new LDAP();
                if (string.IsNullOrEmpty(args.DomainName))
                {
                    domainComputers = domainQuery.CaptureComputers();
                }
                else
                {
                    domainComputers = domainQuery.CaptureComputers(args.DomainName);
                }
                return domainComputers.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
