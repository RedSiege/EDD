using System;
using EDDLib.Models;

namespace EDDLib.Functions
{
    public class GetDomainSid : EDDFunction
    {
        public override string FunctionName => "GetDomainSid";

        public override string FunctionDesc => "Fetch SID of domain";

        public override string FunctionUsage => "EDD.exe -f GetDomainSid -d [domain name]";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.DomainName))
                    throw new EDDException("DomainName cannot be empty");

                LDAP ldapSIDFinder = new LDAP();
                string sid = ldapSIDFinder.GetDomainSID(args.DomainName);

                return new string[] { sid };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
