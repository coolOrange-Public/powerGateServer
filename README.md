powerGateServer
===============

This repository contains different extensions for powerGateServer

At the moment the samples are:
- SAP
- Vault
- LinqToDatabase

To be able to build the addins, you have to open the solution in VisualStudio and open the project UserServices.
Be shure that powerGateServer is installed on your development-machine.
Now fix the missing references by re-adding the two assemblies of the powerGateServer (reference them from the pGS installation directory):
 - powerGateServer.dll
 - powerGateServer.Addins.dll

When the project is building, be shure to set the output-directory to the powerGateServer installtion folder.
Set the debug executable to the powerGateServer.exe
