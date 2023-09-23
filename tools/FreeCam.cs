using Godot;

namespace JayStation.tools;

public partial class FreeCam : Camera3D
{
  const float MOUSE_SENSITIVITY = 0.05f;
  const float SPEED_SPRINT_MULTIPLIER = 2.0f;

  float speed = 25;

  readonly StringName forward = "movement_forward";
  readonly StringName backward = "movement_backward";

  readonly StringName left = "movement_left";
  readonly StringName right = "movement_right";
  readonly StringName goDown = "debug_camera_down";
  readonly StringName goUp = "debug_camera_up";

  readonly StringName boost = "movement_sprint";

  public override void _Ready()
  {
    GetWindow().WindowInput += HandleInput;
  }

  void HandleInput(InputEvent @event)
  {
    switch (@event)
    {
      case InputEventMouseButton mouseButtonEvent:
        CaptureOrReleaseCursor(mouseButtonEvent);
        break;
      case InputEventMouseMotion mouseMotionEvent:
        if (Input.MouseMode != Input.MouseModeEnum.Captured)
          return;

        RotateWithMouseMotion(mouseMotionEvent);
        break;
    }
  }

  static void CaptureOrReleaseCursor(InputEventMouseButton mouseButtonEvent)
  {
    if (mouseButtonEvent.ButtonIndex != MouseButton.Right)
      return;

    Input.MouseMode = mouseButtonEvent.Pressed switch
    {
      true => Input.MouseModeEnum.Captured,
      false => Input.MouseModeEnum.Visible
    };
  }

  void ChangeSpeedWithMouseWheel(InputEventMouseButton mouseEvent)
  {
    if ((mouseEvent.ButtonIndex != MouseButton.WheelUp) && (mouseEvent.ButtonIndex != MouseButton.WheelDown))
      return;

    speed = mouseEvent.ButtonIndex switch
    {
      MouseButton.WheelUp => Mathf.Clamp(speed + 2, 1, 100),
      MouseButton.WheelDown => Mathf.Clamp(speed - 2, 1, 100),
      _ => speed
    };
  }

  void RotateWithMouseMotion(InputEventMouseMotion mouseMotionEvent)
  {
    RotationDegrees = RotationDegrees with
    {
      X = Mathf.Clamp(RotationDegrees.X - mouseMotionEvent.Relative.Y * MOUSE_SENSITIVITY, -89, 89),
      Y = RotationDegrees.Y - mouseMotionEvent.Relative.X * MOUSE_SENSITIVITY
    };
  }

  public override void _Process(double delta)
  {
    if (!GetWindow().HasFocus())
      return;

    Vector2 movementVector = Input.GetVector(left, right, forward, backward);
    float upOrDown = GetUpOrDown();
    float currentSpeed = GetSpeedWithBoost(speed);

    Vector3 amountToMove = new(
      movementVector.X,
      upOrDown,
      movementVector.Y
    );
    amountToMove = amountToMove.Normalized() * (float)delta * currentSpeed;
    Translate(amountToMove);
  }

  float GetUpOrDown()
  {
    float upOrDown = 0;
    if (Input.IsActionPressed(goDown))
      upOrDown -= 1;
    if (Input.IsActionPressed(goUp))
      upOrDown += 1;
    return upOrDown;
  }

  float GetSpeedWithBoost(float oldSpeed)
  {
    if (Input.IsActionPressed(boost))
      return oldSpeed * SPEED_SPRINT_MULTIPLIER;
    return oldSpeed;
  }
}