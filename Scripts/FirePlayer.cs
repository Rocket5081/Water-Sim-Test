using Godot;

public partial class FirePlayer : Player
{
    [Export] public Node3D FlameVFX;
    [Export] public GpuParticles3D FlameParticles;
    [Export] public GpuParticles3D Sparks;

    [Export] public float ShrinkDuration = 30.0f; // total lifespan in seconds
    [Export] public Vector3 StartScale = Vector3.One;
    [Export] public Vector3 EndScale = Vector3.Zero;

    private float totalDuration;
    private float timeRemaining;
    private bool active = false;

    public override void _Ready()
    {
        StartShrink();
    }

    public override void _PhysicsProcess(double delta)
    {

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

        base._PhysicsProcess(delta); 
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
}