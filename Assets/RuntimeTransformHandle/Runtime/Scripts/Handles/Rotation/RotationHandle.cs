using UnityEngine;

namespace RuntimeTransformHandle
{
  public class RotationHandle : MonoBehaviour
  {
    private TransformHandle transformHandle;

    private RotationAxisHandle[] axisHandles;

    public static RotationHandle Create(TransformHandle transformHandle)
    {
      var handle = transformHandle.gameObject.AddComponent<RotationHandle>();
      handle.transform.SetParent(transformHandle.transform, false);
      handle.transformHandle = transformHandle;
      handle.CreateAxisHandles();
      return handle;
    }

    private void CreateAxisHandles()
    {
      axisHandles = new RotationAxisHandle[3];
      axisHandles[0] = HandleBase.Create<RotationAxisHandle>("RotationAxisHandleX", transformHandle, Vector3.right, Color.red);
      axisHandles[1] = HandleBase.Create<RotationAxisHandle>("RotationAxisHandleY", transformHandle, Vector3.up, Color.green);
      axisHandles[2] = HandleBase.Create<RotationAxisHandle>("RotationAxisHandleZ", transformHandle, Vector3.forward, Color.blue);
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