echo "Latest Nuget commandline can be downloaded from: https://www.nuget.org/downloads"
echo "Starting packing of Nuget package.." 
echo "Assuming that nuget.exe is in bundled Nuget folder in this VS solution"

echo Nuget\nuget.exe 

echo "Remember to update the version attribute of the .nuspec file in this folder before packing and publishing. Edit the WSTrustFiddlerWebTestExport.nuspec file."

echo "Packing done. Push to nuget.org using: nuget\nuget.exe push WSTrustFiddlerWebTestExport.2.6.*.nupkg SECRETPUSHTOKEN -Source https://api.nuget.org/v3/index.json"

echo "Sample push example to Nuget.org:" 

echo "c:\dev\fiddlercustomwebtestexport\nuget\nuget.exe push c:\dev\fiddlercustomwebtestexport\WSTrustFiddlerWebTestExport.2.6.0.nupkg SECRETPUSHTOKEN -Source https://api.nuget.org/v3/index.json"
echo "Pushing WSTrustFiddlerWebTestExport.2.6.0.nupkg to 'https://www.nuget.org/api/v2/package'..."
echo "PUT https://www.nuget.org/api/v2/package/"
echo "WARNING: All published packages should have license information specified. Learn more: https://aka.ms/deprecateLicenseUrl."
echo "Created https://www.nuget.org/api/v2/package/ 2155ms"
echo "Your package was pushed.

