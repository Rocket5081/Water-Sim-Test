using Godot;
using System;

public partial class Player : CharacterBody3D
{
	public const float Speed = 10.0f;
	public const float JumpVelocity = 9.5f;

	[Export] public bool IsInWater = false;
	[Export] public AnimationPlayer Anim;

    private float waterSurfaceY;

	[Export] public float WaterFloatStrength = 1.0f;    
	[Export] public float MaxWaterVerticalSpeed = 2.0f; 
	[Export] public float WaterFloatOffset = -2.0f;

	public override void _Ready(){
		
	}

	public override void _PhysicsProcess(double delta)
	{
		
		Vector3 velocity = Velocity;

		if(Input.IsActionJustPressed("exit")){
			GetTree().Quit();
		}

		if (IsInWater)
		{
    		float targetY = waterSurfaceY + WaterFloatOffset;
    		float difference = targetY - GlobalPosition.Y;
    		velocity.Y = Mathf.Clamp(difference * WaterFloatStrength, -MaxWaterVerticalSpeed, MaxWaterVerticalSpeed);
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
		UpdateAnimation(direction);
	}

	private void UpdateAnimation(Vector3 direction)
	{
		string next;

		if (IsInWater)
    		next = direction != Vector3.Zero ? "Swim" : "Idle";
		else if (!IsOnFloor())
    		next = Velocity.Y > 0.5f ? "Jump" : "Fall";
    	else if (direction != Vector3.Zero)
    	    next = "WalkCycle";
    	else
    	    next = "Idle";

    	if (Anim.CurrentAnimation != next)
    	    Anim.Play(next, 0.2); 
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
