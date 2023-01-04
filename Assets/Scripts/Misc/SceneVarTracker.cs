using System;
using System.Collections.Generic;
using BDeshi.Utility;
using FSM.GameState;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Misc
{
    /// <summary>
    /// Guaranteed to initialize but can't have in-scene refs
    /// </summary>
    public class SceneVarTracker: MonoBehaviourLazySingleton<SceneVarTracker>
    {
        
        public PlayerEntity Player => player;
        [SerializeField] private  PlayerEntity player;
        public Camera Camera=>camera;
        [SerializeField] Camera camera;
        
        private List<Action> temporaryCallbacksWaitingForSceneReload = new List<Action>();
        public event Action onSceneVarsFetched;
        public void queueCallbackForSceneReload(Action callback)
        {
            temporaryCallbacksWaitingForSceneReload.Add(callback);
        }
        /// <summary>
        /// This gets called before Instance is accessed
        /// </summary>
        protected override void initialize()
        {
            refetchSceneVars();

            SceneManager.sceneLoaded += handleSceneLoaded;

            foreach (var callback in temporaryCallbacksWaitingForSceneReload)
            {
                callback();
            }
            
            temporaryCallbacksWaitingForSceneReload.Clear();
        }

        private void handleSceneLoaded(Scene s, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)//active scene has changed, refetch
            {
                refetchSceneVars();
            }
        }


        private void OnDestroy()
        {
            if (GameStateManager.Instance != null)
            {
                SceneManager.sceneLoaded  -= handleSceneLoaded;
            }
        }

        private void refetchSceneVars()
        {
            player = GameObject.FindWithTag("Player").GetComponent<PlayerEntity>();
            camera = Camera.main;
            
            onSceneVarsFetched?.Invoke();
        }


    }
}