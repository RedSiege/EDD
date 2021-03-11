using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDD
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Get a list of domain computers
                LDAP domainQuery = new LDAP();
                List<string> domainComputers = domainQuery.CaptureComputers();

                WMI processSearcher = new WMI();
                processSearcher.CheckProcesses(domainComputers, "putty.exe");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error is - " + e);
            }

            Console.WriteLine("\n[!] EDD is done running!");
        }
    }
}
