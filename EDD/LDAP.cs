using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;
using System.Security.Principal;


namespace EDD
{
    class LDAP
    {
        public List<string> CaptureComputers(string domainName = null)
        {
            List<string> compmodified_pgrpid = new List<string>();
            string computerprimary_Filter = "(&(objectCategory=computer))";
            string[] comppri_params = { "dnsHostName" };
            if (domainName == null)
            {
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
            }
            else
            {
                SearchResultCollection computerprimary_results = CustomSearchLDAP(computerprimary_Filter, comppri_params, domainName);
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

        public string GetDomainSID(string domainInfo)
        {
            string domainDnsName;
            if (domainInfo == null)
            {
                domainDnsName = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
            }
            else
            {
                domainDnsName = domainInfo;
            }

            try
            {
                var domainContext = new DirectoryContext(DirectoryContextType.Domain, domainDnsName);
                var domain = Domain.GetDomain(domainContext);
                DirectoryEntry domainEntry = domain.GetDirectoryEntry();
                byte[] domainSid = domainEntry.Properties["objectSID"].Value as byte[];
                SecurityIdentifier strongDomainSid = new SecurityIdentifier(domainSid, 0);
                return domainDnsName + " - " + strongDomainSid;
            }
            catch (ActiveDirectoryObjectNotFoundException)
            {
                return "\nDomain provided does not exist, or could not be contacted!\nExiting...";
            }
        }

        public List<string> GetAccountsWithSPNs()
        {
            List<string> usersWithSPNs = new List<string>();
            string spn_query = "(&(objectCategory=User)(servicePrincipalName=*))";
            string[] extrauseruac_params = { "primaryGroupID", "name", "userAccountControl", "sAMAccountName" };
            SearchResultCollection results = CustomSearchLDAP(spn_query, extrauseruac_params);
            foreach (SearchResult sr in results)
            {
                if ((sr.Properties["sAMAccountName"].Count > 0) && (sr.Properties["sAMAccountName"][0] != null) && (sr.Properties.Contains("userAccountControl")))
                {
                    int spn_uac = Int32.Parse(sr.Properties["userAccountControl"][0].ToString());
                    if (!((spn_uac & 2) == 2))
                    {
                        usersWithSPNs.Add(sr.Properties["sAMAccountName"][0].ToString());
                    }
                }
            }

            return usersWithSPNs;
        }

        private static SearchResultCollection CustomSearchLDAP(string ldap_query, string[] optional_params = null, string subDomain = null)
        {
            DirectoryEntry newentry;
            if (subDomain == null)
            {
                newentry = new DirectoryEntry(GetCurrentDomainPath());
            }
            else
            {
                string ldapQueryBuild = GetCurrentDomainPath();
                string modified = ldapQueryBuild.Insert(7, "DC=" + subDomain + ",");
                newentry = new DirectoryEntry(modified);
            }
            
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
