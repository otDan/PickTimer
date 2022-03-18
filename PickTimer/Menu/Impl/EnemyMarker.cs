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
    public static class EnemyMarker
    {
        internal static void Menu(GameObject menu)
        {
            var enemyMarkerText = MenuHandler.CreateText("Enemy Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(MenuManager.FindScaler(enemyMarkerText.transform, MenuManager.MarkerType.ENEMY));

            void EnabledChanged(bool val)
            {
                ConfigManager.EnemyEnabledConfig.Value = val;
                MenuManager.enemyMarkerEnabled = ConfigManager.EnemyEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigManager.EnemyEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                ConfigManager.EnemyHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.enemyMarkerSize.x = ConfigManager.EnemyHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigManager.EnemyHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                ConfigManager.EnemyWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                MenuManager.enemyMarkerSize.y = ConfigManager.EnemyWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigManager.EnemyWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(false);
                markerToggle.GetComponent<Toggle>().isOn = false;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }
    }
}
