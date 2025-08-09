using System;
using Godot;


public partial class Npc : CharacterBody3D, IInteractable
{

    [Export]
    float Speed = 5.0f; // Set default speed
    [Export]
    float SpeedModifier = 1.5f; // Set default speed modifier
    float Gravity;

    public bool IsRunning;
    public bool IsStopped;
    public bool LookAtPlayer = true; // Make public so states can control it
    public float DistanceToPlayer;
    public Player Player;

    // IInteractable implementation
    [Export]
    public string interaction_prompt { get; set; } = "Talk";
    public bool is_interactable { get; set; } = true;

    Vector3 MoveDirection;
    float TargetRotation;

    public NavigationAgent3D NavAgent;


    public override void _Ready()
    {
        Gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();
        NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        Player = (Player)GetTree().GetNodesInGroup("Player")[0];

        // GD.Print("Player found: " + Player);
        // Wait for navigation map to be ready
        CallDeferred(nameof(SetupNavigation));
    }

    void SetupNavigation()
    {
        // Ensure navigation map is ready
        NavigationServer3D.MapSetActive(GetWorld3D().NavigationMap, true);
        GD.Print($"NPC Speed: {Speed}, Agent ready: {NavAgent != null}");
    }

    public override void _Process(double delta)
    {
        if (Player != null)
        {
            DistanceToPlayer = Position.DistanceTo(Player.Position);
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!IsOnFloor())
        {
            Velocity = new Vector3(Velocity.X, Velocity.Y - Gravity * (float)delta, Velocity.Z);
        }

        Vector3 nextPosition = NavAgent.GetNextPathPosition();
        Vector3 moveDirection = Position.DirectionTo(nextPosition);
        moveDirection.Y = 0;
        moveDirection = moveDirection.Normalized();

        if (NavAgent.IsNavigationFinished())
        {
            moveDirection = Vector3.Zero;
        }

        float currentSpeed = Speed;

        if (IsRunning)
        {
            currentSpeed = Speed * SpeedModifier;
        }

        Velocity = new Vector3(moveDirection.X * currentSpeed, Velocity.Y, moveDirection.Z * currentSpeed);

        MoveAndSlide();

        if (LookAtPlayer)
        {
            Vector3 directionToPlayer = (Player.Position - Position).Normalized();
            TargetRotation = Mathf.Atan2(directionToPlayer.X, directionToPlayer.Z);
        }
        else if (Velocity.Length() > 0)
        {
            TargetRotation = Mathf.Atan2(Velocity.X, Velocity.Z);
        }

        Rotation = new Vector3(Rotation.X, Mathf.LerpAngle(Rotation.Y, TargetRotation, 0.1f), Rotation.Z);
    }

    public void MoveToPosition(Vector3 toPosition, bool adjustPosition)
    {
        if (NavAgent == null)
        {
            GD.Print("NavAgent not found, getting it");
            NavAgent = GetNode<NavigationAgent3D>("NavigationAgent3D");
        }



        if (adjustPosition)
        {
            Rid navMesh = GetWorld3D().NavigationMap;
            Vector3 mapClosestPoint = NavigationServer3D.MapGetClosestPoint(navMesh, toPosition);
            NavAgent.TargetPosition = mapClosestPoint;
            // GD.Print($"Moving to map target: {mapClosestPoint} (original: {toPosition})");
        }
        else
        {
            NavAgent.TargetPosition = toPosition;
            // GD.Print($"Moving to position: {toPosition}");
        }
    }

    public void _Interact()
    {
        Label label = new Label();
        label.Text = "NPC *runs away*";
        AddChild(label);
        label.Position = new Vector2(100, 100);
        label.Visible = true;
        label.Modulate = new Color(1, 1, 1, 1);
        label.AddThemeFontSizeOverride("font_size", 36);

        // Destroy label after 3 seconds
        GetTree().CreateTimer(3.0f).Timeout += () =>
        {
            label.QueueFree();
        };

        // Transition to flee state
        StateMachine stateMachine = GetNode<StateMachine>("StateMachine");
        stateMachine.ChangeState("Flee");
    }

}
