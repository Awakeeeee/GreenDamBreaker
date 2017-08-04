using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Learn from Cat-like coding
//Deform any mesh, not nessecerily rounded cube
//Target effect: poke and dent the mesh
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshDeformer : MonoBehaviour 
{
	[Tooltip("[Large offset makes focus, dent deformation and low force] | [Small offset makes shallow, surface-spread deformation but large force]")]
	public float forceOffset = 0.1f;
	public enum DeformMethod
	{
		InstantOnce,
		Continuous
	}
	public DeformMethod deformMethod;

	MeshFilter filter;
	MeshRenderer rdr;

	Vector3[] originalVertices;
	Vector3[] deformedVertices;
	Vector3[] vertexVelocity;

	void Start()
	{
		filter = GetComponent<MeshFilter>();
		rdr = GetComponent<MeshRenderer>();

		originalVertices = filter.mesh.vertices;
		Reset();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			Reset();
		}

		if(deformMethod == DeformMethod.Continuous)
		{
			ApplyVertexChange();
		}
	}

	//called in Update while keeping input
	public void Deform(RaycastHit hit, float force)
	{
		//A deform force applied on a meshx, is from somewhere pointing towards every mesh point
		//that somewhere is calculated as forceStart. it is the hit point on collider repositioned a little along hit normal
		//As such, it requires enough mesh vertex to has good result
		Vector3 forceStart = hit.point + hit.normal * forceOffset;

		Debug.DrawLine(Camera.main.transform.position, forceStart);

		for(int i = 0; i < deformedVertices.Length; i++)
		{
			SetVertexVelocity(i, forceStart, force);
		}

		if(deformMethod == DeformMethod.InstantOnce)
		{
			ApplyVertexChange();
			vertexVelocity = new Vector3[deformedVertices.Length];
		}
	}

	//called in Update while keeping input
	void SetVertexVelocity(int vIndex, Vector3 fStart, float force)
	{
		Debug.DrawLine(fStart, deformedVertices[vIndex] + transform.position, Color.red);

		Vector3 forceVec = deformedVertices[vIndex] + transform.position - fStart;

		float attenuationDist = forceVec.sqrMagnitude;
		float attenuatedForce = force / (1 + attenuationDist);	//inverse square rule

		Vector3 forceDirection = forceVec.normalized;

		vertexVelocity[vIndex] += forceDirection * attenuatedForce * Time.deltaTime;	// v = v + dV | dV = F/m * dt
	}

	void UpdateVertexPos(int vIndex)
	{
		Vector3 velocity = vertexVelocity[vIndex];
		deformedVertices[vIndex] += velocity * Time.deltaTime;	// S = S + dS | dS = v * dt
	}

	void ApplyVertexChange()
	{
		for(int i = 0; i < deformedVertices.Length; i++)
		{
			UpdateVertexPos(i);
		}

		filter.mesh.vertices = deformedVertices;
		filter.mesh.RecalculateNormals();
	}

	public void Reset()
	{
		deformedVertices = new Vector3[originalVertices.Length];
		for(int i = 0; i < deformedVertices.Length; i++)
		{
			deformedVertices[i] = originalVertices[i];
		}
		vertexVelocity = new Vector3[deformedVertices.Length];

		ApplyVertexChange();
	}
}
