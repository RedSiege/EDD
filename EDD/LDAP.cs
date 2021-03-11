using System.Collections.Generic;
using System.DirectoryServices;


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
                compmodified_pgrpid.Add(sr.Properties["dnsHostName"][0].ToString());
            }
            return compmodified_pgrpid;
        }

        private static string GetCurrentDomainPath()
        {
            DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
            return "LDAP://" + de.Properties["defaultNamingContext"][0].ToString();
        }

        private static SearchResultCollection CustomSearchLDAP(string ldap_query, string[] optional_params = null)
        {
            DirectoryEntry newentry = new DirectoryEntry(GetCurrentDomainPath());
            if (optional_params == null)
            {
                DirectorySearcher ds = new DirectorySearcher(newentry);
                ds.Filter = ldap_query;
                SearchResultCollection results = ds.FindAll();
                return results;
            }
            else
            {
                DirectorySearcher ds = new DirectorySearcher(newentry);
                ds.Filter = ldap_query;
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
