using EDD.Models;

using System;
using System.Collections.Generic;
using System.DirectoryServices;

// Thanks to Corey Overstreet for finding and sharing the query based off of - 

namespace EDD.Functions
{
    public class GetADCSServers : EDDFunction
    {
        public override string FunctionName => "GetADCSServers";

        public override string FunctionDesc => "Get a list of servers running AD CS within the current domain";

        public override string FunctionUsage => "EDD.exe -f GetADCSServers";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                List<string> adcs_systems = new List<string>();
                string adcssearch = "(&(cacertificate=*))";
                SearchResultCollection adcs_results = CertLDAPSearch(adcssearch);
                foreach (SearchResult sr in adcs_results)
                {
                    if (sr.Properties["dnsHostName"].Count > 0)
                    {
                        adcs_systems.Add(sr.Properties["dnsHostName"][0].ToString());
                    }
                }

                return adcs_systems.ToArray();
            }

            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }

        private SearchResultCollection CertLDAPSearch(string ldap_query)
        {
            string origPath = "";
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            string[] ldapElements = de.Properties["defaultNamingContext"][0].ToString().Split(',');
            if (ldapElements.Length > 2)
            {
                DirectoryEntry den = new DirectoryEntry("LDAP://RootDSE");
                origPath = "GC://CN=Configuration," + ldapElements[ldapElements.Length - 2] + "," +
                           ldapElements[ldapElements.Length - 1];
            }

            else
            {
                origPath = "GC://CN=Configuration," + de.Properties["defaultNamingContext"][0].ToString();
            }

            DirectoryEntry newentry = new DirectoryEntry(origPath);
            DirectorySearcher ds = new DirectorySearcher(newentry);
            ds.Filter = ldap_query;
            ds.SearchScope = SearchScope.Subtree;
            ds.PageSize = 500;
            SearchResultCollection results = ds.FindAll();
            return results;
        }
    }
}
