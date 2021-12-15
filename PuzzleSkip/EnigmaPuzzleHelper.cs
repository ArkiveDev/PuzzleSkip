using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using System;

namespace PuzzleSkip
{
    public static class EnigmaPuzzleHelper
    {
        public static void SkipEnigmaPuzzle(String completeGuidStr, ref BlueprintsCache instance)
        {
            // Load *_complete Etude
            BlueprintGuid completeGuid = BlueprintGuid.Parse(completeGuidStr);
            BlueprintEtude completeEtude = (BlueprintEtude)instance.Load(completeGuid);

            // Create Area check condition
            CurrentAreaIs areaCheck = new CurrentAreaIs();
            areaCheck.m_Area = new BlueprintAreaReference();
            areaCheck.m_Area.ReadGuidFromGuid(BlueprintGuid.Parse("646d29390deeba548b9605329897801f")); //AreshkagalDungeon AKA The Enigma

            completeEtude.ActivationCondition.Conditions = new Condition[1]
                { areaCheck };

            // Update checker in the BlueprintCache
            instance.AddCachedBlueprint(completeGuid, (SimpleBlueprint)completeEtude);
        }
    }
}
