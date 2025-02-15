using UnityEngine;

#if ENABLE_LEGACY_INPUT_MANAGER
#else
using UnityEngine.InputSystem;
#endif

namespace RuntimeTransformHandle.Utils
{
  public static class MouseInput
  {
    public static bool GetMouseClick()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
      return Input.GetMouseButtonDown(0);
#else
      return Mouse.current.leftButton.wasPressedThisFrame;
#endif
    }

    public static bool GetMouseHold()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
      return Input.GetMouseButton(0);
#else

      return Mouse.current.leftButton.isPressed;
#endif
    }

    public static bool GetMouseRelease()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
      return Input.GetMouseButtonUp(0);
#else
      return Mouse.current.leftButton.wasReleasedThisFrame;
#endif
    }

    public static Vector3 GetMousePosition()
    {
#if ENABLE_LEGACY_INPUT_MANAGER
      return Input.mousePosition;
#else
      return Mouse.current.position.ReadValue();
#endif
    }
  }
}