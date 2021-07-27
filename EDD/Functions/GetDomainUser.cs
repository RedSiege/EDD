using System;
using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetDomainUser : EDDFunction
    {
        public override string FunctionName => "GetDomainUser";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                List<string> allDomainUsers = new List<string>();
                Amass userInfo = new Amass();
                if (string.IsNullOrEmpty(args.UserName))
                {
                    if (string.IsNullOrEmpty(args.DomainName))
                    {
                        allDomainUsers = userInfo.GetDomainUsersInfo();
                    }
                    else
                    {
                        allDomainUsers = userInfo.GetDomainUsersInfo(args.DomainName);
                    }
                    return allDomainUsers.ToArray();
                }

                Amass singleUserInfo = new Amass();
                UserObject soleUser = singleUserInfo.GetDomainUserInfo(args.UserName);

                List<string> domainUser = new List<string>
                {
                    $"SamAccountName: {soleUser.SamAccountName}",
                    $"Name: {soleUser.Name}",
                    $"Description: {soleUser.Description}",
                    $"Distinguished Name: {soleUser.DistinguishedName}",
                    $"SID: {soleUser.SID}",
                };

                string groups = "Domain Groups: ";

                foreach (string singleGroupName in soleUser.DomainGroups)
                    groups += $"{singleGroupName}, ";

                groups = groups.TrimEnd(',', ' ');
                domainUser.Add(groups);

                return domainUser.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
