## **Danh sách câu lệnh CLI cài đặt packages**

### **1. Domain Layer (TalkFlow.Domain)**
```bash
cd src/TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
```

### **2. Application Layer (TalkFlow.Application)**
```bash
cd src/TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
```

### **3. Infrastructure Layer (TalkFlow.Infrastructure)**
```bash
cd src/TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
```

### **4. Presentation Layer (TalkFlow.Presentation)**
```bash
cd src/TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
```

### **5. Shared Layer (TalkFlow.Shared)**
```bash
cd src/TalkFlow.Shared
# Không cần packages bổ sung - chỉ chứa utilities và constants
```

### **6. Unit Tests (TalkFlow.UnitTests)**
```bash
cd tests/TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
```

### **7. Integration Tests (TalkFlow.IntegrationTests)**
```bash
cd tests/TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
```

### **8. Architecture Tests (TalkFlow.ArchitectureTests)**
```bash
cd tests/TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
```

## **Script tổng hợp để chạy tất cả:**

Tạo file `install-packages.sh` (Linux/Mac) hoặc `install-packages.bat` (Windows):

### **Linux/Mac (install-packages.sh):**
```bash
#!/bin/bash

echo "Installing packages for TalkFlow DDD project..."

# Domain Layer
echo "Installing Domain Layer packages..."
cd src/TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
cd ../..

# Application Layer
echo "Installing Application Layer packages..."
cd src/TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
cd ../..

# Infrastructure Layer
echo "Installing Infrastructure Layer packages..."
cd src/TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
cd ../..

# Presentation Layer
echo "Installing Presentation Layer packages..."
cd src/TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
cd ../..

# Unit Tests
echo "Installing Unit Tests packages..."
cd tests/TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
cd ../..

# Integration Tests
echo "Installing Integration Tests packages..."
cd tests/TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
cd ../..

# Architecture Tests
echo "Installing Architecture Tests packages..."
cd tests/TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
cd ../..

echo "All packages installed successfully!"
echo "Running dotnet restore..."
dotnet restore

echo "Building solution..."
dotnet build

echo "Running tests..."
dotnet test
```

### **Windows (install-packages.bat):**
```batch
@echo off
echo Installing packages for TalkFlow DDD project...

REM Domain Layer
echo Installing Domain Layer packages...
cd src\TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
cd ..\..

REM Application Layer
echo Installing Application Layer packages...
cd src\TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
cd ..\..

REM Infrastructure Layer
echo Installing Infrastructure Layer packages...
cd src\TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
cd ..\..

REM Presentation Layer
echo Installing Presentation Layer packages...
cd src\TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
cd ..\..

REM Unit Tests
echo Installing Unit Tests packages...
cd tests\TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
cd ..\..

REM Integration Tests
echo Installing Integration Tests packages...
cd tests\TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
cd ..\..

REM Architecture Tests
echo Installing Architecture Tests packages...
cd tests\TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
cd ..\..

echo All packages installed successfully!
echo Running dotnet restore...
dotnet restore

echo Building solution...
dotnet build

echo Running tests...
dotnet test
```

##    **Các câu lệnh bổ sung:**

### **Restore và Build:**
```bash
# Restore tất cả packages
dotnet restore

# Build toàn bộ solution
dotnet build

# Build với configuration Release
dotnet build --configuration Release

# Clean và rebuild
dotnet clean
dotnet build
```

### **Chạy tests:**
```bash
# Chạy tất cả tests
dotnet test

# Chạy tests với coverage
dotnet test --collect:"XPlat Code Coverage"

# Chạy tests cụ thể
dotnet test tests/TalkFlow.UnitTests
dotnet test tests/TalkFlow.IntegrationTests
dotnet test tests/TalkFlow.ArchitectureTests
```

### **Chạy ứng dụng:**
```bash
# Chạy Presentation layer
cd src/TalkFlow.Presentation
dotnet run

# Chạy với specific environment
dotnet run --environment Development
dotnet run --environment Production
```

### **EF Core Migrations:**
```bash
# Tạo migration mới
cd src/TalkFlow.Presentation
dotnet ef migrations add InitialCreate --project ../TalkFlow.Infrastructure

# Update database
dotnet ef database update

# Remove migration
dotnet ef migrations remove
```

Tất cả các packages này đều tương thích với .NET 8.0 và global.json 8.0.410 của bạn!

```shellscript
cd src/TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
```

```shellscript
cd src/TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
```

```shellscript
cd src/TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
```

```shellscript
cd src/TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
```

```shellscript
cd src/TalkFlow.Shared
# Không cần packages bổ sung - chỉ chứa utilities và constants
```

