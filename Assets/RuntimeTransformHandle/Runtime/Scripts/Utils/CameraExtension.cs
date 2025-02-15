using UnityEngine;

namespace RuntimeTransformHandle.Utils
{
  public static class CameraExtensions
  {
    public static Ray GetMouseRay(this Camera camera)
    {
      return camera.ScreenPointToRay(MouseInput.GetMousePosition());
    }
  }
}