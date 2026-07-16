# Configuration & Externalized Settings Inventory

Configuration is primarily centralized in classic ASP.NET `Web.config` with supplementary startup registration classes and project build properties. The repository has a small configuration surface and no external config server or managed secret store wiring.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|---|---|---|---|
| `Web.config` | Runtime app configuration | `ContosoUniversity/Web.config` | Connection strings, app settings, runtime and IIS settings |
| `Views/Web.config` | MVC view engine configuration | `ContosoUniversity/Views/Web.config` | Razor view compilation settings |
| `ContosoUniversity.csproj` | Build and packaging configuration | `ContosoUniversity/ContosoUniversity.csproj` | Framework target, references, Debug/Release groups, IIS Express profile |
| `RouteConfig.cs` | Route configuration | `ContosoUniversity/App_Start/RouteConfig.cs` | Default `{controller}/{action}/{id}` route |
| `BundleConfig.cs` | Asset pipeline config | `ContosoUniversity/App_Start/BundleConfig.cs` | Script/style bundle definitions |
| `FilterConfig.cs` | MVC global filter configuration | `ContosoUniversity/App_Start/FilterConfig.cs` | `HandleErrorAttribute`; global auth filter commented out |

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---|---|---|---|
| Debug | `Configuration=Debug` | Local debugging with symbols and non-optimized build | Debug symbols, `DEBUG;TRACE` constants |
| Release | `Configuration=Release` | Optimized production-style build output | `TRACE` constant, optimized compilation |

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---|---|---|---|
| Default (single runtime profile) | IIS/IIS Express application start | `Web.config` | `DefaultConnection`, request size/timeouts, queue path |
| IIS Express local profile | Visual Studio/IIS Express launch | `ContosoUniversity.csproj` web properties | URL `https://localhost:44300/`, Windows auth enabled, anonymous disabled |

## Properties Inventory

| Property Key | Default | Profiles | Source |
|---|---|---|---|
| `ConnectionStrings:DefaultConnection` | `Data Source=(LocalDb)\MSSQLLocalDB;...` | Default | `Web.config` |
| `webpages:Version` | `3.0.0.0` | Default | `Web.config` |
| `webpages:Enabled` | `false` | Default | `Web.config` |
| `ClientValidationEnabled` | `true` | Default | `Web.config` |
| `UnobtrusiveJavaScriptEnabled` | `true` | Default | `Web.config` |
| `NotificationQueuePath` | `.\Private$\ContosoUniversityNotifications` | Default | `Web.config` |
| `httpRuntime.maxRequestLength` | `10240` | Default | `Web.config` |
| `httpRuntime.executionTimeout` | `3600` | Default | `Web.config` |
| `requestLimits.maxAllowedContentLength` | `10485760` | Default | `Web.config` |

## Startup Parameters & Resource Requirements

| Service | JVM/Runtime Options | Memory | Instance Count |
|---|---|---|---|
| ContosoUniversity | .NET Framework 4.8 ASP.NET runtime; no explicit CLI startup flags in repo | Not explicitly declared in repository | Not specified |

## Startup Dependency Chain

1. ASP.NET application startup triggers `Application_Start` in `Global.asax`.
2. MVC registration occurs (`RouteConfig`, `BundleConfig`, `FilterConfig`).
3. Database initialization runs via `DbInitializer.Initialize` against `DefaultConnection`.
4. Notification operations depend on MSMQ queue availability when controller actions emit events.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage (masked) |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Database connection string | `Web.config` (integrated security, no explicit password) |
| `NotificationQueuePath` | Infrastructure path | `Web.config` appSetting |

### Secrets Provisioning Workflow

Configuration values are read directly from checked-in `Web.config` and loaded by `ConfigurationManager` at runtime. There is no evidence of external secret providers (Key Vault, Vault, AWS Secrets Manager) or identity-based secret retrieval in this codebase.

## Feature Flags

| Flag Name | Default | Controlled By |
|---|---|---|
| None detected | N/A | No feature-flag framework or toggle keys found |

## Framework & Runtime Versions

| Component | Version | Source |
|---|---|---|
| .NET Framework target | v4.8 | `ContosoUniversity.csproj` |
| ASP.NET MVC | 5.2.9 | `packages.config` |
| Entity Framework Core | 3.1.32 | `packages.config` |
| SQL client | 2.1.4 (`Microsoft.Data.SqlClient`) | `packages.config` |
| Newtonsoft.Json | 13.0.3 | `packages.config` |
| jQuery | 3.7.1 package / 3.4.1 bundled static script | `packages.config` and `Scripts/` |
| bootstrap | 5.3.3 package | `packages.config` |
