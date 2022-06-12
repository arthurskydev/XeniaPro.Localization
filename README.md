# XeniaPro.Localization

# Simple .Net library that gets localized strings from JSON files

## Installation & usage with Blazor
    
    dotnet add package XeniaPro.Localization.Core

In Program.cs (Possibly Startup.cs in Blazor Server) add following lines:
```csharp
builder.Services.AddXeniaLocalization(options =>
{
    // If the locale string can not be found use this value instead. A "." would pass the key through.
    options.PlaceholderString = "";
    
    /*
     * Preferably load this from a configuration file.
     * 
     * The fist language will be picked as default.
     *
     * Make sure to manually cache the selected language on your client.
     */
    options.Languages = new List<Language>
    {
        "de-DE, de",
        "en-EN, en"
    };
});
```

### Blazor WASM : Get locales from web

If you are using Blazor WASM you might want to fetch the locale files from somewhere

    dotnet add package XeniaPro.Localization.Web
    
Find where you configure XeniePro.Localization and add:
```csharp
builder.Services.AddXeniaLocalization(options =>
{

  // ...
        
  options.UseWebLocalization(webOptions =>
  {
    // Place your locale files in a directory "locales" in "wwwroot".
    webOptions.ResourceUrl = $"{builder.HostEnvironment.BaseAddress}/locales/";
  });
});
```

### Blazor Server : Get locales from filesystem

If you are using Blazor Server you might want to get the locale files from your filesystem

    dotnet add package XeniaPro.Localization.Files
    
Find where you configure XeniePro.Localization and add:
```csharp
builder.Services.AddXeniaLocalization(options =>
{

  // ...
        
  options.UseFileLocalization(fileOptions =>
  {
    // Place your locale files in a directory "locales" in your project root.
    fileOptions.ResourcePath = $"{Directory.GetCurrentDirectory()}/locales/";
  });
});
```
                  
