

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Service;

namespace ExpandWorld.Code;

public class SavedData
{

  private static Dictionary<string, string> Database = [];

  public static void LoadSavedData()
  {
    if (UnsavedChanges) return;
    if (!Directory.Exists(Yaml.BaseDirectory))
      Directory.CreateDirectory(Yaml.BaseDirectory);
    if (!File.Exists(SavedDataFile)) return;
    var data = File.ReadAllText(SavedDataFile);
    Database = Yaml.DeserializeData(data);
    Log.Info($"Reloaded saved data ({Database.Count} entries).");
  }
  private static bool UnsavedChanges = false;
  private static long LastSave = 0;
  private static readonly string SavedDataFile = Path.Combine(Yaml.BaseDirectory, "ewp_data.yaml");
  public static void SaveSavedData()
  {
    if (!UnsavedChanges) return;
    // Save every 10 seconds at most.
    if (DateTime.Now.Ticks - LastSave < 10000000) return;
    LastSave = DateTime.Now.Ticks;
    if (!Directory.Exists(Yaml.BaseDirectory))
      Directory.CreateDirectory(Yaml.BaseDirectory);
    var yaml = Yaml.SerializeData(Database);
    File.WriteAllText(SavedDataFile, yaml);
    UnsavedChanges = false;
  }

  public static string GetString(string key, string defaultValue = "") => Database.TryGetValue(key, out var value) ? value : defaultValue;
  public static int GetInt(string key, int defaultValue = 0) => Database.TryGetValue(key, out var value) ? Parse.Int(value) : defaultValue;
  public static float GetFloat(string key, float defaultValue = 0f) => Database.TryGetValue(key, out var value) ? Parse.Float(value) : defaultValue;
  public static bool GetBool(string key, bool defaultValue = false) => Database.TryGetValue(key, out var value) ? bool.Parse(value) : defaultValue;
  public static long GetLong(string key, long defaultValue = 0) => Database.TryGetValue(key, out var value) ? Parse.Long(value) : defaultValue;
  public static void SetInt(string key, int value)
  {
    Database[key] = value.ToString(CultureInfo.InvariantCulture);
    UnsavedChanges = true;
  }
  public static void SetFloat(string key, float value)
  {
    Database[key] = value.ToString(CultureInfo.InvariantCulture);
    UnsavedChanges = true;
  }
  public static void SetString(string key, string value)
  {
    Database[key] = value;
    UnsavedChanges = true;
  }
  public static void SetBool(string key, bool value)
  {
    Database[key] = value.ToString();
    UnsavedChanges = true;
  }
  public static void SetLong(string key, long value)
  {
    Database[key] = value.ToString(CultureInfo.InvariantCulture);
    UnsavedChanges = true;
  }

  public static void SetupWatcher()
  {
    if (!Directory.Exists(Yaml.BaseDirectory))
      Directory.CreateDirectory(Yaml.BaseDirectory);
    Yaml.SetupWatcher(Yaml.BaseDirectory, "ewp_data.yaml", LoadSavedData);
    LoadSavedData();
  }
}
