# JacobDixon.AspNetCore.LiveSassCompile
Compiles Sass/Scss when files change while the server is running.

## How To Use
Add the NuGet package JacobDixon.AspNetCore.LiveSassCompile to your project. [See NuGet.org for more info on installing the package.](https://www.nuget.org/packages/JacobDixon.AspNetCore.LiveSassCompile/)

Add services.AddLiveSassCompile() to the ConfigureServices method inside Startup.cs
```
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddLiveSassCompile();
}
```

Configure the options in appsettings.json

## Dependencies

The project depends on [LibHostSass](https://github.com/Taritsyn/LibSassHost). By default the win-x64 bit is included but other OS support is available from the [LibHostSass](https://github.com/Taritsyn/LibSassHost) repository.

## Options

The available options are:
```
"LiveSassCompileOptions": {
    "EnableLiveCompile": true,
    "SassFileWatchers": [
        {
            "SourcePath": "src\\sass",
            "DestinationPath": "wwwroot\\css",
            "FileNameFilters": [
                "*.sass",
                "*.scss"
            ],
            "FileNameExclusions": [
                "_*"
            ],
            "CompileOnStart": true
        }
    ]
}
```

### LiveCompileEnabled
A boolean value which controls whether live compile is on (true) or off (false).
Default: false

### SassFileWatchers
The folders to monitor for sass/scss file changes and the matching destination folders.
Default: []

The following options are available for each SassFileWatcher:

#### SourcePath
The source path to watch for file changes. Can be a file or directory.

#### DestinationPath
The destination path to write compiles css files out to. Must be a directory.
Default: "wwwroot\css"

#### CompileOnStart
Compile the sass file(s) after starting to watch for changes
Default: true

#### FileNameFilters
The file extensions to watch for changes. 
Accepts an array of glob patterns
Default: \[ "\*.scss", "\*.sass" \]

#### FileNameExclusions
The file name patters to exclude from compiling. 
Accepts an array of glob patterns.
Default: \[ "_\*" \]

## Contributing

All contributions are welcome.