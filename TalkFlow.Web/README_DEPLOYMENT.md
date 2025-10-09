# TalkFlow - Azure Deployment Guide

## Prerequisites
- Azure subscription
- Azure CLI installed
- .NET 8.0 SDK installed

## Configuration Steps

### 1. Update Production Settings
Edit `appsettings.Production.json`:
```json
{
  "APIUrl": "https://your-actual-api-url.azurewebsites.net/"
}
```

### 2. Azure Web App Configuration

#### Application Settings (Environment Variables)
Add these in Azure Portal → Configuration → Application Settings:
- `ASPNETCORE_ENVIRONMENT` = `Production`
- `APIUrl` = `https://your-actual-api-url.azurewebsites.net/`

#### General Settings
- **Stack**: .NET
- **Version**: .NET 8 (LTS)
- **Platform**: 64-bit
- **Always On**: Enable (for production)
- **HTTPS Only**: Enable

### 3. Build & Publish

#### Option A: Visual Studio
1. Right-click project → Publish
2. Select Azure → Azure App Service (Windows/Linux)
3. Choose your subscription and app service
4. Click Publish

#### Option B: Azure CLI
```bash
# Login to Azure
az login

# Build the project
dotnet publish -c Release

# Deploy to Azure (replace placeholders)
az webapp deployment source config-zip \
  --resource-group <your-resource-group> \
  --name <your-app-name> \
  --src <path-to-zip-file>
```

#### Option C: GitHub Actions
Create `.github/workflows/azure-deploy.yml`:
```yaml
name: Deploy to Azure Web App

on:
  push:
    branches: [ main ]
  workflow_dispatch:

jobs:
  build-and-deploy:
    runs-on: windows-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Build
      run: dotnet build --configuration Release
      working-directory: ./TalkFlow.Web
    
    - name: Publish
      run: dotnet publish -c Release -o ${{env.DOTNET_ROOT}}/myapp
      working-directory: ./TalkFlow.Web
    
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'your-app-name'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/myapp
```

### 4. Post-Deployment Checks

1. **CORS Settings** (if API is on different domain):
   - Azure Portal → API App → CORS
   - Add your web app URL

2. **SSL Certificate**:
   - Azure Portal → Custom domains → Add custom domain
   - Enable HTTPS redirect

3. **Monitoring**:
   - Enable Application Insights
   - Set up alerts for errors/performance

### 5. Environment-Specific Features

#### Development
- Debug logging enabled
- Detailed error pages
- Local API URL

#### Production
- Minimal logging
- Generic error pages
- Azure API URL
- HTTPS enforcement
- HSTS enabled

## Troubleshooting

### Issue: API Connection Failed
- Check `APIUrl` in Application Settings
- Verify API is running and accessible
- Check CORS settings on API

### Issue: 500 Internal Server Error
- Check Azure logs: Portal → App Service → Log Stream
- Enable detailed errors temporarily in `web.config`
- Check Application Insights for exceptions

### Issue: Static Files Not Loading
- Verify `web.config` is deployed
- Check file paths are correct
- Clear browser cache

## Performance Optimization

1. **Enable Response Compression**:
   Add in `Program.cs`:
   ```csharp
   builder.Services.AddResponseCompression();
   ```

2. **Enable CDN for static files** (optional):
   - Azure Portal → CDN → Add CDN profile
   - Point to your web app

3. **Scale Up/Out**:
   - Scale Up: Increase instance size
   - Scale Out: Add more instances

## Security Checklist

- ✅ HTTPS enforced
- ✅ HSTS enabled
- ✅ Secure cookies
- ✅ API keys in Azure Key Vault (recommended)
- ✅ CORS configured properly
- ✅ Authentication/Authorization configured
- ✅ SQL injection prevention (parameterized queries)
- ✅ XSS prevention (proper encoding)

## Monitoring URLs

- **Application**: https://your-app-name.azurewebsites.net
- **Azure Portal**: https://portal.azure.com
- **Kudu (SCM)**: https://your-app-name.scm.azurewebsites.net

## Support

For issues related to:
- **Azure Deployment**: Check Azure documentation
- **Application Bugs**: Check application logs
- **API Issues**: Check API service status
