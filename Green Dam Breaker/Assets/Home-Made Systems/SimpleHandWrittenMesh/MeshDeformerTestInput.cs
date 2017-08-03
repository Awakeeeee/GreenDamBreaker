using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeformerTestInput : MonoBehaviour 
{
	public float force;

	void Update()
	{
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if(Physics.Raycast(ray, out hit, 100))	//TODO what if the collider does not exactly fit the mesh?
			{
				MeshDeformer deformer = hit.transform.GetComponent<MeshDeformer>();
				if(deformer != null)
				{
					deformer.Deform(hit, force);
				}
			}
		}
	}
}
