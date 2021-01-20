# Vault Plugin

The Vault plugin lets you connect to a Vault Server.
It can be used to retrieve data from Vault by communicating via Odata.

#Sample Queries
##GET

###Folders
* *All Folders*
	* `http://localhost:8080/PGS/VaultService/Folders`

* *Get Folder by Name*
	* `http://localhost:8080/PGS/VaultService/Properties?$filter=ParentType eq 'Folder' and Name eq 'Name' and Value eq 'SecretStuff'`

* *Get parentfolder Id of folder*
	* `http://localhost:8080/PGS/VaultService/Links?$filter=ChildId eq 23 and ChildType eq 'Folder' and ParentType eq 'Folder'`
	* `http://localhost:8080/PGS/VaultService/Folders(Id=23)/Parents?$filter=ParentType eq 'Folder'`

###Files
* *Get all files where *:
* *Get all latest Files*: 
	* `http://localhost:8080/PGS/VaultService/Files`

* *Get All Files from a Folder*: 
	* `http://localhost:8080/PGS/VaultService/Folders(1L)/Children?$filter=ChildType eq 'File'and $select=ChildId`
	* `http://localhost:8080/PGS/VaultService/Links?$filter=ParentType eq 'Folder' and ParentId eq 1 and ChildType eq 'File' and $select=ChildId`

* *Search all File Ids with Property Value 'Catch Assembly'*:
	* `http://localhost:8080/PGS/VaultService/Properties?$filter=ParentType eq 'File' and substringof('Catch Assembly',Value) eq true&$select=ParentId`
 
* *Get all Ids of all versions of a File*:
	* `http://localhost:8080/PGS/VaultService/Properties?$filter=ParentType eq 'File'and Name eq 'MasterId' and Value eq 3&$select=ParentId`
	
* *Download a file content*:
	* `http://localhost:8080/PGS/VaultService/Files(3L)/$value`
	
###Items
* *All Items*: 
	* `http://localhost:8080/PGS/VaultService/Items`
* *Get Item Bom*:
	* `http://localhost:8080/PGS/VaultService/Properties(ParentId=1,ParentType='Item',Name='BOM')`
	
###Properties
* *All Properties*: 
	* `http://localhost:8080/PGS/VaultService/Properties`

###Links
* *All Links*: 
	* `http://localhost:8080/PGS/VaultService/Links`
* *All Links which are dependencies*: 
	* `http://localhost:8080/PGS/VaultService/Links?$filter=Description eq "Dependency"`

###ECOs
* *All ECOs*: 
	* `http://localhost:8080/PGS/VaultService/Links`

##PUT
* *Move File from one to other folder*:

---
	PUT : http://localhost:8080/PGS/VaultService/Links(ChildId=1,ChildType='File',Description='Dependency',ParentId=1,ParentType='Folder')/ParentId
	{
		"ParentId": 2
	}

* *Rename from Item the property "Number"*:

---
	PUT : http://localhost:8080/PGS/VaultService/Properties(ParentId=1,ParentType='Item',Name='Number')
	{
		"Value": "ItemNewNumber"
	}

* *Set file to Released*:

---
	PUT : http://localhost:8080/PGS/VaultService/Properties(ParentId=10,ParentType='File',Name='State')
	{
		"Value": "Released"
	}


##POST

###Link(s)

* *Create PrimaryLink for Item*:

---
	
	POST: http://localhost:8080/PGS/VaultService/Links	
	{
		"ParentId": 1,
		"ParentType": "Item",
		"ChildId": 1,
		"ChildType": "File",
		"Description": "Primary"
	}

###File(s)

* *Create new File* -> Multipart Request with streamable file

First Request: The file will be created under the Folder with Id 2 with two children.

---
	POST: http://localhost:8080/PGS/VaultService/Files
	{ 
		"Id"= 3,
		"Type" = "File",
		"Name" = "Pad Lock.iam",
		"CreateUser" = "Patrick",
		"CreateDate" = "23/09/2015"
		"Children" = 
		(
			{
				"ParentId" = 3,
				"ParentType" = "File",
				"ChildId" = 4,
				"ChildType" = "File",
				"Description" = "Dependency"
			},
			{
				"ParentId" = 3,
				"ParentType" = "File",
				"ChildId" = 5,
				"ChildType" = "File",
				"Description" = "Dependency"
			}
		),
		Parents = 
		(
			{
				"ParentId" = 2,
				"ParentType" = "Folder",
				"ChildId" = 3,
				"ChildType" = "File",
				"Description" = "Dependency"
			}
		),
		Properties =
		(
			{
				"ParentId" = "3",
				"ParentType" = "File",
				"Name" = "Comments",
				"Value" = "Created by REST API",
				"Type" = "string"
			},
			{
				"ParentId" = "3",
				"ParentType" = "File",
				"Name" = "Description",
				"Value" = "Front left wheel",
				"Type" = "string"
			},
			{
				"ParentId" = "3",
				"ParentType" = "File",
				"Name" = "Keywords",
				"Value" = "PGS,wheel,Patrick",
				"Type" = "string"
			}
		)
	}

Second Request

---
	POST http://localhost:8080/PGS/VaultService/Files
	Content-Length: ###
	Slug: Id=3
 
	%PDF-1.4
	...
	%%EOF

###Folders

* *Create new folder*

---
	POST: http://localhost:8080/PGS/VaultService/Folders
	{ 
		"Id"= 23,
		"Type" = "Folder",
		"Name" = "SecretStuff",
		"CreateUser" = "Patrick",
		"CreateDate" = "23/09/2015"
		Parents = 
		(
			{9/23/2015 2:37:07 PM 
				"ParentId" = 65,
				"ParentType" = "Folder",
				"ChildId" = 23,
				"ChildType" = "Folder",
				"Description" = "Dependency"
			}
		),
		Properties =
		(
			{
				"ParentId" = "23",
				"ParentType" = "File",
				"Name" = "Comments",
				"Value" = "Created by REST API",
				"Type" = "string"
			},
			{
				"ParentId" = "3",
				"ParentType" = "File",
				"Name" = "Description",
				"Value" = "Front left wheel",
				"Type" = "string"
			},
			{
				"ParentId" = "3",
				"ParentType" = "File",
				"Name" = "Keywords",
				"Value" = "PGS,wheel,Patrick",
				"Type" = "string"
			}
		)
	}