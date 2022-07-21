using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.InteropServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Principal;

namespace EDD
{
    class UserObject
    {
        public string SamAccountName = null;
        public string Name = null;
        public string Description = null;
        public string DistinguishedName = null;
        public string SID = null;
        public List<string> DomainGroups { get; set; }
        public UserObject()
        {
            DomainGroups = new List<string>();
        }
    }

    class Amass
    {
        public List<string> GetLocalGroupMembers(string targetedComputer, string GroupName)
        {
            List<string> groupMembers = new List<string>();
            int read;
            int total;
            int resume;
            IntPtr pbuf;
            int ret = NetLocalGroupGetMembers(targetedComputer, GroupName, 3, out pbuf, -1, out read, out total, out resume);

            if (ret != 0)
            {
                return groupMembers;
            }
            List<string> members = new List<string>();
            if (read > 0)
            {
                var m = new LOCALGROUP_MEMBERS_INFO_3();
                IntPtr pItem = pbuf;
                for (int i = 0; i < read; ++i)
                {
                    Marshal.PtrToStructure(pItem, m);
                    pItem = new IntPtr(pItem.ToInt64() + Marshal.SizeOf(typeof(LOCALGROUP_MEMBERS_INFO_3)));
                    groupMembers.Add(m.domainandname);
                }
            }
            NetApiBufferFree(pbuf);
            return groupMembers;
        }

        public List<string> GetDomainGroupMembers(string groupName)
        {
            List<string> domainGroupMembers = new List<string>();
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);

            // find the group you're interested in
            GroupPrincipal myGroup = GroupPrincipal.FindByIdentity(ctx, groupName);

            // if you found it - get its members
            if (myGroup != null)
            {
                foreach(Principal groupMember in myGroup.GetMembers())
                {
                    domainGroupMembers.Add(groupMember.SamAccountName);
                }
            }

            return domainGroupMembers;
        }

