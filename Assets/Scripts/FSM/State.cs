using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract void enterState(State prev);
    public abstract void updateState();
    public abstract State getNextState();
    public abstract void exitState(State next);
}