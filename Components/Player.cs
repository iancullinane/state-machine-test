using System;
using Godot;

public partial class Player : CharacterBody3D
{

    [ExportGroup("Movement")]
    [Export]
    public float max_speed = 4.0f;
    [Export]
    public float acceleration = 20.0f;

    float gravity;

    [Export]
    public float braking = 20.0f;
    [Export]
    public float air_acceleration = 40f;
    [Export]
    public float jump_force = 5.0f;
    [Export]
    public float gravity_modifier = 1.5f;
    [Export]
    public float max_run_speed = 6.0f;

    bool is_running = false;

    [ExportGroup("Camera")]
    [Export]
    float look_sensitivity = 0.005f;
    Camera3D camera;

    Vector2 camera_look_input;
    float camera_x_rotation = 0.0f;

    public override void _Ready()
    {
        camera = GetNode<Camera3D>("PlayerCamera");
        gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle() * gravity_modifier;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Apply gravity
        if (!IsOnFloor())
        {
            Velocity = new Vector3(Velocity.X, Velocity.Y - gravity * (float)delta, Velocity.Z);
        }

        // Jumping
        if (Input.IsActionPressed("jump") && IsOnFloor())
        {
            GD.Print("jump");
            Velocity = new Vector3(Velocity.X, jump_force, Velocity.Z);
        }

        is_running = Input.IsActionPressed("sprint");

        CalculateDirection();
        MoveAndSlide();


        if (Input.IsActionJustPressed("ui_cancel"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured
                ? Input.MouseModeEnum.Visible
                : Input.MouseModeEnum.Captured;
        }
    }

    public void CalculateDirection()
    {
        float speed = max_speed;

        // Get movement input
        Vector2 move_input =
            Input.GetVector(
                "move_left",
                "move_right",
                "move_forward",
                "move_backward"
            );
        Vector3 move_direction = (Transform.Basis * new Vector3(move_input.X, 0, move_input.Y)).Normalized();

        if (is_running)
        {
            speed = max_run_speed;
            float run_dot = -move_direction.Dot(Transform.Basis.Z);  // Forward direction
            move_direction *= Mathf.Clamp(run_dot, 0f, 1f);
        }

        float current_smoothing = acceleration;
        if (!IsOnFloor())
        {
            current_smoothing = air_acceleration;
        }
        else if (move_direction.Length() < 0.1f)
        {
            current_smoothing = braking;
        }

        // Lerp to target velocity with smoothing
        Vector3 target_velocity = move_direction * speed;
        Vector3 current_horizontal = new Vector3(Velocity.X, 0, Velocity.Z);
        Vector3 smoothed_horizontal = current_horizontal.Lerp(target_velocity, current_smoothing * (float)GetPhysicsProcessDeltaTime());

        Velocity = new Vector3(smoothed_horizontal.X, Velocity.Y, smoothed_horizontal.Z);

        // Camera rotation - store and apply rotations directly
        RotateY(-camera_look_input.X * look_sensitivity);
        camera_x_rotation -= camera_look_input.Y * look_sensitivity;
        camera_x_rotation = Mathf.Clamp(camera_x_rotation, Mathf.DegToRad(-90), Mathf.DegToRad(90));

        // Apply camera X rotation directly (no accumulation)
        camera.Rotation = new Vector3(camera_x_rotation, 0, 0);

        camera_look_input = Vector2.Zero;
    }

    public override void _UnhandledInput(InputEvent @event)
    {

        if (@event is InputEventMouseMotion eventMouseMotion)
            camera_look_input = eventMouseMotion.Relative;
    }


}
