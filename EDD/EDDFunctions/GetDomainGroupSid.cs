using EDD.Models;

namespace EDD.EDDFunctions
{
    public class GetDomainGroupSid : EDDFunction
    {
        public override string FunctionName => "GetDomainGroupSid";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.DomainName))
                throw new EDDException("DomainName cannot be empty");

            Amass domainGroupSid = new Amass();
            string incomingSid = domainGroupSid.GetDomainGroupSID(args.DomainName);

            return new string[] { incomingSid };
        }
    }
}
