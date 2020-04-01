[cmdletbinding()]
param(
    [string]$AppInsightsName = $env:AppInsightsName
)

###############################################################################
# get app insights instrumentation key
###############################################################################
$resourceName = $AppInsightsName
$env:ApplicationInsights_IsDisabled = "true"
$env:ApplicationInsights_InstrumentationKey = ""
if ($AppInsightsName) {
    Write-Host "Getting Azure Application Insights instrumentation key"
    import-module az.resources
    $resource = Get-AzResource -Name $AppInsightsName -ResourceType "Microsoft.Insights/components"
    if ($resource -is [system.array]) {    
        throw "Multiple resources with the name '$AppInsightsName' were found in the following resource groups: $(($resource | select -ExpandProperty ResourceGroupName) -join ", "). Remove one of the resources and run the task again."
    }
    elseif ($resource) {       
        $details = Get-AzResource -ResourceId $resource.Id
        $env:ApplicationInsights_IsDisabled = "false"
        $env:ApplicationInsights_InstrumentationKey = $details.Properties.InstrumentationKey                
    }
    else {
        write-warning "Specified AppInsights resource was not found: $resourceName"        
    }
}
write-host "##vso[task.setvariable variable=ApplicationInsights_IsDisabled;]$env:ApplicationInsights_IsDisabled"
write-host "##vso[task.setvariable variable=ApplicationInsights_InstrumentationKey;]$env:ApplicationInsights_InstrumentationKey"
