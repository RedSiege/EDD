using EDD.Models;

using Mono.Options;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace EDD
{
    class Program
    {
        static List<EDDFunction> _eddFunctions = new List<EDDFunction>();

        static void Main(string[] args)
        {
            ParsedArgs parsedArgs = new ParsedArgs();

            string functionName = null;
            string fileSavePath = null;
            bool show_help = false;

            var p = new OptionSet() {
                    { "f|function=", "the function you want to use", (v) => functionName = v },
                    { "o|output=", "the path to the file to save", (v) => fileSavePath = v },
                    { "c|computername=", "the computer you are targeting", (v) => parsedArgs.ComputerName = v },
                    { "d|domainname=", "the computer you are targeting", (v) => parsedArgs.DomainName = v },
                    { "g|groupname=", "the domain group you are targeting", (v) => parsedArgs.GroupName = v },
                    { "p|processname=", "the process you are targeting", (v) => parsedArgs.ProcessName = v },
                    { "u|username=", "the domain account you are targeting", (v) => parsedArgs.UserName = v },
                    { "h|help",  "show this message and exit", v => show_help = v != null },
                };

            try
            {
                p.Parse(args);

                if (show_help)
                {
                    ShowHelp(p);
                    return;
                }

                InitFunctions();

                EDDFunction function = _eddFunctions.FirstOrDefault(f => f.FunctionName.Equals(functionName, StringComparison.InvariantCultureIgnoreCase));

                if (function is null)
                {
                    Console.WriteLine($"Function {functionName} does not exist");
                    return;
                }

                string[] results = function.Execute(parsedArgs);

                if (results is null || results.Length < 1)
                {
                    Console.WriteLine("No results");
                    return;
                }

                foreach (string result in results)
                    Console.WriteLine(result);

                if (!string.IsNullOrEmpty(fileSavePath))
                {
                    File.AppendAllText(fileSavePath, $"{functionName}:{Environment.NewLine}");
                    File.AppendAllLines(fileSavePath, results);
                    File.AppendAllText(fileSavePath, Environment.NewLine);
                }
            }
            catch (OptionException e)
            {
                Console.Write("EDD.exe: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `EDD.exe --help' for more information.");
            }
            catch (EDDException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("\n[!] EDD is done running!");
            }
        }

        static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: EDD.exe -f <function name> -<extra options>");
            Console.WriteLine("Provide the function you want to run to enumerate that data from the domain");
            Console.WriteLine("Also provide any other extra options that you need for the specific function");
            Console.WriteLine();
            Console.WriteLine("Arguments:");
            p.WriteOptionDescriptions(Console.Out);
        }

        static void InitFunctions()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (type.IsSubclassOf(typeof(EDDFunction)))
                {
                    EDDFunction function = Activator.CreateInstance(type) as EDDFunction;
                    _eddFunctions.Add(function);
                }
            }
        }
    }
}
