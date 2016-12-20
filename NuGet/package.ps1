$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'

$targetNugetExe = "$root\NuGet\nuget.exe"

$packageArtifacts = "$root\NuGet\Artifacts"
$packageVersion = "$root\NuGet\.pack-version"

If (Test-Path $packageArtifacts)
{
    # Delete any existing output.
    Remove-Item $packageArtifacts\*.nupkg
}

If (!(Test-Path $targetNugetExe))
{
    $sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

    Write-Host "NuGet.exe not found - downloading latest from $sourceNugetExe"

    $sourceNugetExe = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"

    Invoke-WebRequest $sourceNugetExe -OutFile $targetNugetExe
}

$versionStr = (Get-Content $packageVersion) 

Write-Host "Setting .nuspec version tag to $versionStr"

$compiledNuspec = "$root\nuget\compiled.nuspec"

# Create new packages for any nuspec files that exist in this directory.
Foreach ($nuspec in $(Get-Item *.nuspec))
{
    $content = (Get-Content $nuspec)
    $content = $content -replace '\$version\$',$versionStr
    $content | Out-File $compiledNuspec

	& $targetNugetExe pack $compiledNuspec -outputdirectory $packageArtifacts
}

# Delete compiled temporary nuspec.
If (Test-Path $compiledNuspec)
{
	Remove-Item $compiledNuspec
}