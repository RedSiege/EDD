using System;
using EDDLib.Models;

using System.DirectoryServices.ActiveDirectory;

namespace EDDLib.Functions
{
    public class GetCurrentDomain : EDDFunction
    {
        public override string FunctionName => "GetCurrentDomain";

        public override string FunctionDesc => "Retrieves the curernt domain";

        public override string FunctionUsage => "EDD.exe -f GetCurrentDomain";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                Domain domain = Domain.GetCurrentDomain();
                return new string[] { domain.Name };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
