using UnityEngine;

namespace FSM.GameState
{
    /// <summary>
    /// Set gamestate from a gameobject
    /// Allows changing gamestates from unity events
    /// As they cannot access singletons normally
    /// </summary>
    public class GameStateChangeHelper: MonoBehaviour
    {
        public void setInitialStateToGameplayState()
        {
            GameStateManager.initialStateID = GameStateManager.gameplayStateID;
        }

        
        public void setInitialStateToGameoverState()
        {
            GameStateManager.initialStateID = GameStateManager.gameOverStateID;
        }
        
        public void setInitialStateToPauseMenuState()
        {
            GameStateManager.initialStateID = GameStateManager.pauseMenuStateID;

        }
        
        public void setInitialStateToTitleMenuState()
        {
            GameStateManager.initialStateID = GameStateManager.titleScreenStateID;

        }
        
        public void transitionToGameplayState()
        {
            GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.gameplayState);
        }

        
        public void transitionToGameoverState()
        {
            GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.gameOverState);
        }
        
        public void transitionToPauseMenuState()
        {
            GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.pauseMenuState);

        }
        
        public void transitionToTitleMenuState()
        {
            GameStateManager.Instance.fsm.transitionToState(GameStateManager.Instance.titleScreenState);

        }
    }
}