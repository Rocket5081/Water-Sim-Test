using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 10.0f;
	public const float JumpVelocity = 6.5f;
	[Export] public Node3D FlameVFX;
	[Export] public GpuParticles3D FlameParticles;
	[Export] public GpuParticles3D Sparks;

	[Export] public float ShrinkDuration = 30.0f; // total lifespan in seconds
    [Export] public Vector3 StartScale = Vector3.One;
    [Export] public Vector3 EndScale = Vector3.Zero;

    private float totalDuration;
    private float timeRemaining;
    private bool active = false;

	[Export] public bool IsInWater = false;

    private float waterSurfaceY;

    [Export] public float WaterFloatStrength = 5.0f;

    [Export] public float MaxWaterVerticalSpeed = 3.0f;

	public override void _Ready(){
		StartShrink();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;

		if(Input.IsActionJustPressed("exit")){
			GetTree().Quit();
		}

		if (!active) return;

        timeRemaining -= (float)delta;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            active = false;
            FlameVFX.Visible = false;
            return;
        }

        float t = 1f - (timeRemaining / totalDuration);
        FlameVFX.Scale = StartScale.Lerp(EndScale, t);

		float amountRatio = Mathf.Lerp(1.0f, 0.0f, t);
        FlameParticles.AmountRatio = amountRatio;
        Sparks.AmountRatio = amountRatio;

		// Add the gravity.
		if (IsInWater)
    	{
        	float difference = waterSurfaceY - GlobalPosition.Y;

        	velocity.Y = difference * WaterFloatStrength;

        	velocity.Y = Mathf.Clamp(
            	velocity.Y,
            	-MaxWaterVerticalSpeed,
           		MaxWaterVerticalSpeed
        	);
    	}
    	else if (!IsOnFloor())
    	{
        	velocity += GetGravity() * (float)delta;
    	}

		// Handle Jump.
		if (Input.IsActionJustPressed("jump") && IsOnFloor())
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "forward", "back");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
		if (direction != Vector3.Zero)
		{
			velocity.X = direction.X * Speed;
			velocity.Z = direction.Z * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	public void StartShrink()
    {
        totalDuration = ShrinkDuration;
        timeRemaining = ShrinkDuration;
        active = true;
        FlameVFX.Scale = StartScale;
		FlameParticles.AmountRatio = 1.0f;
		Sparks.AmountRatio = 1.0f;
        FlameVFX.Visible = true;
    }

	public void AddTime(float bonusSeconds)
    {
        timeRemaining = Mathf.Min(timeRemaining + bonusSeconds, totalDuration);
        // clamped so pickups can't scale the particle bigger than StartScale
    }

    public void SetWaterSurface(float surfaceY)
    {
        waterSurfaceY = surfaceY;
        IsInWater = true;
    }

    public void ExitWater()
    {
        IsInWater = false;
    }
}
