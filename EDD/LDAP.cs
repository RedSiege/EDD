using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.InteropServices;


namespace EDD
{
    class LDAP
    {
        public List<string> CaptureComputers()
        {
            List<string> compmodified_pgrpid = new List<string>();
            string computerprimary_Filter = "(&(objectCategory=computer))";
            string[] comppri_params = { "dnsHostName" };
            SearchResultCollection computerprimary_results = CustomSearchLDAP(computerprimary_Filter, comppri_params);
            foreach (SearchResult sr in computerprimary_results)
            {
                try
                {
                    compmodified_pgrpid.Add(sr.Properties["dnsHostName"][0].ToString());
                }
                catch (Exception e)
                {
                    // threw an odd error
                }
                
            }
            return compmodified_pgrpid;
        }

        public List<string> CaptureDomainControllers()
        {
            List<string> dcList = new List<string>();
            string computerprimary_Filter = "(&(objectCategory=computer)(objectClass=computer)(userAccountControl:1.2.840.113556.1.4.803:=8192))";
            string[] comppri_params = { "dnsHostName" };
            SearchResultCollection computerprimary_results = CustomSearchLDAP(computerprimary_Filter, comppri_params);
            foreach (SearchResult sr in computerprimary_results)
            {
                try
                {
                    dcList.Add(sr.Properties["dnsHostName"][0].ToString());
                }
                catch (Exception e)
                {
                    // threw an odd error
                }
            }
            return dcList;
        }

        private static string GetCurrentDomainPath()
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
                return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
            }
            catch (COMException)
            {
                Console.WriteLine("\nError:");
                Console.WriteLine("Could not contact domain controller, exiting...");
                Environment.Exit(1);
            }

            return "";
        }

        private static SearchResultCollection CustomSearchLDAP(string ldap_query, string[] optional_params = null)
        {
            DirectoryEntry newentry = new DirectoryEntry(GetCurrentDomainPath());
            if (optional_params == null)
            {
                DirectorySearcher ds = new DirectorySearcher(newentry);
                ds.Filter = ldap_query;
                ds.SearchScope = SearchScope.Subtree;
                ds.PageSize = 500;
                SearchResultCollection results = ds.FindAll();
                return results;
            }
            else
            {
                DirectorySearcher ds = new DirectorySearcher(newentry);
                ds.Filter = ldap_query;
                ds.SearchScope = SearchScope.Subtree;
                ds.PageSize = 500;
                foreach (string param in optional_params)
                {
                    ds.PropertiesToLoad.Add(param);
                }
                SearchResultCollection results = ds.FindAll();
                return results;
            }
        }
    }
}
