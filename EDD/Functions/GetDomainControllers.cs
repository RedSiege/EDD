using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetDomainControllers : EDDFunction
    {
        public override string FunctionName => "GetDomainControllers";

        public override string[] Execute(ParsedArgs args)
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
    }
}
