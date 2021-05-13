using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetUsersWithSpns : EDDFunction
    {
        public override string FunctionName => "GetUsersWithSpns";

        public override string[] Execute(ParsedArgs args)
        {
            LDAP spnLookup = new LDAP();
            List<string> userListWitSPNs = spnLookup.GetAccountsWithSPNs();

            return userListWitSPNs.ToArray();
        }
    }
}
