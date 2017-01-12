# LinqToDatabase plugin
LinqToDatabase plugin can be used as template or sample for connecting to different types of databases.

## Working with the LinqToDatabase-Plugin
The current implementation uses SQLite database stored in following directory:
> %ProgramData%\coolOrange\powerGateServer\Plugins\LinqToDb\LinqToDbService.sdf

The key thing of this plugin is the usage of the IQToolkit library coming from LinqPad (http://www.linqpad.net/).
This library supports by default different LinqToEntity types, like:
- LinqToSql
- LinqToMySql
- LinqToSqlite
- LinqToOracle
- LinqToAccess
- LinqToOleDb

You can also write your custom provider of corse: http://blogs.msdn.com/b/mattwar/archive/2009/06/16/building-a-linq-iqueryable-provider-part-xv-iqtoolkit-v0-15.aspx

By using the OleDbProvider you could also work with Microsoft excel if you want:
- https://github.com/firestrand/IQToolkit/blob/master/IQToolkit.Data.Access/AccessQueryProvider.cs
- http://csharp.net-informations.com/excel/csharp-excel-oledb.htm


Our LinqToDatabase plugin does the following things:
- it delegates the expression from the Query function to the LinqToEntity provider
- it uses the LinqToEntityProvider for automatically dealing with Inserts, Updates and deleting entities

After building the plugin, press F5 and run powerGateServer.

## Here are some Http requests that can be directly send to powerGateServer now
The following query should return an emtpty list now, because no File is stored in the Sqlite-Database.
  [http://.../sap/opu/odata/LinqToDb/LINQ_SRV/Files?$format=json](http://localhost:8080/sap/opu/odata/LinqToDb/LINQ_SRV/Files?$format=json)
  
You can start adding a new entity now!

## Getting started
LinqToDbService.cs is the WebService class. Here we open a new Sqlite connection and we are constructing a new SQLiteQueryProvider.
Because we construct our provider with default queryMapping and queryPolicy, we have to follow certain rules when constructing the FileContext and Files-ServiceMethod:
- note that FileContext class has a key field ending with 'ID' : FileID. when using ImplicitMapping, the field ending with ID is used as primarykey field in the database.
- note that Files-ServiceMethod is creating a new Table in the database if the table does not exists. LinqToEntity needs a fully setup database (at least the tables and columns need to be setup).
- note that the table for the entity 'FileContext' is called 'File Contexts': this is because we are using implicit query-mapping.
