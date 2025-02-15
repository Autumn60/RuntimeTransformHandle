using RuntimeTransformHandle.Utils;
using UnityEngine;

namespace RuntimeTransformHandle
{
  public class RotationAxisHandle : HandleBase
  {
    private Quaternion startRotation;
    private Vector3 rotationAxis;
    private Plane rotationAxisPlane;
    private Vector3 tangent;
    private Vector3 biTangent;

    protected override void CreateMesh()
    {
      GameObject obj = new GameObject("Torus");
      obj.transform.SetParent(transform, false);
      obj.transform.localPosition = Vector3.zero;
      obj.transform.localRotation = Quaternion.FromToRotation(Vector3.up, axis);
      MeshRenderer renderer = obj.AddComponent<MeshRenderer>();
      renderer.material = mat;
      MeshFilter filter = obj.AddComponent<MeshFilter>();
      MeshCollider collider = obj.AddComponent<MeshCollider>();
      filter.mesh = filter.sharedMesh = collider.sharedMesh = MeshUtils.CreateTorus(0.025f, 1.0f, 16, 16);
    }

    public override void BeginInteraction(Vector3 hitPoint)
    {
      startRotation = transformHandle.Target.localRotation;
      rotationAxis = startRotation * axis;
      rotationAxisPlane = new Plane(rotationAxis, transformHandle.Target.position);

      Vector3 startHitPoint;
      Ray ray = Camera.main.GetMouseRay();
      if (rotationAxisPlane.Raycast(ray, out float distance))
      {
        startHitPoint = ray.GetPoint(distance);
      }
      else
      {
        startHitPoint = rotationAxisPlane.ClosestPointOnPlane(hitPoint);
      }

      tangent = (startHitPoint - transformHandle.Target.position).normalized;
      biTangent = Vector3.Cross(rotationAxis, tangent);
    }

    public override void UpdateInteraction()
    {
      Ray ray = Camera.main.GetMouseRay();
      if (!rotationAxisPlane.Raycast(ray, out float distance))
      {
        base.UpdateInteraction();
        return;
      }

      Vector3 hitPoint = ray.GetPoint(distance);
      Vector3 hitDirection = (hitPoint - transformHandle.Target.position).normalized;
      float x = Vector3.Dot(hitDirection, tangent);
      float y = Vector3.Dot(hitDirection, biTangent);
      float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
      transformHandle.Target.localRotation = startRotation * Quaternion.AngleAxis(angle, axis);
      base.UpdateInteraction();
    }
  }
}
