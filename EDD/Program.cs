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

                // Searches across domain systems for a specific process running
                //WMI processSearcher = new WMI();
                //processSearcher.CheckProcesses(domainComputers, "putty.exe");

                // Searches for domain shares that current account can access
                Amass shepherd = new Amass();
                shepherd.GetShares(domainComputers);

                // Get list of local group members
                //shepherd.GetGroupMembers("192.168.202.67", "Administrators");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error is - " + e);
            }

            Console.WriteLine("\n[!] EDD is done running!");
        }
    }
}
