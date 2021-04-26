using EDD.Models;

using System.Collections.Generic;

namespace EDD.EDDFunctions
{
    public class GetDomainShares : EDDFunction
    {
        public override string FunctionName => "GetDomainShares";

        public override string[] Execute(ParsedArgs args)
        {
            LDAP computerQuery = new LDAP();
            List<string> domainSystems = computerQuery.CaptureComputers();
            Amass shareMe = new Amass();
            List<string> allShares = shareMe.GetShares(domainSystems);

            return allShares.ToArray();
        }
    }
}
