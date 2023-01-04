using System;
using BDeshi.Utility;
using Core.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FSM.GameState
{
    public class GameStateManager : MonoBehaviourSingletonPersistent<GameStateManager>
    {
        public FiniteStateMachine fsm;

        public State gameplayState;
        public State pauseMenuState;
        public State gameOverState;
        public State titleScreenState;
        
        public static string initialStateID = null;
        public static readonly string gameplayStateID = "gameplayState";
        public static readonly string pauseMenuStateID = "pauseMenuState";
        public static readonly string gameOverStateID = "gameOverState";
        public static readonly string titleScreenStateID = "titleScreenState";

        public bool IsPaused { get; private set; }

        /// <summary>
        /// toggles the bool, does not change state
        /// Transition to pausemenustate if you want to actually go to the pause menu
        /// </summary>
        /// <param name="shouldBePaused"></param>
        public void togglePause(bool shouldBePaused)
        {
            IsPaused = shouldBePaused;
        }
        
        // ugly but whatever works. also switch case has issues  with strings so if else
        protected override void initialize()
        {
            if (initialStateID == gameplayStateID)
            {
                fsm.transitionToState(gameplayState);
            }else if (initialStateID == pauseMenuStateID)
            {
                fsm.transitionToState(pauseMenuState);
            }else if (initialStateID == gameOverStateID)
            {
                fsm.transitionToState(gameOverState);
            }else if (initialStateID == titleScreenStateID)
            {
                fsm.transitionToState(titleScreenState);
            }else if (initialStateID == gameOverStateID)
            {
                fsm.transitionToState(gameOverState);
            }
            else
            {
                Debug.Log("unsupported initialStateID = " + initialStateID);
            }
        }
        
        /// <summary>
        /// Scene loading is not synchronous
        /// There is a one frame delay
        /// Do not assume you will get access to the scene vars immediately
        /// Send a callback if needed
        /// This will also send the CallbackBeforeActiveChange event
        /// </summary>
        public void reloadLevel(Action callback = null)
        {
            CallbackBeforeActiveChange?.Invoke();
            if (callback != null)
            {
                SceneVarTracker.Instance.queueCallbackForSceneReload(callback);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public event Action CallbackBeforeActiveChange;
    }
}