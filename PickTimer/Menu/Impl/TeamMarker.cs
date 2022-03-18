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
    public static class TeamMarker
    {
        internal static void Menu(GameObject menu)
        {
            var teamMarkerText = MenuHandler.CreateText("Team Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(MenuManager.FindScaler(teamMarkerText.transform, MenuManager.MarkerType.TEAM));

            void EnabledChanged(bool val)
            {
                ConfigManager.TeamEnabledConfig.Value = val;
                MenuManager.teamMarkerEnabled = ConfigManager.TeamEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigManager.TeamEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                ConfigManager.TeamHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.teamMarkerSize.x = ConfigManager.TeamHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigManager.TeamHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                ConfigManager.TeamWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.teamMarkerSize.y = ConfigManager.TeamWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigManager.TeamWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
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
