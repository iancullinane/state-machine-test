using Godot;
using System;


public partial class WanderState : State
{

    [Export]
    public Vector3 HomePosition;
    [Export]
    public float WanderRadius = 10f;

    [Export]
    public float MinWaitTime = .5f;
    [Export]
    public float MaxWaitTime = 1.5f;
    [Export]
    public float ChaseRange = 5f;

    // [Export]
    // public float _wanderJitter = 1f;
    // [Export]
    // public float _wanderRate = 1f;

    public override void Enter()
    {
        base.Enter();
        HomePosition = Npc.Position;

        // Don't look at player while wandering - look in movement direction instead
        Npc.LookAtPlayer = false;

        // Wait a short time for navigation to be fully ready before first wander
        GetTree().CreateTimer(1.5f).Timeout += _newWanderPosition;
    }

    public override void Exit()
    {
        base.Exit();

        // Restore looking at player when exiting wander state
        Npc.LookAtPlayer = true;
    }

    public override void Update(double delta)
    {
        // Check if player is within chase range
        if (Npc.DistanceToPlayer <= ChaseRange)
        {
            GD.Print($"Player within chase range ({Npc.DistanceToPlayer:F2}m) - switching to chase");
            GetParent<StateMachine>().ChangeState("Chase");
        }
    }

    public override void NavigationComplete()
    {
        // Wait a bit before moving to next position
        GetTree().CreateTimer(GD.RandRange(MinWaitTime, MaxWaitTime)).Timeout += _newWanderPosition;
    }

    void _newWanderPosition()
    {
        Vector3 randomPosition =
            HomePosition +
            RandomOffset() *
            (float)GD.RandRange(0, WanderRadius);
        Npc.MoveToPosition(randomPosition, true);
    }
}
