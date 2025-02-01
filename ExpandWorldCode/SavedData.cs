

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExpandWorld.Prefab;
using Service;

namespace ExpandWorld.Code;

public class SavedData
{
  public static string GetString(string key, string defaultValue = "") => DataStorage.TryGetValue(key, out var value) ? value : defaultValue;
  public static int GetInt(string key, int defaultValue = 0) => DataStorage.TryGetValue(key, out var value) ? Parse.Int(value) : defaultValue;
  public static float GetFloat(string key, float defaultValue = 0f) => DataStorage.TryGetValue(key, out var value) ? Parse.Float(value) : defaultValue;
  public static bool GetBool(string key, bool defaultValue = false) => DataStorage.TryGetValue(key, out var value) ? bool.Parse(value) : defaultValue;
  public static long GetLong(string key, long defaultValue = 0) => DataStorage.TryGetValue(key, out var value) ? Parse.Long(value) : defaultValue;
  public static void SetInt(string key, int value) => DataStorage.SetValue(key, value.ToString(CultureInfo.InvariantCulture));
  public static void SetFloat(string key, float value) => DataStorage.SetValue(key, value.ToString(CultureInfo.InvariantCulture));
  public static void SetString(string key, string value) => DataStorage.SetValue(key, value);
  public static void SetBool(string key, bool value) => DataStorage.SetValue(key, value.ToString());
  public static void SetLong(string key, long value) => DataStorage.SetValue(key, value.ToString(CultureInfo.InvariantCulture));
}
