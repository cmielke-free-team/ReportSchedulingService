[cmdletbinding()]
param(    
    [string]$EventLogSource,    
    [string]$LogFolder,  
    [string]$PathToExecutable,
    [string]$MSDeployPackageFile,
    [string]$MSDeployParametersFile
)

###############################################################################
# wait for running processes to exit
###############################################################################
$processName = [system.io.path]::GetFileNameWithoutExtension($PathToExecutable)
Write-Host "Waiting for running $processName processes"
Get-Process -Name $processName -ErrorAction SilentlyContinue | foreach {
    Write-Host "Waiting for process $($_.Name) ($($_.Id)) to exit"
    if ($_.WaitForExit(20000)) {
        Write-Host "Process $($_.Name) ($($_.Id)) exited"
    }
    else {
        Write-Host "Process $($_.Name) ($($_.Id)) did not exit. Killing process."
        $_.Kill()
    }
}

###############################################################################
# create event log source
###############################################################################
if ($EventLogSource) {
    if (!([System.Diagnostics.EventLog]::SourceExists($EventLogSource))) {
        Write-Host "Creating event log source: $EventLogSource"
        New-EventLog -LogName Application -Source $EventLogSource
    }
    else {
        Write-Host "Event log source alread exists: $EventLogSource"
    }
}

###############################################################################
# create log folder
###############################################################################
if ($LogFolder) {
    Write-Host "Creating log folder: $LogFolder"
    New-Item -Path $LogFolder -ItemType Directory -Force
}

###############################################################################
# transform MSDeploy parameters
###############################################################################
Write-Host "Transforming MSDeploy parameters file: $MSDeployParametersFile"
[xml]$msDeployParametersContent = Get-Content -Path $MSDeployParametersFile
$outputParametersFilePath = [system.io.path]::ChangeExtension($MSDeployParametersFile, ".transformed.xml")
foreach ($parameter in $msDeployParametersContent.parameters.parameter) {        
    Write-Host "Transforming parameter: $($parameter.name)"
    $parameterName = $parameter.name
    $envEntry = Get-Item "env:$parameterName" -ErrorAction SilentlyContinue
    if (!$envEntry) {
        #replace periods with underscores and check for a matching environment variable again
        $parameterNameWithUnderscores = ($parameterName -replace "\.", "_")
        $envEntry = Get-Item "env:$parameterNameWithUnderscores" -ErrorAction SilentlyContinue
    }

    if ($envEntry) {
        $parameterValue = $envEntry.Value		
    }
    elseif ($parameter.value) {
        $parameterValue = $parameter.value     
    }
    elseif ($parameter.defaultValue) {
        $parameterValue = $parameter.defaultValue
    } 

    foreach ($envvar in get-childitem env:\) {        
        $parameterValue = $parameterValue -ireplace "%\($($envvar.Name)\)%", $envvar.value        
    }

    if ($parameter.value) {
        $parameter.value = $parameteValue     
    }
    elseif ($parameter.defaultValue) {
        $parameter.defaultValue = $parameterValue
    }

    Write-Host "$($parameter.name) = $parameterValue"
}
$msDeployParametersContent.Save($outputParametersFilePath)


###############################################################################
# run MSDeploy
###############################################################################
$msdeployexe = Join-Path $env:ProgramFiles "IIS\Microsoft Web Deploy V3\msdeploy.exe"
$msdeploycmdline = "& `"$msdeployexe`" --%"
$msdeploycmdline += " -verb:sync" 
$msdeploycmdline += " -source:package=""$MSDeployPackageFile""" 
$msdeploycmdline += " -dest:dirPath=`"$([system.io.path]::GetDirectoryName($PathToExecutable))`""
$msdeploycmdline += " -setParamFile=""$outputParametersFilePath"""
$msdeploycmdline += " -verbose"
Write-Host "Executing $msdeploycmdline"
Invoke-Expression $msdeploycmdline