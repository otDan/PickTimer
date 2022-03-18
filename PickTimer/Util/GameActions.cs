using PlayerMarkers.Menu;
using PlayerMarkers.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnboundLib.Cards;
using UnboundLib.Extensions;
using UnboundLib.GameModes;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PlayerMarkers.Util
{
    public static class GameActions
    {
        internal static IEnumerator GameStart(IGameModeHandler gameModeHandler)
        {
            try
            {
                MenuManager.inGame = true;
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    if (ConfigManager.OwnEnabledConfig.Value)
                    {
                        var markerObject = ObjectManager.CreateObject("PlayerMarker", player.GetTeamColors().color);
                        markerObject.transform.SetParent(player.gameObject.transform);
                        markerObject.transform.localPosition = new Vector3(0, 2, -10);
                        markerObject.transform.localScale = new Vector3(MenuManager.ownMarkerSize.x, MenuManager.ownMarkerSize.y, 0.75f);
                        markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                    }

                    if (ConfigManager.TeamEnabledConfig.Value)
                    {
                        var teamPlayers = PlayerManager.instance.players.Where(p => p.teamID == player.teamID && p.playerID != player.playerID);
                        foreach (Player teamPlayer in teamPlayers)
                        {
                            var markerObject = ObjectManager.CreateObject("TeamMarker", teamPlayer.GetTeamColors().color);
                            markerObject.transform.SetParent(teamPlayer.gameObject.transform);
                            markerObject.transform.localPosition = new Vector3(0, 2, -10);
                            markerObject.transform.localScale = new Vector3(MenuManager.teamMarkerSize.x, MenuManager.teamMarkerSize.y, 0.75f);
                            markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                        }
                    }

                    if (ConfigManager.EnemyEnabledConfig.Value)
                    {
                        var enemyPlayers = PlayerManager.instance.players.Where(p => p.teamID != player.teamID && !p.data.view.IsMine);
                        foreach (Player enemyPlayer in enemyPlayers)
                        {
                            var markerObject = ObjectManager.CreateObject("EnemyMarker", enemyPlayer.GetTeamColors().color);
                            markerObject.transform.SetParent(enemyPlayer.gameObject.transform);
                            markerObject.transform.localPosition = new Vector3(0, 2, -10);
                            markerObject.transform.localScale = new Vector3(MenuManager.enemyMarkerSize.x, MenuManager.enemyMarkerSize.y, 0.75f);
                            markerObject.transform.localRotation = Quaternion.Euler(0, 180, 180);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }

            yield break;
        }

        internal static IEnumerator GameEnd(IGameModeHandler gameModeHandler)
        {
            try
            {
                MenuManager.inGame = false;
                var myPlayers = PlayerManager.instance.players.Where(p => p.data.view.IsMine);
                foreach (Player player in myPlayers)
                {
                    var markerObject = player.gameObject.transform.Find("Marker");
                    Object.Destroy(markerObject);
                }
            }
            catch (Exception)
            {
                // ignored
            }

            yield break;
        }
    }
}
