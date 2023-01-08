using System;
using System.Collections.Generic;
using BDeshi.Utility;
using FSM.GameState;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Misc
{
    /// <summary>
    /// Contains and maintains refs to in-scene objects that change depending on the scene 
    /// Guaranteed to initialize but can't have in-scene refs
    /// </summary>
    public class SceneVarTracker: MonoBehaviourLazySingleton<SceneVarTracker>
    {
        public static readonly string PlayerTag = "Player";

        /// <summary>
        /// Can be null for scenes that do not have the player
        /// </summary>
        [CanBeNull]public PlayerEntity Player => player;
        [SerializeField] private  PlayerEntity player;
        /// <summary>
        /// Can be null for scenes that are not levels
        /// </summary>
        [CanBeNull]public LevelData CurLevelData => curLevelData;
        [SerializeField] private LevelData curLevelData;
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
            Debug.Log("loaded" + mode);
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

        public TComponent findAndGetComponent<TComponent>(string tag) where TComponent : Component
        {
            var go = GameObject.FindWithTag(tag);
            if (go == null)
            {
                return null;
            }

            return go.GetComponent<TComponent>();
        }

        private void refetchSceneVars()
        {
            Debug.Log("refetch");
            player = findAndGetComponent<PlayerEntity>(PlayerTag);
            camera = Camera.main;
            curLevelData = findAndGetComponent<LevelData>("LevelData");
            
            onSceneVarsFetched?.Invoke();
        }


    }
}