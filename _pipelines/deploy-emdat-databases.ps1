[cmdletbinding()]
param
(    
    [ValidateNotNullOrEmpty()]
    [string]$SourceDacPacsFolder = "$env:System_DefaultWorkingDirectory\dacpacs",

    [ValidateNotNullOrEmpty()]
    [string]$SQL01Endpoint = "(localdb)\$env:AGENT_NAME",
    
    [ValidateNotNullOrEmpty()]
    [string]$SQL02Endpoint = "(localdb)\$env:AGENT_NAME",        
    
    [ValidateNotNullOrEmpty()]
    [string]$EmdatTSharePath = "\\dontmatter\t",
    
    [ValidateNotNullOrEmpty()]
    [string]$SSRSEndpoint = "localhost",
    
    [ValidateNotNullOrEmpty()]
    [string]$ASREnvironmentCode = "LDB",
    
    [ValidateNotNullOrEmpty()]
    [string]$WebSiteHostName = "www.escription-one.com",
    
    [ValidateNotNullOrEmpty()]
    [string]$CBDAPlatformCode = "LDB",
    
    [ValidateNotNullOrEmpty()]
    [string]$DefaultInTouchPhone = "dontmatter"
)

#########################################
# functions
#########################################
function New-SqlLocalDbInstance {
    param
    (
        [ValidateNotNullOrEmpty()]
        [string]$Name,
                        
        [string[]]$DatabasesToCreate
    )
    $instances = sqllocaldb info
    if($instances -icontains $Name)
    {  
        sqllocaldb stop "$Name"
        sqllocaldb delete "$Name"      
    }  
    $dataFilesFolder = "$env:LocalAppData\Microsoft\Microsoft SQL Server Local DB\Instances\$Name"  
    if(Test-Path $dataFilesFolder)
    {
        Remove-Item $dataFilesFolder -Recurse -Force -ErrorAction SilentlyContinue
    }
    sqllocaldb create "$Name"
    sqllocaldb share Administrators "$Name" "$Name"
    sqllocaldb start "$Name"

    $createDatabaseStatement = ""
    foreach($db in $DatabasesToCreate) {
        $createDatabaseStatement += "
            IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '$db') 
            BEGIN
                CREATE DATABASE [$db]
                ON
                PRIMARY ( NAME = $($db)_dat, FILENAME = '$dataFilesFolder\$($db)_dat.mdf' )
                LOG ON  ( NAME = $($db)_log, FILENAME = '$dataFilesFolder\$($db)_log.ldf' );                     
            END
        "
    }

    if($createDatabaseStatement) {
        write-host "Creating databases..."
        write-verbose $createDatabaseStatement
        $sqlConnection = new-object System.Data.SqlClient.SqlConnection
        $sqlConnection.ConnectionString = "Data Source=(localdb)\$Name; Integrated Security=True;"
        $sqlConnection.Open()
        $sqlCommand = $sqlConnection.CreateCommand()
        $sqlCommand.CommandText = $createDatabaseStatement
        $sqlCommand.ExecuteNonQuery()
        $sqlConnection.Close()
    }
}

workflow Invoke-ExpressionsParallel { 
    [cmdletbinding()]
    param
    (      
        [ValidateNotNullOrEmpty()]
        [string[]]$Expressions       
    )   
        
    foreach -parallel ($expression in $Expressions) {        
        Invoke-Expression $expression -ErrorAction Continue
    }        
}

