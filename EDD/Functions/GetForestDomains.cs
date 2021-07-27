using System;
using EDD.Models;

using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;

namespace EDD.Functions
{
    class GetForestDomains : EDDFunction
    {
        public override string FunctionName => "GetForestDomains";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                Amass forestDomains = new Amass();
                Forest theCurrentForest = forestDomains.GetForestObject();
                DomainCollection forestDomainList = theCurrentForest.Domains;

                List<string> result = new List<string>();

                foreach (Domain internalDomain in forestDomainList)
                    result.Add(internalDomain.Name);

                return result.ToArray();
            }
            catch (Exception e)
            {
                return new string[] { "[X] Failure to enumerate info - " + e };
            }
        }
    }
}
