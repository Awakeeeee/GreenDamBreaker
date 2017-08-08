using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshDeformer))]
public class DeformerBlockHP : Health 
{
	public float damageToForceMultiplier = 350f;
	public float maxDeformForce = 1000f;
	MeshDeformer deformer;

	void Start()
	{
		deformer = GetComponent<MeshDeformer>();
	}

	public override void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		float deformerForce = damage * damageToForceMultiplier;
		deformerForce = Mathf.Clamp(deformerForce, 0f, maxDeformForce);

		deformer.deformMethod = MeshDeformer.DeformMethod.InstantOnce;
		deformer.Deform(hit, deformerForce);
	}
}
