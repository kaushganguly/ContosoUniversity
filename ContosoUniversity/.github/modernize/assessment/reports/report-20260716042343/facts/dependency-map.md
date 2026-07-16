# Dependency Map

ContosoUniversity declares a moderate dependency set centered on ASP.NET MVC, Entity Framework Core, and SQL Server integration. The project has roughly 19 non-test external library groups and no declared automated test dependencies.

## Dependencies

```mermaid
flowchart LR
    App["ContosoUniversity"]

    subgraph Web["Web Frameworks"]
        MVC["ASP.NET MVC 5.2.9"]
        Razor["ASP.NET Razor 3.2.9"]
        WebPages["ASP.NET WebPages 3.2.9"]
        Opt["Web Optimization 1.1.3"]
    end
    subgraph DB["Database / ORM"]
        EFCore["EF Core 3.1.32"]
        EFRel["EF Core Relational 3.1.32"]
        EFSql["EF Core SqlServer 3.1.32"]
        SqlClient["Microsoft.Data.SqlClient 2.1.4"]
    end
    subgraph Messaging["Messaging"]
        MSMQRef["System.Messaging"]
    end
    subgraph Cache["Caching"]
        MemCache["Extensions MemoryCache 3.1.32"]
    end
    subgraph Log["Logging"]
        ExtLog["Extensions Logging 3.1.32"]
    end
    subgraph Sec["Security"]
        Identity["Microsoft.Identity.Client 4.21.1"]
    end
    subgraph Obs["Observability"]
        Diag["DiagnosticSource 4.7.1"]
    end
    subgraph Util["Utilities"]
        Json["Newtonsoft.Json 13.0.3"]
        DI["Extensions DI 3.1.32"]
        Config["Extensions Configuration 3.1.32"]
        Async["AsyncInterfaces 1.1.1"]
        Immutable["Collections.Immutable 1.7.1"]
        Buffers["System.Buffers 4.5.1"]
        NetStd["NETStandard.Library 2.0.3"]
        WebGrease["WebGrease 1.5.2"]
    end
    subgraph Frontend["Client Libraries"]
        JQuery["jQuery 3.7.1"]
        JQueryVal["jQuery.Validation 1.21.0"]
        Bootstrap["Bootstrap 5.3.3"]
        Modernizr["Modernizr 2.6.2"]
    end

    App -->|"web"| Web
    App -->|"persistence"| DB
    App -->|"notifications"| Messaging
    App -->|"caching"| Cache
    App -->|"logging"| Log
    App -->|"security"| Sec
    App -->|"diagnostics"| Obs
    App -->|"utilities"| Util
    App -->|"ui"| Frontend
    EFCore -.->|"provider"| EFSql
    EFCore -.->|"relational APIs"| EFRel
```

### Dependency Summary

| Category | Count | Key Libraries | Notes |
|---|---:|---|---|
| Web Frameworks | 4 | ASP.NET MVC, Razor, WebPages, Web Optimization | Legacy MVC stack hosted on .NET Framework |
| Database / ORM | 4 | EF Core, EF Core SqlServer, SqlClient | Modern ORM layered onto classic MVC app |
| Messaging | 1 | System.Messaging | Relies on Windows MSMQ, which is not cross-platform |
| Caching | 1 | Microsoft.Extensions.Caching.Memory | Declared dependency; no significant cache usage found in code |
| Logging | 1 | Microsoft.Extensions.Logging | Package is present but app mainly uses Debug output |
| Security | 1 | Microsoft.Identity.Client | Present as dependency but not actively integrated into request pipeline |
| Observability | 1 | System.Diagnostics.DiagnosticSource | Diagnostic support through supporting libraries |
| Utilities | 8 | Newtonsoft.Json, DI, Configuration, WebGrease | Includes compatibility and infrastructure libraries |
| Client Libraries | 4 | jQuery, Validation, Bootstrap, Modernizr | Traditional browser-side MVC assets |

### Version & Compatibility Risks

The dependency set combines ASP.NET MVC 5 on .NET Framework 4.8 with EF Core 3.1.x, which is out of support and introduces modernization friction. MSMQ and System.Web-based packages are tightly coupled to Windows hosting, while the hybrid use of Microsoft.Extensions packages and legacy WebApplication targets creates upgrade and cross-platform build risks.

### Notable Observations

- No automated test framework packages are declared, so dependency changes cannot be validated through existing unit or integration tests.
- The repository includes a Bootstrap 5 package reference while the checked-in site assets still reflect a traditional MVC bundling model, indicating some client-library drift.
- `Microsoft.Identity.Client` is referenced even though authentication and authorization are not actively enforced in the controller pipeline.
- `NETStandard.Library` and multiple compatibility packages are needed to support EF Core and Extensions libraries on .NET Framework.

## Test Dependencies

| Framework | Version | Notes |
|---|---|---|
| None detected | N/A | No xUnit, NUnit, MSTest, or mocking/assertion packages declared |

Total test-scope dependencies: 0
No test dependencies detected. The repository does not contain a test project or test-scoped package declarations, so modernization work will rely on manual verification unless test infrastructure is introduced separately.
