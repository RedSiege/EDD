using System;
using EDD.Models;

using System.Collections.Generic;
using System.IO;

namespace EDD.Functions
{
    public class FindWritableShares : EDDFunction
    {
        public override string FunctionName => "FindWritableShares";

        public override string[] Execute(ParsedArgs args)
        {
            try
            {
                LDAP computerQuery = new LDAP();
                List<string> domainSystems = computerQuery.CaptureComputers();
                Amass shareMe = new Amass();
                string[] allShares = shareMe.GetShares(domainSystems, args.Threads);

                List<string> successfulShareWrites = new List<string>();

                foreach (string sharePath in allShares)
                {
                    // Get current date to have something to write
                    string time = DateTime.Now.ToString();

                    // try to write directly to the root of the share
                    try
                    {
                        using (StreamWriter outputFile = new StreamWriter(Path.Combine(sharePath, "testwritefile.txt")))
                        {
                            outputFile.WriteLine(time);
                            successfulShareWrites.Add(sharePath);
                        }

                        File.Delete(Path.Combine(sharePath, "testwritefile.txt"));
                        if (File.Exists(Path.Combine(sharePath, "testwritefile.txt")))
                        {
                            Console.WriteLine("[-] ALERT: Successfully wrote file but could not delete it at this location: " + Path.Combine(sharePath, "testwritefile.txt"));
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // do nothing
                    }
                    catch (IOException)
                    {
                        // do nothing
                    }

                    try
                    {
                        // enumerate folders in the share
                        string[] dirNames = Directory.GetDirectories(sharePath, "*", SearchOption.TopDirectoryOnly);

                        // try to write to the 1st level of folders
                        foreach (string subdirPath in dirNames)
                        {
                            try
                            {
                                using (StreamWriter outputFile =
                                    new StreamWriter(Path.Combine(subdirPath, "testwritefile.txt")))
                                {
                                    outputFile.WriteLine(time);
                                    successfulShareWrites.Add(subdirPath);
                                }
                                File.Delete(Path.Combine(subdirPath, "testwritefile.txt"));
                                if (File.Exists(Path.Combine(subdirPath, "testwritefile.txt")))
                                {
                                    Console.WriteLine("[-] ALERT: Successfully wrote file but could not delete it at this location: " + Path.Combine(subdirPath, "testwritefile.txt"));
                                }
                            }
                            catch (UnauthorizedAccessException)
                            {
                                // do nothing
                            }
                            catch (IOException)
                            {
                                // do nothing
                            }
                        }
                    }
                    catch (IOException)
                    {
                        // ignore
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // ignore
                    }
                }

                return successfulShareWrites.ToArray();
            }
            catch (Exception e)
            {
                foreach (string path in successfulShareWrites)
                {
                    Console.WriteLine(path);
                }

                Console.WriteLine("[X] ERROR State Occurred - Paths above are current status prior to error!");
                return new string[] {"[X] Failure to enumerate info - " + e};
            }
        }
    }
}
