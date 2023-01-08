using System;
using System.Collections;
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
        public GameObject fadeToBlack;
        public DamageFlash damageFlash;
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
        /// Reload current active scene. In most cases, this means reloading the level
        /// Scene loading is not synchronous
        /// There is a one frame delay
        /// Do not assume you will get access to the scene vars immediately
        /// pass a callback if you need run anything immediately after loading
        /// This will also send the CallbackBeforeActiveSceneChanges event
        /// </summary>
        public void reloadLevel(Action callback = null)
        {
            CallbackBeforeActiveSceneChanges?.Invoke();
            if (callback != null)
            {
                SceneVarTracker.Instance.queueCallbackForSceneReload(callback);
            }
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        /// <summary>
        /// Load arbitrary level 
        /// Scene loading is not synchronous
        /// There is a one frame delay
        /// Do not assume you will get access to the scene vars immediately
        /// pass a callback if you need run anything immediately after loading
        /// This will also send the CallbackBeforeActiveSceneChanges event
        /// </summary>
        public void loadScene(string sceneName, Action callback = null)
        {
            CallbackBeforeActiveSceneChanges?.Invoke();
            if (callback != null)
            {
                SceneVarTracker.Instance.queueCallbackForSceneReload(callback);
            }
            SceneManager.LoadScene(sceneName);
            StartCoroutine(dofadeToBlack());
        }

        IEnumerator dofadeToBlack()
        {
            fadeToBlack.SetActive(true);
            yield return null;//wait one frame
            fadeToBlack.SetActive(false);
        }

        public event Action CallbackBeforeActiveSceneChanges;
    }
}