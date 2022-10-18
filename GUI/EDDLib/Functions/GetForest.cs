using System;
using EDDLib.Models;

using System.DirectoryServices.ActiveDirectory;

namespace EDDLib.Functions
{
    public class GetForest : EDDFunction
    {
        public override string FunctionName => "GetForest";

        public override string FunctionDesc => "Returns the name of the current forest";

        public override string FunctionUsage => "EDD.exe -f GetForest";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                Amass forestInfo = new Amass();
                Forest currentForest = forestInfo.GetForestObject();
                return new string[] { currentForest.Name };
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }

        }
    }
}
