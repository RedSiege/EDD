using System;
using EDD.Models;

namespace EDD.Functions
{
    public class GetSidData : EDDFunction
    {
        public override string FunctionName => "GetSidData";

        public override string FunctionDesc => "Return username from SID";

        public override string FunctionUsage => "EDD.exe -f GetSidData";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.UserName))
                    throw new EDDException("UserName cannot be empty");

                Amass sidConverter = new Amass();
                string sid = sidConverter.GetUsernameFromSID(args.UserName);

                return new string[] { sid };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }

        }
    }
}
