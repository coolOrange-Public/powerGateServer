This is the source code for our default SAP-Service that comes with powerGateServer.

The plugin is build to the folder %ProgramData%\coolOrange\powerGateServer\Plugins\SAP.
In the subfolder Store you can find a ton of xml-files.
This is our xml-database for all the different entities from the SAP webservice.

or more details and some test queries see: http://wiki.coolorange.com/display/powerGate/Getting_started

For getting started with our SAP plugin you should open the architecture project that gives some more overview about different WebServices, ServiceMethods and our SAP-Entitites:
https://github.com/coolOrange-Public/powerGateServer/tree/master/Plugins/SAP/Architektur

Getting started
===============
Open SapServices.csproj in visualstudio. 
In the subfolder Services you can find our SAP Services: MaterialService,DocumentInfoRecordService and BillofMaterialService.
All this three WebServices are derived from SapServiceBase. This is the composition root for our plugin, where we create and construct the XmlDatabase and the main EntitiesStore.
Each service registers ServiceMethods for the different entities and the lookups.

Important to know is, that each webservice has one main context entity. MaterialSvc has e.g. MaterialContext as a main entity. 
It's serviceMethod MaterialContextCollection is derived from ContextEntitySetBase, like the other main service-methods.
The subentity methods (Navigation properties) like e.g. BasicDataCollection, are derived from NavigationPropertyEntitySet<BasicData, MaterialContext> (the second type is the main-entity type).
Navigation property collections like DescriptionCollection, are derived from NavigationPropertyCollectionEntitySet<Description, MaterialContext> instead.
If you open that base class, you can see how simple the CRUD functions are implemented.
When a navigation proeprty is removed from the list, than all the other entites are removed and readded to the entityStore.
When updating a entity from a collection, the entity itself is deleted and readded again to the database.

A really special servicemethod is the DocumentInfoRecordOriginalCollection. This ServiceMethod is derived from IStreamableServiceMethod too.
That means, the service implements Upload, Download and DeleteStream functionality. The binary-data is stored in the Store\Files folder
