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
    internal static class PickTimerHandler
    {
        private static readonly System.Random Random = new();
        private static GameObject _timerUi;
        private static Image _progressImage;
        private static TextMeshProUGUI _timerText;
        internal static GameObject TimerCanvas;
        internal static Coroutine TimerCr;

        internal static IEnumerator StartPickTimer(CardChoice instance)
        {
            if (TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(TimerCr);
            }

            TimerCr = Unbound.Instance.StartCoroutine(Timer(PickTimer.PickTimerTime));
            yield return new WaitForSecondsRealtime(PickTimer.PickTimerTime);

            var traverse = Traverse.Create(instance);
            var spawnedCards = (List<GameObject>) traverse.Field("spawnedCards").GetValue();

            instance.Pick(spawnedCards [Random.Next(0, spawnedCards.Count)]);
            traverse.Field("pickrID").SetValue(-1);
        }

        private static IEnumerator Timer(float timeToWait)
        {
            float start = Time.time;
            if (_timerText == null)
            {
                InitializeTimerUi();
            }
            _timerText.color = Color.white;
            TimerCanvas.SetActive(true);

            while (Time.time < start + timeToWait)
            {
                float timerTimerForProgress = start + timeToWait - Time.time;
                int timerTime = Mathf.CeilToInt(timerTimerForProgress);
                
                _timerText.text = timerTime.ToString();

                float progress = timerTime > 0 ? timerTimerForProgress / PickTimer.PickTimerTime : 0;
                // UnityEngine.Debug.Log($"Progress: {timerTime}/{PickTimer.PickTimerTime} value {progress}");
                _progressImage.fillAmount = progress;
                if (timerTime <= 3)
                {
                    _timerText.color = Color.red;
                }
                else if (timerTime <= 5)
                {
                    _timerText.color = Color.yellow;
                }
                yield return null;
            }
            TimerCanvas.SetActive(false);
        }

        private static void InitializeTimerUi()
        {
            TimerCanvas = new GameObject("TimerCanvas", typeof(Canvas));
            TimerCanvas.transform.SetParent(Unbound.Instance.canvas.transform);

            _timerUi = Object.Instantiate(AssetManager.TimerUI, TimerCanvas.transform, true);
            _timerText = _timerUi.GetComponentInChildren<TextMeshProUGUI>();
            _timerText.text = "";
            _timerText.fontSize = 200f;
            _timerText.enableWordWrapping = false;
            _timerText.overflowMode = TextOverflowModes.Overflow;
            _timerText.alignment = TextAlignmentOptions.Center;

            _progressImage = _timerUi.transform.Find("Timer/TimerFillImage").gameObject.GetComponent<Image>();

            TimerCanvas.transform.position = new Vector2(Screen.width / 2f, 150f);
            TimerCanvas.SetActive(false);
        }

        internal static IEnumerator Cleanup(IGameModeHandler gm)
        {
            if (TimerHandler.Timer != null) { Unbound.Instance.StopCoroutine(TimerHandler.Timer); }
            if (TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(TimerCr);
            }
            if (TimerCanvas != null) { TimerCanvas.SetActive(false); }
            yield break;
        }
    }
    internal static class TimerHandler
    {
        internal static Coroutine Timer;
        internal static IEnumerator Start(IGameModeHandler gm)
        {
            if (Timer != null) { Unbound.Instance.StopCoroutine(Timer); }
            if (PickTimer.PickTimerTime > 0)
            {
                Timer = Unbound.Instance.StartCoroutine(PickTimerHandler.StartPickTimer(CardChoice.instance));
            }
            yield break;
        }
    }
    [Serializable]
    [HarmonyPatch(typeof(CardChoice), "RPCA_DoEndPick")]
    class CardChoicePatchRPCA_DoEndPick
    {
        private static void Postfix(CardChoice __instance)
        {
            if (PickTimerHandler.TimerCanvas != null && PickTimerHandler.TimerCanvas.gameObject.activeInHierarchy)
            {
                PickTimerHandler.TimerCanvas.gameObject.SetActive(false);
            }
            if (PickTimerHandler.TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(PickTimerHandler.TimerCr);
            }
            if (TimerHandler.Timer != null)
            {
                __instance.StopCoroutine(TimerHandler.Timer);
            }
        }
    }

}
