using System;
using EDD.Models;

namespace EDD.Functions
{
    public class GetDomainGroupSid : EDDFunction
    {
        public override string FunctionName => "GetDomainGroupSid";

        public override string FunctionDesc => "Fetch the SID of a group";

        public override string FunctionUsage => "EDD.exe -f GetDomainGroupSid -g [group name]";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.GroupName))
                    throw new EDDException("GroupName cannot be empty");

                Amass domainGroupSid = new Amass();
                string incomingSid = domainGroupSid.GetDomainGroupSID(args.GroupName);

                return new string[] { incomingSid };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
