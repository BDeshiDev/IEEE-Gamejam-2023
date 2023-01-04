using System;
using UnityEngine;

namespace FSM.GameState
{
    public class GameOverState : ModularState
    {
        public GameObject gameOverMenu;

        private void Start()
        {
            PlayerEntity.playerDied.add(gameObject, handlePlayerDeath);
        }

        private void handlePlayerDeath(PlayerEntity obj)
        {
            GameStateManager.Instance.fsm.transitionToState(this);
        }

        public override void enterState(State prev)
        {
            gameOverMenu.SetActive(true);
            GameStateManager.Instance.togglePause(true);
        }



        public override void updateState()
        {
            //we just wait for the button to be clicked
        }

        public void handleRestartClicked()
        {
            GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.gameplayState);
        }


        public override void exitState(State next)
        {
            gameOverMenu.SetActive(false);
            GameStateManager.Instance.togglePause(false);
        }
    }
}