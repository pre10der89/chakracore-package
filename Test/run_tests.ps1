$AllTests = get-childitem -recurse *Tester\bin\**\*.exe 

foreach ($test in $AllTests)
{
	if (!($test -like '*ARM*')) 
	{ 
		Write-Host "Running $test..."
		Start-Process -FilePath $test -ArgumentList "/NOPAUSE"
	}
	else
	{
		Write-Host "Skipping $test..."
	}
   
}

#$AnyCPUReleaseTesters = get-childitem -recurse *Tester\bin\Release\Chakra.Package.Tester.exe

#foreach ($test in $AnyCPUReleaseTesters)
#{
    #Write-Host "fff $test"
#}