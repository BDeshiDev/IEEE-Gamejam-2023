using UnityEngine;

public class FiniteStateMachine : MonoBehaviour
{
    [SerializeField] private State curState;
    [SerializeField] private State startingState;

    public void transitionToState(State newState)
    {
        if (newState != curState)
        {
            Debug.Log("newState = " + newState);

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

    private void Start()
    {
        transitionToState(startingState);
    }

    private void Update()
    {
        curState.updateState();
        transitionToState(curState.getNextState());
    }
}