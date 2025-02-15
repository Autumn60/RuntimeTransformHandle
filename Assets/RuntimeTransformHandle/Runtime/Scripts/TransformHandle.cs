using UnityEngine;

using RuntimeTransformHandle.Utils;

namespace RuntimeTransformHandle
{
  public class TransformHandle : MonoBehaviour
  {
    [SerializeField]
    private Shader shader;
    public Shader Shader => shader;

    [SerializeField, Min(0.1f)]
    private float handleScaleFactor = 1f;

    public Transform Target;
    private Transform lastTarget = null;

    public HandleType HandleType = HandleType.Position;

    private PositionHandle positionHandle;
    private RotationHandle rotationHandle;

    private HandleBase currentHandle;
    private HandleType currentHandleType;

    private void Start()
    {
      CreateHandles();
    }

    private void CreateHandles()
    {
      switch (HandleType)
      {
        case HandleType.Position:
          positionHandle = PositionHandle.Create(this);
          break;
        case HandleType.Rotation:
          rotationHandle = RotationHandle.Create(this);
          break;
        default:
          break;
      }
      currentHandleType = HandleType;
    }

    private void DestroyHandles()
    {
      currentHandle = null;
      currentHandleType = HandleType.None;
      if (positionHandle) positionHandle.Destroy();
      if (rotationHandle) rotationHandle.Destroy();
    }

    private void Update()
    {
      if (!Target)
      {
        DestroyHandles();
        return;
      }
      UpdateHandleType();

      if (MouseInput.GetMouseClick() && TryGetPointingHandle(out HandleBase pointingHandle, out Vector3 hitPoint))
      {
        currentHandle = pointingHandle;
        currentHandle.BeginInteraction(hitPoint);
      }

      if (MouseInput.GetMouseHold() && currentHandle)
      {
        currentHandle.UpdateInteraction();
      }

      if (MouseInput.GetMouseRelease() && currentHandle)
      {
        currentHandle.EndInteraction();
        currentHandle = null;
      }

      transform.position = Target.position;
      transform.rotation = Target.rotation;
      UpdateHandleScale();
    }

    private void UpdateHandleType()
    {
      if (currentHandleType != HandleType)
      {
        DestroyHandles();
        CreateHandles();
      }
    }

    private bool TryGetPointingHandle(out HandleBase pointingHandle, out Vector3 hitPoint)
    {
      pointingHandle = null;
      hitPoint = Vector3.zero;

      Ray ray = Camera.main.GetMouseRay();
      RaycastHit[] hits = Physics.RaycastAll(ray);
      if (hits.Length == 0) return false;

      foreach (var hit in hits)
      {
        pointingHandle = hit.collider.GetComponentInParent<HandleBase>();
        if (pointingHandle)
        {
          hitPoint = hit.point;
          return true;
        }
      }
      return false;
    }

    private void UpdateHandleScale()
    {
      if (!Target) return;
      if (lastTarget == Target) return;
      Bounds bounds = CalculateBounds(Target.gameObject);
      Vector3 max = bounds.size;
      float radius = Mathf.Max(max.x, Mathf.Max(max.y, max.z));
      transform.localScale = Vector3.one * radius * handleScaleFactor;
      lastTarget = Target;
    }

    private Bounds CalculateBounds(GameObject obj)
    {
      Bounds bounds = new Bounds(obj.transform.position, Vector3.zero);
      Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
      foreach (Renderer renderer in renderers)
      {
        bounds.Encapsulate(renderer.bounds);
      }
      return bounds;
    }
  }
}
