using System;
using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class FindDomainProcess : EDDFunction
    {
        public override string FunctionName => "FindDomainProcess";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.ProcessName))
                    throw new EDDException("ProcessName cannot be empty");

                LDAP procQuery = new LDAP();
                List<string> procComputers = procQuery.CaptureComputers();
                WMI processSearcher = new WMI();
                List<string> systemsWithProc = processSearcher.CheckProcesses(procComputers, args.ProcessName);

                return systemsWithProc.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
            
        }
    }
}
