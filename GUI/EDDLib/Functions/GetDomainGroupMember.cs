using System;
using EDDLib.Models;

using System.Collections.Generic;

namespace EDDLib.Functions
{
    public class GetDomainGroupMember : EDDFunction
    {
        public override string FunctionName => "GetDomainGroupMember";

        public override string FunctionDesc => "Returns a list of all users in a domain group";

        public override string FunctionUsage => "EDD.exe -f GetDomainGroupMember -g <group name>";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.GroupName))
                    throw new EDDException("GroupName cannot be empty");

                Amass groupMemberEnum = new Amass();
                List<string> groupMembers = groupMemberEnum.GetDomainGroupMembers(args.GroupName);
                return groupMembers.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
