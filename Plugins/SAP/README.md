# SAP Plugin

The pluggin was created for demo and learning purposes. It only works with it's respective mock databases that is installed together with it.
It is helpful to become familiar with powerGateServer.

## Working with the SAP-Plugin

PowerGateServer installs our default SAP-plugin into the folder:
> %ProgramData%\coolOrange\powerGateServer\Plugins\SAP

In the subfolder *Store* you can find xml-files. This is the xml-database for all the different entities from the SAP webservice.

## Here are some Http requests that can be directly send to powerGateServer now

1. Returns the **metadata** of materialservice: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/$metadata](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/$metadata)
 
2. Returns the plantlookups in **json-format**: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/PlantLookupCollection?$format=json](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/PlantLookupCollection?$format=json)
 
3. Returning a specific **amount** of language-entities: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/LanguForLanguIsoCollection?$format=json&$top=3](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/LanguForLanguIsoCollection?$format=json&$top=3)
 
4. *Skipping* the first 3 materialgroup entities: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/MatlGroupLookupCollection?$format=json&$skip=3](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/MatlGroupLookupCollection?$format=json&$skip=3)
 
5. *Ordering* unit of measures: 
  - ascending by "Description": 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/BaseUomLookupCollection?$format=json&$orderby=Description](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/BaseUomLookupCollection?$format=json&$orderby=Description)

  - descending by "BaseUOM": 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/BaseUomLookupCollection?$format=json&$orderby=BaseUom desc](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/BaseUomLookupCollection?$format=json&$orderby=BaseUom desc)
 
6. Accessing an entity by it's *key*: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection('FOOD')?$format=json](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection('FOOD')?$format=json)
 
7. Finding entites via *filter criterias*: 
  - where Description is 'Waste': 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection?$format=json&$filter=Description eq 'Waste'](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection?$format=json&$filter=Description eq 'Waste')

  - where MatlType has 4 characters and Description contains 'products': 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection?$format=json&$filter=length(MatlType) eq 4 and substringof('products',Description) eq true](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/MatlTypeLookupCollection?$format=json&$filter=length(MatlType) eq 4 and substringof('products',Description) eq true)
 
8. *Creating* a new MaterialContext and it's navigation properties (via deep create):
  ```
<code>
POST /sap/opu/odata/Arcona6/MATERIAL_SRV/MaterialContextCollection HTTP/1.1
Host: localhost:8080
Content-Type: application/json
Accept: application/json
Cache-Control: no-cache
 
{ "Description": [ { "Material": "ITEM-00116", "Langu": "DE", "MatlDesc": "Hallo" }, { "Material": "ITEM-00116", "Langu": "EN", "MatlDesc": "hello" } ], "PlantData": { "Material": "ITEM-00116", "Plant": "AP01", "PurGroup": "001", "Availcheck": "01", "PurStatus": "01" }, "Plant": "asdf", "Material": "ITEM-00116", "ValuationType": "asdf", "ValuationArea": "asdf" }
</code>
  ```
9. Now we can access the MaterialContext *navigation-property* 'PlantData' like this: 
[http://.../sap/opu/odata/Arcona6/MATERIAL_SRV/MaterialContextCollection(Material='ITEM-00116',Plant='asdf',ValuationArea='asdf',ValuationType='asdf')/PlantData?$format=json](http://localhost:8080/sap/opu/odata/Arcona6/MATERIAL_SRV/MaterialContextCollection(Material='ITEM-00116',Plant='asdf',ValuationArea='asdf',ValuationType='asdf')/PlantData?$format=json)
 
10. *Updating* the 'MatlDesc' of a specific Description:
  ```
<code>
PUT /sap/opu/odata/Arcona6/MATERIAL_SRV/DescriptionCollection(Langu='DE',Material='ITEM-00116') HTTP/1.1
Host: localhost:8080
Accept: application/json
Content-Type: application/json
Cache-Control: no-cache
 
{ "MatlDesc": "Latex" }
</code>
  ```
11. *Deleting* one of the two descriptions from our MaterialContext:

  ```
<code>
DELETE /sap/opu/odata/Arcona6/MATERIAL_SRV/DescriptionCollection(Langu='EN',Material='ITEM-00116') HTTP/1.1
Host: localhost:8080
Cache-Control: no-cache
</code>
  ```

For more possibilities with [Odata](http://www.odata.org/documentation/odata-version-2-0/uri-conventions/|Odata) give a look to this link.
