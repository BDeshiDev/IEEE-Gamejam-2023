using System;

public class Transition
{
    public readonly Func<bool> condition;
    public State successState;

    public Transition(Func<bool> condition, State successState)
    {
        this.condition = condition;
        this.successState = successState;
    }
}