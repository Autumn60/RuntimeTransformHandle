using UnityEngine;

namespace RuntimeTransformHandle.Utils
{
  public static class MeshUtils
  {
    public static Mesh CreateCone(float radius, float height, int segments)
    {
      Mesh mesh = new Mesh();
      mesh.name = "Cone";

      Vector3[] vertices = new Vector3[segments + 2];
      Vector3[] normals = new Vector3[segments + 2];
      Vector2[] uv = new Vector2[segments + 2];

      vertices[0] = Vector3.zero;
      normals[0] = Vector3.down;
      uv[0] = new Vector2(0.5f, 0.5f);

      for (int i = 0; i < segments; i++)
      {
        float angle = i * Mathf.PI * 2f / segments;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        vertices[i + 1] = new Vector3(x, 0, z);
        normals[i + 1] = Vector3.down;
        uv[i + 1] = new Vector2((x / radius + 1f) * 0.5f, (z / radius + 1f) * 0.5f);
      }

      vertices[segments + 1] = new Vector3(0f, height, 0f);
      normals[segments + 1] = Vector3.up;
      uv[segments + 1] = new Vector2(0.5f, 1f);

      int[] triangles = new int[segments * 3 + segments * 3];

      for (int i = 0; i < segments; i++)
      {
        int current = i + 1;
        int next = (i + 1) % segments + 1;

        triangles[i * 3] = 0;
        triangles[i * 3 + 1] = next;
        triangles[i * 3 + 2] = current;
      }

      for (int i = 0; i < segments; i++)
      {
        int current = i + 1;
        int next = (i + 1) % segments + 1;
        triangles[segments * 3 + i * 3] = current;
        triangles[segments * 3 + i * 3 + 1] = next;
        triangles[segments * 3 + i * 3 + 2] = segments + 1;
      }

      mesh.vertices = vertices;
      mesh.normals = normals;
      mesh.uv = uv;
      mesh.triangles = triangles;

      return mesh;
    }

    public static Mesh CreateCylinder(float radius, float height, int segments)
    {
      Mesh mesh = new Mesh();
      mesh.name = "Cylinder";

      Vector3[] vertices = new Vector3[segments * 2 + 2];
      Vector3[] normals = new Vector3[segments * 2 + 2];
      Vector2[] uv = new Vector2[segments * 2 + 2];

      vertices[0] = Vector3.zero;
      normals[0] = Vector3.down;
      uv[0] = new Vector2(0.5f, 0.5f);

      vertices[segments + 1] = new Vector3(0f, height, 0f);
      normals[segments + 1] = Vector3.up;
      uv[segments + 1] = new Vector2(0.5f, 0.5f);

      for (int i = 0; i < segments; i++)
      {
        float angle = i * Mathf.PI * 2f / segments;
        float x = Mathf.Cos(angle) * radius;
        float z = Mathf.Sin(angle) * radius;
        vertices[i + 1] = new Vector3(x, 0f, z);
        normals[i + 1] = Vector3.down;
        uv[i + 1] = new Vector2((x / radius + 1f) * 0.5f, (z / radius + 1f) * 0.5f);

        vertices[i + segments + 2] = new Vector3(x, height, z);
        normals[i + segments + 2] = Vector3.up;
        uv[i + segments + 2] = new Vector2((x / radius + 1f) * 0.5f, (z / radius + 1f) * 0.5f);
      }

      int[] triangles = new int[segments * 6 + segments * 6];

      for (int i = 0; i < segments; i++)
      {
        int current = i + 1;
        int next = (i + 1) % segments + 1;

        triangles[i * 3] = 0;
        triangles[i * 3 + 1] = next;
        triangles[i * 3 + 2] = current;
      }

      for (int i = 0; i < segments; i++)
      {
        int current = i + segments + 2;
        int next = (i + 1) % segments + segments + 2;

        triangles[segments * 3 + i * 3] = segments + 1;
        triangles[segments * 3 + i * 3 + 1] = current;
        triangles[segments * 3 + i * 3 + 2] = next;
      }

      for (int i = 0; i < segments; i++)
      {
        int currentBottom = i + 1;
        int nextBottom = (i + 1) % segments + 1;
        int currentTop = i + segments + 2;
        int nextTop = (i + 1) % segments + segments + 2;

        triangles[segments * 6 + i * 6] = currentBottom;
        triangles[segments * 6 + i * 6 + 1] = nextBottom;
        triangles[segments * 6 + i * 6 + 2] = currentTop;

        triangles[segments * 6 + i * 6 + 3] = nextBottom;
        triangles[segments * 6 + i * 6 + 4] = nextTop;
        triangles[segments * 6 + i * 6 + 5] = currentTop;
      }

      mesh.vertices = vertices;
      mesh.normals = normals;
      mesh.uv = uv;
      mesh.triangles = triangles;

      return mesh;
    }

    public static Mesh CreateTorus(float minorRadius, float majorRadius, int radialSegments, int tubularSegments)
    {
      Mesh mesh = new Mesh();
      mesh.name = "Torus";

      // 頂点, 法線, UV
      Vector3[] vertices = new Vector3[radialSegments * tubularSegments];
      Vector3[] normals = new Vector3[vertices.Length];
      Vector2[] uv = new Vector2[vertices.Length];

      for (int i = 0; i < radialSegments; i++)
      {
        float radialAngle = i * Mathf.PI * 2f / radialSegments;
        for (int j = 0; j < tubularSegments; j++)
        {
          float tubularAngle = j * Mathf.PI * 2f / tubularSegments;

          float x = (majorRadius + minorRadius * Mathf.Cos(tubularAngle)) * Mathf.Cos(radialAngle);
          float z = (majorRadius + minorRadius * Mathf.Cos(tubularAngle)) * Mathf.Sin(radialAngle);
          float y = minorRadius * Mathf.Sin(tubularAngle);

          vertices[i * tubularSegments + j] = new Vector3(x, y, z);

          normals[i * tubularSegments + j] = new Vector3(Mathf.Cos(tubularAngle) * Mathf.Cos(radialAngle),
                                                          Mathf.Sin(tubularAngle),
                                                          Mathf.Cos(tubularAngle) * Mathf.Sin(radialAngle)).normalized;

          uv[i * tubularSegments + j] = new Vector2(i / (float)(radialSegments - 1), j / (float)(tubularSegments - 1));
        }
      }

      int[] triangles = new int[radialSegments * tubularSegments * 6];

      for (int i = 0; i < radialSegments; i++)
      {
        for (int j = 0; j < tubularSegments; j++)
        {
          int current = i * tubularSegments + j;
          int nextI = (i + 1) % radialSegments;
          int nextJ = (j + 1) % tubularSegments;

          triangles[(i * tubularSegments + j) * 6] = current;
          triangles[(i * tubularSegments + j) * 6 + 1] = (nextI * tubularSegments + j);
          triangles[(i * tubularSegments + j) * 6 + 2] = (i * tubularSegments + nextJ);

          triangles[(i * tubularSegments + j) * 6 + 3] = (nextI * tubularSegments + j);
          triangles[(i * tubularSegments + j) * 6 + 4] = (nextI * tubularSegments + nextJ);
          triangles[(i * tubularSegments + j) * 6 + 5] = (i * tubularSegments + nextJ);
        }
      }

      mesh.vertices = vertices;
      mesh.normals = normals;
      mesh.uv = uv;
      mesh.triangles = triangles;

      return mesh;
    }
  }
}