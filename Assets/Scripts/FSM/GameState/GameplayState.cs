using System;
using Core.Misc;
using UnityEngine;

namespace FSM.GameState
{
    public class GameplayState: State
    {

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
            SceneVarTracker.Instance.Player.camController.enabled = false;
        }
    }
}