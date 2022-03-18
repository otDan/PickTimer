using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerMarkers.Util
{
    public static class ConfigManager
    {
        public static ConfigEntry<bool> OwnEnabledConfig;
        public static ConfigEntry<float> OwnHeightConfig;
        public static ConfigEntry<float> OwnWidthConfig;

        public static ConfigEntry<bool> TeamEnabledConfig;
        public static ConfigEntry<float> TeamHeightConfig;
        public static ConfigEntry<float> TeamWidthConfig;

        public static ConfigEntry<bool> EnemyEnabledConfig;
        public static ConfigEntry<float> EnemyHeightConfig;
        public static ConfigEntry<float> EnemyWidthConfig;
    }
}
