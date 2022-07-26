using EDD.Models;

using System;
using System.Collections.Generic;

namespace EDD.Functions
{
    public class FindDomainUser : EDDFunction
    {
        public override string FunctionName => "FindDomainUser";

        public override string FunctionDesc => "Searches the domain environment for a specified user or group and tries to find active sessions (default searches for Domain Admins)";

        public override string FunctionUsage => "EDD.exe -f FindDomainUser -u <username> -g <group name>";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (!string.IsNullOrEmpty(args.GroupName) && !string.IsNullOrEmpty(args.UserName))
                    throw new EDDException("Please use GroupName or UserName, not both");

                LDAP compQuery = new LDAP();

                List<string> windowsComputers = compQuery.CaptureComputers();

                if (windowsComputers.Count < 1)
                    throw new EDDException("No domain computers could be found");

                string[] results = new string[] { };

                // if group and user is null, search for domain admins
                if (string.IsNullOrEmpty(args.GroupName) && string.IsNullOrEmpty(args.UserName))
                    results = FindMembersOfGroup(windowsComputers, "Domain Admins");

                // if group is not null and user is null, search for group
                if (!string.IsNullOrEmpty(args.GroupName) && string.IsNullOrEmpty(args.UserName))
                    results = FindMembersOfGroup(windowsComputers, args.GroupName);

                // if group is null and user is not null, search for user
                if (string.IsNullOrEmpty(args.GroupName) && !string.IsNullOrEmpty(args.UserName))
                    results = FindUser(windowsComputers, args.UserName);

                return results;
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }

        }

        string[] FindMembersOfGroup(List<string> computers, string groupName)
        {
            try
            {
                List<string> results = new List<string>();

                Amass findUser = new Amass();
                List<string> groupList = findUser.GetDomainGroupMembers(groupName);

                foreach (string computerHostName in computers)
                {
                    List<Amass.WKSTA_USER_INFO_1> currentLoggedInAccounts = findUser.GetLoggedOnUsers(computerHostName);

                    foreach (string actualUser in groupList)
                    {
                        foreach (Amass.WKSTA_USER_INFO_1 loggedInHere in currentLoggedInAccounts)
                        {
                            if (String.Equals(loggedInHere.wkui1_username, actualUser, StringComparison.OrdinalIgnoreCase))
                            {
                                results.Add($"{loggedInHere.wkui1_username} is currently logged into {computerHostName}");
                            }
                        }
                    }

                    List<Amass.SESSION_INFO_10> currentSessionInfo = findUser.GetRemoteSessionInfo(computerHostName);

                    foreach (string actualDAAgain in groupList)
                    {
                        foreach (Amass.SESSION_INFO_10 sessInformation in currentSessionInfo)
                        {
                            if (String.Equals(sessInformation.sesi10_username, actualDAAgain, StringComparison.OrdinalIgnoreCase))
                            {
                                results.Add($"{sessInformation.sesi10_username} has a session on {computerHostName}");
                            }
                        }
                    }
                }

                return results.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }

        }

        string[] FindUser(List<string> computers, string username)
        {
            try
            {
                List<string> results = new List<string>();

                Amass findUser = new Amass();

                foreach (string computerHostName in computers)
                {
                    List<Amass.WKSTA_USER_INFO_1> currentLoggedInAccounts = findUser.GetLoggedOnUsers(computerHostName);

                    foreach (Amass.WKSTA_USER_INFO_1 loggedInHere in currentLoggedInAccounts)
                    {
                        if (String.Equals(loggedInHere.wkui1_username, username, StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add($"{loggedInHere.wkui1_username} is currently logged into {computerHostName}");
                        }
                    }

                    List<Amass.SESSION_INFO_10> currentSessionInfo = findUser.GetRemoteSessionInfo(computerHostName);

                    foreach (Amass.SESSION_INFO_10 sessInformation in currentSessionInfo)
                    {
                        if (String.Equals(sessInformation.sesi10_username, username, StringComparison.OrdinalIgnoreCase))
                        {
                            results.Add($"{sessInformation.sesi10_username} has a session on {computerHostName}");
                        }
                    }
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
