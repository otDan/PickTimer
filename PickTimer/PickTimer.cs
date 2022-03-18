using BepInEx;
using HarmonyLib;
using UnboundLib.GameModes;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using TMPro;
using BepInEx.Configuration;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using PlayerMarkers.Menu;
using PlayerMarkers.Util;

namespace PlayerMarkers
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PickTimer : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.playermarkers";
        private const string ModName = "Player Markers";
        public const string Version = "1.1.1";
        public const string ModInitials = "PM";
        private const string CompatibilityModName = "PlayerMarkers";
        public static PickTimer instance { get; private set; }

        void Awake()
        {
            instance = this;
            Unbound.RegisterClientSideMod(ModId);

            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            SetupConfig();
        }

        void Start()
        {
            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameActions.GameStart);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameActions.GameEnd);

            MenuManager.Initialize();
        }

        void SetupConfig()
        {
            ConfigManager.OwnEnabledConfig = Config.Bind(CompatibilityModName, "OwnEnabled", true, "Own Marker Enabled");
            ConfigManager.OwnHeightConfig = Config.Bind(CompatibilityModName, "OwnHeight", 0.75f, "Own Marker Height");
            ConfigManager.OwnWidthConfig = Config.Bind(CompatibilityModName, "OwnWidth", 0.75f, "Own Marker Width");

            ConfigManager.TeamEnabledConfig = Config.Bind(CompatibilityModName, "TeamEnabled", true, "Team Marker Enabled");
            ConfigManager.TeamHeightConfig = Config.Bind(CompatibilityModName, "TeamHeight", 0.75f, "Team Marker Height");
            ConfigManager.TeamWidthConfig = Config.Bind(CompatibilityModName, "TeamWidth", 0.75f, "Team Marker Width");

            ConfigManager.EnemyEnabledConfig = Config.Bind(CompatibilityModName, "EnemyEnabled", false, "Enemy Marker Enabled");
            ConfigManager.EnemyHeightConfig = Config.Bind(CompatibilityModName, "EnemyHeight", 0.75f, "Enemy Marker Height");
            ConfigManager.EnemyWidthConfig = Config.Bind(CompatibilityModName, "EnemyWidth", 0.75f, "Enemy Marker Width");
        }
    }
}

