using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.DirectoryServices.AccountManagement;

namespace EDD
{
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
                // if your call the GetMembers, you can optionally specify a "Recursive" flag - done here
                var allMembers = myGroup.GetMembers(true);
                foreach (var groupMember in allMembers)
                {
                    domainGroupMembers.Add(groupMember.ToString());
                }
            }

            return domainGroupMembers;
        }

        public List<string> GetShares(List<string> targetedComputers)
        {
            List<string> filePathstoReview = new List<string>();
            foreach (string singleComp in targetedComputers)
            {
                IntPtr buffer;
                uint entriesread;
                uint totalentries;
                uint resume_handle;

                if (NetShareEnum(singleComp, 1, out buffer, -1, out entriesread, out totalentries, out resume_handle) == 0)
                {
                    Int64 ptr = buffer.ToInt64();
                    ArrayList alShare = new ArrayList();
                    for (int i = 0; i < entriesread; i++)
                    {
                        SHARE_INFO_1 shareInfo = (SHARE_INFO_1)Marshal.PtrToStructure(new IntPtr(ptr), typeof(SHARE_INFO_1));
                        if (shareInfo.shi1_type == 0) //Disk drive
                        {
                            alShare.Add(shareInfo.shi1_netname);
                        }
                        ptr += Marshal.SizeOf(shareInfo);
                    }
                    for (int i = 0; i < alShare.Count; i++)
                    {
                        filePathstoReview.Add("\\\\" + singleComp + "\\" + alShare[i].ToString());
                    }
                }
            }

            return filePathstoReview;
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
    }
}
