This plugin allows it to connect Autodesk Vault (2014 or later) via REST.

The plugin is build to the folder %ProgramData%\coolOrange\powerGateServer\Plugins\Vault.
In the VaultServices.dll.config file you can configure the login-credentials for your vault.

When loaded in powerGateServer, you can query all the files from your Vault like this:
  http://localhost:8080/sap/opu/odata/Arcona6/VAULT_SRV/Documents?$format=json

Or let's search all the files where the name contains the string 'Catch':
  http://localhost:8080/sap/opu/odata/Arcona6/VAULT_SRV/Documents?$format=json&$filter=substringof('Catch', Name) eq true
  
Getting started
===============
Open VaultServices.csproj in visualstudio and build the project.
In the file VaultService.cs we build the VaultConnection and we are connecting our Vault with the login-credentials from the app.config file.
The DocumentService is the servicemethod for the File entity. This entity represents a vault file in a very simple way.
It has e.g. a list of it's properties as a navigation-property.
Because Vault provides an API for searching files by properties, or retrieving them by Id or MasterId, we have to implement different serach strategies (subfolder FindStrategies).
1. FindVaultFileByIds: 
    If the Where clause contains a therm Id equals ... or (Id=...), than foreach Id the DocSvc.GetDocumentById method is called.
2. FindVaultFilesByMasterIds:
    If the Where clause contains therms with MasterId equals ... , than the lastest file-versions are retrieved by there MasterId.
3. FindVaultFilesByProperties:
    For all the other properties, the FindFilesBySearchConditions function is used. Therefore we have to build a list of SrchCond-object from the Where-tokens, and a list or SrchSort-objects from the OrderBy clause.
