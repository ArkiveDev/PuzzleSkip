using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using System;

namespace PuzzleSkip
{
    public static class MaskHelper
    {
        public static void SkipMaskPuzzle(String LootContCloseActGUIDStr, ref BlueprintsCache instance)
        {
            String[] maskGUIDS = {
                "69c74236d17f98a41acb82996c4613bf", //PuzzleMask1_Item  Circle Mask
                "43f591e7eb249de40b578f1f6a4aa3cf", //PuzzleMask2_Item  Summit Mask
                "bb065d5357ce84942b9132a4ce42e43f", //PuzzleMask3_Item  Darkness
                "2d24b105bc6d4ed4586e4052f8e4d621"  //PuzzleMask4_Item  Question
            };

            // Load Statue's MaskPuzzleLootCont CloseAction
            BlueprintGuid LootContCloseActionGUID = BlueprintGuid.Parse(LootContCloseActGUIDStr);
            ActionsHolder AH = (ActionsHolder)instance.Load(LootContCloseActionGUID);

            // Find the conditional that checks for Masks in the right places
            Conditional masksConditional = null;
            foreach (GameAction GA in AH.Actions.Actions)
            {
                if (GA.AssetGuid == "231bc467-627e-4998-8c7c-9ea9cb00d03a") //Masks in the right places
                    masksConditional = (Conditional)GA;
            }

            // Create new mask check conditional
            OrAndLogic maskCheck = new OrAndLogic();
            maskCheck.Comment = "Check for all 4 masks in inventory";
            maskCheck.ConditionsChecker = new ConditionsChecker();
            maskCheck.ConditionsChecker.Operation = Operation.And;
            maskCheck.ConditionsChecker.Conditions = new Condition[maskGUIDS.Length];
            int i = 0;
            foreach (string guidStr in maskGUIDS)
            {
                ItemsEnough IEMask = new ItemsEnough();
                IEMask.Quantity = 1;
                IEMask.m_ItemToCheck = new BlueprintItemReference();
                IEMask.m_ItemToCheck.ReadGuidFromGuid(BlueprintGuid.Parse(guidStr));
                maskCheck.ConditionsChecker.Conditions[i++] = IEMask;
            }

            // Update conditional
            masksConditional.ConditionsChecker.Conditions = new Condition[1]
                { maskCheck };

            // Update checker in the BlueprintCache
            instance.AddCachedBlueprint(LootContCloseActionGUID, (SimpleBlueprint)AH);
        }
    }
}
