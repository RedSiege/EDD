using System;
using EDD.Models;

using System.DirectoryServices.ActiveDirectory;

namespace EDD.Functions
{
    public class GetCurrentDomain : EDDFunction
    {
        public override string FunctionName => "GetCurrentDomain";

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
