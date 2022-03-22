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
        private static readonly System.Random Random = new System.Random();
        private static GameObject _timerUi;
        private static Image _progressImage;
        private static TextMeshProUGUI _timerText;
        internal static GameObject timerCanvas;
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
            timerCanvas.SetActive(true);

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
            timerCanvas.SetActive(false);
        }

        private static void InitializeTimerUi()
        {
            timerCanvas = new GameObject("TimerCanvas", typeof(Canvas));
            timerCanvas.transform.SetParent(Unbound.Instance.canvas.transform);

            _timerUi = Object.Instantiate(AssetManager.TimerUI, timerCanvas.transform, true);
            _timerText = _timerUi.GetComponentInChildren<TextMeshProUGUI>();
            _timerText.text = "";
            _timerText.fontSize = 200f;
            _timerText.enableWordWrapping = false;
            _timerText.overflowMode = TextOverflowModes.Overflow;
            _timerText.alignment = TextAlignmentOptions.Center;

            _progressImage = _timerUi.transform.Find("Timer/TimerFillImage").gameObject.GetComponent<Image>();

            timerCanvas.transform.position = new Vector2(Screen.width / 2f, 150f);
            timerCanvas.SetActive(false);
        }

        internal static IEnumerator Cleanup(IGameModeHandler gm)
        {
            if (TimerHandler.timer != null) { Unbound.Instance.StopCoroutine(TimerHandler.timer); }
            if (TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(TimerCr);
            }
            if (timerCanvas != null) { timerCanvas.SetActive(false); }
            yield break;
        }
    }
    internal static class TimerHandler
    {
        internal static Coroutine timer;
        internal static IEnumerator Start(IGameModeHandler gm)
        {
            if (timer != null) { Unbound.Instance.StopCoroutine(timer); }
            if (PickTimer.PickTimerTime > 0)
            {
                timer = Unbound.Instance.StartCoroutine(PickTimerHandler.StartPickTimer(CardChoice.instance));
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
            if (PickTimerHandler.timerCanvas != null && PickTimerHandler.timerCanvas.gameObject.activeInHierarchy)
            {
                PickTimerHandler.timerCanvas.gameObject.SetActive(false);
            }
            if (PickTimerHandler.TimerCr != null)
            {
                Unbound.Instance.StopCoroutine(PickTimerHandler.TimerCr);
            }
            if (TimerHandler.timer != null)
            {
                __instance.StopCoroutine(TimerHandler.timer);
            }
        }
    }

}
