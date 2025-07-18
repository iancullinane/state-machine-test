using Godot;
using System;

public partial class FleeState : State
{
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
        Vector3 fleePosition = Npc.Position + (fleeDirection * 20f);

        // Move to the flee position
        Npc.MoveToPosition(fleePosition, true);

        // After 3 seconds, return to wander state
        GetTree().CreateTimer(3.0f).Timeout += () =>
        {
            GetParent<StateMachine>().ChangeState("Wander");
        };
    }

    public override void Exit()
    {
        base.Exit();

        // Stop running when exiting flee state
        Npc.IsRunning = false;
    }
}