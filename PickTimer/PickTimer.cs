using BepInEx;
using HarmonyLib;
using Photon.Pun;
using PickTimer.Menu;
using PickTimer.Util;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.GameModes;
using UnboundLib.Networking;

namespace PickTimer
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound")]
    [BepInIncompatibility("pykess.rounds.plugins.competitiverounds")]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PickTimer : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.picktimer";
        private const string ModName = "Pick Timer";
        public const string Version = "1.0.1";
        public const string ModInitials = "PT";
        private const string CompatibilityModName = "PickTimer";
        public static PickTimer Instance { get; private set; }
        public static int PickTimerTime;

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

            PickTimerTime = ConfigManager.PickTimerConfig.Value;

            GameModeManager.AddHook(GameModeHooks.HookPlayerPickStart, TimerHandler.Start);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, PickTimerHandler.Cleanup);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, PickTimerHandler.Cleanup);

            Unbound.RegisterHandshake(ModId, OnHandShakeCompleted);
        }

        private void SetupConfig()
        {
            ConfigManager.PickTimerConfig = Config.Bind(CompatibilityModName, "PickTimer", 0, "Pick Timer Time");
        }

        private static void OnHandShakeCompleted()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkingManager.RPC_Others(typeof(PickTimer), nameof(SyncSettings), PickTimerTime);
            }
        }

        [UnboundRPC]
        private static void SyncSettings(int pickTimer)
        {
            PickTimerTime = pickTimer;
        }
    }
}