        public string GetDomainGroupSID(string groupName)
        {
            // set up domain context
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);
            if (group != null)
            {
                SecurityIdentifier groupSid = group.Sid;
                return groupSid.Value;
            }
            else
            {
                return "\nCould not find domain group, did you misspell the name?\nExiting...";
            }
        }

        public List<string> GetDomainUsersInfo(string domainName = null)
        {
            List<string> domainUsers = new List<string>();
            Forest currentForest = GetForestObject();
            if (domainName == null)
            {
                PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain);
                UserPrincipal insUserPrincipal = new UserPrincipal(insPrincipalContext);
                insUserPrincipal.SamAccountName = "*";
                PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
                insPrincipalSearcher.QueryFilter = insUserPrincipal;
                PrincipalSearchResult<Principal> results = insPrincipalSearcher.FindAll();
                foreach (Principal p in results)
                {
                    domainUsers.Add(p.SamAccountName);
                }
            }
            else
            {
                LDAP pathGetter = new LDAP();
                string ldapQueryBuild = GetCurrentDomainPath();
                string[] ldap_split = ldapQueryBuild.Split(',');
                StringComparison stringCompare = StringComparison.CurrentCultureIgnoreCase;
                string buildDomainName = "";
                string domaintoUse = "";
                if (ldapQueryBuild.IndexOf(domainName, stringCompare) >= 0)
                {
                    if (ldap_split.Length > 2)
                    {
                        if (ldap_split[ldap_split.Length - 2].IndexOf(domainName, stringCompare) >= 0)
                        {
                            buildDomainName = ldap_split[ldap_split.Length - 2] + ',' +
                                              ldap_split[ldap_split.Length - 1];
                            domaintoUse = currentForest.Name;
                        }
                        else
                        {
                            buildDomainName = "DC=" + domainName + "," + ldap_split[ldap_split.Length - 2] + ',' + ldap_split[ldap_split.Length - 1];
                            domaintoUse = domainName + "." + currentForest.Name;
                        }
                    }
                }
                else
                {
                    if (ldap_split.Length > 2)
                    {
                        buildDomainName = "DC=" + domainName + "," + ldap_split[ldap_split.Length - 2] + ',' + ldap_split[ldap_split.Length - 1];
                        domaintoUse = domainName + "." + currentForest.Name;
                    }
                    else
                    {
                        buildDomainName = ldapQueryBuild.Insert(5, "DC=" + domainName + ",");
                        domaintoUse = domainName + "." + currentForest.Name;
                    }
                }

                PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain, domaintoUse,buildDomainName);
                UserPrincipal insUserPrincipal = new UserPrincipal(insPrincipalContext);
                insUserPrincipal.SamAccountName = "*";
                PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
                insPrincipalSearcher.QueryFilter = insUserPrincipal;
                PrincipalSearchResult<Principal> results = insPrincipalSearcher.FindAll();
                foreach (Principal p in results)
                {
                    domainUsers.Add(p.SamAccountName);
                }
            }
            return domainUsers;
        }

        public UserObject GetDomainUserInfo(string singledOutUser)
        {
            PrincipalContext insPrincipalContext = new PrincipalContext(ContextType.Domain);
            UserPrincipal insUserPrincipal = new UserPrincipal(insPrincipalContext);
            insUserPrincipal.SamAccountName = singledOutUser;
            PrincipalSearcher insPrincipalSearcher = new PrincipalSearcher();
            insPrincipalSearcher.QueryFilter = insUserPrincipal;
            PrincipalSearchResult<Principal> results = insPrincipalSearcher.FindAll();
            UserObject singleUser = new UserObject();
            foreach (Principal p in results)
            {
                singleUser.SamAccountName = p.SamAccountName;
                singleUser.Name = p.Name;
                singleUser.Description = p.Description;
                singleUser.DistinguishedName = p.DistinguishedName;
                singleUser.SID = p.Sid.ToString();
                foreach (Principal groupName in p.GetGroups())
                {
                    singleUser.DomainGroups.Add(groupName.ToString());
                }
            }

            return singleUser;
        }

        public Forest GetForestObject()
        {
            return Forest.GetCurrentForest();
        }

        private static string GetCurrentDomainPath()
        {
            try
            {
                DirectoryEntry de = new DirectoryEntry("LDAP://RootDSE");
                return de.Properties["defaultNamingContext"][0].ToString();
            }
            catch (COMException)
            {
                Console.WriteLine("\nError:");
                Console.WriteLine("Could not contact domain controller, exiting...");
                Environment.Exit(1);
            }

            return "";
        }
        public List<SESSION_INFO_10> GetRemoteSessionInfo(string targetedSystem)
        {
            IntPtr Bufptr;
            int nStatus = 0;
            List<SESSION_INFO_10> sessionInfo = new List<SESSION_INFO_10>();
            Int32 dwEntriesread = 0, dwTotalentries = 0, dwResume_handle = 0;

            Bufptr = (IntPtr)Marshal.SizeOf(typeof(SESSION_INFO_10));
            SESSION_INFO_10[] results = new SESSION_INFO_10[0];
            do
            {
                nStatus = NetSessionEnum(targetedSystem, null, null, 10, out Bufptr, -1, ref dwEntriesread, ref dwTotalentries, ref dwResume_handle);
                results = new SESSION_INFO_10[dwEntriesread];
                if (nStatus == 234 || nStatus == 0)
                {
                    Int64 p = Bufptr.ToInt64();
                    for (int i = 0; i < dwEntriesread; i++)
                    {

                        SESSION_INFO_10 si = (SESSION_INFO_10)Marshal.PtrToStructure(new IntPtr(p), typeof(SESSION_INFO_10));
                        sessionInfo.Add(si);
                        p += Marshal.SizeOf(typeof(SESSION_INFO_10));
                    }
                }
                Marshal.FreeHGlobal(Bufptr);
            }
            while (nStatus == 234);

            return sessionInfo;
        }

        public List<WKSTA_USER_INFO_1> GetLoggedOnUsers(string targetedComp)
        {
            IntPtr Bufptr;
            List<WKSTA_USER_INFO_1> loggedonInfo = new List<WKSTA_USER_INFO_1>();
            int nStatus = 0;
            Int32 dwEntriesread = 0, dwTotalentries = 0, dwResumehandle = 0;

            Bufptr = (IntPtr)Marshal.SizeOf(typeof(WKSTA_USER_INFO_1));
            WKSTA_USER_INFO_1[] results = new WKSTA_USER_INFO_1[0];
            do
            {
                nStatus = NetWkstaUserEnum(targetedComp, 1, out Bufptr, 32768, out dwEntriesread, out dwTotalentries, ref dwResumehandle);
                if ((nStatus == 0) || (nStatus == 234))
                {
                    if (dwEntriesread > 0)
                    {
                        IntPtr pstruct = (IntPtr)Bufptr;
                        
                        for (int i = 0; i < dwEntriesread; i++)
                        {
                            WKSTA_USER_INFO_1 wui1 = (WKSTA_USER_INFO_1)Marshal.PtrToStructure(pstruct, typeof(WKSTA_USER_INFO_1));
                            if (!wui1.wkui1_username.EndsWith("$"))
                            {
                                loggedonInfo.Add(wui1);
                                pstruct = (IntPtr)((long)pstruct + Marshal.SizeOf(typeof(WKSTA_USER_INFO_1)));
                            }
                        }
                    }
                }

                if (Bufptr != IntPtr.Zero)
                    NetApiBufferFree(Bufptr);

            } while (nStatus == 234);
            return loggedonInfo;
        }

        public string[] GetShares(List<string> targetedComputers, int threads)
        {
            List<string> filePathsList = new List<string>();
            // These aren't currently needed but if we run into issues in the future we'll implement them
            string[] excludeShares = new[] { "C$", "Admin$", "IPC$", "Print$" };

            string[][] filePaths = targetedComputers
                .AsParallel().WithDegreeOfParallelism(threads)
                .Select(DoGetShares).ToArray();

            foreach (var filePath in filePaths)
            {
                foreach (var share in filePath)
                {
                    filePathsList.Add(share);
                }
            }

            return filePathsList.ToArray();
        }

        public string[] DoGetShares(string target)
        {
            List<string> filePathstoReview = new List<string>();

            IntPtr buffer;
            uint entriesread;
            uint totalentries;
            uint resume_handle;

            if (NetShareEnum(target, 1, out buffer, -1, out entriesread, out totalentries, out resume_handle) == 0)
            {
                Int64 ptr = buffer.ToInt64();
                ArrayList alShare = new ArrayList();
                for (int i = 0; i < entriesread; i++)
                {
                    SHARE_INFO_1 shareInfo =
                        (SHARE_INFO_1) Marshal.PtrToStructure(new IntPtr(ptr), typeof(SHARE_INFO_1));
                    if (shareInfo.shi1_type == 0) //Disk drive
                    {
                        alShare.Add(shareInfo.shi1_netname);
                    }

                    ptr += Marshal.SizeOf(shareInfo);
                }

                for (int i = 0; i < alShare.Count; i++)
                {
                    filePathstoReview.Add("\\\\" + target + "\\" + alShare[i].ToString());
                }
            }

            return filePathstoReview.ToArray();
        }

        public string GetUsernameFromSID(string sid)
        {
            try
            {
                SecurityIdentifier s = new SecurityIdentifier(sid);
                return s.Translate(typeof(NTAccount)).Value;
            }
            catch (IdentityNotMappedException)
            {
                return "\nCould not map SID to user/group name.\nDid you provide the correct SID value?\nExiting...";
            }
        }

        [DllImport("Netapi32.dll", EntryPoint = "NetShareEnum")]
        protected static extern int NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.U4)] uint level, out IntPtr bufptr,
            [MarshalAs(UnmanagedType.U4)] int prefmaxlen, [MarshalAs(UnmanagedType.U4)] out uint entriesread,
            [MarshalAs(UnmanagedType.U4)] out uint totalentries, [MarshalAs(UnmanagedType.U4)] out uint resume_handle);

        [DllImport("Netapi32.dll", EntryPoint = "NetLocalGroupGetMembers", CharSet = CharSet.Unicode)]
        internal extern static int NetLocalGroupGetMembers(string servername, string groupname, int level, out IntPtr bufptr, int prefmaxlen,
            out int entriesread, out int totalentries, out int resumehandle);

        [DllImport("Netapi32.dll", EntryPoint = "NetApiBufferFree")]
        public extern static int NetApiBufferFree(IntPtr Buffer);

        [DllImport("netapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int NetWkstaUserEnum(
            string servername,
            int level,
            out IntPtr bufptr,
            int prefmaxlen,
            out int entriesread,
            out int totalentries,
            ref int resume_handle);

        // found here https://github.com/RcoIl/CSharp-Tools
        [DllImport("netapi32.dll", SetLastError = true)]
        private static extern int NetSessionEnum(
            [In, MarshalAs(UnmanagedType.LPWStr)] string ServerName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string UncClientName,
            [In, MarshalAs(UnmanagedType.LPWStr)] string UserName,
            Int32 Level,
            out IntPtr bufptr,
            int prefmaxlen,
            ref Int32 entriesread,
            ref Int32 totalentries,
            ref Int32 resume_handle);

        [StructLayout(LayoutKind.Sequential)]
        protected struct SHARE_INFO_1
        {
            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi1_netname;

            [MarshalAs(UnmanagedType.U4)]
            public uint shi1_type;

            [MarshalAs(UnmanagedType.LPWStr)]
            public string shi1_remark;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        class LOCALGROUP_MEMBERS_INFO_3
        {
            public string domainandname;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SESSION_INFO_10
        {
            [MarshalAs(UnmanagedType.LPWStr)] public string sesi10_cname;
            [MarshalAs(UnmanagedType.LPWStr)] public string sesi10_username;
            public uint sesi10_time;
            public uint sesi10_idle_time;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct WKSTA_USER_INFO_1
        {
            public string wkui1_username;
            public string wkui1_logon_domain;
            public string wkui1_oth_domains;
            public string wkui1_logon_server;
        }
    }
}