function Get-SqlPackageCommandLine {
    [cmdletbinding()]
    param
    (
        [Parameter(Mandatory=$true)]
        [ValidateSet("Script","Publish","DeployReport")]
        [string]$Action,
        
        [string]$OutputPath,    
        [bool]$OverwriteFiles = $true,
        
        [Parameter(Mandatory=$true)]
        [ValidateSet("File","ConnectionString")]
        [string]$SourceType,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]$Source,            

        [Parameter(Mandatory=$true)]
        [ValidateSet("File","ConnectionString")]
        [string]$TargetType,

        [Parameter(Mandatory=$true)]
        [ValidateNotNullOrEmpty()]
        [string]$Target,        

        [hashtable]$SQLCMDVariables, 
        
        [string[]]$IncludeObjectTypes,    
        [string[]]$ExcludeObjectTypes,
        [string[]]$DoNotDropObjectTypes,

        [string]$AdditionalDeploymentContributorArguments,
        [string[]]$AdditionalDeploymentContributors, 
        
        [bool]$AllowDropBlockingAssemblies = $false,
        [bool]$AllowIncompatiblePlatform = $false,
        [bool]$AllowUnsafeRowLevelSecurityDataMovement = $false,
        [bool]$BackupDatabaseBeforeChanges = $false,
        [bool]$BlockOnPossibleDataLoss = $true,
        [bool]$BlockWhenDriftDetected = $true,
        [int]$CommandTimeout = 300,
        [bool]$CommentOutSetVarDeclarations = $false,
        [bool]$CompareUsingTargetCollation = $false,
        [bool]$CreateNewDatabase = $false,
        [int]$DatabaseLockTimeout = 300,
        [bool]$DeployDatabaseInSingleUserMode = $false,
        [bool]$DisableAndReenableDdlTriggers = $true,
        [bool]$DoNotAlterChangeDataCaptureObjects = $true,
        [bool]$DoNotAlterReplicatedObjects = $true,        
        [bool]$DropConstraintsNotInSource = $true,
        [bool]$DropDmlTriggersNotInSource = $true,
        [bool]$DropExtendedPropertiesNotInSource = $true,
        [bool]$DropIndexesNotInSource = $true,
        [bool]$DropObjectsNotInSource = $false,
        [bool]$DropPermissionsNotInSource = $false,
        [bool]$DropRoleMembersNotInSource = $false,
        [bool]$DropStatisticsNotInSource = $true,        	    
        [bool]$GenerateSmartDefaults = $false,
        [bool]$IgnoreAnsiNulls = $true,
        [bool]$IgnoreAuthorizer = $false,
        [bool]$IgnoreColumnCollation = $false,
        [bool]$IgnoreComments = $false,
        [bool]$IgnoreCryptographicProviderFilePath = $true,
        [bool]$IgnoreDdlTriggerOrder = $false,
        [bool]$IgnoreDdlTriggerState = $false,
        [bool]$IgnoreDefaultSchema = $false,
        [bool]$IgnoreDmlTriggerOrder = $false,
        [bool]$IgnoreDmlTriggerState = $false,
        [bool]$IgnoreExtendedProperties = $false,
        [bool]$IgnoreFileAndLogFilePath = $true,
        [bool]$IgnoreFilegroupPlacement = $true,
        [bool]$IgnoreFileSize = $true,
        [bool]$IgnoreFillFactor = $true,
        [bool]$IgnoreFullTextCatalogFilePath = $true,
        [bool]$IgnoreIdentitySeed = $false,
        [bool]$IgnoreIncrement = $false,
        [bool]$IgnoreIndexOptions = $false,
        [bool]$IgnoreIndexPadding = $true,
        [bool]$IgnoreKeywordCasing = $true,
        [bool]$IgnoreLockHintsOnIndexes = $false,
        [bool]$IgnoreLoginSids = $true,
        [bool]$IgnoreNotForReplication = $false,
        [bool]$IgnoreObjectPlacementOnPartitionScheme = $true,
        [bool]$IgnorePartitionSchemes = $false,
        [bool]$IgnorePermissions = $false,
        [bool]$IgnoreQuotedIdentifiers = $true,
        [bool]$IgnoreRoleMembership = $false,
        [bool]$IgnoreRouteLifetime = $true,
        [bool]$IgnoreSemicolonBetweenStatements = $true,
        [bool]$IgnoreTableOptions = $false,
        [bool]$IgnoreUserSettingsObjects = $false,
        [bool]$IgnoreWhitespace = $true,
        [bool]$IgnoreWithNocheckOnCheckConstraints = $false,
        [bool]$IgnoreWithNocheckOnForeignKeys = $false,
        [bool]$IncludeCompositeObjects = $false,
        [bool]$IncludeTransactionalScripts = $false,
        [bool]$NoAlterStatementsToChangeClrTypes = $false,
        [bool]$PopulateFilesOnFileGroups = $true,
        [bool]$RegisterDataTierApplication = $false,
        [bool]$RunDeploymentPlanExecutors = $false,
        [bool]$ScriptDatabaseCollation = $false,
        [bool]$ScriptDatabaseCompatibility = $false,
        [bool]$ScriptDatabaseOptions = $true,
        [bool]$ScriptDeployStateChecks = $false,
        [bool]$ScriptFileSize = $false,
        [bool]$ScriptNewConstraintValidation = $true,
        [bool]$ScriptRefreshModule = $true,
        
        [ValidateSet("Memory","File")]
        [string]$Storage = "Memory",
        
        [bool]$TreatVerificationErrorsAsWarnings = $false,
        [bool]$UnmodifiableObjectWarnings = $true,
        [bool]$VerifyCollationCompatibility = $true,
        [bool]$VerifyDeployment = $true,
        
        [string]$AdditionalArguments,
        [string]$SqlPackageExePath
    )

    #######################
    # Find SqlPackage.exe
    #######################
    $defaultSqlPackageExePath = "C:\Program Files\Microsoft SQL Server\150\DAC\bin\SqlPackage.exe"
    if(!(Test-Path $defaultSqlPackageExePath))
    {    
        $defaultSqlPackageExePath = "C:\Program Files\Microsoft SQL Server\140\DAC\bin\SqlPackage.exe"
    }
    if(!$SqlPackageExePath)
    {
        $SqlPackageExePath = $defaultSqlPackageExePath
    }
    elseif(!(Test-Path $SqlPackageExePath))
    {
        write-warning "The specified path was not found: $SqlPackageExePath. Using $defaultSqlPackageExePath."
        $SqlPackageExePath = $defaultSqlPackageExePath
    }
    if(!(Test-Path $SqlPackageExePath))
    {
        #TODO: use a local copy of SqlPackage.exe
        throw "$SqlPackageExePath was not found"
    }

    $sqlPackageCmdLine = "& `"$SqlPackageExePath`" --% /Action:$Action"

    #######################
    # Set /Source and /Target
    #######################
    $sqlPackageCmdLine += " /Source$($SourceType):`"$Source`""
    $sqlPackageCmdLine += " /Target$($TargetType):`"$Target`""


    #######################
    # Set /OutputPath and /OverwriteFiles
    #######################
    if($Action -iin "Script","DeployReport")
    {
        if(!$OutputPath)
        {
            throw "OutputPath is required for $Action action"
        }

        $sqlPackageCmdLine += " /OutputPath:`"$OutputPath`""
        $sqlPackageCmdLine += " /OverwriteFiles:$OverwriteFiles"
    }

    #######################
    # Set /Variables
    #######################
    foreach($key in $SQLCMDVariables.Keys)
    {            
        $sqlPackageCmdLine += " /v:$($key)=$($SQLCMDVariables[$key])"
    }

    #######################
    # Set /ExcludeObjectTypes
    #######################
    $AllObjectTypes = @("Aggregates", "ApplicationRoles", "Assemblies", "AsymmetricKeys", "BrokerPriorities", 
        "Certificates", "ColumnEncryptionKeys", "ColumnMasterKeys", "Contracts", "DatabaseRoles", "DatabaseTriggers", 
        "Defaults", "ExtendedProperties", "ExternalDataSources", "ExternalFileFormats", "ExternalTables", "Filegroups", 
        "FileTables", "FullTextCatalogs", "FullTextStoplists", "MessageTypes", "PartitionFunctions", "PartitionSchemes", 
        "Permissions", "Queues", "RemoteServiceBindings", "RoleMembership", "Rules", "ScalarValuedFunctions", 
        "SearchPropertyLists", "SecurityPolicies", "Sequences", "Services", "Signatures", "StoredProcedures", 
        "SymmetricKeys", "Synonyms", "Tables", "TableValuedFunctions", "UserDefinedDataTypes", "UserDefinedTableTypes", 
        "ClrUserDefinedTypes", "Users", "Views", "XmlSchemaCollections", "Audits", "Credentials", "CryptographicProviders", 
        "DatabaseAuditSpecifications", "DatabaseScopedCredentials", "Endpoints", "ErrorMessages", "EventNotifications", 
        "EventSessions", "LinkedServerLogins", "LinkedServers", "Logins", "Routes", "ServerAuditSpecifications", 
        "ServerRoleMembership", "ServerRoles", "ServerTriggers")
    if($IncludeObjectTypes)
    {
        $NotIncludedObjectTypes = $AllObjectTypes | where { $_ -inotin $IncludeObjectTypes }
        $ExcludeObjectTypes = ($ExcludeObjectTypes + $NotIncludedObjectTypes) | select -Unique  
    }
    if($ExcludeObjectTypes)
    {
        $sqlPackageCmdLine += " /p:ExcludeObjectTypes=$([string]::Join(";", $ExcludeObjectTypes))"
    }

    #######################
    # Set /DoNotDropObjectTypes
    #######################
    if($DoNotDropObjectTypes)
    {
        $sqlPackageCmdLine += " /p:DoNotDropObjectTypes=$([string]::Join(";", $DoNotDropObjectTypes))"
    }

    #######################
    # Set /p:AdditionalDeploymentContributors and /p:AdditionalDeploymentContributorArguments
    #######################
    if($AdditionalDeploymentContributors)
    {
        $sqlPackageCmdLine += " /p:AdditionalDeploymentContributors=$([string]::Join(";", $AdditionalDeploymentContributors))"
        $sqlPackageCmdLine += " /p:AdditionalDeploymentContributorArguments=$AdditionalDeploymentContributorArguments"
    }

    #######################
    # Set /Properties
    #######################
    $sqlPackageCmdLine += " /p:AllowDropBlockingAssemblies=$AllowDropBlockingAssemblies"
    $sqlPackageCmdLine += " /p:AllowIncompatiblePlatform=$AllowIncompatiblePlatform"
    $sqlPackageCmdLine += " /p:AllowUnsafeRowLevelSecurityDataMovement=$AllowUnsafeRowLevelSecurityDataMovement"
    $sqlPackageCmdLine += " /p:BackupDatabaseBeforeChanges=$BackupDatabaseBeforeChanges"
    $sqlPackageCmdLine += " /p:BlockOnPossibleDataLoss=$BlockOnPossibleDataLoss"
    $sqlPackageCmdLine += " /p:BlockWhenDriftDetected=$BlockWhenDriftDetected"
    $sqlPackageCmdLine += " /p:CommandTimeout=$CommandTimeout"
    $sqlPackageCmdLine += " /p:CommentOutSetVarDeclarations=$CommentOutSetVarDeclarations"
    $sqlPackageCmdLine += " /p:CompareUsingTargetCollation=$CompareUsingTargetCollation"
    $sqlPackageCmdLine += " /p:CreateNewDatabase=$CreateNewDatabase"
    $sqlPackageCmdLine += " /p:DatabaseLockTimeout=$DatabaseLockTimeout"
    $sqlPackageCmdLine += " /p:DeployDatabaseInSingleUserMode=$DeployDatabaseInSingleUserMode"
    $sqlPackageCmdLine += " /p:DisableAndReenableDdlTriggers=$DisableAndReenableDdlTriggers"
    $sqlPackageCmdLine += " /p:DoNotAlterChangeDataCaptureObjects=$DoNotAlterChangeDataCaptureObjects"
    $sqlPackageCmdLine += " /p:DoNotAlterReplicatedObjects=$DoNotAlterReplicatedObjects"        
    $sqlPackageCmdLine += " /p:DropConstraintsNotInSource=$DropConstraintsNotInSource"
    $sqlPackageCmdLine += " /p:DropDmlTriggersNotInSource=$DropDmlTriggersNotInSource"
    $sqlPackageCmdLine += " /p:DropExtendedPropertiesNotInSource=$DropExtendedPropertiesNotInSource"
    $sqlPackageCmdLine += " /p:DropIndexesNotInSource=$DropIndexesNotInSource"
    $sqlPackageCmdLine += " /p:DropObjectsNotInSource=$DropObjectsNotInSource"
    $sqlPackageCmdLine += " /p:DropPermissionsNotInSource=$DropPermissionsNotInSource"
    $sqlPackageCmdLine += " /p:DropRoleMembersNotInSource=$DropRoleMembersNotInSource"
    $sqlPackageCmdLine += " /p:DropStatisticsNotInSource=$DropStatisticsNotInSource"        	    
    $sqlPackageCmdLine += " /p:GenerateSmartDefaults=$GenerateSmartDefaults"
    $sqlPackageCmdLine += " /p:IgnoreAnsiNulls=$IgnoreAnsiNulls"
    $sqlPackageCmdLine += " /p:IgnoreAuthorizer=$IgnoreAuthorizer"
    $sqlPackageCmdLine += " /p:IgnoreColumnCollation=$IgnoreColumnCollation"
    $sqlPackageCmdLine += " /p:IgnoreComments=$IgnoreComments"
    $sqlPackageCmdLine += " /p:IgnoreCryptographicProviderFilePath=$IgnoreCryptographicProviderFilePath"
    $sqlPackageCmdLine += " /p:IgnoreDdlTriggerOrder=$IgnoreDdlTriggerOrder"
    $sqlPackageCmdLine += " /p:IgnoreDdlTriggerState=$IgnoreDdlTriggerState"
    $sqlPackageCmdLine += " /p:IgnoreDefaultSchema=$IgnoreDefaultSchema"
    $sqlPackageCmdLine += " /p:IgnoreDmlTriggerOrder=$IgnoreDmlTriggerOrder"
    $sqlPackageCmdLine += " /p:IgnoreDmlTriggerState=$IgnoreDmlTriggerState"
    $sqlPackageCmdLine += " /p:IgnoreExtendedProperties=$IgnoreExtendedProperties"
    $sqlPackageCmdLine += " /p:IgnoreFileAndLogFilePath=$IgnoreFileAndLogFilePath"
    $sqlPackageCmdLine += " /p:IgnoreFilegroupPlacement=$IgnoreFilegroupPlacement"
    $sqlPackageCmdLine += " /p:IgnoreFileSize=$IgnoreFileSize"
    $sqlPackageCmdLine += " /p:IgnoreFillFactor=$IgnoreFillFactor"
    $sqlPackageCmdLine += " /p:IgnoreFullTextCatalogFilePath=$IgnoreFullTextCatalogFilePath"
    $sqlPackageCmdLine += " /p:IgnoreIdentitySeed=$IgnoreIdentitySeed"
    $sqlPackageCmdLine += " /p:IgnoreIncrement=$IgnoreIncrement"
    $sqlPackageCmdLine += " /p:IgnoreIndexOptions=$IgnoreIndexOptions"
    $sqlPackageCmdLine += " /p:IgnoreIndexPadding=$IgnoreIndexPadding"
    $sqlPackageCmdLine += " /p:IgnoreKeywordCasing=$IgnoreKeywordCasing"
    $sqlPackageCmdLine += " /p:IgnoreLockHintsOnIndexes=$IgnoreLockHintsOnIndexes"
    $sqlPackageCmdLine += " /p:IgnoreLoginSids=$IgnoreLoginSids"
    $sqlPackageCmdLine += " /p:IgnoreNotForReplication=$IgnoreNotForReplication"
    $sqlPackageCmdLine += " /p:IgnoreObjectPlacementOnPartitionScheme=$IgnoreObjectPlacementOnPartitionScheme"
    $sqlPackageCmdLine += " /p:IgnorePartitionSchemes=$IgnorePartitionSchemes"
    $sqlPackageCmdLine += " /p:IgnorePermissions=$IgnorePermissions"
    $sqlPackageCmdLine += " /p:IgnoreQuotedIdentifiers=$IgnoreQuotedIdentifiers"
    $sqlPackageCmdLine += " /p:IgnoreRoleMembership=$IgnoreRoleMembership"
    $sqlPackageCmdLine += " /p:IgnoreRouteLifetime=$IgnoreRouteLifetime"
    $sqlPackageCmdLine += " /p:IgnoreSemicolonBetweenStatements=$IgnoreSemicolonBetweenStatements"
    $sqlPackageCmdLine += " /p:IgnoreTableOptions=$IgnoreTableOptions"
    $sqlPackageCmdLine += " /p:IgnoreUserSettingsObjects=$IgnoreUserSettingsObjects"
    $sqlPackageCmdLine += " /p:IgnoreWhitespace=$IgnoreWhitespace"
    $sqlPackageCmdLine += " /p:IgnoreWithNocheckOnCheckConstraints=$IgnoreWithNocheckOnCheckConstraints"
    $sqlPackageCmdLine += " /p:IgnoreWithNocheckOnForeignKeys=$IgnoreWithNocheckOnForeignKeys"
    $sqlPackageCmdLine += " /p:IncludeCompositeObjects=$IncludeCompositeObjects"
    $sqlPackageCmdLine += " /p:IncludeTransactionalScripts=$IncludeTransactionalScripts"
    $sqlPackageCmdLine += " /p:NoAlterStatementsToChangeClrTypes=$NoAlterStatementsToChangeClrTypes"
    $sqlPackageCmdLine += " /p:PopulateFilesOnFileGroups=$PopulateFilesOnFileGroups"
    $sqlPackageCmdLine += " /p:RegisterDataTierApplication=$RegisterDataTierApplication"
    $sqlPackageCmdLine += " /p:RunDeploymentPlanExecutors=$RunDeploymentPlanExecutors"
    $sqlPackageCmdLine += " /p:ScriptDatabaseCollation=$ScriptDatabaseCollation"
    $sqlPackageCmdLine += " /p:ScriptDatabaseCompatibility=$ScriptDatabaseCompatibility"
    $sqlPackageCmdLine += " /p:ScriptDatabaseOptions=$ScriptDatabaseOptions"
    $sqlPackageCmdLine += " /p:ScriptDeployStateChecks=$ScriptDeployStateChecks"
    $sqlPackageCmdLine += " /p:ScriptFileSize=$ScriptFileSize"
    $sqlPackageCmdLine += " /p:ScriptNewConstraintValidation=$ScriptNewConstraintValidation"
    $sqlPackageCmdLine += " /p:ScriptRefreshModule=$ScriptRefreshModule"
    $sqlPackageCmdLine += " /p:Storage=$Storage"    
    $sqlPackageCmdLine += " /p:TreatVerificationErrorsAsWarnings=$TreatVerificationErrorsAsWarnings"
    $sqlPackageCmdLine += " /p:UnmodifiableObjectWarnings=$UnmodifiableObjectWarnings"
    $sqlPackageCmdLine += " /p:VerifyCollationCompatibility=$VerifyCollationCompatibility"
    $sqlPackageCmdLine += " /p:VerifyDeployment=$VerifyDeployment"

    if($AdditionalArguments)
    {
        $sqlPackageCmdLine += " "
        $sqlPackageCmdLine += $AdditionalArguments
    }

    return $sqlPackageCmdLine
}

