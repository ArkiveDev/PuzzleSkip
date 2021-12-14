using Kingmaker.AreaLogic.Etudes;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

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
            and.Comment = "PuzzleSkip - only check location and inventory slabs";
            and.ConditionsChecker = new ConditionsChecker();
            and.ConditionsChecker.Operation = Operation.And;
            and.ConditionsChecker.Conditions = new Condition[2]
                {dominoCheck, areaCheck};

            checkerEtude.ActivationCondition.Conditions = new Condition[1]
                {and};

            // Update checker in the BlueprintCache
            instance.AddCachedBlueprint(checkerGuid, (SimpleBlueprint)checkerEtude);
        }
    }
}
