using System.DirectoryServices;
using System.Collections.Generic;

using EDD.Models;

namespace EDD.Functions
{
    internal class CustomLDAPQuery : EDDFunction
    {
        public override string FunctionName => "Query";

        public override string[] Execute(ParsedArgs args)
        {
            List<string> QueryOutList = new List<string>();

            if (args.ldapQuery == null) { throw new EDDException("No LDAP query supplied"); }

            SearchResultCollection QueryOut = LDAP.CustomSearchLDAP(args.ldapQuery);
            foreach (SearchResult res in QueryOut) { QueryOutList.Add($"{res.Properties["CN"][0]}\t\t{res.Path}"); }

            return QueryOutList.ToArray();
        }
    }
}