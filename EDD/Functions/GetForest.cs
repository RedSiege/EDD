using EDD.Models;

using System.DirectoryServices.ActiveDirectory;

namespace EDD.Functions
{
    public class GetForest : EDDFunction
    {
        public override string FunctionName => "GetForest";

        public override string[] Execute(ParsedArgs args)
        {
            Amass forestInfo = new Amass();
            Forest currentForest = forestInfo.GetForestObject();
            return new string[] { currentForest.Name };
        }
    }
}
