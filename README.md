# Hermes (.NET)

Configuration manager for .NET services.
This module can be used to find and load configuration files based on `MICROSERVICE_ENV` (--env) environment variable.
Hermes looks for specified config file in entire CONFIG_PATH and loads an appropriate one.

## Installation

To install Armada.net and add it to your project simply use Nuget Manager if using Visual Studio and type:

```posh
Install-Package Armada.net 
```

Or add dependency to your `project.json` if using .NET Core (VSCode or CLI):

```json
  "dependencies": {
    "Armada.net": "1.0.0"
  }
```

## Usage

Import package:

```csharp
using Armada;
```

Retrieve configuration specific to current environment (let's assume configuration file is named `config.json`):

```csharp
var mainConfig = Hermes.GetConfig("config.json");
```

For instance `config.json` could be structured as follows:

```json
{
    "api_secret": "123412341234",
    "db": {
        "url": "192.168.0.1",
        "port": 3306
    }
}
```

Then in our code we can access these values using following methods:

```csharp
var apiSecret = mainConfig.GetValue<string>("api_secret");

var dbPort = mainConfig["db"].GetValue<int>("port");
```

Attempt to access not defined property like:

```csharp
var dummyEntry = mainConfig.GetValue<string>("dummy_thing");
```

Will result with `ArgumentException` being thrown. If you want to retrieve values that might be optional in config file, simply pass default value to `GetValue` method (this way you may also ommit type specifier as it can be inferred from second argument).

```csharp
var dummyEntry = mainConfig.GetValue("dummy_thing", "optional_value");
```

To check if it's safe to access optional configuration section you can use `Contains` method:

```csharp
var dummySection = mainConfig["dummy"]; //throws ArgumentException
mainConfig.Contains("dummy"); //returns false
mainConfig.Contains("db"); //returns true
mainConfig.Contains("api_secret"); //returns true
```