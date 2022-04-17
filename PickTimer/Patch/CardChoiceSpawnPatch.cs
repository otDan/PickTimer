using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using PickTimer.Util;
using UnityEngine;

namespace PickTimer.Patch
{
    [Serializable]
    [HarmonyPatch(typeof(CardChoice), "Spawn")]
    internal class CardChoiceSpawnPatch
    {
        [HarmonyPriority(int.MinValue)]
        private static void Prefix(CardChoice __instance, ref GameObject objToSpawn, int ___pickrID, List<GameObject> ___spawnedCards, Transform[] ___children)
        {
            if (!__instance.IsPicking) return;

            int numDraws = (int) typeof(DrawNCards.DrawNCards).GetField("numDraws", BindingFlags.NonPublic | BindingFlags.Static)?.GetValue(null)!;

            PickTimerController.CurrentPicks += 1;
            
            if (PickTimerController.CurrentPicks == numDraws)
            {
                PickTimerController.InPickPhase = true;
            }
        }
    }
}
