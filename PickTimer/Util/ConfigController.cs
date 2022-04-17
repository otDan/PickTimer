using BepInEx.Configuration;

namespace PickTimer.Util
{
    public static class ConfigController
    {
        public static ConfigEntry<bool> TimerEnabledConfig;
        public static ConfigEntry<int> TimerTimerConfig;
        public static ConfigEntry<float> TimerVolumeConfig;
        public static ConfigEntry<bool> TimerPunishConfig;

        public static int PickTimerTime;
        public static bool PickTimerEnabled;
        public static bool PickTimerPunish;
    }
}
