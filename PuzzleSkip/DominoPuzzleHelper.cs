using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators.Items;
using Kingmaker.Designers.EventConditionActionSystem.Events;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Items;
using Kingmaker.UI.Loot;
using Kingmaker.Utility;
using Kingmaker.View.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleSkip
{
    public static class DominoPuzzleHelper
    {
        public static void SkipDominoReqs(String checkerGuidStr, String areaGuidStr, String[] dominoGuidStrs, ref BlueprintsCache instance)
        {

            // Load Checker Etude
            BlueprintGuid checkerGuid = BlueprintGuid.Parse(checkerGuidStr);
            BlueprintEtude checkerEtude = (BlueprintEtude)instance.Load(checkerGuid);

            // Create Domino check conditions
            OrAndLogic dominoCheck = new OrAndLogic();
            dominoCheck.Comment = "Check for at least 1 slab from the set";
            dominoCheck.ConditionsChecker = new ConditionsChecker();
            dominoCheck.ConditionsChecker.Operation = Operation.Or;
            dominoCheck.ConditionsChecker.Conditions = new Condition[dominoGuidStrs.Length];
            int i = 0;
            foreach (string guidStr in dominoGuidStrs)
            {
                ItemsEnough IEDominos = new ItemsEnough();
                IEDominos.Quantity = 1;
                IEDominos.m_ItemToCheck = new BlueprintItemReference();
                IEDominos.m_ItemToCheck.ReadGuidFromGuid(BlueprintGuid.Parse(guidStr));
                dominoCheck.ConditionsChecker.Conditions[i++] = IEDominos;
            }

            // Create Area check condition
            CurrentAreaIs areaCheck = new CurrentAreaIs();
            areaCheck.m_Area = new BlueprintAreaReference();
            areaCheck.m_Area.ReadGuidFromGuid(BlueprintGuid.Parse(areaGuidStr));

            // Combine checks
            OrAndLogic and = new OrAndLogic();
            and.Comment = "PuzzleSkip - only check inventory slabs and location";
            and.ConditionsChecker = new ConditionsChecker();
            and.ConditionsChecker.Operation = Operation.And;
            and.ConditionsChecker.Conditions = new Condition[2]
                {dominoCheck, areaCheck};

            checkerEtude.ActivationCondition.Conditions = new Condition[1]
                {and};


            


            // Update checker in the BlueprintCache
            instance.AddCachedBlueprint(checkerGuid, (SimpleBlueprint)checkerEtude);
        }

        public static void SolveDominoPuzzle(String checkerGuidStr, ref BlueprintsCache instance)
        {
            // Load Checker Etude
            BlueprintGuid checkerGuid = BlueprintGuid.Parse(checkerGuidStr);
            BlueprintEtude checkerEtude = (BlueprintEtude)instance.Load(checkerGuid);

            EtudePlayTrigger ept = (EtudePlayTrigger)checkerEtude.Components.FirstItem(x => x.GetType() == typeof(EtudePlayTrigger));

            //// Create new preChecker Etude
            //BlueprintEtude preCheckerEtude = new BlueprintEtude()
            //{
                
            //}

            // Add actions to put slabs in slots

            // loop through checker's activation conditions to build slot/slab key/value pairs
            foreach (OrAndLogic condition in checkerEtude.ActivationCondition.Conditions)
            {
                // We don't care about the IsLootEmpty conditions, just the ItemBlueprint conditions
                if (condition.ConditionsChecker.Conditions.OfType<ItemBlueprint>().Count() > 0)
                {
                    foreach (Condition condition2 in condition.ConditionsChecker.Conditions)
                    {
                        ItemBlueprint ibCondition = (ItemBlueprint)condition2;
                        string slotGuid = ((FirstItemFromLootEvaluator)ibCondition.Item).LootObject.UniqueId;
                        string slabGuid = ibCondition.Blueprint.ToString();

                        AddItemsToCollection moveAct = CreateMoveSlabAction(slotGuid, slabGuid);
                        ept.Actions.Actions = ept.Actions.Actions.Concat(moveAct).ToArray();
                        checkerEtude.AddToElementsList(moveAct);

                        RemoveItemFromPlayer removeAct = new RemoveItemFromPlayer()
                        {
                            Quantity = 1,
                            m_ItemToRemove = new BlueprintItemReference()
                        };
                        removeAct.m_ItemToRemove.ReadGuidFromGuid(BlueprintGuid.Parse(slabGuid));
                        ept.Actions.Actions = ept.Actions.Actions.Concat(removeAct).ToArray();
                        checkerEtude.AddToElementsList(removeAct);
                    }
                }
            }
        }

        private static AddItemsToCollection CreateMoveSlabAction(String slot, String slab)
        {
            MapObjectLoot mol = new MapObjectLoot()
            {
                MapObject = new MapObjectFromScene()
                {
                    MapObject = new EntityReference()
                    {
                        UniqueId = slot
                    }
                }
            };

            LootEntry LE = new LootEntry
            {
                m_Item = new BlueprintItemReference()
            };
            LE.m_Item.ReadGuidFromGuid(BlueprintGuid.Parse(slab)); //DominoOrange1x3

            AddItemsToCollection moveAct = new AddItemsToCollection()
            {
                UseBlueprintUnitLoot = false,
                ItemsCollection = mol,
                Loot = new List<LootEntry>
                {
                    LE
                }
            };

            return moveAct;
        }
    }
}
