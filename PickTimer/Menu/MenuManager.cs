using PlayerMarkers.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnboundLib;
using UnityEngine;

namespace PlayerMarkers.Menu
{
    public static class MenuManager
    {
        public static bool inGame;

        private static GameObject ownMarker;
        public static bool ownMarkerEnabled;
        public static Vector3 ownMarkerSize;

        private static GameObject teamMarker;

        internal static IEnumerator FindScaler(Transform transform, object oWN)
        {
            throw new NotImplementedException();
        }

        public static bool teamMarkerEnabled;
        public static Vector3 teamMarkerSize;

        private static GameObject enemyMarker;
        public static bool enemyMarkerEnabled;
        public static Vector3 enemyMarkerSize;

        static MenuManager()
        {
            Unbound.RegisterMenu("Player Markers", () => { }, GeneralSettings.Menu, null, false);

            inGame = false;

            Unbound.Instance.StartCoroutine(MarkerUpdater());
        }

        internal static void Initialize()
        {

        }

        public static IEnumerator FindScaler(Transform transform, MarkerType markerType)
        {
            while (FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector") == null)
            {
                yield return new WaitForSeconds(1);
            }

            var playerObject = FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector");
            var activePlayerObject = Unbound.Instantiate(playerObject, transform);

            activePlayerObject.name = "TestObjectPlayer";
            activePlayerObject.transform.localPosition = new Vector3(0, 80, 0);
            activePlayerObject.transform.localScale = new Vector3(60, 60, 60);
            activePlayerObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var spriteRenderer = activePlayerObject.GetComponentInChildren<SpriteRenderer>();

            switch (markerType)
            {
                case (MarkerType.OWN):
                    {
                        var color = new Color(0.747f, 0.061f, 0.022f, 0.58f);
                        spriteRenderer.color = color;

                        ownMarker = ObjectManager.CreateObject("OwnMarker", color);
                        ownMarker.transform.SetParent(activePlayerObject.transform);
                        ownMarker.transform.localPosition = new Vector3(0, 2, 0);
                        ownMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        ownMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
                case (MarkerType.TEAM):
                    {
                        var color = new Color(0.047f, 0.061f, 0.722f, 0.58f);
                        spriteRenderer.color = color;

                        teamMarker = ObjectManager.CreateObject("TeamMarker", color);
                        teamMarker.transform.SetParent(activePlayerObject.transform);
                        teamMarker.transform.localPosition = new Vector3(0, 2, 0);
                        teamMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        teamMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
                case (MarkerType.ENEMY):
                    {
                        var color = new Color(0.047f, 0.761f, 0.122f, 0.58f);
                        spriteRenderer.color = color;

                        enemyMarker = ObjectManager.CreateObject("EnemyMarker", color);
                        enemyMarker.transform.SetParent(activePlayerObject.transform);
                        enemyMarker.transform.localPosition = new Vector3(0, 2, 0);
                        enemyMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        enemyMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
            }


            var gameObject = FindInActiveObjectByName("P_E_Neutral 33", "");
            var activeGameObject = Unbound.Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";
            activeGameObject.transform.localPosition = new Vector3(0, 0, 0);
            activeGameObject.transform.localScale = new Vector3(0.15f, 0.15f, 1);
            activeGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

            spriteRenderer = activeGameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            gameObject = FindInActiveObjectByName("P_A_X12", "");
            activeGameObject = Unbound.Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";
            activeGameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            activeGameObject.transform.localScale = new Vector3(0.235f, 0.235f, 1);
            activeGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        private static IEnumerator MarkerUpdater()
        {
            while (!inGame)
            {
                if (ownMarker != null)
                {
                    ownMarker.SetActive(ConfigManager.OwnEnabledConfig.Value);
                    ownMarker.transform.localScale = new Vector3(ownMarkerSize.y, ownMarkerSize.x, 1f);
                }
                if (teamMarker != null)
                {
                    teamMarker.SetActive(ConfigManager.TeamEnabledConfig.Value);
                    teamMarker.transform.localScale = new Vector3(teamMarkerSize.y, teamMarkerSize.x, 1f);
                }
                if (enemyMarker != null)
                {
                    enemyMarker.SetActive(ConfigManager.EnemyEnabledConfig.Value);
                    enemyMarker.transform.localScale = new Vector3(enemyMarkerSize.y, enemyMarkerSize.x, 1f);
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(10);
        }

        private static GameObject FindInActiveObjectByName(string name, string parent)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        if (parent != "")
                        {
                            if (objs[i].parent.parent.parent.name == parent)
                            {
                                return objs[i].gameObject;
                            }
                        }
                        else
                        {
                            return objs[i].gameObject;
                        }
                    }
                }
            }
            return null;
        }

        public enum MarkerType
        {
            OWN,
            TEAM,
            ENEMY
        }
    }
}
