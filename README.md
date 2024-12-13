# Expand World Code

Allows adding custom functions that can be executed by Expand World Prefabs.

Install on the server (modding [guide](https://youtu.be/L9ljm2eKLrk)).

## Features

Expand World Prefabs automatically compiles all ".cs" files in the "/config/expand_world" folder.

Each public and static function is converted to a parameter with the same name as the function.

Note: This is still a server side mod, you can't run code on the player clients.

## Basic example

`sum.cs`

```csharp
public class MyMath {
  public static int Sum(int a, int b) {
    return a + b;
  }
}
```

Then this function can be used in the yaml files by writing `<Sum_1_2>` which will be replaced by `3`.

## Persistent data

Expand World Prefabs provides a CodeLoading class to store data between game sessions, with following functions:

- SetBool, SetFloat, SetInt, SetLong, SetString
- GetBool, GetFloat, GetInt, GetLong, GetString

All data is saved as key-value pairs in the `config/expand_world/ewp_data.yaml` file.

All data is saved as text with automatic conversion to the correct type.

`dataLoading.cs`

```csharp
using ExpandWorld.Code;

public class ValueLoading {
  // <Set_KEY_VALUE>
  public static void Set(string key, int value) {
    SavedData.SetInt(key, value);
  }
  // <Get_KEY>
  public static string Get(string key) {
    return SavedData.GetString(key);
  }
}
```

## Advanced example

This function can be used to return the weather for a specific biome and day. For example `<Forecast_Meadows_10>`.

`forecast.cs`

```csharp
using System;

public class ForecastClass
{
  public static string Forecast(string biome, float day)
  {
    if (!Enum.TryParse<Heightmap.Biome>(biome, true, out var b))
      return "Invalid biome";
    var env = EnvMan.instance;
    var setup = env.GetAvailableEnvironments(b);
    if (setup == null)
      return "No weather";
    UnityEngine.Random.State state = UnityEngine.Random.state;
    var period = (int)(day * env.m_dayLengthSec / env.m_environmentDuration);
    UnityEngine.Random.InitState(period);
    EnvSetup envSetup = env.SelectWeightedEnvironment(setup);
    UnityEngine.Random.state = state;
    return envSetup.m_name;
  }
}
```

## Patching functions

HarmonyLib can be used manually to patch existing functions.

Note: Unsure what happens to already patched functions when changing the code files. Most likely you need to restart the game if things start working weird.

```csharp
// Expand World Prefabs has a Harmony instance that can be used to patch functions.
using ExpandWorld.Prefab;
// AccessTools is used to get method references.
using HarmonyLib;

public class EventDisabler
{
  // Track state to avoid unnecessary patching.
  private static bool Disabled = false;

  public static void DisableEvents()
  {
    if (Disabled) return;
    Disabled = true;
    var method = AccessTools.Method(typeof(RandEventSystem), nameof(RandEventSystem.StartRandomEvent));
    var patch = AccessTools.Method(typeof(EventDisabler), nameof(Disabler));
    EWP.Harmony.Patch(method, prefix: new HarmonyMethod(patch));
  }
  public static void EnableEvents()
  {
    if (!Disabled) return;
    Disabled = false;
    var method = AccessTools.Method(typeof(RandEventSystem), nameof(RandEventSystem.StartRandomEvent));
    var patch = AccessTools.Method(typeof(EventDisabler), nameof(Disabler));
    EWP.Harmony.Unpatch(method, patch);
  }

  // Returning false in a prefix will prevent the original function from running.
  private static bool Disabler()
  {
    return false;
  }
}
```

## Credits

Thanks for Azumatt for creating the mod icon!

Sources: [GitHub](https://github.com/JereKuusela/valheim-expand_world_code)
Donations: [Buy me a computer](https://www.buymeacoffee.com/jerekuusela)
