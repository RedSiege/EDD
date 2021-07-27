using System;
using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetNetLoggedOn : EDDFunction
    {
        public override string FunctionName => "GetNetLoggedOn";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.ComputerName))
                    throw new EDDException("ComputerName cannot be empty");

                Amass loggedInInfo = new Amass();
                List<Amass.WKSTA_USER_INFO_1> loggedInAccounts = loggedInInfo.GetLoggedOnUsers(args.ComputerName);

                List<string> results = new List<string>();

                foreach (Amass.WKSTA_USER_INFO_1 sessionInformation in loggedInAccounts)
                {
                    results.Add($"Account Name: {sessionInformation.wkui1_username}");
                    results.Add($"Domain Used by Account: {sessionInformation.wkui1_logon_domain}");
                    results.Add($"Operating System Domains: {sessionInformation.wkui1_oth_domains}");
                    results.Add($"Logon server: {sessionInformation.wkui1_logon_server}");
                }

                return results.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
