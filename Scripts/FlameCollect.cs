using Godot;
using System;

public partial class FlameCollect : Area3D
{
    [Export] public float TimeAmount = 5.0f;

    private void OnBodyEntered(Node3D body){
        if(body is Player player){
            player.AddTime(TimeAmount);
            QueueFree();
        }
    }
}
