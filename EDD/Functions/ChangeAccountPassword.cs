using System;
using System.DirectoryServices.AccountManagement;
using EDD.Models;

using System.DirectoryServices.ActiveDirectory;

namespace EDD.Functions
{
    public class ChangeAccountPassword : EDDFunction
    {
        public override string FunctionName => "ChangeAccountPassword";

        public override string[] Execute(ParsedArgs args)
        {
            if (args.UserName == null)
            {
                return new string[] {"You need to provide a username to change their password"};
            }

            if (args.Password == null)
            {
                return new string[] {"You need to provide the password that you are setting"};
            }

            try
            {
                using (var context = new PrincipalContext(ContextType.Domain))
                {
                    using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, args.UserName))
                    {
                        user.SetPassword(args.Password);
                        user.Save();
                    }
                }

                return new string[] {"Successfully changed password"};
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to join user to group - " + e };
            }
        }
    }
}