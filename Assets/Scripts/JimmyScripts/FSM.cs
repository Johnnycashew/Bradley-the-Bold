using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM : MonoBehaviour
{
    State state;

    public void SetState(State newState)
    {
        state = newState;
    }

    public State GetState()
    {
        return state;
    }
}
