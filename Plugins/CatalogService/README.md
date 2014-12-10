This is the source code for our default CatalogService that comes with powerGateServer.


CatalogService plugin is installed in the folder: %ProgramData%\coolOrange\powerGateServer\Plugins\CatalogService.
This service returns a list of the currently loaded webservices with some more interesting data (MetadataUrl, ServiceUrl, Title, ...).
The field UpdatedData is filled with the date, when your plugin assembly was modified the last time (= the build date of your assembly).
Author returns the value of AssemblyCompany from your ProductInfo.cs: [assembly: AssemblyCompany(....)]

For more details and some test queries see: http://wiki.coolorange.com/display/powerGate/Getting_started
