# WPF Frontend

<p align="center">
	<img align="center" src="https://raw.githubusercontent.com/whiterabb17/sifter/master/docs/sifter.png">
</p>

# EDD
Enumerate Domain Data is designed to be similar to PowerView but in .NET. PowerView is essentially the ultimate domain enumeration tool, and we wanted a .NET implementation that we worked on ourselves. This tool was largely put together by viewing implementations of different functionality across a wide range of existing projects and combining them into EDD. 

## Usage

To use EDD, you just need to call the application, provide the function that you want to run (listed below) and provide any optional/required parameters used by the function.

Arguments:

	-f, --function=VALUE       the function you want to use
	-o, --output=VALUE         the path to the file to save
	-c, --computername=VALUE   the computer you are targeting
	-n, --canonicalname=VALUE  canonical name for domain user
	-d, --domainname=VALUE     the computer you are targeting
	-g, --groupname=VALUE      the domain group you are targeting
	-p, --processname=VALUE    the process you are targeting
	-w, --password=VALUE       the password to authenticate with or what you are
	                           setting it to
	-u, --username=VALUE       the domain account you are targeting
	-t, --threads=VALUE        the number of threads to run (default: 5)
	-q, --query=VALUE          custom LDAP filter to search
	-a, --adright=VALUE        Active Directory Rights to return, separated by
	                           commas
	-s, --search=VALUE         the search term(s) for
	                             FindInterestingDomainShareFile separated by a
	                             comma (,), accepts wildcards
	--sharepath=VALUE      the specific share to search for interesting files
	-i, --info                 returns information on a specifed function
	-l, --listfunction         returns all available functions

	-h, --help                 show this message and exit


## Functions

The following functions can be used with the -f flag to specify the data you want to enumerate/action you want to take.

### Forest/Domain Information
	getdomainsid - Returns the domain sid (by default current domain if no domain is provided)
	getforest - returns the name of the current forest
	getforestdomains - returns the name of all domains in the current forest
	getsiddata - Converts a SID to the corresponding group or domain name (use the -u option for providing the SID value)
	getadcsservers - Get a list of servers running AD CS within the current domain

### Computer Information
	getdomaincomputers - Get a list of all computers in the domain
	getdomaincontrollers - Gets a list of all domain controllers
	getdomainshares - Get a list of all domain shares
	getreadabledomainshares - Get a list of all readable domain shares

### User Information
	changeaccountpassword - Change the password for a targeted account
	customldapquery - Set arbitrary LDAP filter to search for objects
	getuserdacl - Returns DACL of a specified domain object
	getnetlocalgroupmember - Returns a list of all users in a local group on a remote system
	getdomaingroupmember - Returns a list of all users in a domain group
	getdomainuser - Retrieves info about specific user (name, description, SID, Domain Groups)
	getdomaindescriptions - returns domain objects with non-standard account descriptions
	getnetsession - Returns a list of accounts with sessions on the targeted system
	getnetloggedon - Returns a list of accounts logged into the targeted system
	getuserswithspns - Returns a list of all domain accounts that have a SPN associated with them
	getdomaingroupsid - Fetch the SID of a group
	getdomainsid - Fetch SID of domain
	getsiddata - Return username from SID
	joingroupbysid - Join an account to a group via the group's sid
	joingroupbyname - Join an account to a group via the group's name

### Chained Information
	findadminsch - Uses the task scheduler to query for admin rights within a domain
	findadminwmi - Uses WMI to search for admin rights within a domain
	finddomainprocess - Search for a specific process across all systems in the domain (requires admin access on remote systems)
	finddomainuser - Searches the domain environment for a specified user or group and tries to find active sessions (default searches for Domain Admins)
	findinterestingdomainsharefile - Searches the domain environment for all accessible shares. Once found, it parses all filenames for "interesting" strings
	findwritableshares - Enumerates all shares in the domain and then checks to see if the current account can create a text file in the root level share, and one level deep.


## References
	PowerView - https://github.com/PowerShellMafia/PowerSploit/blob/master/Recon/PowerView.ps1
	CSharp-Tools - https://github.com/RcoIl/CSharp-Tools
	StackOverflow - Random questions (if this isn't somehow listed as a reference, we know we're forgetting it :))
	SharpView - https://github.com/tevora-threat/SharpView

