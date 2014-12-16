accept-api & accept framework
=============================

#	Downloading, Compiling and Deployment instructions. 


Getting Started:
================

-	Before getting started make sure the .Net environment is up and running for the .Net Framework 4.5.
-	Make sure Visual Studio 2010 or greater is installed.
-	Make sure the NuGet package manager is up to date - https://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c
-	Unexpected problem? Extra question? Please contact davidluzsilva@gmail.com
 
1 – Download ACCEPT API repository folder.

2 – Open file “AcceptApi.csproj” with Visual Studio.

3 – Click “File”, click “Add”, click “Existing Project…”, double click “AcceptFramework” folder, choose file “AcceptFramework.csproj”, click “Open”.

4 – Click “File”, click “Save AcceptApi.sln”, choose a directory path, click “Save”.

-	At this stage the ACCEPT Framework dependency is automatically sorted and the API Solution file is created.
-	Now the next step is to sort all ACCEPT Framework dependencies and compile the Framework.

Solving the ACCEPT Framework Project Dependencies:
==================================================

1 – Click “Tools”, click “Library Package Manager”, click “Package Manager Console”.

-	The Console Tab should be displayed somewhere in the solution.
-	Next step is to proper configure Nuget to enable automatic package restore. 

2 – Within the package manager console Tab, type the following command: “Install-Package NuGetPowerTools”.

-	If success the following message should be prompted: “Successfully installed 'NuGetPowerTools 0.29'.”.

3 – Within the package manager console Tab, type the following command: “Enable-PackageRestore”.

-	The following messages should be prompt:
-	“Enabled package restore for AcceptFramework”.
-	“Enabled package restore for AcceptApi”.

4 – Right click  on the “Solution ‘AcceptApi’” file(in the top of the Solution Explorer), and click “Enable NuGet Package Restore”.

-	More detailed info on step 4 here: http://docs.nuget.org/docs/Workflows/Using-NuGet-without-committing-packages

5 – Within the package manager console Tab, type the following command: “Install packages.config”, when the command completes, right click in the “AcceptFramework” file(within the Solution Explorer), and click compile.

-	The compilation in step 5 will fail, that is expected for now.

6 – Within the package manager console Tab, type the following command: “Install-Package FluentNHibernate”.

7 – Within the package manager console Tab, type the following command: “Update-Package -reinstall Microsoft.AspNet.Web.Optimization”.

8 – Within the package manager console Tab, type the following command: “Install-Package LinFu.DynamicProxy.OfficialRelease”.

-	Now that all packages are downloaded lets manually add some of the recently downloaded dependencies that are not automatically added.

9 – Within the Solution Explorer find the “AcceptFramework” project and right click References, then click “Add Reference…”

On the dialog let’s click brows, find the packages main folder and add the following .DLL files:

-	Antlr3.Runtime.dll
-	DotNetOpenAuth.AspNet.dll (the one within “net40-full” folder)
-	DotNetOpenAuth.Core.dll (the one within “net40-full” folder)
-	DotNetOpenAuth.OAuth.Consumer.dll (the one within “net40-full” folder)
-	DotNetOpenAuth.OAuth.dll (the one within “net40-full” folder)
-	DotNetOpenAuth.OpenId.dll (the one within “net40-full” folder)
-	DotNetOpenAuth.OpenId.RelyingParty.dll (the one within “net40-full” folder)
-	Microsoft.Web.Mvc.FixedDisplayModes.dll
-	Newtonsoft.Json.dll (the one within  “packages\Newtonsoft.Json.5.0.8\lib\net45” folder)
-	LinFu.DynamicProxy.dll (the one within “packages\LinFu.DynamicProxy.OfficialRelease.1.0.5\lib\net” folder)

10 – There are at least two libraries that need to be downloaded in separate:

	For “RemotionDataLink” library:

-	Go to http://relinq.codeplex.com/releases/view/38673.
-	Download “RemotionRelinq_1.13.41.0.zip”
-	Unzip the file, copy the content folder into your project solution folder “packages”.
-	Back to Visual Studio, apply the same logic that in step 9, find the just unzipped folder(“RemotionRelinq_1.13.41.0”).
-	Select the file “Remotion.Data.Linq.dll” (within  “…\net-3.5\bin\release” folder) and click “Add”.

For  “NHibernate.ByteCode.LinFu”

-	Go to https://github.com/sibartlett/NHibernate.ByteCode.
-	Click the link button “Download ZIP” to download the solution zip file.
-	Unzip the file, open the unzipped folder and search for the “.sln” file  and compile the project “NHibernate.ByteCode.LinFu”.
-	Open the bin folder(“NHibernate.ByteCode-master\src\NHibernate.ByteCode.LinFu\bin\Debug”) and copy the file “NHibernate.ByteCode.LinFu.dll” into your ACCEPT solution, within the packages folder(a folder can be created).
-	Back in the ACCEPT project Visual Studio solution, apply the same logic that in step 9, find the just copied file “NHibernate.ByteCode.LinFu.DLL” and click “Add”.


Solving the ACCEPT API Project Dependencies:
============================================

At this stage all libraries references should be solved, remains the logging library “Elmah”:

1 – Within the Solution Explorer find the “AcceptApi” project and right click References, then click “Add Reference…”
2 – Within the Solution Explorer find the “AcceptApi” project and right click it, then click “Build”. 

-	The project should successfully compile. 
-	If the compilation fails, means not all References are sorted, step 9 should be replicated for this project too.

Compiling the Solution:
=======================

If all dependencies and respective references are solved, the solution can now be successfully compiled.

#How to compile:
Within the Solution Explorer very top find the solution label: “Solution ‘AcceptApi’” and right click it, then click “Build”.

Deploying the Solution:
=======================

There are more than one way to deploy .Net projects: http://www.asp.net/mvc/overview/deployment
