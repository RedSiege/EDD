using EDD.Models;

using System;
using System.Collections.Generic;
using System.Management;
using System.Net;

namespace EDD.Functions
{
    public class FindAdminWMI : EDDFunction
    {
        public override string FunctionName => "FindAdminWMI";

        public override string FunctionDesc => "Uses WMI to search for admin rights within a domain";

        public override string FunctionUsage => "EDD.exe -f FindAdminWMI";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                // Property containing the local system's hostname
                string hostName = Dns.GetHostName();
                List<string> wmiSystems = new List<string>();
                // Get a list of domain systems
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();

                foreach (string domainComputer in domainSystems)
                {
                    ConnectionOptions options = new ConnectionOptions();
                    options.Impersonation = ImpersonationLevel.Impersonate;

                    ManagementScope scope = new ManagementScope("\\\\" + domainComputer + "\\root\\cimv2", options);

                    try
                    {
                        scope.Connect();

                        //Query system for Operating System information
                        ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                        ManagementObjectCollection queryCollection = searcher.Get();
                        foreach (ManagementObject m in queryCollection)
                        {
                            if (!wmiSystems.Contains(domainComputer))
                            {
                                if (hostName != domainComputer.Split('.')[0])
                                {
                                    wmiSystems.Add(domainComputer);
                                }
                            }
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

                return wmiSystems.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}