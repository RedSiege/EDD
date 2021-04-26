using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetDomainControllers : EDDFunction
    {
        public override string FunctionName => "GetDomainControllers";

        public override string[] Execute(ParsedArgs args)
        {
            LDAP findDCs = new LDAP();
            List<string> domainControllers = findDCs.CaptureDomainControllers();
            return domainControllers.ToArray();
        }
    }
}
