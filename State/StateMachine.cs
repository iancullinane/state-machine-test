using System;
using System.Collections.Generic;
using Godot;

public partial class StateMachine : Node
{
    [Export]
    public State _defaultState;
    State _currentState;
    Dictionary<string, State> _states;

    public override void _Ready()
    {

        // Get the NavigationServer3D
        _states = new Dictionary<string, State>();
        foreach (Node child in GetChildren())
        {
            if (child is State state)
            {
                _states[child.Name] = state;
                state.Initialize();
            }
        }

        if (_defaultState != null)
        {
            ChangeState(_defaultState.Name);
        }

    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (_currentState != null)
        {
            _currentState.Update(delta);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_currentState != null)
        {
            _currentState._PhysicsProcess(delta);
        }
    }

    public void ChangeState(string stateName)
    {
        State newState = _states[stateName];
        if (newState == null)
        {
            GD.PrintErr("State not found: " + stateName);
            return;
        }

        if (newState == _currentState)
        {
            return;
        }

        _currentState?.Exit();

        _currentState = newState;
        _currentState.Enter();
        // GD.Print("Changed to state: " + stateName);
    }



    public void OnNavigationAgent3dTargetReached()
    {
        if (_currentState != null)
        {
            _currentState.NavigationComplete();
        }

    }

}
