using EDD.Models;

using System;
using System.Collections.Generic;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;

namespace EDD.Functions
{
    public class FindEmptySystem : EDDFunction
    {
        public override string FunctionName => "FindEmptySystem";

        public override string FunctionDesc => "Uses WMI to find a domain system with no one logged into it.";

        public override string FunctionUsage => "EDD.exe -f FindEmptySystem";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                // Property containing the local system's hostname
                string hostName = Dns.GetHostName();
                List<string> emptySystems = new List<string>();
                // Get a list of domain systems
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();

                foreach (string domainComputer in domainSystems)
                {
                    ConnectionOptions options = new ConnectionOptions();
                    options.Impersonation = ImpersonationLevel.Impersonate;
                    ManagementScope scope = new ManagementScope("\\\\" + domainComputer + "\\root\\cimv2", options);
                    bool isSystemEmpty = false;

                    try
                    {
                        scope.Connect();

                        //Query system for Operating System information
                        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_LoggedOnUser");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                        ManagementObjectCollection queryCollection = searcher.Get();
                        bool theCurrentUser = false;
                        bool notCurrentUser = false;
                        
                        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\')[1];
                        foreach (ManagementObject m in queryCollection)
                        {
                            string wmiLoggedInData = m.ToString();
                            // Is the current user account in the results
                            theCurrentUser = wmiLoggedInData.IndexOf(userName, StringComparison.OrdinalIgnoreCase) >= 0;
                            int timesHostName = Regex.Matches(wmiLoggedInData, domainComputer.Split('.')[0], RegexOptions.IgnoreCase).Count;

                            // Check and see if the current username isn't used, but it's a domain account
                            if (!theCurrentUser && timesHostName < 2)
                            {
                                notCurrentUser = true;
                            }
                        }

                        if (!notCurrentUser)
                        {
                            emptySystems.Add(domainComputer);
                        }
                    }

                    catch (System.Runtime.InteropServices.COMException)
                    {
                        //pass
                    }

                    catch (System.UnauthorizedAccessException)
                    {
                        //pass
                    }

                    catch (System.Management.ManagementException)
                    {
                        //pass
                    }
                }



                return emptySystems.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
