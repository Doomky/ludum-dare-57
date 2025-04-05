using System;
using Framework.Core;
using Framework.Databases;
using Framework.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Framework
{
    public class Game : SerializedMonoBehaviour
    {
        protected SuperManager _superManager = null;

        public SuperManager SuperManager => this._superManager;
    }

    /// <summary>
    /// This is the core class of the game. It is a singleton and is responsible for initializing the game.
    /// </summary>
    /// <typeparam name="TGame">the type of game.</typeparam>
    /// <typeparam name="TGameStateMachine">the state machine of the game.</typeparam>
    /// <typeparam name="TGameAction">the action that can be feeded to the state machine.</typeparam>
    public abstract class Game<TGame, TGameStateMachine, TGameAction> : Game
        where TGame : Game<TGame, TGameStateMachine, TGameAction>
        where TGameStateMachine : GameStateMachine<TGameStateMachine, TGameAction>, new()
        where TGameAction : Enum
    {
        public static TGame Singleton = null;

        [TitleGroup("State Machine", Order = 1)]
        [HideInEditorMode, ShowInInspector, InlineProperty, HideReferenceObjectPicker, HideLabel]
        protected TGameStateMachine _stateMachine = null;

        [TitleGroup("Game Layer", Order = 2), SerializeField]
        private ManagerLayerDefinition _systemManagerLayerDefinition = null;

        [TitleGroup("Game Layer", Order = 3), SerializeField]
        private SuperDatabase _superDatabase = null;

        public TGameStateMachine StateMachine => this._stateMachine;

        public event Action<ManagerLayer> LayerLoaded;

        public event Action<ManagerLayer> LayerUnloaded;

        public event Action<GameState> GameStateEntered;

        public event Action<GameState> GameStateExited;

        public bool IsInState<TGameState>() where TGameState : GameState
        {
            if (this._stateMachine.CurrentState == null)
            {
                return false;
            }

            return this._stateMachine.CurrentState.GetType() == typeof(TGameState);
        }

        protected abstract void StartGame();

        protected abstract void StopGame();

        protected virtual void Startup()
        {
            this._stateMachine = new();
            this._stateMachine.EnterState += this.OnEnterState;
            this._stateMachine.ExitState += this.OnExitState;

            this._superManager = this.gameObject.AddComponent<SuperManager>();
            SuperManager.InitSingleton(this._superManager);

            this.LoadLayer(this._systemManagerLayerDefinition);

            this.StartGame();
        }

        protected virtual void Shutdown()
        {
            this.StopGame();

            this.UnloadLayer(this._systemManagerLayerDefinition);
            
            this._superManager = null;

            this._stateMachine.EnterState -= this.OnEnterState;
            this._stateMachine.ExitState -= this.OnExitState;
            this._stateMachine = null;
        }

        protected void Awake()
        {
            Singleton = (TGame)this;

            this.Startup();
        }

        protected void OnDestroy()
        {
            this.Shutdown();
        }

        public void LoadLayer(ManagerLayerDefinition managerLayerDefinition)
        {
            this._superManager.LoadLayer(managerLayerDefinition, this._superDatabase);
        }

        public void UnloadLayer(ManagerLayerDefinition managerLayerDefinition)
        {
            this._superManager.UnloadLayer(managerLayerDefinition);
        }

        protected virtual void OnEnterState(TGameStateMachine gameStateMachine, GameState gameState)
        {
            DebugHelper.Log(this, $"Entering {gameState.GetType().Name}");

            this.GameStateEntered?.Invoke(gameState);
        }

        protected virtual void OnExitState(TGameStateMachine gameStateMachine, GameState gameState)
        {
            this.GameStateEntered?.Invoke(gameState);

            DebugHelper.Log(this, $"Exiting {gameState.GetType().Name}");
        }
    }
}
