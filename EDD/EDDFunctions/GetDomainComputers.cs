using EDD.Models;

using System.Collections.Generic;

namespace EDD.EDDFunctions
{
    public class GetDomainComputers : EDDFunction
    {
        public override string FunctionName => "GetDomainComputers";

        public override string[] Execute(ParsedArgs args)
        {
            LDAP domainQuery = new LDAP();
            List<string> domainComputers = domainQuery.CaptureComputers();
            return domainComputers.ToArray();
        }
    }
}
