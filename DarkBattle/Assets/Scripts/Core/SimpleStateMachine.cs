using UnityEngine;
using System.Collections;
using System;

public class SimpleState
{
    public Action onEnter;
    public Action onLeave;
    public Action onUpdate;
}

public class SimpleStateMachine
{
    private SimpleState _state;
    public SimpleState State
    {
        get
        {
            return _state;
        }
        set
        {
            if (_state != null && _state.onLeave != null) _state.onLeave();
            _state = value;
            if (_state != null && _state.onEnter != null) _state.onEnter();
        }
    }
}
