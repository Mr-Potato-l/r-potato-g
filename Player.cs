using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Player : CharacterBody3D
{
    [Export]
    public int MoveSpeed { get; set; } = 8;

    [Export]
    public int FallAcceleration { get; set; } = 65;

    [Export]
    public int JumpImpulse { get; set; } = 20;

    [Export]
    public float MouseSensitivity = 0.003f;

    [Export]
    public float TiltLimit = 30.0f;

    private Vector3 _targetVelocity = Vector3.Zero;
    private Camera3D camera;
    private Node3D cameraPivot;
    private float rotation;

    public override void _Ready()
    {
        cameraPivot = GetNode<Node3D>("CameraPivot");
        camera = GetNode<Camera3D>("CameraPivot/Camera3D");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion motion)
        {
            RotateY(-motion.Relative.X * MouseSensitivity);
            cameraPivot.RotateX(-motion.Relative.Y * MouseSensitivity);

            Vector3 cameraRotation = cameraPivot.Rotation;
            cameraRotation.X = Mathf.Clamp(
                cameraRotation.X, Mathf.DegToRad(-TiltLimit), Mathf.DegToRad(TiltLimit)
            );
            cameraPivot.Rotation = cameraRotation;
            Input.MouseMode = Input.MouseModeEnum.Captured;
        }

        if (@event.IsActionPressed("exit"))
        {
            GetTree().Quit();
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        if (!IsOnFloor())
        {
            velocity.Y -= FallAcceleration * (float)delta;
        }

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = JumpImpulse;
        }

        Vector2 inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_back");
        Vector3 direction = (cameraPivot.GlobalTransform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * MoveSpeed;
            velocity.Z = direction.Z * MoveSpeed;
        }
        else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, MoveSpeed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, MoveSpeed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    
}
