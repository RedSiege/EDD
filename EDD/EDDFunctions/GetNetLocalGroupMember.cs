using EDD.Models;

using System.Collections.Generic;

namespace EDD.EDDFunctions
{
    public class GetNetLocalGroupMember : EDDFunction
    {
        public override string FunctionName => "GetNetLocalGroupMember";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.ComputerName) || string.IsNullOrEmpty(args.GroupName))
                throw new EDDException("ComputerName and GroupName cannot be empty");

            Amass shepherd = new Amass();
            List<string> localGroupMembers = shepherd.GetLocalGroupMembers(args.ComputerName, args.GroupName);
            return localGroupMembers.ToArray();
        }
    }
}
