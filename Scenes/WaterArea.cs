using Godot;

public partial class WaterArea : Area3D
{
    [Export] public float SurfaceOffset = 0.0f;

    private void OnBodyEntered(Node3D body)
    {
        if (body is Player player)
        {
            player.IsInWater = true;
            player.SetWaterSurface(GlobalPosition.Y + SurfaceOffset);
        }
    }

    private void OnBodyExited(Node3D body)
    {
        if (body is Player player)
        {
            player.IsInWater = false;
        }
    }
}