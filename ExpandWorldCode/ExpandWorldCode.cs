﻿using System;
using BepInEx;
using Data;
using HarmonyLib;
using Service;
namespace ExpandWorld.Code;
[BepInPlugin(GUID, NAME, VERSION)]
[BepInDependency("expand_world_prefabs", "1.34")]
public class EWP : BaseUnityPlugin
{
  public const string GUID = "expand_world_code";
  public const string NAME = "Expand World Code";
  public const string VERSION = "1.1";
#nullable disable
  public static Harmony Harmony;
#nullable enable
  public void Awake()
  {
    Harmony = new(GUID);
    Harmony.PatchAll();
    Log.Init(Logger);
    Yaml.Init();
    try
    {
      CodeLoading.SetupWatcher();
    }
    catch (Exception e)
    {
      Log.Error(e.StackTrace);
    }
    Parameters.ExecuteCode = CodeLoading.Execute;
    Parameters.ExecuteCodeWithValue = CodeLoading.Execute;
  }
}
