using System;

public class Transition
{
    readonly Func<bool> condition;
    public State successState;

    public bool evaluate()
    {
        // if no condition assume false
        // this is useful when we only want to trigger this transition manually
        // Ex: gameoverstate -> gameplaystate transitions happens through button
        // if we check a condition for it in update() it will be tied to unity's update system,
        // it can result in delay of one frame. 
        // That doesn't matter for gameplay but it prevent synchronous state transitions
        if (condition == null)
            return false;
        return condition();
    }

    public Transition(State successState, Func<bool> condition = null)
    {
        this.condition = condition;
        this.successState = successState;
    }
}