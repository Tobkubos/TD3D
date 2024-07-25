using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MeshGenerator : MonoBehaviour
{
	public int width = 15;
	public int height = 15;

	void Start()
	{
		GenerateMesh(0,0);
	}

	void GenerateMesh(int startX, int startY)
	{
		Mesh mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;

		Vector3[] vertices = new Vector3[(width + 1) * (height + 1)];
		int[] triangles = new int[width * height * 6];
		Vector2[] uv = new Vector2[(width + 1) * (height + 1)];

		int vertIndex = 0;
		int triIndex = 0;

		for (int z = 0; z <= height; z++)
		{
			for (int x = 0; x <= width; x++)
			{
				vertices[vertIndex] = new Vector3(x, 0, z);
				uv[vertIndex] = new Vector2((float)x / width, (float)z / height);

				if (x < width && z < height)
				{
					triangles[triIndex + 0] = vertIndex;
					triangles[triIndex + 1] = vertIndex + width + 1;
					triangles[triIndex + 2] = vertIndex + width + 2;

					triangles[triIndex + 3] = vertIndex;
					triangles[triIndex + 4] = vertIndex + width + 2;
					triangles[triIndex + 5] = vertIndex + 1;

					triIndex += 6;
				}

				vertIndex++;
			}
		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uv;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds(); // Rekalkulacja granic siatki

		// Ustawienie MeshCollider
		MeshCollider meshCollider = GetComponent<MeshCollider>();
		meshCollider.sharedMesh = null; // Usuniêcie aktualnej siatki z collidera
		meshCollider.sharedMesh = mesh; // Ustawienie nowej siatki w colliderze
	}
}
