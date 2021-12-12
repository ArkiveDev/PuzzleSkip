﻿using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints;
using Kingmaker.AreaLogic.Etudes;
using Kingmaker.ElementsSystem;
using System;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Blueprints.Items;

namespace PuzzleSkip
{

    public class Main
    {
        public static UnityModManager.ModEntry.ModLogger Logger;

        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            Harmony harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            return true;
        }
    }

    [HarmonyPatch(typeof(BlueprintsCache), nameof(BlueprintsCache.Init))]
    public static class AddBlueprintsInCode
    {
        [HarmonyPostfix]
        public static void Init(ref BlueprintsCache __instance)
        {
            Main.Logger.Log("PuzzleSkip start");

            // Load etude for Conundrum Unsolved completion
            BlueprintGuid guid = BlueprintGuid.Parse("93386bb28305e764f91f81a506c866fc");
            SimpleBlueprint SB = __instance.Load(guid);
            Main.Logger.Log("Loaded " + SB.name);
            BlueprintEtude ConundrumUnsolvedChecker = (BlueprintEtude)SB;
            
            // Alter completion conditions
            ItemsEnough dominoCheck = new ItemsEnough();
            dominoCheck.Quantity = 1;
            dominoCheck.m_ItemToCheck = new BlueprintItemReference();
            dominoCheck.m_ItemToCheck.ReadGuidFromGuid(BlueprintGuid.Parse("afc2f5e747d8d8c4bba5409cd1c7e719"));
            BlueprintItem item = dominoCheck.m_ItemToCheck.Get();
            ConundrumUnsolvedChecker.ActivationCondition.Conditions = new Condition[1];
            ConundrumUnsolvedChecker.ActivationCondition.Conditions[0] = dominoCheck;

            // Update the Blueprint cache with the updated etude blueprint
            __instance.AddCachedBlueprint(guid,(SimpleBlueprint)ConundrumUnsolvedChecker);
        }

    }
}

