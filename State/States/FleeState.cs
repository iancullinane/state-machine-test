using Godot;
using System;

public partial class FleeState : State
{

    float fleeRange = 5f;

    public override void Enter()
    {
        base.Enter();

        // Set NPC to running
        Npc.IsRunning = true;
        Npc.IsStopped = false;
        Npc.LookAtPlayer = false;

        // Get opposite direction of current facing
        Vector3 fleeDirection = -new Vector3(
            Mathf.Sin(Npc.Rotation.Y),
            0,
            Mathf.Cos(Npc.Rotation.Y)
        );

        // Calculate flee position by moving in opposite direction
        Vector3 fleePosition = Npc.Position + (fleeDirection * fleeRange);

        // Move to the flee position
        Npc.MoveToPosition(fleePosition, true);
    }

    public override void Exit()
    {
        base.Exit();

        // Stop running when exiting flee state
        Npc.IsRunning = false;
    }

    public override void NavigationComplete()
    {
        base.NavigationComplete();
        GetParent<StateMachine>().ChangeState("Wander");
    }
}