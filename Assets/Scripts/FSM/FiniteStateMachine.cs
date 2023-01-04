using FSM.GameState;
using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField] private State curState;
    [SerializeField] private State startingState;
    [SerializeField] private bool pausable;
    public void transitionToState(State newState)
    {
        if (newState != curState)
        {
            Debug.Log("curState "+ curState + " -> " + newState,gameObject);

            if (newState != null)
            {
                var prevState = curState;
                if (prevState != null)
                {
                    prevState.exitState(newState);
                }
                curState = newState;
                curState.enterState(prevState); 
            }
                                 
        }
    }
    /// <summary>
    /// Trnasition to the transitions success state without checking the conditions 
    /// </summary>
    /// <param name="transition"></param>
    public void forceTakeTransition(Transition transition)
    {
        transitionToState(transition.successState);
    }

    private void Start()
    {
        transitionToState(startingState);
    }

    private void Update()
    {
        if (pausable && GameStateManager.Instance.IsPaused)
        {
            return;
        }
        if (curState != null)
        {
            curState.updateState();
            transitionToState(curState.getNextState());
        }

    }
}