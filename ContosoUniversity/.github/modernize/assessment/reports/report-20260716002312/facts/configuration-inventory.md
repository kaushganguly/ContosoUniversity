# Configuration & Externalized Settings Inventory

The project uses classic ASP.NET configuration with Web.config as the central source, plus build/runtime settings in the legacy csproj and package manifests.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | Application config | `ContosoUniversity/Web.config` | Connection string, appSettings, runtime binding redirects, HTTP limits |
| Global.asax.cs | Startup bootstrap | `ContosoUniversity/Global.asax.cs` | Registers routes/bundles and initializes database |
| RouteConfig.cs | Routing config | `ContosoUniversity/App_Start/RouteConfig.cs` | Conventional controller/action/id route map |
| packages.config | Dependency manifest | `ContosoUniversity/packages.config` | NuGet package version pinning |
| ContosoUniversity.csproj | Build/project config | `ContosoUniversity/ContosoUniversity.csproj` | Target framework, build profiles, IIS Express settings |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | default local build | Developer diagnostics and non-optimized builds | `DefineConstants=DEBUG;TRACE`, full debug symbols |
| Release | explicit build configuration | Optimized build for deployment | `DefineConstants=TRACE`, optimize enabled |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | IIS/IIS Express app startup | Web.config | SQL LocalDB connection string and request limits |
| IIS Express Local | Visual Studio launch settings in csproj metadata | csproj web project properties | `IISUrl=https://localhost:44300/`, Windows auth enabled |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `DefaultConnection` | `Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=ContosoUniversityNoAuthEFCore;Integrated Security=True;MultipleActiveResultSets=True` | Default | Web.config connectionStrings |
| `webpages:Version` | `3.0.0.0` | Default | Web.config appSettings |
| `webpages:Enabled` | `false` | Default | Web.config appSettings |
| `ClientValidationEnabled` | `true` | Default | Web.config appSettings |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | Web.config appSettings |
| `NotificationQueuePath` | `.\Private$\ContosoUniversityNotifications` | Default | Web.config appSettings |
| `httpRuntime.maxRequestLength` | `10240` | Default | Web.config |
| `requestLimits.maxAllowedContentLength` | `10485760` | Default | Web.config |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| ContosoUniversity | .NET Framework 4.8 ASP.NET runtime, IIS Express metadata in csproj | Not explicitly configured | Not explicitly configured |

## Startup Dependency Chain

1. IIS/ASP.NET runtime starts `MvcApplication`.
2. `Application_Start` registers MVC filters, routes, and bundles.
3. `InitializeDatabase` builds `SchoolContext` and calls `DbInitializer.Initialize`.
4. Application serves MVC and notification endpoints after database readiness.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `DefaultConnection` | Database connection string (integrated auth) | Web.config `[MASKED]` |
| `NotificationQueuePath` | Queue endpoint path | Web.config (non-secret operational setting) |

### Secrets Provisioning Workflow

Configuration is file-based and loaded directly at runtime from Web.config. No external vault, managed identity, or secret-rotation workflow is declared in the repository; database access relies on integrated security semantics.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| `ClientValidationEnabled` | true | Web.config appSettings |
| `UnobtrusiveJavaScriptEnabled` | true | Web.config appSettings |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework target | v4.8 | ContosoUniversity.csproj |
| ASP.NET MVC | 5.2.9 | packages.config |
| Entity Framework Core | 3.1.32 | packages.config |
| SQL Client | 2.1.4 | packages.config |
| Newtonsoft.Json | 13.0.3 | packages.config |
| jQuery | 3.7.1 | packages.config |
| Bootstrap | 5.3.3 | packages.config |
