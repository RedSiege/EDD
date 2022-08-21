using System;
using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetUsersWithSpns : EDDFunction
    {
        public override string FunctionName => "GetUsersWithSpns";

        public override string FunctionDesc => "Returns a list of all domain accounts that have a SPN associated with them";

        public override string FunctionUsage => "EDD.exe -f GetUsersWithSpns ";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                LDAP spnLookup = new LDAP();
                List<string> userListWitSPNs = spnLookup.GetAccountsWithSPNs();

                return userListWitSPNs.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
