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
                ConfigController.PickTimerEnabled = ConfigController.TimerEnabledConfig.Value;
            }
            MenuHandler.CreateToggle(ConfigController.PickTimerEnabled, "Enabled", menu, TimerEnabled, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 10);
            MenuHandler.CreateText("<i><color=#9e9e9e>Have a timer in the pick phase that picks a card once it ends.</color></i>", menu, out TextMeshProUGUI _, 18);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 25);
            void TimerChanged(float val)
            {
                ConfigController.TimerTimerConfig.Value = Mathf.RoundToInt(val);
                ConfigController.PickTimerTime = ConfigController.TimerTimerConfig.Value;
            }
            MenuHandler.CreateSlider("Seconds", menu, 30, 5f, 60f, ConfigController.TimerTimerConfig.Value, TimerChanged, out _, true);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 10);
            MenuHandler.CreateText("<i><color=#9e9e9e>Seconds on the pick phase timer.</color></i>", menu, out TextMeshProUGUI _, 18);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 25);
            void PunishEnabled(bool val)
            {
                ConfigController.TimerPunishConfig.Value = val;
                ConfigController.PickTimerPunish = ConfigController.TimerPunishConfig.Value;
            }
            MenuHandler.CreateToggle(ConfigController.PickTimerEnabled, "Punish Player", menu, PunishEnabled, 30);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 10);
            MenuHandler.CreateText("<i><color=#9e9e9e>Punish the player by picking a random card instead of the one he is on.</color></i>", menu, out TextMeshProUGUI _, 18);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 25);
            void GlobalVolAction(float val)
            {
                ConfigController.TimerVolumeConfig.Value = val;
            }
            MenuHandler.CreateSlider("Audio Volume", menu, 30, 0f, 1f, ConfigController.TimerVolumeConfig.Value, new UnityAction<float>(GlobalVolAction), out Slider slider, false, null, Slider.Direction.LeftToRight, true, null, null, null, null);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 10);
            MenuHandler.CreateText("<i><color=#9e9e9e>Timer ticking sound volume.</color></i>", menu, out TextMeshProUGUI _, 18);
        }
    }
}