using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetDomainUser : EDDFunction
    {
        public override string FunctionName => "GetDomainUser";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.UserName))
            {
                Amass userInfo = new Amass();
                List<string> allDomainUsers = userInfo.GetDomainUsersInfo();
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
    }
}
