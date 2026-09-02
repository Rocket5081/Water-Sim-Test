using Godot;
using System;

public partial class main : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ColorRect cr = GetNode<ColorRect>("Simulation/ColorRect");
        MeshInstance3D water = GetNode<MeshInstance3D>("Water");

        SubViewport simulation = GetNode<SubViewport>("Simulation");
        SubViewport collision = GetNode<SubViewport>("Collision");

        Texture2D simTex = simulation.GetTexture();
        Texture2D colTex = collision.GetTexture();

        // Set the shader parameter on the water material
        ShaderMaterial waterMaterial = water.Mesh.SurfaceGetMaterial(0) as ShaderMaterial;

        waterMaterial.SetShaderParameter("simulation", simTex);
        waterMaterial.SetShaderParameter("simulation2", simTex);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
