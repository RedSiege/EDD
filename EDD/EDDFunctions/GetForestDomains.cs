using EDD.Models;

using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;

namespace EDD.EDDFunctions
{
    class GetForestDomains : EDDFunction
    {
        public override string FunctionName => "GetForestDomains";

        public override string[] Execute(ParsedArgs args)
        {
            Amass forestDomains = new Amass();
            Forest theCurrentForest = forestDomains.GetForestObject();
            DomainCollection forestDomainList = theCurrentForest.Domains;

            List<string> result = new List<string>();

            foreach (Domain internalDomain in forestDomainList)
                result.Add(internalDomain.Name);

            return result.ToArray();
        }
    }
}
