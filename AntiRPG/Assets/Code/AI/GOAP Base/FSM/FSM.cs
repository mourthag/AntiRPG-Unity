using System.Collections.Generic;
using UnityEngine;


public class FSM
{
    public delegate void FsmState(GameObject obj, FSM fsm);

    private readonly Stack<FsmState> _actionStack = new Stack<FsmState>();

    public void Update(GameObject obj)
    {
        if (_actionStack.Peek() != null)
        {
            _actionStack.Peek().Invoke(obj, this);
        }
    }

    public void PushState(FsmState state)
    {
        _actionStack.Push(state);
    }

    public void PopState()
    {
        _actionStack.Pop();
    }
}

