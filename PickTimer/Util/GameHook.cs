using System;
using System.Collections;
using System.Collections.Generic;
using UnboundLib.GameModes;
using UnityEngine;

namespace PickTimer.Util
{
    public class GameHook : MonoBehaviour
    {
        public static GameHook instance { get; private set; }

        private readonly List<IGameStartHookHandler> _gameStartHooks = new();
        private readonly List<IGameEndHookHandler> _gameEndHooks = new();
        private readonly List<IPlayerPickEndHookHandler> _playerPickEndHooks = new();
        private readonly List<IPlayerPickStartHookHandler> _playerPickStartHooks = new();
        private readonly List<IPickStartHookHandler> _pickStartHooks = new();
        private readonly List<IPickEndHookHandler> _pickEndHooks = new();
        private readonly List<IPointStartHookHandler> _pointStartHooks = new();
        private readonly List<IPointEndHookHandler> _pointEndHooks = new();
        private readonly List<IRoundStartHookHandler> _roundStartHooks = new();
        private readonly List<IRoundEndHookHandler> _roundEndHooks = new();
        private readonly List<IBattleStartHookHandler> _battleStartHooks = new();

        private void Start()
        {
            if (instance != null)
            {
                DestroyImmediate(this);
                return;
            }
            instance = this;

            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameEnd);
            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameStart);
            GameModeManager.AddHook(GameModeHooks.HookBattleStart, BattleStart);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickStart, PlayerPickStart);
            GameModeManager.AddHook(GameModeHooks.HookPlayerPickEnd, PlayerPickEnd);
            GameModeManager.AddHook(GameModeHooks.HookPointStart, PointStart);
            GameModeManager.AddHook(GameModeHooks.HookPointEnd, PointEnd);
            GameModeManager.AddHook(GameModeHooks.HookPickStart, PickStart);
            GameModeManager.AddHook(GameModeHooks.HookPickEnd, PickEnd);
            GameModeManager.AddHook(GameModeHooks.HookRoundStart, RoundStart);
            GameModeManager.AddHook(GameModeHooks.HookRoundEnd, RoundEnd);
        }

        public void RegisterHooks(object obj)
        {
            switch (obj)
            {
                case IGameEndHookHandler gameEnd:
                    _gameEndHooks.Add(gameEnd);
                    break;
                case IGameStartHookHandler gameStart:
                    _gameStartHooks.Add(gameStart);
                    break;
                case IRoundEndHookHandler roundEnd:
                    _roundEndHooks.Add(roundEnd);
                    break;
                case IRoundStartHookHandler roundStart:
                    _roundStartHooks.Add(roundStart);
                    break;
                case IPointStartHookHandler pointStart:
                    _pointStartHooks.Add(pointStart);
                    break;
                case IPointEndHookHandler pointEnd:
                    _pointEndHooks.Add(pointEnd);
                    break;
                case IBattleStartHookHandler battleStart:
                    _battleStartHooks.Add(battleStart);
                    break;
                case IPickStartHookHandler pickStart:
                    _pickStartHooks.Add(pickStart);
                    break;
                case IPickEndHookHandler pickEnd:
                    _pickEndHooks.Add(pickEnd);
                    break;
                case IPlayerPickStartHookHandler playerPickStart:
                    _playerPickStartHooks.Add(playerPickStart);
                    break;
                case IPlayerPickEndHookHandler playerPickEnd:
                    _playerPickEndHooks.Add(playerPickEnd);
                    break;
            }
        }

        public void RemoveHooks(object obj)
        {
            switch (obj)
            {
                case IGameEndHookHandler gameEnd:
                    _gameEndHooks.Remove(gameEnd);
                    break;
                case IGameStartHookHandler gameStart:
                    _gameStartHooks.Remove(gameStart);
                    break;
                case IRoundEndHookHandler roundEnd:
                    _roundEndHooks.Remove(roundEnd);
                    break;
                case IRoundStartHookHandler roundStart:
                    _roundStartHooks.Remove(roundStart);
                    break;
                case IPointStartHookHandler pointStart:
                    _pointStartHooks.Remove(pointStart);
                    break;
                case IPointEndHookHandler pointEnd:
                    _pointEndHooks.Remove(pointEnd);
                    break;
                case IBattleStartHookHandler battleStart:
                    _battleStartHooks.Remove(battleStart);
                    break;
                case IPickStartHookHandler pickStart:
                    _pickStartHooks.Remove(pickStart);
                    break;
                case IPickEndHookHandler pickEnd:
                    _pickEndHooks.Remove(pickEnd);
                    break;
                case IPlayerPickStartHookHandler playerPickStart:
                    _playerPickStartHooks.Remove(playerPickStart);
                    break;
                case IPlayerPickEndHookHandler playerPickEnd:
                    _playerPickEndHooks.Remove(playerPickEnd);
                    break;
            }
        }

        private IEnumerator GameStart(IGameModeHandler gm)
        {
            foreach (var hook in _gameStartHooks)
            {
                hook.OnGameStart();
            }

            yield break;
        }
        private IEnumerator GameEnd(IGameModeHandler gm)
        {
            foreach (var hook in _gameEndHooks)
            {
                hook.OnGameEnd();
            }

            yield break;
        }
        private IEnumerator RoundStart(IGameModeHandler gm)
        {
            foreach (var hook in _roundStartHooks)
            {
                hook.OnRoundStart();
            }

            yield break;
        }
        private IEnumerator RoundEnd(IGameModeHandler gm)
        {
            foreach (var hook in _roundEndHooks)
            {
                hook.OnRoundEnd();
            }

            yield break;
        }
        private IEnumerator PointStart(IGameModeHandler gm)
        {
            foreach (var hook in _pointStartHooks)
            {
                hook.OnPointStart();
            }

            yield break;
        }
        private IEnumerator PointEnd(IGameModeHandler gm)
        {
            foreach (var hook in _pointEndHooks)
            {
                hook.OnPointEnd();
            }

            yield break;
        }
        private IEnumerator BattleStart(IGameModeHandler gm)
        {
            foreach (var hook in _battleStartHooks)
            {
                hook.OnBattleStart();
            }

            yield break;
        }
        private IEnumerator PickStart(IGameModeHandler gm)
        {
            foreach (var hook in _pickStartHooks)
            {
                hook.OnPickStart();
            }

            yield break;
        }
        private IEnumerator PickEnd(IGameModeHandler gm)
        {
            foreach (var hook in _pickEndHooks)
            {
                hook.OnPickEnd();
            }

            yield break;
        }
        private IEnumerator PlayerPickStart(IGameModeHandler gm)
        {
            foreach (var hook in _playerPickStartHooks)
            {
                hook.OnPlayerPickStart();
            }

            yield break;
        }
        private IEnumerator PlayerPickEnd(IGameModeHandler gm)
        {
            foreach (var hook in _playerPickEndHooks)
            {
                hook.OnPlayerPickEnd();
            }

            yield break;
        }
    }

    internal interface IGameStartHookHandler
    {
        public abstract Func<IGameModeHandler, IEnumerator> OnGameStart();
        void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps);
        void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps);
    }

    internal interface IGameEndHookHandler
    {
        public abstract void OnGameEnd();
    }

    internal interface IPlayerPickStartHookHandler
    {
        public abstract void OnPlayerPickStart();
    }

    internal interface IPlayerPickEndHookHandler
    {
        public abstract void OnPlayerPickEnd();
    }

    internal interface IPointEndHookHandler
    {
        public abstract void OnPointEnd();
    }

    internal interface IPointStartHookHandler
    {
        public abstract void OnPointStart();
    }

    internal interface IRoundEndHookHandler
    {
        public abstract void OnRoundEnd();
    }

    internal interface IRoundStartHookHandler
    {
        public abstract void OnRoundStart();
    }

    internal interface IPickStartHookHandler
    {
        public abstract void OnPickStart();
    }

    internal interface IPickEndHookHandler
    {
        public abstract void OnPickEnd();
    }

    internal interface IBattleStartHookHandler
    {
        public abstract void OnBattleStart();
    }
}
