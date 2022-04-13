using PickTimer.Util;
using TMPro;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PickTimer.Menu.Impl
{
    public static class GeneralSettings
    {
        internal static void Menu(GameObject menu)
        {
            MenuHandler.CreateText("Pick Timer Options", menu, out TextMeshProUGUI _);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 45);

            void TimerEnabled(bool val)
            {
                ConfigController.TimerEnabledConfig.Value = val;
                PickTimer.PickTimerEnabled = ConfigController.TimerEnabledConfig.Value;
            }
            MenuHandler.CreateToggle(PickTimer.PickTimerEnabled, "Enabled", menu, TimerEnabled, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            void TimerChanged(float val)
            {
                ConfigController.TimerTimerConfig.Value = Mathf.RoundToInt(val);
                PickTimer.PickTimerTime = ConfigController.TimerTimerConfig.Value;
            }
            MenuHandler.CreateSlider("Seconds", menu, 30, 5f, 60f, ConfigController.TimerTimerConfig.Value, TimerChanged, out _, true);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 25);
            void GlobalVolAction(float val)
            {
                ConfigController.TimerVolumeConfig.Value = val;
            }
            MenuHandler.CreateSlider("Audio Volume", menu, 30, 0f, 1f, ConfigController.TimerVolumeConfig.Value, new UnityAction<float>(GlobalVolAction), out Slider slider, false, null, Slider.Direction.LeftToRight, true, null, null, null, null);
        }
    }
}
