$root = (split-path -parent $MyInvocation.MyCommand.Definition)

$AllOBJDirs = get-childitem -recurse *obj\ 

foreach ($objdir in $AllOBJDirs)
{
    Write-Host "Removing $objdir..."

    Remove-Item $objdir -Force -Recurse
}

$AllBINDirs = get-childitem -recurse *bin\ 

foreach ($bindir in $AllBINDirs)
{
    Write-Host "Removing $bindir..."

    Remove-Item $bindir -Force -Recurse
}