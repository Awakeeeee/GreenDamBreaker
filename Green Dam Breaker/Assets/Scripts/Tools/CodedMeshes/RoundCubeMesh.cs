using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RoundCubeMesh : MonoBehaviour 
{
	public int sizeX, sizeY, sizeZ;

	[Tooltip("Range from 0 to half of the smallest dimension.")]
	public float roundness;

	public enum MeshSplit{ XYZ_Split, Whole }
	public MeshSplit meshSplit;

	public Material wholeMat;
	public Material xMat;
	public Material yMat;
	public Material zMat;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;
	public Mesh MeshAsset{ get { return mesh; }}

	private Vector3[] vertices;
	private int[] triangles;
	private int[] trianglesX;
	private int[] trianglesY;
	private int[] trianglesZ;
	private Vector3[] normals;

	//save this to mesh.color, so shader can use it, the SPECIAL thing is the value in color represents original position
	//Color32 range from 0 -255
	private Color32[] originalPosInColor;

	void Awake()
	{
		GenerateRoundCube();
	}

	public void GenerateRoundCube()
	{
		ClearMesh();

		meshFilter = GetComponent<MeshFilter>();
		meshRenderer = GetComponent<MeshRenderer>();

		mesh = new Mesh();
		meshFilter.mesh = mesh;

		mesh.name = "Round Cube";

		SetupVertices();
		RepositionVerticeAndNormals();	//ROUND CUBE CORE
		mesh.vertices = vertices;
		mesh.normals = normals;
		if(originalPosInColor != null && originalPosInColor.Length > 0)
		{
			mesh.colors32 = originalPosInColor;
		}

		switch(meshSplit)
		{
		case MeshSplit.Whole:
			SetupTriangles();
			meshRenderer.materials = new Material[]{wholeMat};
			mesh.triangles = triangles;
			break;
		case MeshSplit.XYZ_Split:
			SetupSplitTriangles();
			mesh.subMeshCount = 3;
			meshRenderer.materials = new Material[]{xMat, yMat, zMat};
			mesh.SetTriangles(trianglesX, 0);
			mesh.SetTriangles(trianglesY, 1);
			mesh.SetTriangles(trianglesZ, 2);
			break;
		default:
			break;
		}

		//mesh.uv is set in shader, not in here
	}

	public void ClearMesh()
	{
		if(mesh != null)
		{
			mesh = new Mesh();
			meshFilter.mesh = mesh;
			mesh.name = "Empty";

			meshRenderer.materials = new Material[0];

			vertices = default(Vector3[]);
			triangles = default(int[]);
			normals = default(Vector3[]);
		}
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

		Triangles_FillTopCap(ref t, ref v, ring, triangles);
		Triangles_FillBottomCap(ref t, ref v, ring, triangles);
	}

	void SetupSplitTriangles()
	{
		trianglesX = new int[sizeY * sizeZ * 2 * 2 * 3];	//the faces that perpendicular to X
		trianglesY = new int[sizeX * sizeZ * 12];
		trianglesZ = new int[sizeX * sizeY * 12];

		int ring = (sizeX + sizeZ) * 2;

		int tX = 0;	//triangle index track
		int tY= 0;
		int tZ = 0;
		int v = 0;	//vertice index track

		for(int y = 0; y < sizeY; y++)
		{
			//good to remember: the first vertice index of a new ring is x+ring
			for(int x = 0; x < sizeX; x++)
			{
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int z = 0; z < sizeZ; z++)
			{
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int x = 0; x < sizeX; x++)
			{
				tZ = SetQuad(trianglesZ, tZ, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			for(int z = 0; z < sizeZ - 1; z++)
			{
				tX = SetQuad(trianglesX, tX, v, v + 1, v + ring, v + ring + 1);
				v = v + 1;
			}

			tX = SetQuad(trianglesX, tX, v, y * ring, v + ring, y * ring + ring);
			v = v + 1;
		}

		Triangles_FillTopCap(ref tY, ref v, ring, trianglesY);
		Triangles_FillBottomCap(ref tY, ref v, ring, trianglesY);
	}

	void Triangles_FillTopCap(ref int t, ref int v, int ring, int[] triangles)
	{
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
	}

	void Triangles_FillBottomCap(ref int t, ref int v, int ring, int[] triangles)
	{
		int vi = vertices.Length - (sizeX - 1) * (sizeZ - 1);
		int vTrack = 1;
		int vb = vi;
		//for bottom triangles, switch order of v01 and v10 to make if anti-clock-wise
		t = SetQuad(triangles, t, 0, ring - 1, 1, vi);
		for(int x = 1; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, vTrack, vi, vTrack + 1, vi + 1);
			vi = vi + 1;
			vTrack = vTrack + 1;
		}
		t = SetQuad(triangles, t, vTrack, vi, vTrack + 1, vTrack + 2);
		vTrack = vTrack + 2;
		vi = vi + 1;

		int vMin = ring - 1;
		for(int z = 1; z < sizeZ - 1; z++)
		{
			t = SetQuad(triangles, t, vMin, vMin - 1, vb, vb + sizeX - 1);

			for(int x = 1; x < sizeX - 1; x++)
			{
				t = SetQuad(triangles, t, vb, vb + sizeX - 1, vb + 1, vb + sizeX);
				vb = vb + 1;
			}
			t = SetQuad(triangles, t, vb, vb + sizeX - 1, vTrack, vTrack + 1);

			vTrack = vTrack + 1;
			vb = vb + 1;
			vMin = vMin - 1;
		}

		t = SetQuad(triangles, t, vMin, vMin - 1, vb, vMin - 2);
		vMin = vMin - 1;
		for(int x = 1; x < sizeX - 1; x++)
		{
			t = SetQuad(triangles, t, vb, vMin - 1, vb + 1, vMin - 2);
			vMin = vMin - 1;
			vb = vb + 1;
		}
		t = SetQuad(triangles, t, vb, vTrack + 2, vTrack, vTrack + 1);
	}

	void SetupNormals()
	{
		mesh.RecalculateNormals();
	}

	void RepositionVerticeAndNormals()
	{
		normals = new Vector3[vertices.Length];
		originalPosInColor = new Color32[vertices.Length];
		Vector3[] innerCube = new Vector3[vertices.Length];

		for(int i = 0; i < vertices.Length; i++)
		{
			originalPosInColor[i] = new Color32((byte)vertices[i].x, (byte)vertices[i].y, (byte)vertices[i].z, 0);
			innerCube[i] = vertices[i];

			if(innerCube[i].x < roundness)	//for a x=5 r=2 cube, vertex 0,1 all squeeze to the pos of vertex 2, to form the inner cube
			{
				innerCube[i].x = roundness;
			}
			else if(innerCube[i].x > sizeX - roundness)	//vertex 5,4 all squeeze ti vertex 3
			{
				innerCube[i].x = sizeX - roundness;
			}
			//else do nothing, the rest of the inner cube vertice are the same as the original vertice

			if(innerCube[i].y < roundness)
			{
				innerCube[i].y = roundness;
			}
			else if(innerCube[i].y > sizeY - roundness)
			{
				innerCube[i].y = sizeY - roundness;
			}

			if(innerCube[i].z < roundness)
			{
				innerCube[i].z = roundness;
			}
			else if(innerCube[i].z > sizeZ - roundness)
			{
				innerCube[i].z = sizeZ - roundness;
			}

			normals[i] = (vertices[i] - innerCube[i]).normalized;
			vertices[i] = innerCube[i] + normals[i] * roundness;	//take X direction for example, inner vertex 0,1,2 are at same pos, so vertex 0,1,2 are also pushed to the same pos, which is the end point of X edge
		}
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

		Gizmos.color = Color.blue;
		for(int i = 0; i < normals.Length; i++)
		{
			Gizmos.DrawRay(vertices[i], normals[i]);
		}
	}
}
