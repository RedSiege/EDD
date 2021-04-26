using EDD.Models;

namespace EDD.EDDFunctions
{
    public class ConvertSidToName : EDDFunction
    {
        public override string FunctionName => "ConvertSidToName";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.UserName))
                throw new EDDException("UserName cannot be empty");

            Amass sidConverter = new Amass();
            string sid = sidConverter.GetUsernameFromSID(args.UserName);

            return new string[] { sid };
        }
    }
}
