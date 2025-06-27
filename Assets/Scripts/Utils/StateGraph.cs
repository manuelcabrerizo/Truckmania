using System.Collections.Generic;

public class StateGraph<Type>
{
    Dictionary<State<Type>, List<State<Type>>> stateTransitions;

    public StateGraph()
    { 
        stateTransitions = new Dictionary<State<Type>, List<State<Type>>>();
    }

    public void AddStateTransitions(State<Type> state, List<State<Type>> states)
    { 
        stateTransitions.Add(state, states);
    }

    public bool IsValid(State<Type> currentState, State<Type> targetState)
    {
        if (stateTransitions.ContainsKey(currentState))
        {
            List<State<Type>> states = stateTransitions[currentState];
            foreach (var state in states)
            {
                if (state == targetState)
                {
                    return true;
                }
            }
        }
        return false;
    }
}