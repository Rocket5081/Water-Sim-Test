using Godot;
using System;

public partial class CharacterBody3d : CharacterBody3D
{
    public const float Speed = 5.0f;
    public const float JumpVelocity = 4.5f;

    [Export] public bool IsInWater = false;

    // Height of the water surface.
    private float waterSurfaceY;

    // How strongly the player is pulled toward the surface.
    [Export] public float WaterFloatStrength = 5.0f;

    // Maximum vertical speed while floating.
    [Export] public float MaxWaterVerticalSpeed = 3.0f;

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        float deltaFloat = (float)delta;

        // --------------------------------------------------
        // GRAVITY
        // --------------------------------------------------

        // Only apply normal gravity when NOT in water.
        if (!IsOnFloor() && !IsInWater)
        {
            velocity += GetGravity() * deltaFloat;
        }

        // --------------------------------------------------
        // WATER FLOATING
        // --------------------------------------------------

        if (IsInWater)
        {
            // Difference between the water surface and player.
            float difference = waterSurfaceY - GlobalPosition.Y;

            // Pull the player toward the surface.
            velocity.Y = difference * WaterFloatStrength;

            // Prevent the player from moving vertically too quickly.
            velocity.Y = Mathf.Clamp(
                velocity.Y,
                -MaxWaterVerticalSpeed,
                MaxWaterVerticalSpeed
            );
        }

        // --------------------------------------------------
        // JUMP
        // --------------------------------------------------

        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
        {
            velocity.Y = JumpVelocity;
        }

        // --------------------------------------------------
        // MOVEMENT
        // --------------------------------------------------

        Vector2 inputDir = Input.GetVector(
            "ui_left",
            "ui_right",
            "ui_up",
            "ui_down"
        );

        Vector3 direction =
            (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y))
            .Normalized();

        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(
                velocity.X,
                0,
                Speed * deltaFloat
            );

            velocity.Z = Mathf.MoveToward(
                velocity.Z,
                0,
                Speed * deltaFloat
            );
        }

        // --------------------------------------------------
        // APPLY MOVEMENT
        // --------------------------------------------------

        Velocity = velocity;

        MoveAndSlide();
    }

    // Called by the WaterArea when the player enters the water.
    public void SetWaterSurface(float surfaceY)
    {
        waterSurfaceY = surfaceY;
        IsInWater = true;
    }

    // Called by the WaterArea when the player leaves the water.
    public void ExitWater()
    {
        IsInWater = false;
    }
}