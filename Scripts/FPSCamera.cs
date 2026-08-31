using Godot;

public partial class FPSCamera : Node3D
{
    [Export] public Player player;
    [Export] public SpringArm3D springArm; // assign the child SpringArm3D in the inspector

    [Export] public float cameraSpeed = 2f;
    [Export] Vector2 cameraXBound = new Vector2(0.75f, -0.75f);
    [Export] float maxCameraMovementPerFrame = 15f;

    [Export] public float springArmLength = 4f; // how far behind the player the camera sits

    float frameDelta;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;

        if (springArm != null)
            springArm.SpringLength = springArmLength;
    }

    public override void _Process(double delta)
    {
        frameDelta = (float)delta;
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMovement)
        {
            Vector2 velocity = mouseMovement.Relative;
            Vector3 rotation = Rotation; // now rotating the PIVOT, not the camera directly

            velocity = new Vector2(
                Mathf.Clamp(velocity.X, -maxCameraMovementPerFrame, maxCameraMovementPerFrame),
                Mathf.Clamp(velocity.Y, -maxCameraMovementPerFrame, maxCameraMovementPerFrame)
            );

            rotation.X += -velocity.Y * cameraSpeed * frameDelta;
            rotation.X = Mathf.Clamp(rotation.X, cameraXBound.Y, cameraXBound.X);

            Rotation = rotation;

            Vector3 playerRot = player.Rotation;
            playerRot.Y += -velocity.X * cameraSpeed * frameDelta;
            player.Rotation = playerRot;
        }
    }
}