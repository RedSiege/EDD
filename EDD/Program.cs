using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDD
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get a list of domain computers
            LDAP domainQuery = new LDAP();
            List<string> domainComputers = domainQuery.CaptureComputers();

            WMI processSearcher = new WMI();
            processSearcher.CheckProcesses(domainComputers, "notepad.exe");
        }
    }
}
