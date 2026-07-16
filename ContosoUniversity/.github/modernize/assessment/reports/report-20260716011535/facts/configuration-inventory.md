# Configuration & Externalized Settings Inventory

ContosoUniversity uses a single `Web.config` file as its only configuration source with no environment-specific overrides, no secrets management, and no cloud configuration service integration.

## Configuration Sources

| Source | Type | Path/Location | Notes |
|--------|------|--------------|-------|
| Web.config | XML Application Config | ContosoUniversity/Web.config | Primary config: connection strings, appSettings, runtime assembly redirects |
| packages.config | NuGet Packages | ContosoUniversity/packages.config | NuGet dependency declarations (legacy format) |
| Global.asax.cs | Startup Code | ContosoUniversity/Global.asax.cs | Application lifecycle; reads connection string at startup for DB seeding |
| ContosoUniversity.csproj | MSBuild Project | ContosoUniversity/ContosoUniversity.csproj | Build configuration, project type GUIDs, IIS Express settings |

No Spring Cloud Config, Azure App Configuration, Consul KV, HashiCorp Vault, Azure KeyVault, or AWS Secrets Manager integrations are present.

## Build Profiles

| Profile | Activation | Purpose | Key Dependencies/Plugins |
|---------|-----------|---------|--------------------------|
| Debug | Default (no flag) | Development build with debug symbols | `<DebugSymbols>true</DebugSymbols>`, `<Optimize>false</Optimize>` |
| Release | Manual (`-p:Configuration=Release`) | Production build with optimizations | `<Optimize>true</Optimize>`, PDB-only symbols |

No Maven-style multi-profile setup. The project has two standard MSBuild configurations (Debug/Release) defined in the `.csproj`.

## Runtime Profiles

| Profile | Activation Method | Config Files | Key Overrides |
|---------|------------------|-------------|---------------|
| Default | Always (single environment) | Web.config | No environment-specific overrides exist |

No `appsettings.Development.json`, `appsettings.Production.json`, or `application-{profile}.yml` equivalents. All environments share the same `Web.config`. There is no `ASPNETCORE_ENVIRONMENT` or `DOTNET_ENVIRONMENT` variable usage.

## Properties Inventory

### ContosoUniversity

| Property Key | Default Value | Profiles | Source |
|-------------|--------------|----------|--------|
| ConnectionStrings:DefaultConnection | `Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=ContosoUniversityNoAuthEFCore;Integrated Security=True;MultipleActiveResultSets=True` | All | Web.config `<connectionStrings>` |
| webpages:Version | `3.0.0.0` | All | Web.config `<appSettings>` |
| webpages:Enabled | `false` | All | Web.config `<appSettings>` |
| ClientValidationEnabled | `true` | All | Web.config `<appSettings>` |
| UnobtrusiveJavaScriptEnabled | `true` | All | Web.config `<appSettings>` |
| NotificationQueuePath | `.\Private$\ContosoUniversityNotifications` | All | Web.config `<appSettings>` |
| system.web/compilation/@debug | `true` | All | Web.config `<system.web>` |
| system.web/httpRuntime/@targetFramework | `4.8` | All | Web.config `<system.web>` |
| system.web/httpRuntime/@maxRequestLength | `10240` (KB = 10 MB) | All | Web.config `<system.web>` |
| system.web/httpRuntime/@executionTimeout | `3600` (seconds) | All | Web.config `<system.web>` |
| system.webServer/requestFiltering/maxAllowedContentLength | `10485760` (bytes = 10 MB) | All | Web.config `<system.webServer>` |

## Startup Parameters & Resource Requirements

| Service | Runtime Options | Memory | Instance Count |
|---------|----------------|--------|---------------|
| ContosoUniversity (IIS Express) | .NET Framework 4.8 CLR, IIS Express w/ Windows Auth enabled | Not specified | 1 (single IIS Express instance) |

No JVM heap settings (not Java). No Docker container resource limits. No Kubernetes resource requests/limits. IIS Express is used for local development only.

## Startup Dependency Chain

1. **IIS Express starts** → loads the ASP.NET MVC 5 application
2. **`MvcApplication.Application_Start()`** (Global.asax.cs) runs:
   - `AreaRegistration.RegisterAllAreas()`
   - `FilterConfig.RegisterGlobalFilters()`
   - `RouteConfig.RegisterRoutes()` (default route: `{controller}/{action}/{id}`)
   - `BundleConfig.RegisterBundles()` (jQuery, Bootstrap, Modernizr bundles)
   - `InitializeDatabase()` — reads connection string, creates `SchoolContext`, calls `DbInitializer.Initialize()` which calls `EnsureCreated()` and seeds data if empty
3. **First HTTP request** → controller instantiates `SchoolContextFactory.Create()` and `NotificationService()` (opens MSMQ queue)

No explicit health checks, readiness probes, or wait-for mechanisms are configured.

## Secrets & Sensitive Configuration

| Secret Reference | Type | Storage |
|-----------------|------|---------|
| ConnectionStrings:DefaultConnection | Database connection string | Web.config (plaintext) — uses Windows Integrated Security, no password |
| NotificationQueuePath | MSMQ queue path | Web.config (plaintext) — local path, not a secret |

The database uses Windows Integrated Security (no password in the connection string). However, in a cloud deployment, a proper connection string with a service account password or managed identity would be required.

### Secrets Provisioning Workflow

No secrets provisioning workflow exists. All configuration is in the plaintext `Web.config` file checked into source control. There is no Key Vault integration, no environment variable substitution, and no encrypted properties. For cloud deployment, the connection string and any future API keys must be migrated to Azure Key Vault or environment variables, using managed identity authentication.

## Feature Flags

No feature flag framework (LaunchDarkly, Unleash, .NET FeatureManagement) is used. No `[ConditionalOnProperty]` or custom toggle patterns are present.

| Flag Name | Default | Controlled By |
|-----------|---------|---------------|
| webpages:Enabled | false | Web.config (disables WebMatrix WebPages) |
| ClientValidationEnabled | true | Web.config (enables jQuery client-side validation) |
| UnobtrusiveJavaScriptEnabled | true | Web.config (enables unobtrusive JS for validation) |

## Framework & Runtime Versions

| Component | Version | Source |
|-----------|---------|--------|
| .NET Framework | 4.8 | ContosoUniversity.csproj `<TargetFrameworkVersion>v4.8</TargetFrameworkVersion>` |
| ASP.NET MVC | 5.2.9 | packages.config `Microsoft.AspNet.Mvc` |
| ASP.NET Razor | 3.2.9 | packages.config `Microsoft.AspNet.Razor` |
| ASP.NET WebPages | 3.2.9 | packages.config `Microsoft.AspNet.WebPages` |
| Entity Framework Core | 3.1.32 | packages.config `Microsoft.EntityFrameworkCore` |
| EF Core SQL Server Provider | 3.1.32 | packages.config `Microsoft.EntityFrameworkCore.SqlServer` |
| Microsoft.Data.SqlClient | 2.1.4 | packages.config |
| Newtonsoft.Json | 13.0.3 | packages.config |
| Bootstrap | 5.3.3 | packages.config |
| jQuery | 3.7.1 | packages.config |
| Microsoft.Identity.Client (MSAL) | 4.21.1 | packages.config |
| MSBuild | 15.0 (ToolsVersion) | ContosoUniversity.csproj |
| NuGet | packages.config format | packages.config |
| IIS Express | Local development | ContosoUniversity.csproj `<UseIISExpress>true</UseIISExpress>` |
