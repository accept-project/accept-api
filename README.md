ACCEPT API & ACCEPT Framework
=============================

#Configuration and Deployment instructions. 

##Getting Started:

-	Before getting started make sure the .Net environment is up and running for the .Net Framework 4.5.
-	Make sure Visual Studio 2010 or greater is installed.
-	Make sure the NuGet package manager is up to date - https://visualstudiogallery.msdn.microsoft.com/27077b70-9dad-4c64-adcf-c7cf6bc9970c

##First Steps:

1. Download ACCEPT API repository folder.

2. Open file “AcceptApi.csproj” with Visual Studio.

3. Click “File”, click “Add”, click “Existing Project…”, double click “AcceptFramework” folder, choose file "AcceptFramework.csproj", click “Open”.

4. Click “File”, click “Save All”, choose a directory path(usually .Net solutions are kept in the root folder of the project), click “Save”.

-	At this stage the ACCEPT Framework dependency is automatically resolved and the API Solution file is created.

-	Now the next step is to sort all ACCEPT Framework dependencies and compile the project.

####Solving Dependencies:Some of the dependencies might be tricky to find with NuGet, mainly the ones related with NHiberante and the proxy class. If so let me know by email and I'll provide you with the correct libs.

##Solving the ACCEPT Framework Project Dependencies:

1.Click “Tools”, click “Library Package Manager”, click “Package Manager Console”.
-	The Console Tab should be displayed somewhere in the solution.
	
-	Next step is to proper configure Nuget to enable automatic package restore. 

2.Within the package manager console Tab, make sure the Default project is set to "AcceptFramework". If not select it from the drop down list. When selected, type the following command: “Install-Package NuGetPowerTools”.

-	If success the following message should be prompted: “Successfully installed 'NuGetPowerTools ...'.”.

3.Right click over “Solution ‘AcceptApi’”(in the top of the Solution Explorer), and click “Enable NuGet Package Restore”.

4.Back to the package manager console, type the following command: “Install packages.config”. If this command somehow fails(there were reports of such behaviour) then:

-	Right click over "AcceptFramework"(in the top of the Solution Explorer), then click "Manage NuGet Packages". Whitin the pop-up window, in the very top of it a yellow bar requests permission to restore the NuGet packages. Clicking the "Restore" button initiates the restore process.

Now that all packages are downloaded(default folder should be located: ~/AcceptApi/packages) it is possible to manually add/resolve the missing dependencies since they are not automatically referenced within the solution:

Within the Solution Explorer find the “AcceptFramework” project and right click References, then click “Add Reference…”.

On the dialog, click browse, find the packages main folder(default folder should be located: ~/AcceptApi/packages) and resolve the following .DLL files:

	-FluentNhibernate(within "packages\FluentNHibernate..." folder)
	-NHibernate(within "packages\NHibernate...\lib\net40" folder)

#####Note: Repeat the logic above for all missing libraries.

##Solving the ACCEPT API Project Dependencies:

1.At this stage most of the libraries references should be resolved, if not, repeat the process above:

-	Within the Solution Explorer find the “AcceptApi” project and right click References, then click “Add Reference…”, click browse to search for missing libraries. For libraries added in the context of the AcceptFramework the same path can be provided.

2.Within the Solution Explorer find the "AcceptApi" project and right click it, then click "Build". 

-	The project should successfully compile. 

##Solving JavaScript Dependencies:

Since the ACCEPT API is using Microsoft Signal R, some JavaScript is needed and therefore dependencies need to be resolved.
And with the steps taken previously most of them are already resolved, however, the following need to be NuGet resolved:

-	Within the package manager console Tab, type the following command: “Install-Package MicrosoftAjax”.	

-	Within the package manager console Tab, type the following command: “Install-Package MicrosoftMvcAjax.Mvc5”. 

Now that all JavaScript libraries are resolved, it is needed to reference them manually within the Solution.
To do so, within the Solution Explorer, under the "AcceptApi" project label, expand folder "Scrips" and check for missing the references identified with a "currently missing" icon.

The same folder\file name can found within the "~\packages" folder. 
Finally, copy and paste all missing files within the "Scripts" folder, when finishing, all "currently missing" icons should be gone.

#Compiling the Solution:

If all dependencies and respective references are resolved, the solution can now be successfully compiled.

##How to compile:
Within the Solution Explorer very top find the solution label: “Solution ‘AcceptApi’” and right click it, then click “Build”.

#Deploying the Solution:

There are more than one way to deploy .Net projects: http://www.asp.net/mvc/overview/deployment

#Initialization Process(Important!):

1. When proper deployed, the API initialization method needs to be called in order to:

*  Create default user Roles in DB.
*  Create default Languages in DB.

The method can be found under the Admin Controller:
http://[ACCEPT_API_URL]/api/v1/Admin/InitialiseAccept

If the init process was sucessfull then the following response should be returned:

```json
{
    "ResponseObject": "The ACCEPT system was successfully initialized.",
    "ResponseStatus": "OK",
    "TimeStamp": "/Date(1423133466466)/",
    "AcceptSessionCode": "",
    "GlobalSessionId": ""
}
```

2. Other very important step is to make sure a valid e-mail Server is configured - for example: when registering a new user in the ACCEPT Portal a confirmation email is sent to the email account provided as user name.

To do so, check the [Configuration](https://github.com/accept-project/accept-api/blob/master/AcceptApi/Web.config) file "mailSettings" section:

```xml
    <mailSettings>
      <smtp deliveryMethod="Network" from="noreply@accept-portal.eu">
        <network host="" port="" userName="" password="" />
      </smtp>
    </mailSettings>
```

##Support Contact
Any issue/question on the ACCEPT API can be posted [here](https://github.com/accept-project/accept-api/issues).
Or contact me directly via davidluzsilva@gmail.com

##Citing

If you use the ACCEPT API or Framework in your research work, please cite one of the following:

  * *Pre-Edit*: Seretan, V., Roturier, J., Silva, D. & Bouillon, P. 2014. "The ACCEPT Portal: An Online Framework for the Pre-editing and Post-editing of User-Generated Content". In Proceedings of the Workshop on Humans and Computer-Assisted Translation, pp. 66-71, Gothenburg, Sweden, April. ([Bib file](https://raw.githubusercontent.com/accept-project/accept-portal/master/cite.bib))
  * *Post-Edit*: Roturier, J., Mitchell, L., Silva, D. (2013). The ACCEPT post-editing environment: A flexible and customisable online tool to perform and analyse machine translation post-editing. In Proceedings of MT Summit XIV Workshop on Post-editing Technology and Practice, pp. 119-128, Nice, France, September. ([Bib file](https://raw.githubusercontent.com/accept-project/accept-post-edit/master/cite.bib))
  * *Evaluation*: Mitchell, L., Roturier, J., Silva, D. (2014). Using the ACCEPT framework to conduct an online community-based translation evaluation study. In Proceedings of the Seventeenth Annual Conference of the European Association for Machine Translation (EAMT), June 2014, Dubrovnik, Croatia. ([Bib file](https://raw.githubusercontent.com/accept-project/accept-evaluation/master/cite.bib))
