using System;
using System.Collections.Generic;
using System.Management;

namespace EDD
{
    class WMI
    {
        public List<string> CheckProcesses(List<string> systemsToCheck, string procName)
        {
            List<string> systemsRunningTargetedProc = new List<string>();

            foreach (string computerName in systemsToCheck)
            {
                ConnectionOptions options = new ConnectionOptions();
                options.Impersonation = ImpersonationLevel.Impersonate;

                ManagementScope scope = new ManagementScope("\\\\" + computerName + "\\root\\cimv2", options);
                try
                {
                    scope.Connect();

                    //Query system for Operating System information
                    ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_Process");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);

                    ManagementObjectCollection queryCollection = searcher.Get();
                    foreach (ManagementObject process in queryCollection)
                    {
                        var wmiObject = (ManagementObject) process;
                        string name = (string) wmiObject["Name"];
                        if (String.Equals(name, procName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (!systemsRunningTargetedProc.Contains(computerName))
                            {
                                systemsRunningTargetedProc.Add(computerName);
                            }
                        }
                    }

                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    //Console.WriteLine(e);
                }

                catch (System.UnauthorizedAccessException e2)
                {
                    //Console.WriteLine(e2);
                }

                catch (System.Management.ManagementException e3)
                {
                    //Console.WriteLine(e3);
                }
            }

            return systemsRunningTargetedProc;
        }
    }
}
