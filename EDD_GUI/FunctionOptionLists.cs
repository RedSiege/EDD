using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDD_GUI
{
    internal class FunctionOptionLists
    {
        List<string> ForestList = new List<string>();
        List<string> CompInfoList = new List<string>();
        List<string> UserInfoList = new List<string>();
        List<string> ChainedInfoList = new List<string>();
        List<string> FunctionList = new List<string>();

        internal FunctionOptionLists()
        {
            /*
            CreateChainedList();
            CreateCompInfoList();
            CreateUserInfoList();
            CreateForestList();
            CreateFunctionList();
            */
        }

        internal List<string> CreateFunctionList()
        {
            FunctionList.Add("Forest/Domain Information");
            FunctionList.Add("Computer Information");
            FunctionList.Add("User Information");
            FunctionList.Add("Chained Information");
            return FunctionList;
        }

        internal List<string> CreateForestList()
        {
            ForestList.Add("getdomainsid");
            ForestList.Add("getforest");
            ForestList.Add("getforestdomains");
            ForestList.Add("getsiddata");
            ForestList.Add("getadcsservers");
            return ForestList;
        }

        internal List<string> CreateCompInfoList()
        {
            CompInfoList.Add("getdomaincomputers");
            CompInfoList.Add("getdomaincontrollers");
            CompInfoList.Add("getdomainshares");
            CompInfoList.Add("getreadabledomainshares");
            return CompInfoList;
        }

        internal List<string> CreateUserInfoList()
        {
            UserInfoList.Add("changeaccountpassword");
            UserInfoList.Add("customldapquery");
            UserInfoList.Add("getuserdacl");
            UserInfoList.Add("getnetlocalgroupmember");
            UserInfoList.Add("getdomaingroupmember");
            UserInfoList.Add("getdomainuser");
            UserInfoList.Add("getdomaindescriptions");
            UserInfoList.Add("getnetsession");
            UserInfoList.Add("getnetloggedon");
            UserInfoList.Add("getuserswithspns");
            UserInfoList.Add("getdomaingroupsid");
            UserInfoList.Add("getdomainsid");
            UserInfoList.Add("getsiddata");
            UserInfoList.Add("joingroupbysid");
            UserInfoList.Add("joingroupbyname");
            return UserInfoList;
        }

        internal List<string> CreateChainedList()
        {
            ChainedInfoList.Add("findadminsch");
            ChainedInfoList.Add("findadminwmi");
            ChainedInfoList.Add("finddomainprocess");
            ChainedInfoList.Add("finddomainuser");
            ChainedInfoList.Add("findinterestingdomainsharefile");
            ChainedInfoList.Add("findwritableshares");
            return ChainedInfoList;
        }
    }
}
