using System;
using EDDLib.Models;

using System.Collections.Generic;

namespace EDDLib.Functions
{
    public class GetDomainControllers : EDDFunction
    {
        public override string FunctionName => "GetDomainControllers";

        public override string FunctionDesc => "Get a list of all computers in the domain";

        public override string FunctionUsage => "EDD.exe -f GetDomainControllers";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                List<string> domainControllers = new List<string>();
                LDAP domainQuery = new LDAP();
                LDAP findDCs = new LDAP();
                if (string.IsNullOrEmpty(args.DomainName))
                {
                    domainControllers = findDCs.CaptureDomainControllers();
                }
                else
                {
                    domainControllers = findDCs.CaptureDomainControllers(args.DomainName);
                }
                return domainControllers.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
