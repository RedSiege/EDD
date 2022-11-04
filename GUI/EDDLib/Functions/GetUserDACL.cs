using System;
using System.Text;
using System.Linq;
using System.DirectoryServices;
using System.Security.Principal;
using System.Collections.Generic;

using EDDLib.Models;

namespace EDDLib.Functions
{
    internal class GetUserDACL : EDDFunction
    {
        public override string FunctionName => "GetUserDACL";

        public override string FunctionDesc => "Returns DACL of a specified domain object";

        public override string FunctionUsage => "EDD.exe -f getUserDACL -n [canonical name] -a <ADRights>";

        public override string[] Execute(ParsedArgs args)
        {

            string CN = args.CanonicalName.Replace('.', ' ');
            string[] ADRights = args.ADRights.Split(',');
            bool AllRights = false;

            Data.ControlType ct = Data.ControlType.Inactive;

            List<String> QueryOutList = new List<String>();
            StringBuilder outData = new StringBuilder();

            DirectoryEntry entry = new DirectoryEntry();
            DirectorySearcher searcher = new DirectorySearcher(entry);

            searcher.Filter = $"(cn={CN})";

            ActiveDirectorySecurity objACL = searcher.FindOne().GetDirectoryEntry().ObjectSecurity;

            foreach (ActiveDirectoryAccessRule ACE in objACL.GetAccessRules(true, true, typeof(NTAccount)))
            {
                if (ADRights != Data.ADRights || AllRights)
                {
                    if (ADRights.Any(ACE.ActiveDirectoryRights.ToString().Contains) && ct != Data.ControlType.Inactive)
                    {
                        QueryOutList.Add(ACLUtils.returnACEData(outData, ACE, ct));
                    }
                    else if (ADRights.Any(ACE.ActiveDirectoryRights.ToString().Contains)) { QueryOutList.Add(ACLUtils.returnACEData(outData, ACE)); }
                }
                else { QueryOutList.Add(ACLUtils.returnACEData(outData, ACE)); }
            }

            return QueryOutList.ToArray();
        }

        public class ACLUtils
        {

            public static string returnACEData(StringBuilder sb, ActiveDirectoryAccessRule ACE)
            {
                sb.AppendLine($"{nameof(ACE.ActiveDirectoryRights),-25}: {ACE.ActiveDirectoryRights}\n" +
                    $"{nameof(ACE.AccessControlType),-25}: {ACE.AccessControlType}\n");
                if (ACE.IdentityReference.ToString().Contains("S-1-5-32"))
                {
                    Int32 index = Int32.Parse(ACE.IdentityReference.ToString().Split('-')[4]);
                    sb.AppendLine($"{nameof(ACE.IdentityReference),-25}: {((Data.BuiltinSID)index).ToDescription()}");
                }
                else { sb.AppendLine($"{nameof(ACE.IdentityReference),-25}: {ACE.IdentityReference}"); }
                sb.AppendLine($"{nameof(ACE.ObjectType),-25}: {ACE.ObjectType}\n" +
                    $"{nameof(ACE.ObjectFlags),-25}: {ACE.ObjectFlags}");
                if (ACE.IsInherited)
                {
                    if (ACE.PropagationFlags.ToString() != "None") { sb.AppendLine($"{nameof(ACE.PropagationFlags),-25}: {ACE.PropagationFlags}"); }
                    sb.AppendLine($"{nameof(ACE.InheritanceFlags),-25}: {ACE.InheritanceFlags}\n" +
                        $"{nameof(ACE.InheritanceType),-25}: {ACE.InheritanceType}\n" +
                        $"{nameof(ACE.InheritedObjectType),-25}: {ACE.InheritedObjectType}");
                }

                sb.AppendLine();

                return sb.ToString();
            }

            public static string returnACEData(StringBuilder sb, ActiveDirectoryAccessRule ACE, Data.ControlType ct)
            {
                if (ct.Equals(Enum.Parse(typeof(Data.ControlType), ACE.AccessControlType.ToString())))
                {
                    sb.AppendLine($"{nameof(ACE.ActiveDirectoryRights),-25}: {ACE.ActiveDirectoryRights}\n" +
                        $"{nameof(ACE.AccessControlType),-25}: {ACE.AccessControlType}\n");
                    if (ACE.IdentityReference.ToString().Contains("S-1-5-32"))
                    {
                        Int32 index = Int32.Parse(ACE.IdentityReference.ToString().Split('-')[4]);
                        sb.AppendLine($"{nameof(ACE.IdentityReference),-25}: {((Data.BuiltinSID)index).ToDescription()}");
                    }
                    else { sb.AppendLine($"{nameof(ACE.IdentityReference),-25}: {ACE.IdentityReference}"); }
                    sb.AppendLine($"{nameof(ACE.ObjectType),-25}: {ACE.ObjectType}\n" +
                        $"{nameof(ACE.ObjectFlags),-25}: {ACE.ObjectFlags}");
                    if (ACE.IsInherited)
                    {
                        if (ACE.PropagationFlags.ToString() != "None") { sb.AppendLine($"{nameof(ACE.PropagationFlags)}: {ACE.PropagationFlags}"); }
                        sb.AppendLine($"{nameof(ACE.InheritanceFlags),-25}: {ACE.InheritanceFlags}\n" +
                            $"{nameof(ACE.InheritanceType),-25}: {ACE.InheritanceType}\n" +
                            $"{nameof(ACE.InheritedObjectType),-25}: {ACE.InheritedObjectType}");
                    }
                    sb.AppendLine();
                }

                return sb.ToString();
            }
        }
    }
}
