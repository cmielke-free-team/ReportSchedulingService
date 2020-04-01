function Install-Service
{
    [CmdletBinding]
    param
    (
        [String]$ServiceName,
	    [String]$PathToExecutable,
        [String]$ExecutableArgs,
	    [String]$DisplayName,
		[ValidateSet("Manual","Automatic","Disabled")]
		[String]$StartMode,
		[Boolean]$DesktopInteract,
	    [String]$StartName,
	    [String]$StartPassword,
		[Boolean]$UseInstallUtil,
		[String]$InstallUtilExePath        
    )

    write-host "Installing $ServiceName..."
	
	$serviceWmiObject = Get-WmiObject -Class Win32_Service -Filter "Name='$ServiceName'"
	if($serviceWmiObject)
	{
		write-host "Service '$ServiceName' already exists. Skipping installation."
	}
	else
	{
		if($UseInstallUtil)
		{      
            write-host "Using InstallUtil"      
			if(!$InstallUtilExePath)
			{			
				$InstallUtilExePath = $(join-path $env:windir "Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe")
				write-host "InstallUtilExePath was not specified. Use $InstallUtilExePath."
			}
			if(!(Test-Path $InstallUtilExePath))
			{
				throw "Unable to find InstallUtil at the specified location: $InstallUtilExePath"
			}

			$installUtilCmdLine = "& `"$InstallUtilExePath`" --% `"$PathToExecutable`""
			write-host "Executing $installUtilCmdLine"
			Invoke-Expression $installUtilCmdLine | Write-Host  

		}
		else
		{            
			$newServiceArgs = @{
				Name = $ServiceName;
				BinaryPathName = "`"$PathToExecutable`" $ExecutableArgs";
			};
			if($DisplayName)
			{
				$newServiceArgs.Add("DisplayName", $DisplayName)
			}
			if($StartMode)
			{
				$newServiceArgs.Add("StartupType", $StartMode)
			}
			New-Service @newServiceArgs
		}
	}

    write-host "Configuring service '$ServiceName'"    

    $serviceWmiObject = Get-WmiObject -Class Win32_Service -Filter "Name='$ServiceName'"    
	if($StartName -inotin @("LocalSystem","NT AUTHORITY\LocalService","NT AUTHORITY\NetworkService"))	
	{	     
		if($StartName -ilike ".\*")
		{
			$StartName = $StartName.Replace(".\", "$env:COMPUTERNAME\")            
		}
		elseif($StartName -inotlike "*\*" -and $StartName -inotlike "*@*")
		{
			$StartName = "$env:COMPUTERNAME\$StartName"
		}

		import-module (join-path $PSScriptRoot "psmodules\UserRights.ps1")
    	write-host "Granting SeServiceLogonRight to $StartName"
    	Grant-UserRight -Account $WindowsServiceRunAsUsername -Right SeServiceLogonRight
	}

	$serviceParams = new-object psobject -Property @{
		DisplayName=$(if($DisplayName) { $DisplayName } else { $null });
		PathName="`"$PathToExecutable`" $ExecutableArgs";
		ServiceType=$null;
		ErrorControl=$null;
		StartMode=$StartMode;
		DesktopInteract=$DesktopInteract;
		StartName=$StartName;
		StartPassword=$StartPassword;
		LoadOrderGroup=$null;
		LoadOrderGroupDependencies=$null;
		ServiceDependencies=$null;
	}
	$serviceChangeResult = $serviceWmiObject.Change(
		$serviceParams.DisplayName,
		$serviceParams.PathName,
		$serviceParams.ServiceType,
		$serviceParams.ErrorControl,
		$serviceParams.StartMode,
		$serviceParams.DesktopInteract,
		$serviceParams.StartName,
		$serviceParams.StartPassword,
		$serviceParams.LoadOrderGroup,
		$serviceParams.LoadOrderGroupDependencies,
		$serviceParams.ServiceDependencies);

	if($serviceChangeResult.ReturnValue -ne 0)
	{
		throw "Unable to configure the service due to error code $($serviceChangeResult.ReturnValue). See https://msdn.microsoft.com/en-us/library/windows/desktop/aa384901.aspx for more info."
	}
	write-host "$ServiceName installed."
    
}

function Uninstall-Service
{
    [CmdletBinding]
    param
    (
        [String]$ServiceName,
        [Boolean]$UseInstallUtil,
		[String]$InstallUtilExePath
    )
	
	$serviceWmiObject = Get-WmiObject -Class Win32_Service -Filter "Name='$ServiceName'"
	if(!$serviceWmiObject)
	{
		write-host "Service '$ServiceName' is not installed"
	}
	else
	{
		Write-Host "Stopping service '$ServiceName'"
		get-service $ServiceName | stop-service -verbose

		if($UseInstallUtil)
		{
			if(!$InstallUtilExePath)
			{			
				$InstallUtilExePath = $(join-path $env:windir "Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe")
				write-host "InstallUtilExePath was not specified. Use $InstallUtilExePath."
			}
			if(!(Test-Path $InstallUtilExePath))
			{
				throw "Unable to find InstallUtil at the specified location: $InstallUtilExePath"
			}
			
			$installUtilCmdLine = "& `"$InstallUtilExePath`" --% /u $($serviceWmiObject.PathName)"
			write-host "Executing $installUtilCmdLine"
			Invoke-Expression $installUtilCmdLine | Write-Host  

		}			
	}

	Get-WmiObject -Class Win32_Service -Filter "Name='$ServiceName'" |
	foreach { 
		write-host "Uninstalling service '$($_.Name)'..."
		$_.Delete() | out-null
		write-host "$ServiceName uninstalled."
	}	
}
