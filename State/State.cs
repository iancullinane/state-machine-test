using System;
using System.Collections.Generic;
using Godot;

public partial class State : Node
{

    StateMachine _stateMachine;

    bool _active;
    public Npc Npc;


    [Export]
    public NodePath NpcNodePath;




    public virtual void Initialize()
    {
        _stateMachine = (StateMachine)GetParent();
        Npc = GetNode<Npc>(NpcNodePath);
    }

    public virtual void Enter()
    {
        _active = true;
    }

    public virtual void Exit()
    {
        _active = false;
    }

    public virtual void Update(double delta)
    {

    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
    }

    public virtual void NavigationComplete()
    {

    }

    public Vector3 RandomOffset()
    {
        Vector3 offset = new Vector3((float)GD.RandRange(-1.0, 1.0), 0, (float)GD.RandRange(-1.0, 1.0));
        return offset.Normalized();
    }

}
