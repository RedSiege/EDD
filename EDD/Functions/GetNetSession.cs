using EDD.Models;

using System.Collections.Generic;

namespace EDD.Functions
{
    public class GetNetSession : EDDFunction
    {
        public override string FunctionName => "GetNetSession";

        public override string[] Execute(ParsedArgs args)
        {
            if (string.IsNullOrEmpty(args.ComputerName))
                throw new EDDException("ComputerName cannot be empty");

            Amass sessionInfo = new Amass();
            List<Amass.SESSION_INFO_10> incomingSessions = sessionInfo.GetRemoteSessionInfo(args.ComputerName);

            List<string> results = new List<string>();

            foreach (Amass.SESSION_INFO_10 sessionInformation in incomingSessions)
            {
                results.Add($"Connection From: {sessionInformation.sesi10_cname}");
                results.Add($"Idle Time: {sessionInformation.sesi10_idle_time}");
                results.Add($"Total Active Time: {sessionInformation.sesi10_time}");
                results.Add($"Username: {sessionInformation.sesi10_username}");
            }

            return results.ToArray();
        }
    }
}
