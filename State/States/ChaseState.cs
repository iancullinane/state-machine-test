using Godot;
using System;

public partial class ChaseState : State
{
    [Export]
    float StopRange = 1.0f;
    [Export]
    float LoseInterestRange = 10.0f;


    double _pathUpdateRate = 0.5f;
    double _lastPathUpdateTime = 0.0f;

    public override void Initialize()
    {
        base.Initialize();
        // Fallback to find NPC if NpcNodePath wasn't set in the scene
        if (Npc == null)
        {
            Npc = GetNode<Npc>("../../");
        }
    }

    public override void Enter()
    {
        base.Enter();
        Npc.IsRunning = true;
        Npc.LookAtPlayer = true;
    }

    public override void Exit()
    {
        base.Exit();
        Npc.IsRunning = false;
        // Npc.LookAtPlayer = false;
    }

    public override void Update(double delta)
    {
        // Check if player has moved too far away - return to wandering
        if (Npc.DistanceToPlayer > LoseInterestRange)
        {
            GD.Print($"Player too far away ({Npc.DistanceToPlayer:F2}m) - returning to wander");
            GetParent<StateMachine>().ChangeState("Wander");
            return;
        }

        // Check if we're too close to the player
        if (Npc.DistanceToPlayer < StopRange)
        {
            if (!Npc.IsStopped)
            {
                GD.Print("Stopping - too close to player");
                Npc.IsStopped = true;
                // Clear the navigation target to stop movement
                Npc.NavAgent.TargetPosition = Npc.Position;
            }
            return; // Don't update path when stopped
        }
        else
        {
            // Resume movement if we were stopped but now far enough away
            if (Npc.IsStopped)
            {
                GD.Print("Resuming chase");
                Npc.IsStopped = false;
            }
        }

        // Update path to player
        double currentTime = Time.GetUnixTimeFromSystem();
        if (currentTime - _lastPathUpdateTime > _pathUpdateRate)
        {
            GD.Print("Updating path to player");
            _lastPathUpdateTime = currentTime;
            Npc.MoveToPosition(Npc.Player.Position, true);
        }
    }


}
