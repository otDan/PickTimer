using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using PickTimer.Asset;
using TMPro;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace PickTimer.Util
{
    internal static class PickTimerController
    {
        private static readonly System.Random Random = new();
        internal static GameObject TimerUi;
        private static Image _progressImage;
        private static TextMeshProUGUI _timerText;
        internal static Coroutine TimerCr;
        private static int _oldTimerTime = -1;

        public static bool InPickPhase;
        public static int CurrentPicks;

        internal static IEnumerator StartPickTimer(CardChoice instance)
        {
            if (!ConfigController.PickTimerEnabled) yield break;
            if (TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(TimerCr);
            }

            yield return new WaitWhile(() => !InPickPhase);
            TimerCr = Unbound.Instance.StartCoroutine(Timer(ConfigController.PickTimerTime));
            yield return new WaitForSecondsRealtime(ConfigController.PickTimerTime);

            var traverse = Traverse.Create(instance);

            var spawnedCards = (List<GameObject>)traverse.Field("spawnedCards").GetValue();
            if (ConfigController.PickTimerPunish)
            {
                int selectedCard = (int)instance.GetFieldValue("currentlySelectedCard");
                instance.Pick(spawnedCards[selectedCard]);
            }
            else
            {
                instance.Pick(spawnedCards[Random.Next(0, spawnedCards.Count)]);
            }

            traverse.Field("pickrID").SetValue(-1);
        }

        private static IEnumerator Timer(float timeToWait)
        {
            if (!ConfigController.PickTimerEnabled) yield break;

            float start = Time.time;
            if (_timerText == null)
            {
                InitializeTimerUi();
            }
            _timerText.color = Color.white;
            TimerUi.SetActive(true);
            _oldTimerTime = -1;

            while (Time.time < start + timeToWait)
            {
                float timerTimerForProgress = start + timeToWait - Time.time;
                int timerTime = Mathf.CeilToInt(timerTimerForProgress);
                if (_oldTimerTime != timerTime)
                {
                    _oldTimerTime = timerTime;
                    AudioController.PlayRandomTickClip(TimerUi.transform);
                }

                _timerText.text = timerTime.ToString();

                float progress = timerTime > 0 ? timerTimerForProgress / ConfigController.PickTimerTime : 0;
                // UnityEngine.Debug.Log($"Progress: {timerTime}/{PickTimer.PickTimerTime} value {progress}");
                _progressImage.fillAmount = progress;

                _timerText.color = timerTime switch {
                    <= 3 => Color.red,
                    <= 5 => Color.yellow,
                    _ => _timerText.color
                };
                yield return null;
            }

            TimerUi.SetActive(false);
        }

        private static void InitializeTimerUi()
        {
            var gameCanvas = GameObject.Find("/Game/UI").transform.Find("UI_Game").Find("Canvas").gameObject;

            TimerUi = Object.Instantiate(AssetManager.TimerUI, gameCanvas.transform);

            var rect = TimerUi.GetOrAddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.offsetMax = new Vector2(0, -(Screen.width / 4f));

            var fitter = TimerUi.GetOrAddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            // TimerCanvas = new GameObject("TimerCanvas", typeof(Canvas));
            // TimerCanvas.transform.SetParent(Unbound.Instance.canvas.transform);
            // TimerCanvas.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
            // TimerCanvas.transform.localScale = new Vector3(0.25f, 0.25f, 0);
            
            _timerText = TimerUi.GetComponentInChildren<TextMeshProUGUI>();
            _timerText.text = "";
            _timerText.fontSize = 200f;
            _timerText.enableWordWrapping = false;
            _timerText.overflowMode = TextOverflowModes.Overflow;
            _timerText.alignment = TextAlignmentOptions.Center;

            _progressImage = TimerUi.transform.Find("Timer/TimerFillImage").gameObject.GetComponent<Image>();

            // TimerCanvas.transform.position = new Vector2(, 150f);
            TimerUi.SetActive(false);
        }

        internal static IEnumerator Cleanup(IGameModeHandler gm)
        {
            if (!ConfigController.PickTimerEnabled) yield break;
            InPickPhase = false;
            CurrentPicks = 0;
            if (TimerHandler.Timer != null) { Unbound.Instance.StopCoroutine(TimerHandler.Timer); }
            if (TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(TimerCr);
            }
            if (TimerUi != null) { TimerUi.SetActive(false); }
        }
    }
    internal static class TimerHandler
    {
        internal static Coroutine Timer;

        internal static IEnumerator Start(IGameModeHandler gm)
        {
            if (!ConfigController.PickTimerEnabled) yield break;

            PickTimerController.InPickPhase = false;
            PickTimerController.CurrentPicks = 0;
            if (Timer != null) { Unbound.Instance.StopCoroutine(Timer); }

            if (ConfigController.PickTimerTime > 0)
            {
                Timer = Unbound.Instance.StartCoroutine(PickTimerController.StartPickTimer(CardChoice.instance));
            }
        }
    }
    [Serializable]
    [HarmonyPatch(typeof(CardChoice), "RPCA_DoEndPick")]
    class CardChoicePatchRPCA_DoEndPick
    {
        private static void Postfix(CardChoice __instance)
        {
            if (!ConfigController.PickTimerEnabled) return;
            if (PickTimerController.TimerUi != null && PickTimerController.TimerUi.gameObject.activeInHierarchy)
            {
                PickTimerController.TimerUi.SetActive(false);
            }
            if (PickTimerController.TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(PickTimerController.TimerCr);
            }
            if (TimerHandler.Timer != null)
            {
                __instance.StopCoroutine(TimerHandler.Timer);
            }
        }
    }

}
