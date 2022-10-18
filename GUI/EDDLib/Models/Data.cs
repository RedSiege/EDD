using System.ComponentModel;

namespace EDDLib.Models
{
    internal class Data
    {
        public static string[] BlacklistedDesc = {
            "Built-in account for administering the computer/domain",
            "Built-in account for guest access to the computer/domain",
            "Key Distribution Center Service Account",
            "A user account managed by the system."
        };

        public static string[] ADRights = { "GenericAll", "GenericRead", "GenericWrite", "GenericExecute", "ReadProperty", "ReadControl",
                "WriteProperty", "WriteOwner", "WriteDacl", "ListChildren", "ListObject", "CreateChild", "Delete", "DeleteChild", "DeleteTree",
                "ExtendedRight", "Self", "Syncronize", "AccessSystemSecurity"};

        public enum ControlType
        {
            Inactive = 0,
            Deny = 1,
            Allow = 2
        }

        public enum BuiltinSID
        {
            [Description("Administrators")]
            Administrators = 544,
            [Description("Users")]
            Users = 545,
            [Description("Guests")]
            Guests = 546,
            [Description("Power Users")]
            Power_Users = 547,
            [Description("Account Operators")]
            Account_Operators = 548,
            [Description("Server Operators")]
            Server_Operators = 549,
            [Description("Print Operators")]
            Print_Operators = 550,
            [Description("Backup Operators")]
            Backup_Operators = 551,
            [Description("Replicators")]
            Replicators = 552,
            [Description("Builtin\\Pre-Windows 2000 Compatable Access")]
            PreWin2000 = 554,
            [Description("Builtin\\Remote Desktop Users")]
            RemoteDesktopUsers = 555,
            [Description("Builtin\\Network Configuration Operators")]
            NetConfigOperators = 556,
            [Description("Builtin\\Incoming Forest Trust Builders")]
            IncomingForestTrustBuilders = 557,
            [Description("Builtin\\Performance Monitor Users")]
            PerformanceMonitorUsers = 558,
            [Description("Builtin\\Performance Log Users")]
            PerformanceLogUsers = 559,
            [Description("Builtin\\Windows Authorization Access Group")]
            WinAuthGroup = 560,
            [Description("Builtin\\Terminal Server License Servers")]
            TerminalSvr = 561,
            [Description("Builtin\\Distributed COM Users")]
            DCOMUsers = 562,
            [Description("Builtin\\Cryptographic Operators")]
            CrytpoOperators = 569,
            [Description("Builtin\\Event Log Readers")]
            EventLogReaders = 573,
            [Description("Builtin\\Certificate Service DCOM Access")]
            CSDCOMAccess = 574,
            [Description("Builtin\\RDS Remote Access Servers")]
            RemoteAccessSvr = 575,
            [Description("Builtin\\RDS Endpoint Servers")]
            RDSESvr = 576,
            [Description("Builtin\\RDS Management Servers")]
            RDSMSvr = 577,
            [Description("Builtin\\Hyper-V Administrators")]
            HVAdmins = 578,
            [Description("Builtin\\Access Control Assistance Operators")]
            ACAOperators = 579,
            [Description("Builtin\\Remote Management Users")]
            RemoteManageUsers = 580
        }
    }
}
