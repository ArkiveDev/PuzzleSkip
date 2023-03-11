using HarmonyLib;
using UnityModManagerNet;
using System.Reflection;
using Kingmaker.Blueprints.JsonSystem;
using System;

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
            Main.Logger.Log("BlueprintsCache start");


            //testing domino solver
            DominoPuzzleHelper.SolveDominoPuzzle("93386bb28305e764f91f81a506c866fc", ref __instance); //OrangeSolveChecker

            // Skip Domino/Slab puzzles
            string orangeChecker = "93386bb28305e764f91f81a506c866fc"; //OrangeSolveChecker
            string orangeArea = "7f8046185b8c83940aed106445571a4d"; //GlobalPuzzle_Start AKA Conundrum Unsolved
            string[] orangeDominoes =
            {
                "afc2f5e747d8d8c4bba5409cd1c7e719", //DominoOrange2x4
                "01f719f15ba05d94bbc3ca3526d0ee2a", //DominoOrange1x3
                "7605c17de4ef4fcd924c0b28f634adac"  //DominoOrangeKey_1
            };
            DominoPuzzleHelper.SkipDominoReqs(orangeChecker, orangeArea, orangeDominoes, ref __instance);



            //string cyanChecker = "0eda58f3f35c4375b7bd1ad4c42838a1"; //CyanPuzzleSolveChecker
            //string cyanArea = "17501f76cc3af8342ba6eccbd178fa94"; //GlobalPuzzle_Cyan AKA Core of the Riddle
            //string[] cyanDominoes =
            //{
            //    "45a8d763a44a5a844a6a936f17e4b765", //DominoCyan1x3
            //    "80d9f2dd22aa13a4491cb7f039ce89a6"  //DominoCyan1x4
            //};
            //DominoPuzzleHelper.SkipDominoReqs(cyanChecker, cyanArea, cyanDominoes, ref __instance);

            //string purpleChecker = "5edf4fa952634a7fa7bbf327cde8dace"; //PurplePuzzleSolveChecker
            //string purpleArea = "472f64e570afaab45aad7cc304919a40"; //GlobalPuzzle_Purple AKA Legacy of the Ancients
            //string[] purpleDominoes =
            //{
            //    "f2766dfec3a62f6498745939d364f43a" //DominoPurple1x2
            //};
            //DominoPuzzleHelper.SkipDominoReqs(purpleChecker, purpleArea, purpleDominoes, ref __instance);

            //string redChecker = "3ff62c12825a4d5497fe95932e803663"; //redPuzzleSolveChecker
            //string redArea = "b836f67468d1ba843aea89aaf0281c43"; //GlobalPuzzle_Red AKA Final Veil
            //string[] redDominoes =
            //{
            //    "5c546f3b69885b94da8cb2fbf6e2fbe1" //DominoRed1x1
            //};
            //DominoPuzzleHelper.SkipDominoReqs(redChecker, redArea, redDominoes, ref __instance);

            //string greenChecker = "9b8e2bd53bdf4509832d120643b46f2e"; //greenPuzzleSolveChecker
            //string greenArea = "8ea944dbefc7c324599509805e65658d"; //GlobalPuzzle_green AKA Forgotten Secrets
            //string[] greenDominoes =
            //{
            //    "e2ea6c90446bf9d478761a7c95948dee" //Dominogreen1x1
            //};
            //DominoPuzzleHelper.SkipDominoReqs(greenChecker, greenArea, greenDominoes, ref __instance);

            //string yellowChecker = "00016e6128651a548b2c50f23456a3e0"; //yellowPuzzleSolveChecker
            //string yellowArea = "50062f5d95939b746bbbe950ceff89d4"; //GlobalPuzzle_Yellow AKA Heart of Mystery
            //string[] yellowDominoes =
            //{
            //    "9ddb1f6eb52579f42bcc6767c25de24c" //DominoFlask1
            //};
            //DominoPuzzleHelper.SkipDominoReqs(yellowChecker, yellowArea, yellowDominoes, ref __instance);

            //Main.Logger.Log("Domino puzzles skipped");

            //// Skip Mask puzzle in Unnamed Ruins
            //String[] statueLootContainers =
            //{
            //    "b3f6d1173b0f197448e6321a544187a0",
            //    "881e3d32501b4b04683251539978aed5",
            //    "2573e02967e95014c93266c845bc9f2e",
            //    "1c031259c759d134ca117d6ca4ee21fc"
            //};
            //foreach (String guid in statueLootContainers)
            //    MaskHelper.SkipMaskPuzzle(guid, ref __instance);

            //Main.Logger.Log("Mask puzzle skipped");


            //// Skip Enigma puzzles
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("fc34b75fb3f147d398607271584ee70f", ref __instance); //Arrow_Puzzle_1_complete
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("a4bbfd673dc442aba597860b6d3b3f65", ref __instance); //Arrow_Puzzle_2_complete
            //Main.Logger.Log("Arrow puzzles skipped");
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("568a5163b9a8492cbcef5c7aee97ebe4", ref __instance); //CommonPuzzle_1_complete (Memory / Matching)
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("3b22fd8b3c994b53a3732360630c15d0", ref __instance); //CommonPuzzle_2_complete 
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("211e3b7c140d4f40adb9f9d8687c2adc", ref __instance); //CommonPuzzle_3_complete 
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("964a2d978dec46c98754331578591273", ref __instance); //CommonPuzzle_4_complete 
            //Main.Logger.Log("Memory puzzles skipped");
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("2d096987069046118869129458f30517", ref __instance); //XO_Puzzle_1_complete (Tic Tac Toe)
            //EnigmaPuzzleHelper.SkipEnigmaPuzzle("8288ad6ec196461f963d6160edbfaca4", ref __instance); //XO_Puzzle_1_complete
            //Main.Logger.Log("3-in-a-row puzzles skipped");


            Main.Logger.Log("BlueprintsCache finish");
        }

    }
}


