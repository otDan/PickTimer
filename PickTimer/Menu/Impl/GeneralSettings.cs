using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnboundLib.Utils.UI;
using UnityEngine;

namespace PlayerMarkers.Menu
{
    public static class GeneralSettings
    {
        internal static void Menu(GameObject menu)
        {
            MenuHandler.CreateText("Options", menu, out TextMeshProUGUI _, 60);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            var ownMarkerMenu = MenuHandler.CreateMenu("Own Marker", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            OwnMarker.Menu(ownMarkerMenu);

            var teamMarkerMenu = MenuHandler.CreateMenu("Team Marker", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            TeamMarker.Menu(teamMarkerMenu);

            var enemyMarkerMenu = MenuHandler.CreateMenu("Enemy Marker", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            EnemyMarker.Menu(enemyMarkerMenu);
        }
    }
}
