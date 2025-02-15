using RuntimeTransformHandle.Utils;
using UnityEngine;

namespace RuntimeTransformHandle
{
  public class PositionAxisHandle : HandleBase
  {
    private Vector3 startOffset;
    private Ray ray;

    protected override void CreateMesh()
    {
      const float lengthRatio = 0.8f;
      {
        const float coneRadius = 0.075f;
        GameObject obj = new GameObject("Cone");
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = axis * lengthRatio;
        obj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.material = mat;
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        filter.mesh = filter.sharedMesh = collider.sharedMesh = MeshUtils.CreateCone(coneRadius, 1 - lengthRatio, 16);
      }
      {
        const float cylinderRadius = 0.03f;
        GameObject obj = new GameObject("Cylinder");
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);
        MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
        renderer.material = mat;
        MeshFilter filter = obj.AddComponent<MeshFilter>();
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        filter.mesh = filter.sharedMesh = collider.sharedMesh = MeshUtils.CreateCylinder(cylinderRadius, lengthRatio, 16);
      }
    }

    public override void BeginInteraction(Vector3 hitPoint)
    {
      Transform target = transformHandle.Target;
      Vector3 startPosition = target.position;
      ray = new Ray(startPosition, target.rotation * axis);
      startOffset = startPosition - ray.GetClosestPoint(Camera.main.GetMouseRay());
    }

    public override void UpdateInteraction()
    {
      Vector3 hitPoint = ray.GetClosestPoint(Camera.main.GetMouseRay());
      transformHandle.Target.position = hitPoint + startOffset;
      base.UpdateInteraction();
    }
  }
}
