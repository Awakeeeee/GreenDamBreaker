using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SurfaceMesh : MonoBehaviour 
{
	public int sizeX;
	public int sizeY;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;

	private Vector3[] vertices;
	private int[] triangles;	//triangles is an int array, element represents vertice index
	private Vector2[] uv;
	private Vector4[] tangents;
	private Color[] colors;

	void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		Generate();
	}

	void Generate()
	{
		mesh = new Mesh();
		meshFilter.mesh = mesh;
		mesh.name = "Coded Surface";

		SetVertices();
		mesh.vertices = vertices;

		SetTriangles();
		mesh.triangles = triangles;

		SetNormals();

		SetTextureUV();
		mesh.uv = uv;

		SetTangents();
		mesh.tangents = tangents;

//		SetColors();
//		mesh.colors = colors;
	}

	void SetVertices()
	{
		vertices = new Vector3[(sizeX + 1) * (sizeY + 1)];

		//use a 2D array, the loop can be clearer, but mesh vertices is a 1D array - this is what vCount is used for
		for(int vCount = 0, y = 0; y <= sizeY; y++)
		{
			for(int x = 0; x <= sizeX; x++)
			{
				vertices[vCount] = new Vector3(x, y, 0);
				vCount++;
			}
		}
	}

	void SetTriangles()
	{
		triangles = new int[2 * sizeX * sizeY * 3];
		int rowVertices = sizeX + 1;

		//i is the index track of triangles array
		//set vertices in clock-wise order
		for(int y = 0; y < sizeY; y++)
		{
			for(int i = 0, x = 0; x < sizeX; x++, i+=6)
			{
				triangles[i] = x;
				triangles[i + 1] = y * rowVertices + sizeX + x + 1;
				triangles[i + 2] = x + 1;

				triangles[i + 3] = x + 1;
				triangles[i + 4] = y * rowVertices + sizeX + x + 1;
				triangles[i + 5] = y * rowVertices + sizeX + x + 2;
			}
		}
	}

	void SetNormals()
	{
		//default normal is (0, 0, 1)
		mesh.RecalculateNormals();
	}

	void SetTextureUV()
	{
		//defulat uv in every vertice is (0, 0)
		uv = new Vector2[vertices.Length];

		for(int i = 0, y = 0; y <= sizeY; y++)
		{
			for(int x = 0; x <= sizeX; x++)
			{
				uv[i] = new Vector2((float)x / (float)sizeX, (float)y / (float)sizeY);
				i++;
			}
		}
	}

	void SetTangents()
	{
		tangents = new Vector4[vertices.Length];

		for(int i = 0, y = 0; y <= sizeY; y++)
		{
			for(int x = 0; x <= sizeX; x++)
			{
				tangents[i] = new Vector4(1, 0, 0, 1);
				i++;
			}
		}
	}

	void SetColors()
	{
		colors = new Color[vertices.Length];

		for(int i = 0, y = 0; y <= sizeY; y++)
		{
			for(int x = 0; x <= sizeX; x++)
			{
				colors[i] = Random.ColorHSV();
				i++;
			}
		}
	}

	//--------------------------

	void OnDrawGizmos()
	{
		if(vertices == null || vertices.Length <= 0)
			return;

		Gizmos.color = Color.green;
		for(int i = 0; i < vertices.Length; i++)
		{
			Gizmos.DrawCube(vertices[i], Vector3.one * 0.1f);
		}
	}
}
