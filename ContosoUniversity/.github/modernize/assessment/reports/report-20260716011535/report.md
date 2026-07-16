# ContosoUniversity

## Summary

| Metric | Value |
|--------|-------|
| Total Issues | 25 |
| Mandatory Blockers | 15 |
| Potential Issues | 9 |

## Component Information

| Property | Value |
|----------|-------|
| Language | .NET C# |
| Frameworks | ASP.NET MVC 5.2.9, .NET Framework 4.8, Entity Framework Core 3.1.32 |
| Build tools | MSBuild, NuGet packages.config |
| Description | University management web application for courses, students, instructors, and departments |
| Application type | Web Application |
| Lines of code | 2800 |

## Cloud Readiness Issues

| Issue Name | Criticality | Story Points | Occurrences |
|------------|-------------|--------------|-------------|
| MSMQ (System.Messaging) not supported in cloud | Mandatory | 13 | [3](#MSMQ_System_Messaging_not_supported_in_cloud) |
| HttpPostedFileBase file upload requires migration | Mandatory | 8 | [2](#HttpPostedFileBase_file_upload_requires_migration) |
| Server.MapPath not available in cloud hosting | Mandatory | 5 | [1](#Server_MapPath_not_available_in_cloud_hosting) |
| Web.config-based configuration not supported in cloud | Mandatory | 8 | [1](#Web_config-based_configuration_not_supported_in_cloud) |
| Legacy ASP.NET MVC 5 on .NET Framework 4.8 | Mandatory | 40 | [1](#Legacy_ASP_NET_MVC_5_on_NET_Framework_4_8) |
| Global.asax application lifecycle not available | Mandatory | 5 | [1](#Global_asax_application_lifecycle_not_available) |
| System.Configuration.ConfigurationManager usage | Potential | 5 | [2](#System_Configuration_ConfigurationManager_usage) |
| LocalDB connection string not suitable for cloud | Potential | 3 | [1](#LocalDB_connection_string_not_suitable_for_cloud) |
| Legacy packages.config format | Potential | 3 | [1](#Legacy_packages_config_format) |
| IIS-specific hosting configuration | Potential | 3 | [1](#IIS-specific_hosting_configuration) |

### Issue Details

<details id="MSMQ_System_Messaging_not_supported_in_cloud">
<summary><b>MSMQ (System.Messaging) not supported in cloud</b> — affected files</summary>

- `ContosoUniversity/Services/NotificationService.cs (line 1)`
- `ContosoUniversity/Services/NotificationService.cs (line 18)`
- `ContosoUniversity/Services/NotificationService.cs (line 27)`

</details>

<details id="HttpPostedFileBase_file_upload_requires_migration">
<summary><b>HttpPostedFileBase file upload requires migration</b> — affected files</summary>

- `ContosoUniversity/Controllers/CoursesController.cs (line 35)`
- `ContosoUniversity/Controllers/CoursesController.cs (line 95)`

</details>

<details id="Server_MapPath_not_available_in_cloud_hosting">
<summary><b>Server.MapPath not available in cloud hosting</b> — affected files</summary>

- `ContosoUniversity/Controllers/CoursesController.cs (line 65)`

</details>

<details id="Web_config-based_configuration_not_supported_in_cloud">
<summary><b>Web.config-based configuration not supported in cloud</b> — affected files</summary>

- `ContosoUniversity/Web.config (line 1)`

</details>

<details id="Legacy_ASP_NET_MVC_5_on_NET_Framework_4_8">
<summary><b>Legacy ASP.NET MVC 5 on .NET Framework 4.8</b> — affected files</summary>

- `ContosoUniversity/ContosoUniversity.csproj (line 10)`

</details>

<details id="Global_asax_application_lifecycle_not_available">
<summary><b>Global.asax application lifecycle not available</b> — affected files</summary>

- `ContosoUniversity/Global.asax (line 1)`

</details>

<details id="System_Configuration_ConfigurationManager_usage">
<summary><b>System.Configuration.ConfigurationManager usage</b> — affected files</summary>

- `ContosoUniversity/Data/SchoolContextFactory.cs (line 10)`
- `ContosoUniversity/Services/NotificationService.cs (line 14)`

</details>

<details id="LocalDB_connection_string_not_suitable_for_cloud">
<summary><b>LocalDB connection string not suitable for cloud</b> — affected files</summary>

- `ContosoUniversity/Web.config (line 8)`

</details>

<details id="Legacy_packages_config_format">
<summary><b>Legacy packages.config format</b> — affected files</summary>

- `ContosoUniversity/packages.config (line 1)`

</details>

<details id="IIS-specific_hosting_configuration">
<summary><b>IIS-specific hosting configuration</b> — affected files</summary>

- `ContosoUniversity/ContosoUniversity.csproj (line 13)`

</details>

## DotNET Upgrade Issues [View Details](scenarios/dotnet-version-upgrade/assessment.md)

| Issue Category | Criticality | Story Points | Occurrences |
|----------------|-------------|--------------|-------------|
| ASP.NET MVC 5 (System.Web) requires migration to ASP.NET Core MVC | Mandatory | 40 | [6](#ASP_NET_MVC_5_System_Web_requires_migration_to_ASP_NET_Core_MVC) |
| System.Web namespace not available in .NET 10 | Mandatory | 20 | [3](#System_Web_namespace_not_available_in_NET_10) |
| System.Configuration.ConfigurationManager not available | Mandatory | 5 | [3](#System_Configuration_ConfigurationManager_not_available) |
| Global.asax application lifecycle must be migrated | Mandatory | 5 | [2](#Global_asax_application_lifecycle_must_be_migrated) |
| System.Web.Optimization (bundling) not supported | Mandatory | 3 | [1](#System_Web_Optimization_bundling_not_supported) |
| System.Messaging (MSMQ) not available in .NET 10 | Mandatory | 13 | [1](#System_Messaging_MSMQ_not_available_in_NET_10) |
| Entity Framework Core 3.1 must be upgraded to EF Core 9+ | Mandatory | 8 | [1](#Entity_Framework_Core_3_1_must_be_upgraded_to_EF_Core_9) |
| Legacy .csproj format (non-SDK style) must be converted | Mandatory | 5 | [1](#Legacy_csproj_format_non-SDK_style_must_be_converted) |
| Razor views need migration from MVC 5 Razor to ASP.NET Core Razor | Potential | 13 | [1](#Razor_views_need_migration_from_MVC_5_Razor_to_ASP_NET_Core_Razor) |
| packages.config must be migrated to PackageReference | Potential | 3 | [1](#packages_config_must_be_migrated_to_PackageReference) |
| Microsoft.Data.SqlClient upgrade required | Potential | 2 | [1](#Microsoft_Data_SqlClient_upgrade_required) |
| Newtonsoft.Json can be replaced with System.Text.Json | Optional | 5 | [1](#Newtonsoft_Json_can_be_replaced_with_System_Text_Json) |

### Issue Details

<details id="ASP_NET_MVC_5_System_Web_requires_migration_to_ASP_NET_Core_MVC">
<summary><b>ASP.NET MVC 5 (System.Web) requires migration to ASP.NET Core MVC</b> — affected files</summary>

- `ContosoUniversity/Controllers/BaseController.cs (line 1)`
- `ContosoUniversity/Controllers/StudentsController.cs (line 1)`
- `ContosoUniversity/Controllers/CoursesController.cs (line 1)`
- `ContosoUniversity/Controllers/DepartmentsController.cs (line 1)`
- `ContosoUniversity/Controllers/InstructorsController.cs (line 1)`
- `ContosoUniversity/Controllers/NotificationsController.cs (line 1)`

</details>

<details id="System_Web_namespace_not_available_in_NET_10">
<summary><b>System.Web namespace not available in .NET 10</b> — affected files</summary>

- `ContosoUniversity/Controllers/CoursesController.cs (line 5)`
- `ContosoUniversity/Controllers/StudentsController.cs (line 5)`
- `ContosoUniversity/Global.asax.cs (line 2)`

</details>

<details id="System_Configuration_ConfigurationManager_not_available">
<summary><b>System.Configuration.ConfigurationManager not available</b> — affected files</summary>

- `ContosoUniversity/Data/SchoolContextFactory.cs (line 10)`
- `ContosoUniversity/Global.asax.cs (line 26)`
- `ContosoUniversity/Services/NotificationService.cs (line 14)`

</details>

<details id="Global_asax_application_lifecycle_must_be_migrated">
<summary><b>Global.asax application lifecycle must be migrated</b> — affected files</summary>

- `ContosoUniversity/Global.asax (line 1)`
- `ContosoUniversity/Global.asax.cs (line 1)`

</details>

<details id="System_Web_Optimization_bundling_not_supported">
<summary><b>System.Web.Optimization (bundling) not supported</b> — affected files</summary>

- `ContosoUniversity/App_Start/BundleConfig.cs (line 1)`

</details>

<details id="System_Messaging_MSMQ_not_available_in_NET_10">
<summary><b>System.Messaging (MSMQ) not available in .NET 10</b> — affected files</summary>

- `ContosoUniversity/Services/NotificationService.cs (line 1)`

</details>

<details id="Entity_Framework_Core_3_1_must_be_upgraded_to_EF_Core_9">
<summary><b>Entity Framework Core 3.1 must be upgraded to EF Core 9+</b> — affected files</summary>

- `ContosoUniversity/ContosoUniversity.csproj (line 1)`

</details>

<details id="Legacy_csproj_format_non-SDK_style_must_be_converted">
<summary><b>Legacy .csproj format (non-SDK style) must be converted</b> — affected files</summary>

- `ContosoUniversity/ContosoUniversity.csproj (line 1)`

</details>

<details id="Razor_views_need_migration_from_MVC_5_Razor_to_ASP_NET_Core_Razor">
<summary><b>Razor views need migration from MVC 5 Razor to ASP.NET Core Razor</b> — affected files</summary>

- `ContosoUniversity/Views/Instructors/Index.cshtml (line 1)`

</details>

<details id="packages_config_must_be_migrated_to_PackageReference">
<summary><b>packages.config must be migrated to PackageReference</b> — affected files</summary>

- `ContosoUniversity/packages.config (line 1)`

</details>

<details id="Microsoft_Data_SqlClient_upgrade_required">
<summary><b>Microsoft.Data.SqlClient upgrade required</b> — affected files</summary>

- `ContosoUniversity/packages.config (line 1)`

</details>

<details id="Newtonsoft_Json_can_be_replaced_with_System_Text_Json">
<summary><b>Newtonsoft.Json can be replaced with System.Text.Json</b> — affected files</summary>

- `ContosoUniversity/packages.config (line 1)`

</details>

## Security Issues

> **Note:** These issues were generated by AI and may contain inaccuracies or incomplete information. Please review carefully.

| Issue Name | Criticality | Story Points | Files |
|------------|-------------|--------------|-------|
| SEC-001: No authentication or authorization implemented | Mandatory | 13 | [5](#SEC-001_No_authentication_or_authorization_implemented) |
| SEC-002: File upload without path traversal protection | Potential | 5 | [1](#SEC-002_File_upload_without_path_traversal_protection) |
| SEC-003: MSMQ queue permissions set to Everyone/FullControl | Potential | 3 | [1](#SEC-003_MSMQ_queue_permissions_set_to_Everyone_FullControl) |

### Security Issue Details

<details id="SEC-001_No_authentication_or_authorization_implemented">
<summary><b>SEC-001: No authentication or authorization implemented</b> — affected files</summary>

- `ContosoUniversity/Controllers/BaseController.cs`
- `ContosoUniversity/Controllers/StudentsController.cs`
- `ContosoUniversity/Controllers/CoursesController.cs`
- `ContosoUniversity/Controllers/InstructorsController.cs`
- `ContosoUniversity/Controllers/DepartmentsController.cs`

</details>

<details id="SEC-002_File_upload_without_path_traversal_protection">
<summary><b>SEC-002: File upload without path traversal protection</b> — affected files</summary>

- `ContosoUniversity/Controllers/CoursesController.cs`

</details>

<details id="SEC-003_MSMQ_queue_permissions_set_to_Everyone_FullControl">
<summary><b>SEC-003: MSMQ queue permissions set to Everyone/FullControl</b> — affected files</summary>

- `ContosoUniversity/Services/NotificationService.cs`

</details>

---

## Codebase Insights

> **Note:** These documents are generated by AI and may contain inaccuracies or incomplete information. Please review carefully.

1. **[Architecture Diagram](facts/architecture-diagram.md)** — Understand the big picture: system layers and component relationships
2. **[Dependency Map](facts/dependency-map.md)** — Know what the project depends on and where the risks are
3. **[API & Service Contracts](facts/api-service-contracts.md)** — See how services communicate and what contracts they expose
4. **[Data Architecture](facts/data-architecture.md)** — Explore data models, storage, and data flow patterns
5. **[Configuration Inventory](facts/configuration-inventory.md)** — Review how the application is configured across environments
6. **[Business Workflows](facts/business-workflows.md)** — Trace end-to-end business processes and domain logic

[Share feedback](https://aka.ms/ghcp-appmod/feedback)
