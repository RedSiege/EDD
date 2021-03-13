using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EDD
{
    class Amass
    {
        public List<string> GetGroupMembers(string targetedComputer, string GroupName)
        {
            List<string> groupMembers = new List<string>();
            
            return groupMembers;
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
                        filePathstoReview.Add("\\\\" + singleComp + "\\" + alShare[i].ToString() + "\\");
                    }
                }
            }
            return filePathstoReview;
        }

        [DllImport("Netapi32.dll", EntryPoint = "NetShareEnum")]
        protected static extern int NetShareEnum([MarshalAs(UnmanagedType.LPWStr)] string servername, [MarshalAs(UnmanagedType.U4)] uint level, out IntPtr bufptr,
            [MarshalAs(UnmanagedType.U4)] int prefmaxlen, [MarshalAs(UnmanagedType.U4)] out uint entriesread,
            [MarshalAs(UnmanagedType.U4)] out uint totalentries, [MarshalAs(UnmanagedType.U4)] out uint resume_handle);

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
    }
}
