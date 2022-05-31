using EDD.Models;

using System;
using System.Collections.Generic;
using System.DirectoryServices;

namespace EDD.Functions
{
    internal class GetDomainDescription : EDDFunction
    {
        public override string FunctionName => "Descriptions";

        public override string[] Execute(ParsedArgs args)
        {
            List<String> QueryOutList = new List<String>();
            
            if(args.ldapQuery == null) { args.ldapQuery = "(&(objectclass=user)(description=*))"; }

            SearchResultCollection QueryOut = LDAP.CustomSearchLDAP($"{args.ldapQuery}");
            foreach (SearchResult res in QueryOut) { QueryOutList.Add($"{res.Properties["CN"][0]}\t\t{res.Properties["Description"][0]}"); }

            return QueryOutList.ToArray(); 
        }
    }
}
