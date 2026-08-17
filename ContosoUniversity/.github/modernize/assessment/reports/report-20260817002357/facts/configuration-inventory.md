# Configuration & Externalized Settings Inventory

The application uses legacy ASP.NET configuration through `Web.config`, project-level MSBuild properties, and MVC startup classes. No environment-specific appsettings files, secret stores, feature flag systems, or external configuration services were detected.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | ASP.NET application config | `ContosoUniversity/Web.config` | Connection string, appSettings, request limits, compilation target, runtime binding redirects |
| Views Web.config | Razor view config | `ContosoUniversity/Views/Web.config` | View-engine and namespace settings for Razor views |
| Web.Debug.config | Web.config transform | `ContosoUniversity/Web.Debug.config` | Debug deployment transform included in project |
| Web.Release.config | Web.config transform | `ContosoUniversity/Web.Release.config` | Release deployment transform included in project |
| ContosoUniversity.csproj | MSBuild project config | `ContosoUniversity/ContosoUniversity.csproj` | Target framework, configurations, IIS Express settings, references, content, and web targets imports |
| RouteConfig.cs | MVC route configuration | `App_Start/RouteConfig.cs` | Defines conventional default route |
| FilterConfig.cs | MVC filter configuration | `App_Start/FilterConfig.cs` | Registers global error handling; authorization filter is commented out |
| BundleConfig.cs | Asset bundling configuration | `App_Start/BundleConfig.cs` | Defines jQuery, validation, Modernizr, Bootstrap, and CSS bundles |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug AnyCPU | Default when `Configuration` is unset | Builds with debug symbols, no optimization, `DEBUG;TRACE` constants | Microsoft.CSharp targets, ASP.NET WebApplication targets |
| Release AnyCPU | Manual MSBuild configuration | Builds optimized output with PDB symbols and `TRACE` constant | Microsoft.CSharp targets, ASP.NET WebApplication targets |
| MvcBuildViews | MSBuild property `MvcBuildViews=true` | Runs ASP.NET compiler against Razor views after build | `AspNetCompiler` target |
| CopySQLClientNativeBinaries | AfterBuild target | Copies Microsoft.Data.SqlClient SNI native binaries into output | Microsoft.Data.SqlClient.SNI.runtime package |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | IIS / IIS Express loading `Web.config` | `Web.config` | SQL Server LocalDB connection, MVC validation flags, request size limits, notification queue path |
| Debug transform | Deployment/build transform | `Web.Debug.config` | Debug-specific transform if applied by deployment tooling |
| Release transform | Deployment/build transform | `Web.Release.config` | Release-specific transform if applied by deployment tooling |

## Properties Inventory

### ContosoUniversity

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| ConnectionStrings.DefaultConnection | SQL Server LocalDB connection to `ContosoUniversityNoAuthEFCore` with integrated security | Default | `Web.config` |
| webpages:Version | 3.0.0.0 | Default | `Web.config` |
| webpages:Enabled | false | Default | `Web.config` |
| ClientValidationEnabled | true | Default | `Web.config` |
| UnobtrusiveJavaScriptEnabled | true | Default | `Web.config` |
| NotificationQueuePath | `./Private$/ContosoUniversityNotifications` equivalent local private queue | Default | `Web.config` |
| system.web compilation targetFramework | 4.8 | Default | `Web.config` |
| httpRuntime targetFramework | 4.8 | Default | `Web.config` |
| httpRuntime maxRequestLength | 10240 KB | Default | `Web.config` |
| httpRuntime executionTimeout | 3600 seconds | Default | `Web.config` |
| requestLimits maxAllowedContentLength | 10485760 bytes | Default | `Web.config` |
| TargetFrameworkVersion | v4.8 | Debug, Release | `ContosoUniversity.csproj` |
| IISUrl | https://localhost:44300/ | Development | `ContosoUniversity.csproj` |
| DevelopmentServerPort | 58801 | Development | `ContosoUniversity.csproj` |
| IISExpressAnonymousAuthentication | disabled | Development | `ContosoUniversity.csproj` |
| IISExpressWindowsAuthentication | enabled | Development | `ContosoUniversity.csproj` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| ContosoUniversity | ASP.NET on .NET Framework 4.8; IIS Express URL configured as `https://localhost:44300/`; no explicit process command line found | No memory limits specified | No scaling or instance count specified |
| SQL Server LocalDB | LocalDB instance selected by connection string | Local process default | Single local database instance |
| MSMQ private queue | Queue path from `NotificationQueuePath` | Local OS queue default | Single local queue |

## Startup Dependency Chain

1. ContosoUniversity application starts under IIS or IIS Express.
2. `Application_Start` registers MVC areas, global filters, routes, and bundles.
3. `Application_Start` creates `SchoolContext` using `DefaultConnection` and invokes `DbInitializer.Initialize`.
4. `DbInitializer` calls `Database.EnsureCreated` and seeds data if students do not already exist.
5. Controllers instantiate `NotificationService` through `BaseController`; the service creates or opens the configured MSMQ private queue when first constructed.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `DefaultConnection` | Database connection string | Integrated security LocalDB string; no password present |
| `NotificationQueuePath` | Queue path | Non-secret local queue path |

### Secrets Provisioning Workflow

No external secret provisioning workflow was detected. The application uses Windows integrated security for LocalDB and stores non-secret queue configuration directly in `Web.config`; no Key Vault, Vault, AWS Secrets Manager, encrypted properties, managed identity, or deployment-time secret binding was found.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | No feature flag framework or conditional feature toggles detected |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---:|---|
| .NET Framework target | 4.8 | `ContosoUniversity.csproj`, `Web.config` |
| ASP.NET MVC | 5.2.9 | `packages.config` |
| ASP.NET Razor | 3.2.9 | `packages.config` |
| ASP.NET WebPages | 3.2.9 | `packages.config` |
| Entity Framework Core | 3.1.32 | `packages.config` |
| Microsoft.Data.SqlClient | 2.1.4 | `packages.config` |
| Microsoft.Extensions packages | 3.1.32 | `packages.config` |
| Newtonsoft.Json | 13.0.3 | `packages.config` |
| Bootstrap | 5.3.3 | `packages.config` |
| jQuery | 3.7.1 | `packages.config` |
| jQuery Validation | 1.21.0 | `packages.config` |
| MSBuild ToolsVersion | 15.0 | `ContosoUniversity.csproj` |
