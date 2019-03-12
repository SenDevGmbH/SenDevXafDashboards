# SenDev XAF Dashboards
[![Build Status](https://dev.azure.com/SenDevGmbH/XafDashboards/_apis/build/status/SenDevGmbH.SenDevXafDashboards?branchName=master)](https://dev.azure.com/SenDevGmbH/XafDashboards/_build/latest?definitionId=1&branchName=master)

## Introduction
This module adds to the DevExpress XAF Dashboards module 2 new data sources:
 * C# Script Data Source
 * XAF Data Extract Data Source

### C# Script Data Source
C# Script data source allows to specify dashboard data source with C# Scripts at runtime.

### XAF Data Extract Data Source
XAF Dashboards provides with [Data Extracts](https://documentation.devexpress.com/Dashboard/115900/Creating-Dashboards/Creating-Dashboards-in-the-WinForms-Designer/Providing-Data/Extract-Data-Source) a feature that allows You to store dashboard data in an optimized format for data grouping and other dashboard operations. With data extract large amounts of data can be stored in a compact format and be quickly loaded into dashboards. For example queries with large amounts of data can execute and create data extract nightly. Prepared data extracts can then be quickly loaded into the dashboards. 

XAF Data Extract Data Source has following features:
 * Memory-optimized creation of data extracts for large datasets. 
 * Storing data extracts in the database
 * Specify Data Source with C# scripts


## Getting started
### Installing SenDev.XafDashboard packages into your application modules
We provide Nuget packages for multiple DevExpress versions. If You want to install SenDev.Xaf.Dashboard packages for specific version of DevExpress, please pick the latest Version according to the installed version of the installed DevExpress Suite. You can specify the version with the `-Version` parameter of the `Install-Package` command. If You skip the version parameter, latest Version of the packages for the latest supported version of the DevExpress Suite will be installed.

#### Platform agnostic module
Install the package with following command in the Visual Studio Package Manager Console 
```Console
Install-Package SenDev.Xaf.Dashboards -IncludePrerelease
```

Add following line in the InitializeComponent method in Module.Designer.cs:
```C#
this.RequiredModuleTypes.Add(typeof(SenDev.Xaf.Dashboards.SenDevDashboardsModule));
```

#### Windows module
Install the package with following command in the Visual Studio Package Manager Console 
```Console
Install-Package SenDev.Xaf.Dashboards.Win -IncludePrerelease
```

Add following line in the InitializeComponent method in WinModule.Designer.cs:
```C#
this.RequiredModuleTypes.Add(typeof(SenDev.Xaf.Dashboards.Win.SenDevDashboardsWinModule));

```

#### Web module
Install the package with following command in the Visual Studio Package Manager Console 
```Console
Install-Package SenDev.Xaf.Dashboards.Web -IncludePrerelease
```

Add following line in the InitializeComponent method in WebModule.Designer.cs:
```C#
this.RequiredModuleTypes.Add(typeof(SenDev.Xaf.Dashboards.Web.SenDevDashboardsWebModule));
```

### Using Script Data Source
Start Application. Goto Dashboards and click on "New". Select "C# Script Data Source":
<img src="images/SelectCSScriptDataSource.png" width="600">

Click Next and specify the script:

<img src="images/SpecifyScript.png" width="600">

You can use following script template:

```C#
using System;

using System.Linq;
using DevExpress.Xpo;	

using SenDev.DashboardsDemo.Module.BusinessObjects;
using SenDev.Xaf.Dashboards.Scripting;		


public class Script
{
    public object GetData(ScriptContext context)
    {
        return context.Query<OnlineSales>().Take(1000);
    }
}
```

### Using XAF Data Extract Data Source

First, we must create a data extract. Goto _Dashboard Data Extract_ Navigation Item. Click On "New". Enter a data extract name and the script. You can use the same template as for the Script Data Source above. Then Click on _Save & Close_. In the list view use the "Update Data" button to create data extract: 

<img src="images/UpdateDataAction.png" width="50px">

After the data extract is created, we can use it in a dashboard. Goto dashboards, click on new. 
In the data source window select "XAF Data Extract": 

<img src="images/SelectXAFDataExtract.png" width="600">

On the next page You can select the data extract, You created previously:
<img src="images/SelectDataExtract.png" width="600">

Then click on "Finish". Your Data Extract Data Source is ready!

#### Scheduling Updates of Data Extracts
We are not providing are build-in job scheduler with the packages. However the demo application implements a scheduler by using [Hangfire](https://www.hangfire.io), You can use the code in Your applications.

Also You can easily integrate You preferred scheduler by implementing the `IJobScheduler` interface.


## Development

### Pre-requisites
* Visual Studio 2017
* Microsoft SQL Server 2017 or higher
* DevExpress Universal Suite 18.2

### Database
Download the [Contoso BI Demo DataSet](https://www.microsoft.com/en-us/download/details.aspx?id=18279). Restore the database, use ContosoRetailDW for database name. 
Connection Strings in the applications are configured to use default instance of the SQL Server, if You're not using the default instance, You can modify the Connection Strings accordingly.
After opening the SenDev.Xaf.Dashboards.sln in Visual Studio, go to the solution properties and set the SenDev.Xaf.Dashboards.Win and SenDev.Xaf.Dashboards.Web projects to "Start":
<img src="images/SolutionProperties.png" width="600">

The web application needs always to be started, since it hosting a WCF Service for job scheduler.

