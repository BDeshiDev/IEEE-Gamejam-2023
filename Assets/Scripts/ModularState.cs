using System;
using System.Collections.Generic;

/// <summary>
/// States but with a list of transitions you can append to
/// In most cases, states from different FSMs(enemy AI)
/// can share states but have different transitions
/// So they have been separated
/// </summary>
public abstract class ModularState : State
{
    private List<Transition> transitions = new List<Transition>();
    public override State getNextState()
    {
        foreach (var transition in transitions)
        {
            if (transition.condition())
            {
                return transition.successState;
            }
        }

        return this;
    }

    public void addTransitionTo(State successState, Func<bool> condition)
    {
        transitions.Add(new Transition(condition, successState));
    }
}