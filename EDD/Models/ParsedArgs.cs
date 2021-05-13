using System;

namespace EDD.Models
{
    public class ParsedArgs
    {
        public string GroupName { get; set; }
        public string ComputerName { get; set; }
        public string ProcessName { get; set; }
        public string UserName { get; set; }
        public string DomainName { get; set; }
        public int Threads { get; set; }
        public string[] SearchTerms { get; set; }

        //    public ParsedArgs()
        //    {
        //        SearchTerms = this.SearchTerms;
        //        if (NullArray.IsNullOrEmpty(SearchTerms))
        //        {
        //            SearchTerms = new[] {"*password*", "*sensitive*", "*admin*", "*login*", "*secret*",
        //                "unattend*.xml", "*.vmdk", "*creds*", "*credential*", "*.config"};
        //        }
        //    }
        //}

        //public static class NullArray
        //{
        //    public static bool IsNullOrEmpty(this Array array)
        //    {
        //        return (array == null || array.Length == 0);
        //    }
        //}
    }
}
