using UnityEngine;

namespace RuntimeTransformHandle.Utils
{
  public static class RayExtensions
  {
    public static Vector3 GetClosestPoint(this Ray ray, Ray otherRay)
    {
      float bd = Vector3.Dot(ray.direction, otherRay.direction);
      float cd = Vector3.Dot(otherRay.origin, otherRay.direction);
      float ad = Vector3.Dot(ray.origin, otherRay.direction);
      float bc = Vector3.Dot(ray.direction, otherRay.origin);
      float ab = Vector3.Dot(ray.origin, ray.direction);

      float bottom = bd * bd - 1f;
      if (Mathf.Abs(bottom) < Mathf.Epsilon)
      {
        return ray.origin;
      }

      float top = ab - bc + bd * (cd - ad);
      return ray.GetPoint(top / bottom);
    }
  }
}