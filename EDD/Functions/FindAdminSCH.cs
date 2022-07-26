using EDD.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32.TaskScheduler;

namespace EDD.Functions
{
    public class FindAdminSCH : EDDFunction
    {
        public override string FunctionName => "FindAdminSCH";

        public override string FunctionDesc => "Set arbitrary LDAP filter to search for objects";

        public override string FunctionUsage => "EDD.exe -f FindAdminSCH";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                List<string> schSystems = new List<string>();
                // Get a list of domain systems
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();

                // Loop over them and try to connect to their to their task scheduler
                foreach (string singleSystem in domainSystems)
                {
                    try
                    {
                        var ts = new TaskService(singleSystem, null, null, null, false);
                        foreach (Task task in ts.RootFolder.Tasks)
                        {
                            if (!schSystems.Contains(singleSystem))
                            {
                                schSystems.Add(singleSystem);
                            }
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        continue;
                    }
                    catch (FileNotFoundException)
                    {
                        continue;
                    }
                    catch (COMException)
                    {
                        continue;
                    }
                }

                // Convert list to array and return it
                string[] returnThisArray = schSystems.ToArray();
                return returnThisArray;
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}