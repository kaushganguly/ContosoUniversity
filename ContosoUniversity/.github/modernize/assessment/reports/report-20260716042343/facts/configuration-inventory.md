# Configuration & Externalized Settings Inventory

ContosoUniversity uses a traditional .NET Framework configuration model centered on a single `Web.config` file plus project and package metadata. The configuration footprint is small, with no environment-specific profile files, no remote config service, and no dedicated secrets store integration.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| Web.config | Application configuration | `ContosoUniversity/Web.config` | Primary runtime source for connection strings, app settings, request limits, and binding redirects |
| Web.Debug.config | Transform | `ContosoUniversity/Web.Debug.config` | Debug-specific XML transform placeholder |
| Web.Release.config | Transform | `ContosoUniversity/Web.Release.config` | Release-specific XML transform placeholder |
| packages.config | Package manifest | `ContosoUniversity/packages.config` | Declares NuGet package versions |
| ContosoUniversity.csproj | Build configuration | `ContosoUniversity/ContosoUniversity.csproj` | Declares target framework, IIS Express settings, references, and build properties |
| Global.asax / Global.asax.cs | Startup configuration | `ContosoUniversity/Global.asax*` | Registers routes, bundles, filters, and database initialization |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | Manual build configuration | Enables symbols, disables optimization, uses `DEBUG;TRACE` constants | Standard MSBuild configuration |
| Release | Manual build configuration | Optimized build with `TRACE` constant | Standard MSBuild configuration |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default | Application startup under IIS/IIS Express | `Web.config` | LocalDB connection string, MSMQ queue path, upload/request limits |
| Debug transform | Build/publish transform | `Web.Debug.config` | No meaningful overrides defined in repository |
| Release transform | Build/publish transform | `Web.Release.config` | No meaningful overrides defined in repository |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `DefaultConnection` | `Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=ContosoUniversityNoAuthEFCore;Integrated Security=True;MultipleActiveResultSets=True` | Default | `Web.config` |
| `NotificationQueuePath` | `.\Private$\ContosoUniversityNotifications` | Default | `Web.config` |
| `webpages:Version` | `3.0.0.0` | Default | `Web.config` |
| `webpages:Enabled` | `false` | Default | `Web.config` |
| `ClientValidationEnabled` | `true` | Default | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `Web.config` |
| `system.web/httpRuntime@targetFramework` | `4.8` | Default | `Web.config` |
| `system.web/httpRuntime@maxRequestLength` | `10240` | Default | `Web.config` |
| `system.web/httpRuntime@executionTimeout` | `3600` | Default | `Web.config` |
| `system.webServer/requestLimits@maxAllowedContentLength` | `10485760` | Default | `Web.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| ContosoUniversity | .NET Framework 4.8 under IIS Express; no explicit runtime flags stored in repo | Not specified | 1 local web app instance implied |

## Startup Dependency Chain

1. `MvcApplication.Application_Start` → registers MVC areas, global filters, routes, and bundles.
2. Application startup then invokes `DbInitializer.Initialize()` → waits for LocalDB connectivity so the schema and seed data can be created.
3. CRUD requests that emit notifications depend on the MSMQ private queue path from `Web.config`; `NotificationService` creates the queue on demand if it does not already exist.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `DefaultConnection` | Database connection string | `Integrated Security=True`; no username/password stored |
| `NotificationQueuePath` | Infrastructure endpoint | `.\Private$\[MASKED_QUEUE_NAME]` |

### Secrets Provisioning Workflow

The repository does not implement a formal secrets provisioning workflow. Database access relies on Windows-integrated security, so the hosting identity must already have access to LocalDB. No Azure Key Vault, environment variable placeholder, user-secret store, or encrypted config section is configured in source control.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | No feature flag framework or conditional configuration was found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---:|---|
| .NET Framework | 4.8 | `ContosoUniversity.csproj` |
| ASP.NET MVC | 5.2.9 | `packages.config` / assembly references |
| ASP.NET Razor | 3.2.9 | `packages.config` |
| EF Core | 3.1.32 | `packages.config` |
| SQL Client | 2.1.4 | `packages.config` |
| Microsoft.Extensions.* stack | 3.1.32 | `packages.config` |
| Newtonsoft.Json | 13.0.3 | `packages.config` |
| jQuery | 3.7.1 package / 3.4.1 checked-in script | `packages.config` and `Scripts/` |
| Bootstrap | 5.3.3 package | `packages.config` |
| WebGrease | 1.5.2 | `packages.config` |