```shellscript
cd tests/TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
```

```shellscript
cd tests/TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
```

```shellscript
cd tests/TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
```

```shellscript
#!/bin/bash

echo "Installing packages for TalkFlow DDD project..."

# Domain Layer
echo "Installing Domain Layer packages..."
cd src/TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
cd ../..

# Application Layer
echo "Installing Application Layer packages..."
cd src/TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
cd ../..

# Infrastructure Layer
echo "Installing Infrastructure Layer packages..."
cd src/TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
cd ../..

# Presentation Layer
echo "Installing Presentation Layer packages..."
cd src/TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
cd ../..

# Unit Tests
echo "Installing Unit Tests packages..."
cd tests/TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
cd ../..

# Integration Tests
echo "Installing Integration Tests packages..."
cd tests/TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
cd ../..

# Architecture Tests
echo "Installing Architecture Tests packages..."
cd tests/TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
cd ../..

echo "All packages installed successfully!"
echo "Running dotnet restore..."
dotnet restore

echo "Building solution..."
dotnet build

echo "Running tests..."
dotnet test
```

```bat
@echo off
echo Installing packages for TalkFlow DDD project...

REM Domain Layer
echo Installing Domain Layer packages...
cd src\TalkFlow.Domain
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
cd ..\..

REM Application Layer
echo Installing Application Layer packages...
cd src\TalkFlow.Application
dotnet add package MediatR --version 12.2.0
dotnet add package FluentValidation --version 11.8.1
dotnet add package FluentValidation.DependencyInjectionExtensions --version 11.8.1
dotnet add package AutoMapper --version 12.0.1
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 12.0.1
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
cd ..\..

REM Infrastructure Layer
echo Installing Infrastructure Layer packages...
cd src\TalkFlow.Infrastructure
dotnet add package Microsoft.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 8.0.0
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 8.0.0
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer --version 8.0.0
dotnet add package System.IdentityModel.Tokens.Jwt --version 7.0.3
dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis --version 8.0.0
dotnet add package Serilog.AspNetCore --version 8.0.0
dotnet add package Serilog.Sinks.Console --version 5.0.1
dotnet add package Serilog.Sinks.File --version 5.0.0
cd ..\..

REM Presentation Layer
echo Installing Presentation Layer packages...
cd src\TalkFlow.Presentation
dotnet add package Microsoft.AspNetCore.OpenApi --version 8.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package Microsoft.AspNetCore.SignalR --version 1.1.0
dotnet add package Microsoft.AspNetCore.Cors --version 2.2.0
cd ..\..

REM Unit Tests
echo Installing Unit Tests packages...
cd tests\TalkFlow.UnitTests
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
dotnet add package coverlet.collector --version 6.0.0
dotnet add package Moq --version 4.20.70
dotnet add package NetArchTest.Rules --version 1.3.0
cd ..\..

REM Integration Tests
echo Installing Integration Tests packages...
cd tests\TalkFlow.IntegrationTests
dotnet add package Microsoft.AspNetCore.Mvc.Testing --version 8.0.8
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
dotnet add package FluentAssertions --version 6.12.0
cd ..\..

REM Architecture Tests
echo Installing Architecture Tests packages...
cd tests\TalkFlow.ArchitectureTests
dotnet add package NetArchTest.Rules --version 1.3.2
dotnet add package xunit --version 2.9.0
dotnet add package xunit.runner.visualstudio --version 2.8.2
cd ..\..

echo All packages installed successfully!
echo Running dotnet restore...
dotnet restore

echo Building solution...
dotnet build

echo Running tests...
dotnet test
```

```shellscript
# Restore tất cả packages
dotnet restore

# Build toàn bộ solution
dotnet build

# Build với configuration Release
dotnet build --configuration Release

# Clean và rebuild
dotnet clean
dotnet build
```

```shellscript
# Chạy tất cả tests
dotnet test

# Chạy tests với coverage
dotnet test --collect:"XPlat Code Coverage"

# Chạy tests cụ thể
dotnet test tests/TalkFlow.UnitTests
dotnet test tests/TalkFlow.IntegrationTests
dotnet test tests/TalkFlow.ArchitectureTests
```

```shellscript
# Chạy Presentation layer
cd src/TalkFlow.Presentation
dotnet run

# Chạy với specific environment
dotnet run --environment Development
dotnet run --environment Production
```

```shellscript
# Tạo migration mới
cd src/TalkFlow.Presentation
dotnet ef migrations add InitialCreate --project ../TalkFlow.Infrastructure

# Update database
dotnet ef database update

# Remove migration
dotnet ef migrations remove
```


