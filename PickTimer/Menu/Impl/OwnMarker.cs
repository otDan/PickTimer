using PlayerMarkers.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerMarkers.Menu
{
    public static class OwnMarker
    {
        internal static void Menu(GameObject menu)
        {
            var ownMarkerText = MenuHandler.CreateText("Own Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(MenuManager.FindScaler(ownMarkerText.transform, MenuManager.MarkerType.OWN));

            void EnabledChanged(bool val)
            {
                ConfigManager.OwnEnabledConfig.Value = val;
                MenuManager.ownMarkerEnabled = ConfigManager.OwnEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigManager.OwnEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                ConfigManager.OwnHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.ownMarkerSize.x = ConfigManager.OwnHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigManager.OwnHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                ConfigManager.OwnWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.ownMarkerSize.y = ConfigManager.OwnWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigManager.OwnWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(true);
                markerToggle.GetComponent<Toggle>().isOn = true;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }
    }
}
