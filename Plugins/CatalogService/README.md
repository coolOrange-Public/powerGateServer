# CatalogService

The plugin is collecting all the services of all the other plugins at runtime and can be used to retrieve information about them.  
Only supports get and query functions.

## Working with the CatalogService-Plugin

powerGateServer installs the default CatalogService plugin into the folder:
> %ProgramData%\coolOrange\powerGateServer\Plugins\CatalogService

This service returns a list of the currently loaded webservices with additional data (MetadataUrl, ServiceUrl, Title, ...).  
The field `UpdatedData` is filled with the date, when the plugin assembly was modified the last time (= the build date of the assembly).  
Author returns the value of `AssemblyCompany` from the ProductInfo.cs: `[assembly: AssemblyCompany(....)`]`


## Here are some Http requests that can be directly send to powerGateServer now

Returns all the currently running services:
[http://.../PGS/CATALOGSERVICE/ServiceCollection](http://localhost:8080/PGS/CATALOGSERVICE/ServiceCollection)
 
Returns all the services from coolorange:
[http://.../PGS/CATALOGSERVICE/ServiceCollection?$filter=Author eq 'coolOrange s.r.l.'](<http://localhost:8080/PGS/CATALOGSERVICE/ServiceCollection?$filter=Author eq 'coolOrange s.r.l.'>)
 
Returns all the SAP services:
[http://.../PGS/CATALOGSERVICE/ServiceCollection?$filter=substringof('/PGS/', ServiceUrl) eq true](<http://localhost:8080/PGS/CATALOGSERVICE/ServiceCollection?$filter=substringof('/PGS/', ServiceUrl) eq true>)
