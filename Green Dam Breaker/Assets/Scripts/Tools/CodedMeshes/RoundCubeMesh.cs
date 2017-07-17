using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoundCubeMesh : MonoBehaviour 
{
	public int sizeX, sizeY, sizeZ;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;

	private Vector3[] vertices;
	private int[] triangles;

	void Awake()
	{
		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		GenerateRoundCube();
	}

	void GenerateRoundCube()
	{
		mesh = new Mesh();
		meshFilter.mesh = mesh;

		mesh.name = "Round Cube";

		SetupVertices();
		mesh.vertices = vertices;

		SetupTriangles();
		mesh.triangles = triangles;

		//SetupNormals();
	}

	void SetupVertices()
	{
		int cornerVCount = 8;
		int edgeVCount = 4 * (sizeX + sizeY + sizeZ - 3);
		int faceVCount = 2 * ((sizeX - 1) * (sizeY - 1) + (sizeX - 1) * (sizeZ - 1) + (sizeY - 1) * (sizeZ - 1));

		vertices = new Vector3[cornerVCount + edgeVCount + faceVCount];

		int v = 0;

		for(int y = 0; y <= sizeY; y++) //counting Y layers
		{
			for(int x = 0; x <= sizeX; x++)	//1st edge on a Y-layer
			{
				vertices[v] = new Vector3(x, y, 0);
				v = v + 1;
			}

			for(int z = 1; z <= sizeZ; z++)	//2nd
			{
				vertices[v] = new Vector3(sizeX, y, z);
				v = v + 1;
			}

			for(int x = sizeX - 1; x >= 0; x--)	//3rd
			{
				vertices[v] = new Vector3(x, y, sizeZ);
				v = v + 1;
			}

			for(int z = sizeZ - 1; z > 0; z--)	//4th
			{
				vertices[v] = new Vector3(0, y, z);
				v = v + 1;
			}
		}

		for(int z = 1; z < sizeZ; z++)	//fill in top cap
		{
			for(int x = 1; x < sizeX; x++)
			{
				vertices[v] = new Vector3(x, sizeY, z);
				v = v + 1;
			}
		}
		for(int z = 1; z < sizeZ; z++)	//fill in bottom cap
		{
			for(int x = 1; x < sizeX; x++)
			{
				vertices[v] = new Vector3(x, 0, z);
				v = v + 1;
			}
		}
	}

	void SetupTriangles()
	{
		int quads = sizeX * sizeY * 2 + sizeX * sizeZ * 2 + sizeY * sizeZ * 2;
		triangles = new int[quads * 2 * 3];

		int ring = (sizeX + sizeZ) * 2;

		int t = 0;	//triangle index track
		int v = 0;	//vertice index track

		for(int y = 0; y < sizeY; y++)
		{
			//good to remember: the first vertice index of a new ring is x+ring
			for(int x = 0; x < sizeX; x++)
			{
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int z = 0; z < sizeZ; z++)
			{
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int x = 0; x < sizeX; x++)
			{
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int z = 0; z < sizeZ - 1; z++)
			{
				t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			t = SetQuad(triangles, t, v, y * ring, v + ring, y * ring + ring);
			v = v + 1;
		}

		//---------------fill in top cap------------------------
		//the first row
		for(int x = 0; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + ring);
			v = v + 1;
		}
		t = SetQuad(triangles, t, v, v + 1, v + ring - 1, v + 2);
		v= v + 1;

		//from now on triangle index stop following vertice index, because triangle y stop at sizeY - 1 but vertex y stop at sizeY
		//solution: save the two vertice at left and right, and make a new tracker
		int vt = v;
		int vMin = ring * (sizeY + 1) - 1;
		int vTrack = vMin + 1;

		for(int z = 1; z < sizeZ - 1; z++)
		{
			t = SetQuad(triangles, t, vMin, vTrack, vMin - 1, vTrack + sizeX - 1);
			for(int x = 1; x < sizeX - 1; x++)
			{
				t = SetQuad(triangles, t, vTrack, vTrack + 1, vTrack + sizeX - 1, vTrack + sizeX);
				vTrack = vTrack + 1;
			}
			t = SetQuad(triangles, t, vTrack, vt + 1, vTrack + sizeX - 1, vt + 2);

			vTrack = vTrack + 1;
			vt = vt + 1;
			vMin = vMin - 1;
		}

		t = SetQuad(triangles, t, vMin, vTrack, vMin - 1, vMin - 2);
		vMin = vMin - 1;
		for(int x = 1; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, vTrack, vTrack + 1, vMin - 1, vMin - 2);
			vMin = vMin - 1;
			vTrack = vTrack + 1;
		}
		t = SetQuad(triangles, t, vTrack, vt, vt + 3, vt + 2);
		//------------------end of top cap------------------------

		//------------------fill in bottom cap--------------------
		int vi = vertices.Length - (sizeX - 1) * (sizeZ - 1);
		int bTrack = 1;
		int vb = vi;

		//for bottom triangles, switch order of v01 and v10 to make if anti-clock-wise
		t = SetQuad(triangles, t, 0, ring - 1, 1, vi);
		for(int x = 1; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, bTrack, vi, bTrack + 1, vi + 1);
			vi = vi + 1;
			bTrack = bTrack + 1;
		}
		t = SetQuad(triangles, t, bTrack, vi, bTrack + 1, bTrack + 2);
		bTrack = bTrack + 2;
		vi = vi + 1;

		int vMinB = ring - 1;
		for(int z = 1; z < sizeZ - 1; z++)
		{
			t = SetQuad(triangles, t, vMinB, vMinB - 1, vb, vb + sizeX - 1);

			for(int x = 1; x < sizeX - 1; x++)
			{
				t = SetQuad(triangles, t, vb, vb + sizeX - 1, vb + 1, vb + sizeX);
				vb = vb + 1;
			}
			t = SetQuad(triangles, t, vb, vb + sizeX - 1, bTrack, bTrack + 1);

			bTrack = bTrack + 1;
			vb = vb + 1;
			vMinB = vMinB - 1;
		}

		t = SetQuad(triangles, t, vMinB, vMinB - 1, vb, vMinB - 2);
		vMinB = vMinB - 1;
		for(int x = 1; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, vb, vMinB - 1, vb + 1, vMinB - 2);
			vMinB = vMinB - 1;
			vb = vb + 1;
		}
		t = SetQuad(triangles, t, vb, bTrack + 2, bTrack, bTrack + 1);
	}

	void SetupNormals()
	{
		mesh.RecalculateNormals();
	}

	//Start from element t, set the next 6 elements of triangles array to be the given vertice indices. In clock-wise order.
	int SetQuad(int[] triangles, int t, int vi00, int vi10, int vi01, int vi11)
	{
		triangles[t] = vi00;
		triangles[t + 1] = vi01;
		triangles[t + 2] = vi10;

		triangles[t + 3] = vi10;
		triangles[t + 4] = vi01;
		triangles[t + 5] = vi11;

		t = t + 6;
		return t;
	}

	//-------------
	void OnDrawGizmos()
	{
		if(vertices == null || vertices.Length <= 0)
			return;

		Gizmos.color = Color.green;
		for(int i = 0; i < vertices.Length; i++)
		{
			Gizmos.DrawSphere(vertices[i], 0.1f);
		}
	}
}
