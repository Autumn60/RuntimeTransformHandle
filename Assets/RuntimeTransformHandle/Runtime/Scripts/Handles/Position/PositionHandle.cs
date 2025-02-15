using UnityEngine;

namespace RuntimeTransformHandle
{
  public class PositionHandle : MonoBehaviour
  {
    private TransformHandle transformHandle;

    private PositionAxisHandle[] axisHandles;

    public static PositionHandle Create(TransformHandle transformHandle)
    {
      var handle = transformHandle.gameObject.AddComponent<PositionHandle>();
      handle.transform.SetParent(transformHandle.transform, false);
      handle.transformHandle = transformHandle;
      handle.CreateAxisHandles();
      return handle;
    }

    private void CreateAxisHandles()
    {
      axisHandles = new PositionAxisHandle[3];
      axisHandles[0] = HandleBase.Create<PositionAxisHandle>("PositionAxisHandleX", transformHandle, Vector3.right, Color.red);
      axisHandles[1] = HandleBase.Create<PositionAxisHandle>("PositionAxisHandleY", transformHandle, Vector3.up, Color.green);
      axisHandles[2] = HandleBase.Create<PositionAxisHandle>("PositionAxisHandleZ", transformHandle, Vector3.forward, Color.blue);
    }

    public void Destroy()
    {
      foreach (var axisHandle in axisHandles)
      {
        Destroy(axisHandle.gameObject);
      }
      Destroy(this);
    }
  }
}