using BepInEx.Configuration;

namespace PickTimer.Util
{
    public static class ConfigController
    {
        public static ConfigEntry<bool> TimerEnabledConfig;
        public static ConfigEntry<int> TimerTimerConfig;
        public static ConfigEntry<float> TimerVolumeConfig;
    }
}
