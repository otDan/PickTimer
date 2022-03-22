using System;
using System.Collections;
using Photon.Pun;
using PickTimer.Asset;
using PickTimer.Util;
using TMPro;
using UnboundLib;
using UnboundLib.GameModes;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = System.Collections.Hashtable;

namespace PickTimer.Network
{
    public class LobbyMonitor : MonoBehaviourPunCallbacks, IGameStartHookHandler
    {
        public static LobbyMonitor instance { get; private set; }
        private static bool _enabled;
        private static TextMeshProUGUI _lobbyTimerText;
        internal static GameObject LobbyTimerCanvas;
        private static GameObject _lobbyTimerUi;
        private static Button _minusButton;
        private static Button _plusButton;

        private void Awake()
        {
            instance = this;

            InitializeLobbyTimerUi();
            GameHook.instance.RegisterHooks(this);
        }

        private void Update()
        {
            if (!_enabled) return;

            _lobbyTimerText.text = PickTimer.PickTimerTime.ToString();
        }

        public override void OnCreatedRoom()
        {
            if (PhotonNetwork.IsMasterClient)
            {
            }
        }

        public override void OnJoinedRoom()
        {
            if (_lobbyTimerUi == null)
                InitializeLobbyTimerUi();

            _lobbyTimerUi.SetActive(false);
            if (PhotonNetwork.OfflineMode) return;

            _lobbyTimerUi.SetActive(true);

            if (PhotonNetwork.IsMasterClient) return;
            _minusButton.gameObject.SetActive(false);
            _plusButton.gameObject.SetActive(false);
        }

        private static void IncrementPickTimerValue()
        {
            if (PickTimer.PickTimerTime + 1 <= 60)
            {
                PickTimer.PickTimerTime += 1;
            }

            PickTimer.SyncTimer();
        }

        private static void DecrementPickTimerValue()
        {
            if (PickTimer.PickTimerTime - 1 >= 5)
            {
                PickTimer.PickTimerTime -= 1;
            }

            PickTimer.SyncTimer();
        }

        private static void InitializeLobbyTimerUi()
        {
            LobbyTimerCanvas = GameObject.Find("/Game/UI/UI_Game/Canvas/");

            _lobbyTimerUi = Instantiate(AssetManager.TimerLobbyUI, LobbyTimerCanvas.transform, true);
            _lobbyTimerUi.AddComponent<BringBgToTop>();

            _lobbyTimerText = _lobbyTimerUi.GetComponentInChildren<TextMeshProUGUI>();
            _lobbyTimerText.text = PickTimer.PickTimerTime.ToString();
            _lobbyTimerText.enableWordWrapping = false;
            _lobbyTimerText.overflowMode = TextOverflowModes.Overflow;
            _lobbyTimerText.alignment = TextAlignmentOptions.Center;

            RectTransform rect = _lobbyTimerUi.GetComponent<RectTransform>();
            rect.localScale = new Vector3(0.8f, 0.8f, 1);
            rect.anchorMin = new Vector2(0, 0);
            rect.anchorMax = new Vector2(1, 1);
            rect.pivot = new Vector2(1f, 1f);
            rect.offsetMin = new Vector2(0, 0);
            rect.offsetMax = new Vector2(0, 0);
            rect.localPosition = new Vector3(769, 880, 0);

            foreach (var button in _lobbyTimerUi.gameObject.GetComponentsInChildren<Button>())
            {
                switch (button.gameObject.name)
                {
                    case "Minus":
                        _minusButton = button;
                        _minusButton.onClick.AddListener(DecrementPickTimerValue);
                        break;
                    case "Plus":
                        _plusButton = button;
                        _plusButton.onClick.AddListener(IncrementPickTimerValue);
                        break;
                }
            }

            _lobbyTimerUi.SetActive(false);
            _enabled = true;
        }

        private class BringBgToTop : MonoBehaviour
        {
            private void OnTransformChildrenChanged()
            {
                this.ExecuteAfterFrames(1, () => _lobbyTimerUi.transform.SetAsLastSibling());
            }
        }

        public override void OnLeftRoom()
        {
            _lobbyTimerUi.SetActive(false);
        }

        public void OnGameStart()
        {
            _lobbyTimerUi.SetActive(false);
        }
        
        public void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
        {
        }

        private void OnDestroy()
        {
            GameHook.instance.RemoveHooks(this);
        }
    }
}
