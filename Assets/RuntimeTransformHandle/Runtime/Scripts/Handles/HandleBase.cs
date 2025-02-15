using System;
using Unity.VisualScripting;
using UnityEngine;

namespace RuntimeTransformHandle
{
  public abstract class HandleBase : MonoBehaviour
  {
    protected TransformHandle transformHandle;
    protected Vector3 axis;
    protected Color color;

    protected Material mat;

    protected Vector3 hitPoint;

    public static T Create<T>(string objName, TransformHandle transformHandle, Vector3 axis, Color color) where T : HandleBase
    {
      var handle = new GameObject(objName).AddComponent<T>();
      handle.transform.SetParent(transformHandle.transform, false);
      handle.transformHandle = transformHandle;
      handle.axis = axis;
      handle.color = color;
      handle.InitializeMaterial();
      handle.CreateMesh();
      return handle;
    }

    protected abstract void CreateMesh();

    private void InitializeMaterial()
    {
      mat = new Material(transformHandle.Shader)
      {
        color = color
      };
    }

    public void SetColor(Color color)
    {
      mat.color = color;
    }

    public void ResetColor()
    {
      mat.color = color;
    }

    public virtual bool CanInteract(Vector3 hitPoint) => true;

    public abstract void BeginInteraction(Vector3 hitPoint);

    public virtual void UpdateInteraction()
    {
      SetColor(Color.yellow);
    }

    public virtual void EndInteraction()
    {
      ResetColor();
    }
  }
}