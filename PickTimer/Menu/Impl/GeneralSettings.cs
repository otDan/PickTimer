using PickTimer.Util;
using TMPro;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace PickTimer.Menu.Impl
{
    public static class GeneralSettings
    {
        internal static void Menu(GameObject menu)
        {
            MenuHandler.CreateText("Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            
            void TimerChanged(float val)
            {
                ConfigManager.PickTimerConfig.Value = UnityEngine.Mathf.RoundToInt(val);
                PickTimer.PickTimerTime = ConfigManager.PickTimerConfig.Value;
            }
            MenuHandler.CreateSlider("Pick Timer (seconds)", menu, 30, 0f, 100f, ConfigManager.PickTimerConfig.Value, TimerChanged, out UnityEngine.UI.Slider timerSlider, true);
        }
    }
}
