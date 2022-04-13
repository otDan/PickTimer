using UnityEngine;

namespace PickTimer.Asset
{
    public static class AssetManager
    {
        private static readonly AssetBundle TimerAssetsBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("picktimer_assets", typeof(PickTimer).Assembly);

        public static GameObject TimerUI = TimerAssetsBundle.LoadAsset<GameObject>("TimerUI");
        public static GameObject TimerLobbyUI = TimerAssetsBundle.LoadAsset<GameObject>("TimerLobbyUI");
        public static GameObject TimerParticles = TimerAssetsBundle.LoadAsset<GameObject>("TimerParticles");

        public static AudioClip TimerTicksClip = TimerAssetsBundle.LoadAsset<AudioClip>("TimerTicking");
    }
}
