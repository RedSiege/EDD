using System;
using System.DirectoryServices.AccountManagement;
using EDD.Models;

namespace EDD.Functions
{
    public class JoinGroupByName : EDDFunction
    {
        public override string FunctionName => "JoinGroupByName";

        public override string FunctionDesc => "Join an account to a group via the group's name";

        public override string FunctionUsage => "EDD.exe -f JoinGroupByName -g [group name] -u [username]";

        public override string[] Execute(ParsedArgs args)
        {
            if (args.GroupName == null)
            {
                return new string[] { "You need to provide the group name in the GroupName parameter" };
            }

            if (args.UserName == null)
            {
                return new string[] { "You need to provide a username to join the targeted group" };
            }

            try
            {
                using (PrincipalContext pc = new PrincipalContext(ContextType.Domain))
                {
                    GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, args.GroupName);
                    group.Members.Add(pc, IdentityType.SamAccountName, args.UserName);
                    group.Save();
                }

                return new string[] { "Joined account to group" };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to join user to group - " + e };
            }
        }
    }
}