# Forcer l'encodage UTF-8 pour éviter tout problème d'affichage
[console]::OutputEncoding = [System.Text.Encoding]::UTF8

# Définition des paramètres
$projectPath = "C:\developpement_project\GeoGouv_Courbois\GeoGouv_Courbois"
$publishPath = "C:\inetpub\wwwroot\GeoGouv"
$siteName = "GeoGouv"
$appPoolName = "GeoGouvPool"
$dbName = "GeoGouv"
$expositionPort = 4850
$connectionString = "Server=localhost;Database=$dbName;Trusted_Connection=True;"

Write-Host "Déploiement en cours..."

### 1️) Vérification des prérequis ###

# Vérifier si IIS est installé
$feature = Get-WindowsOptionalFeature -Online -FeatureName IIS-WebServer
if ($feature.State -ne "Enabled") {
    Write-Host "IIS n'est pas installé. Activez-le via 'Ajouter/Supprimer des fonctionnalités Windows'."
    exit 1
}

# Vérifier la présence de dotnet-ef
Write-Host "Vérification de dotnet-ef..."
$efVersion = dotnet ef --version 2>$null
if ($null -eq $efVersion) {
    Write-Host "dotnet-ef introuvable. Installation en cours..."
    dotnet tool install --global dotnet-ef
    Write-Host "dotnet-ef installé."
}

# S'assurer que dotnet-ef est bien accessible
$dotnetEfPath = "C:\Users\$env:USERNAME\.dotnet\tools"
if ($env:Path -notlike "*$dotnetEfPath*") {
    Write-Host "Ajout de dotnet-ef au PATH..."
    $env:Path += ";$dotnetEfPath"
    [System.Environment]::SetEnvironmentVariable("Path", $env:Path, [System.EnvironmentVariableTarget]::User)
}

### 2️) Compilation et publication ###

Write-Host "Restauration des dépendances..."
cd $projectPath
dotnet restore
Write-Host "Compilation en cours..."
dotnet build --configuration Release
Write-Host "Publication de l'application..."
dotnet publish --configuration Release --output $publishPath

### 3️) Configuration IIS ###

Write-Host "Configuration d'IIS..."

# Vérifier et recréer le pool d'application
$pool = Get-IISAppPool | Where-Object { $_.Name -eq $appPoolName }
if ($null -eq $pool) {
    New-WebAppPool -Name $appPoolName
    Write-Host "Pool d'application $appPoolName créé."
} else {
    Write-Host "Pool $appPoolName existant. Suppression et recréation..."
    Remove-WebAppPool -Name $appPoolName
    New-WebAppPool -Name $appPoolName
    Write-Host "Pool d'application $appPoolName recréé."
}

# Vérifier et supprimer le site existant avant recréation
$site = Get-Website | Where-Object { $_.Name -eq $siteName }
if ($site) {
    Write-Host "Le site $siteName existe déjà. Suppression..."
    Stop-Website -Name $siteName -ErrorAction SilentlyContinue
    Remove-Website -Name $siteName
    Write-Host "Ancien site supprimé."
}

# Création du nouveau site IIS
New-Website -Name $siteName -PhysicalPath $publishPath -ApplicationPool $appPoolName
Start-Website -Name $siteName
Set-ItemProperty "IIS:\Sites\$siteName" -Name bindings -Value @{protocol="http";bindingInformation=":${expositionPort}:"}
Write-Host "Site IIS $siteName déployé sur http://localhost:$expositionPort"

### 4️) Configuration de la base de données ###

Write-Host "Configuration de la base de données..."

# Vérifier la connexion SQL Server
try {
    $sqlTest = Invoke-Sqlcmd -ServerInstance "localhost" -Query "SELECT 1" -ErrorAction Stop
    Write-Host "Connexion à SQL Server réussie."
} catch {
    Write-Host "Erreur : Impossible de se connecter à SQL Server. Vérifiez que le service SQL est démarré."
    exit 1
}

# Vérifier l'existence de la base de données
$checkDb = Invoke-Sqlcmd -ServerInstance "localhost" -Query "SELECT name FROM sys.databases WHERE name = '$dbName'"
if ($null -eq $checkDb) {
    Invoke-Sqlcmd -ServerInstance "localhost" -Query "CREATE DATABASE [$dbName]"
    Write-Host "Base de données $dbName créée."
} else {
    Write-Host "Base de données $dbName déjà existante."
}

# Appliquer les migrations Entity Framework Core
Write-Host "Application des migrations..."
dotnet ef database update
Write-Host "Migrations appliquées."

### 🔚 Fin du déploiement ###
Write-Host "Déploiement terminé. Accédez à http://localhost:$expositionPort"
