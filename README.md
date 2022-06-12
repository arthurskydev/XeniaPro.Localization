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

### Blazor WASM - Get locales from web

If you are using Blazor WASM you might want to fetch the locale files from somewhere. We will not fetch locale files until they are used.

    dotnet add package XeniaPro.Localization.Web
    
Find where you configure XeniePro.Localization and add
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

### Blazor Server - Get locales from filesystem

If you are using Blazor Server you might want to get the locale files from your filesystem at application startup.

    dotnet add package XeniaPro.Localization.Files
    
Find where you configure XeniePro.Localization and add
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
                  
### Create a locale file

We specified the language "de-De" for locale file "de"
Go ahead and create a file `de.json`

```javascript
{
  "HelloWorld": "Hello World!",
  "Welcome": "Welcome to your new app!",
  "HowIsBlazor": "How is Blazor working for you?",
  "Home": "Home",
  "Counter": "Counter",
  "FetchData": "Fetch data",
  "TakeSurvey": "Please take our {Survey:brief survey}",
  "CurrentCount": "Current count",
  "ClickMe": "Click me!",
  "WeatherForecast": "Weather Forecast",
  "About": "About",
  "WeatherForecastExplained": "This component demonstrates fetching data from the server.",
  "Date": "Date",
  "Temp": "Temperature",
  "Summary": "Summary"
}
```

Notice that we support interpolation: `{Survey:brief survey}`

| Key   | Value           |
| ----- | --------------- |
|Survey |brief survey     |

You can also nest them: ``{This:is{A:brief}survey}``

| Key | Value |
| --- | ----- |
|This |Is     |
|A    |Brief  |

In order to escape `{` or `}` place two or more in a squence.

Keep in mind that if you intentionally use brackets in squence and do not want them to escape eachother, divide them using the `@` symbol. `@` is escaped using the earlier principle.

An example: `{Brief:brief {Survey:survey}@}`

Text that is around the nested strings is called ***skeleton***. You can access parts of it similar to they way you access an array.

| `Please take our`       | `{Survey:brief survey}`          |
| ----------------------- | -------------------------------- |
| Skeleton with index `0` | Nested string with key `Survey`  |

### Usage in Blazor components

Make sure that the desired comonent inherits `LocalizedComponentBase`
```razor
@inherits LocalizedComponentBase
```

You can now use the `Localizer` service in your component
```razor
<h1>@Localizer["HelloWorld"]</h1>
```
And pass paramters to access nested strings
```razor
<div>
    <span>
        @Localizer["TakeSurvey", "0"]
        <a href="https://go.microsoft.com/fwlink/?linkid=2148851">@Localizer["TakeSurvey", "Survey"]</a>
    </span>
    @Localizer["TakeSurvey", "1"]
</div>
```
A numeric paramter accesses a part of the "skeleton" and a text-style string will serve as the key for a string that was declared using the `{Key:Value}` notaion. Feel free to chain these parameters to reach nested strings and their skeletons.

Notice that the skeleton is 0 indexed.

# Feedback, feature and pull requests

I will *really* appreceate your feedback. Feel free to tell me what I am doing wrong and what I can improve.

Feature requests are appreciated and might be implemented if demand is there.

You have time to spare? Sure do a pull request! But please tell me what you are working on beforehand.

## Contact
<a href="https://discordapp.com/users/764794958955544586/" title="arthursky#9413">
    <img src="https://brandslogos.com/wp-content/uploads/thumbs/discord-logo-vector.svg" alt="discord-logo" width="50"/>
</a>
