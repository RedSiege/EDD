using EDD.Models;

using System.Collections.Generic;

namespace EDD.EDDFunctions
{
    public class GetDomainGroupMember : EDDFunction
    {
        public override string FunctionName => "GetDomainGroupMember";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.GroupName))
                throw new EDDException("GroupName cannot be empty");

            Amass groupMemberEnum = new Amass();
            List<string> groupMembers = groupMemberEnum.GetDomainGroupMembers(args.GroupName);
            return groupMembers.ToArray();
        }
    }
}
