using BepInEx;
using HarmonyLib;
using Photon.Pun;
using PickTimer.Menu;
using PickTimer.Network;
using PickTimer.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnboundLib.Networking;

namespace PickTimer
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInDependency("pykess.rounds.plugins.moddingutils")]
    [BepInDependency("pykess.rounds.plugins.pickncards")]
    [BepInIncompatibility("pykess.rounds.plugins.competitiverounds")]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PickTimer : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.picktimer";
        private const string ModName = "Pick Timer";
        public const string Version = "2.1.0";
        public const string ModInitials = "PT";
        private const string CompatibilityModName = "PickTimer";
        public static PickTimer Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            Unbound.RegisterClientSideMod(ModId);

            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            SetupConfig();
        }

        private void Start()
        {
            MenuManager.Initialize();

            ConfigController.PickTimerEnabled = ConfigController.TimerEnabledConfig.Value;
            ConfigController.PickTimerTime = ConfigController.TimerTimerConfig.Value;

            gameObject.AddComponent<GameHook>();
            gameObject.AddComponent<LobbyMonitor>();

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickStart, TimerHandler.Start);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, PickTimerController.Cleanup);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, PickTimerController.Cleanup);

            Unbound.RegisterHandshake(ModId, OnHandShakeCompleted);

            AudioController.LoadClips();
        }

        private void SetupConfig()
        {
            ConfigController.TimerEnabledConfig = Config.Bind(CompatibilityModName, "PickTimer_Enabled", true, "Pick Timer Enabled");
            ConfigController.TimerTimerConfig = Config.Bind(CompatibilityModName, "PickTimer_Time", 15, "Pick Timer Time");
            ConfigController.TimerPunishConfig = Config.Bind(CompatibilityModName, "PickTimer_Punish", true, "Pick Timer Punish");
            ConfigController.TimerVolumeConfig = Config.Bind(CompatibilityModName, "PickTimer_AudioVolume", 0.75f, "Pick Timer Audio Volume");
        }

        public static void SyncTimer()
        {
            NetworkingManager.RPC_Others(typeof(PickTimer), nameof(SyncSettings), ConfigController.PickTimerEnabled, ConfigController.PickTimerTime, ConfigController.PickTimerPunish);
        }

        private static void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                SyncTimer();
            }
        }

        [UnboundRPC]
        private static void SyncSettings(bool pickTimerEnabled, int pickTimerTime, bool pickTimerPunish)
        {
            ConfigController.PickTimerEnabled = pickTimerEnabled;
            ConfigController.PickTimerTime = pickTimerTime;
            ConfigController.PickTimerPunish = pickTimerPunish;

            LobbyMonitor.InitializeLobbyTimerUi();
        }
    }
}