$stopwatch = [system.diagnostics.stopwatch]::StartNew()

######################################################
# create localdb instance(s) and initialize databases
######################################################
$sql01Databases = @(
    "admin",
    "DATA_001",
    "DATA_Archive", 
    "DATA_Broker", 
    "DATA_Sessions", 
    "DATA_Support", 
    "DATA_Import"
)
$sql02Databases = @(
    "admin",
    "DATA_Reports",
    "DATA_Transcriptions"
)
if($SQL02Endpoint -ieq $SQL01Endpoint) {     
    $sql01Databases += $sql02Databases #SQL01 and SQL02 are same, so deploy all databases to SQL01
    $sql02Databases = @()
}
if ($SQL01Endpoint -ilike "(localdb)\*") {          
    New-SqlLocalDbInstance -Name (($SQL01Endpoint -split "\\")[1]) -DatabasesToCreate $sql01Databases
}
if($sql02Databases -and $SQL02Endpoint -ilike "(localdb)\*") {
    New-SqlLocalDbInstance -Name (($SQL02Endpoint -split "\\")[1]) -DatabasesToCreate $sql02Databases    
}

##################################################################################################
# deploy master_customizations
##################################################################################################
$master = @(
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "master_customizations.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=master; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false `
        -DropObjectsNotInSource $false `
        -IncludeObjectTypes @("StoredProcedures","Audits","ServerAuditSpecifications")
)
if($SQL01Endpoint -ine $SQL02Endpoint)
{
    #deploy master customizations and admin database if SQL02 is not the same as SQL01
    $master += Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "master_customizations.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL02Endpoint; Initial Catalog=master; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false `
        -DropObjectsNotInSource $false `
        -IncludeObjectTypes @("StoredProcedures","Audits","ServerAuditSpecifications")
}
$errors = @()
Invoke-ExpressionsParallel -Expressions $master -ErrorAction Continue -ErrorVariable errors
foreach($e in $errors)
{
    write-host ($e.Exception.Message)
}

##################################################################################################
# deploy admin, DATA_Reports, DATA_Transcriptions, and DATA_001 tables in parallel
##################################################################################################
$batch1 = @(    
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "admin.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=admin; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Reports.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL02Endpoint; Initial Catalog=DATA_Reports; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false    
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Transcriptions.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL02Endpoint; Initial Catalog=DATA_Transcriptions; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_001.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_001; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false `
        -ExcludeObjectTypes @('Audits','DatabaseAuditSpecifications','ScalarValuedFunctions','StoredProcedures','TableValuedFunctions','Views','BrokerPriorities','Contracts','MessageTypes','Queues','Services','EventNotifications') `
        -SQLCMDVariables @{
            EmdatTSharePath=$EmdatTSharePath
            SQL01Endpoint=$SQL01Endpoint
            SQL02Endpoint=$SQL02Endpoint
            SSRSEndpoint=$SSRSEndpoint
            ASREnvironmentCode=$ASREnvironmentCode
            WebSiteHostName=$WebSiteHostName
            CBDAPlatformCode=$CBDAPlatformCode
            EMDAT_ENV_INTOUCH_DEFAULTPHONE=$DefaultInTouchPhone
    }   
)
if($SQL01Endpoint -ine $SQL02Endpoint)
{
    #deploy admin database if SQL02 is not the same as SQL01    
    $batch1 += Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "admin.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL02Endpoint; Initial Catalog=admin; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false
}
$errors = @()
Invoke-ExpressionsParallel -Expressions $batch1 -ErrorAction Continue -ErrorVariable errors
foreach($e in $errors)
{
    write-host ($e.Exception.Message)
}

######################################################################################################
# deploy DATA_Archive, DATA_Broker, DATA_Sessions, DATA_Import
######################################################################################################
$errors = @()
Invoke-ExpressionsParallel -ErrorAction Continue -ErrorVariable errors -Expressions @(
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Archive.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_Archive; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false 
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Broker.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_Broker; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false 
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Sessions.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_Sessions; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_Import.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_Import; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false     
)
foreach($e in $errors)
{
    write-host ($e.Exception.Message)
}

######################################################################################################
# deploy DATA_001
######################################################################################################
$errors = @()
Invoke-ExpressionsParallel -ErrorAction Continue -ErrorVariable errors -Expressions @(
    Get-SqlPackageCommandLine `
        -Action Publish `
        -SourceType File `
        -Source (join-path $SourceDacPacsFolder "DATA_001.dacpac") `
        -TargetType ConnectionString `
        -Target "Data Source=$SQL01Endpoint; Initial Catalog=DATA_001; Integrated Security=True;" `
        -BlockOnPossibleDataLoss $false `
        -IgnoreAnsiNulls $false `
        -IgnoreQuotedIdentifiers $false `
        -ScriptDatabaseOptions $false `
        -SQLCMDVariables @{
            EmdatTSharePath=$EmdatTSharePath
            SQL01Endpoint=$SQL01Endpoint
            SQL02Endpoint=$SQL02Endpoint
            SSRSEndpoint=$SSRSEndpoint
            ASREnvironmentCode=$ASREnvironmentCode
            WebSiteHostName=$WebSiteHostName
            CBDAPlatformCode=$CBDAPlatformCode
            EMDAT_ENV_INTOUCH_DEFAULTPHONE=$DefaultInTouchPhone
        }
)
foreach($e in $errors)
{
    write-host ($e.Exception.Message)
}

write-host "Deployment completed in $($stopwatch.Elapsed)"