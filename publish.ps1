function Publish-Project
{
    param([string]$location)
    
    Push-Location
    Set-Location $location
    dotnet publish -c Release
    
    Pop-Location
}

Publish-Project "src/BootstrapperLoaderDemo.Repository"
Publish-Project "src/BootstrapperLoaderDemo"
