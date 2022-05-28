# EDD
Enumerate Domain Data is designed to be similar to PowerView but in .NET. PowerView is essentially the ultimate domain enumeration tool, and we wanted a .NET implementation that we worked on ourselves. This tool was largely put together by viewing implementations of different functionality across a wide range of existing projects and combining them into EDD. 

## Usage

To use EDD, you just need to call the application, provide the function that you want to run (listed below) and provide any optional/required parameters used by the function.

<img width="589" alt="updated edd help" src="https://user-images.githubusercontent.com/15634696/124296317-0d5f1d00-db17-11eb-9ed2-b8f1dce9f627.png">


## Functions

The following functions can be used with the -f flag to specify the data you want to enumerate/action you want to take.

### Forest/Domain Information
	getdomainsid - Returns the domain sid (by default current domain if no domain is provided)
	getforest - returns the name of the current forest
	getforestdomains - returns the name of all domains in the current forest
	convertsidtoname - Converts a SID to the corresponding group or domain name (use the -u option for providing the SID value)
	getadcsservers - Get a list of servers running AD CS within the current domain

### Computer Information
	getdomaincomputers - Get a list of all computers in the domain
	getdomaincontrollers - Gets a list of all domain controllers
	getdomainshares - Get a list of all domain shares
	getreadabledomainshares - Get a list of all readable domain shares

### User Information
	getnetlocalgroupmember - Returns a list of all users in a local group on a remote system
	getdomaingroupmember - Returns a list of all users in a domain group
	getdomainuser - Retrieves info about specific user (name, description, SID, Domain Groups)
	getnetsession - Returns a list of accounts with sessions on the targeted system
	getnetloggedon - Returns a list of accounts logged into the targeted system
	getuserswithspns - Returns a list of all domain accounts that have a SPN associated with them

### Chained Information
	findadminsch - Uses the task scheduler to query for admin rights within a domain
	finddomainprocess - Search for a specific process across all systems in the domain (requires admin access on remote systems)
	finddomainuser - Searches the domain environment for a specified user or group and tries to find active sessions (default searches for Domain Admins)
	findinterestingdomainsharefile - Searches the domain environment for all accessible shares. Once found, it parses all filenames for "interesting" strings
	findwritableshares - Enumerates all shares in the domain and then checks to see if the current account can create a text file in the root level share, and one level deep.


## References
	PowerView - https://github.com/PowerShellMafia/PowerSploit/blob/master/Recon/PowerView.ps1
	CSharp-Tools - https://github.com/RcoIl/CSharp-Tools
	StackOverflow - Random questions (if this isn't somehow listed as a reference, we know we're forgetting it :))
	SharpView - https://github.com/tevora-threat/SharpView

