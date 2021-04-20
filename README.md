# EDD
Enumerate Domain Data is designed to be similar to PowerView but in .NET. PowerView is essentially the ultimate domain enumeration tool, and we wanted a .NET implementation that we worked on ourselves. This tool was largely put together by viewing implementations of different functionality across a wide range of existing projects and combining them into EDD. 

## Usage

To use EDD, you just need to call the application, provide the function that you want to run (listed below) and provide any optional/required parameters used by the function.

<img width="589" alt="updated edd help" src="https://user-images.githubusercontent.com/1674822/115320081-61747b00-a13e-11eb-88c0-d947af06e00f.png">

## Functions

The following functions can be used with the -f flag to specify the data you want to enumerate/action you want to take.

### Forest/Domain Information
	getdomainsid - Returns the domain sid (by default current domain if no domain is provided)
	getforest - returns the name of the current forest
	getforestdomains - returns the name of all domains in the current forest
	convertsidtoname - Converts a SID to the corresponding group or domain name (use the -u option for providing the SID value)

### Computer Information
	getdomaincomputers - Get a list of all computers in the domain
	getdomaincontrollers - Gets a list of all domain controllers
	getdomainshares - Get a list of all accessible domain shares

### User Information
	getnetlocalgroupmember - Returns a list of all users in a local group on a remote system
	getnetdomaingroupmember - Returns a list of all users in a domain group
	getdomainusers - Get a list of all domains users
	getdomainuser - Retrieves info about specific user (name, description, SID, Domain Groups)
	getnetsession - Returns a list of accounts with sessions on the targeted system
	getnetloggedon - Returns a list of accounts logged into the targeted system
	getuserswithspns - Returns a list of all domain accounts that have a SPN associated with them

### Chained Information
	finddomainprocess - Search for a specific process across all systems in the domain (requires admin access on remote systems)
	finddomainuser - Searches the domain environment for a specified user or group and tries to find active sessions (default searches for Domain Admins)


## References
	PowerView - https://github.com/PowerShellMafia/PowerSploit/blob/master/Recon/PowerView.ps1
	CSharp-Tools - https://github.com/RcoIl/CSharp-Tools
	StackOverflow - Random questions (if this isn't somehow listed as a reference, we know we're forgetting it :))
	SharpView - https://github.com/tevora-threat/SharpView

