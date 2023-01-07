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
            if (transition.evaluate())
            {
                return transition.successState;
            }
        }

        return this;
    }

    public Transition addTransitionTo(State successState, Func<bool> condition = null)
    {
        var newTransition = new Transition(successState, condition);
        transitions.Add(newTransition);
        return newTransition;
    }
    
}