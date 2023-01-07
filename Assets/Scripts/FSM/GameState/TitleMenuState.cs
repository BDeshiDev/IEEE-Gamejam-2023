using Sound;
using UnityEngine.SceneManagement;

namespace FSM.GameState
{
    public class TitleMenuState: State
    {
        public override void enterState(State prev)
        {
            if (SceneManager.GetActiveScene().name != "TitleScene")
            {
                GameStateManager.Instance.loadScene("TitleScene");

            }
            
            BGMManager.Instance.stopPlaying();
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
            
        }
    }
    
    
}