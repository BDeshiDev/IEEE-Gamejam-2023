using System;
using Core.Input;
using Core.Misc;
using Sound;
using UnityEngine;

namespace FSM.GameState
{
    public class GameplayState: State
    {
        [SerializeField]private GameObject cursor;
        private void Start()
        {
            InputManager.pauseButton.addPerformedCallback(gameObject, handlePauseToggle);
        }

        private void handlePauseToggle()
        {
            if (GameStateManager.Instance.fsm.CurState == GameStateManager.Instance.gameplayState)
            {
                GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.pauseMenuState);
            }
            else
            {
                GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.gameplayState);
            }
        }

        public override void enterState(State prev)
        {
            if (prev is GameOverState)
            {
                GameStateManager.Instance.reloadLevel(doSceneVariableInitialization);
            }
            else
            {
                doSceneVariableInitialization();
            }
            
            cursor.SetActive(true);
            
            BGMManager.Instance.playBGM();
        }

        void doSceneVariableInitialization()
        {
            SceneVarTracker.Instance.Player.camController.enabled = true;
        }

        public override void updateState()
        {
           
        }

        public override State getNextState()
        {
            return this;
        }

        public override void exitState(State next)
        {
            cursor.SetActive(false);
            if (SceneVarTracker.Instance.Player != null)
            {
                SceneVarTracker.Instance.Player.camController.enabled = false;
            }
        }
    }
}