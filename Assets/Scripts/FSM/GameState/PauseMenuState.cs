using UnityEngine;

namespace FSM.GameState
{
    public class PauseMenuState : ModularState
    {
        public GameObject pauseMenu;
        
        


        public override void enterState(State prev)
        {
            pauseMenu.SetActive(true);
            GameStateManager.Instance.togglePause(true);
        }



        public override void updateState()
        {
            //we just wait for the return button to be clicked
        }



        public override void exitState(State next)
        {
            pauseMenu.SetActive(false);
            GameStateManager.Instance.togglePause(false);
        }
    }
}