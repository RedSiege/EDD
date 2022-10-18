using EDDLib.Models;

using System;
using System.Linq;
using System.DirectoryServices;
using System.Collections.Generic;

namespace EDDLib.Functions
{
    internal class GetDomainDescription : EDDFunction
    {
        public override string FunctionName => "GetDomainDescriptions";

        public override string FunctionDesc => "Returns domain objects with non-standard account descriptions";

        public override string FunctionUsage => "EDD.exe -f GetDomainDescriptions -q <LDAP query>";

        public override string[] Execute(ParsedArgs args)
        {
            string CN, Desc;
            List<String> QueryOutList = new List<String>();

            if (args.ldapQuery == null) { args.ldapQuery = "(&(objectclass=user)(description=*))"; }

            SearchResultCollection QueryOut = LDAP.CustomSearchLDAP($"{args.ldapQuery}");
            foreach (SearchResult res in QueryOut)
            {
                CN = res.Properties["CN"][0].ToString();
                Desc = res.Properties["Description"][0].ToString();

                if (!Data.BlacklistedDesc.Any(Desc.Contains)) { QueryOutList.Add($"{CN}\t\t{Desc}"); }

            }

            return QueryOutList.ToArray();
        }
    }
}
