using UnityEngine;

namespace PickTimer.Asset
{
    public static class AssetManager
    {
        private static readonly AssetBundle CardBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("picktimer_assets", typeof(PickTimer).Assembly);

        public static GameObject TimerUI = CardBundle.LoadAsset<GameObject>("TimerUI");
        public static GameObject TimerLobbyUI = CardBundle.LoadAsset<GameObject>("TimerLobbyUI");
    }
}
