using EDD.Models;

namespace EDD.Functions
{
    public class GetDomainSid : EDDFunction
    {
        public override string FunctionName => "GetDomainSid";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.DomainName))
                throw new EDDException("DomainName cannot be empty");

            LDAP ldapSIDFinder = new LDAP();
            string sid = ldapSIDFinder.GetDomainSID(args.DomainName);

            return new string[] { sid };
        }
    }
}
