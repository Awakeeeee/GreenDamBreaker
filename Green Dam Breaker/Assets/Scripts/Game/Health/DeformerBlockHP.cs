﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshDeformer))]
public class DeformerBlockHP : Health 
{
	public float damageToForceMultiplier = 350f;
	public float maxDeformForce = 1000f;
	MeshDeformer deformer;

	public ElaborateHPBar hpBar;

	void Start()
	{
		deformer = GetComponent<MeshDeformer>();
		currentHP = maxHP;
		hpBar.ResetBar(maxHP, 1f);
	}

	void Update()
	{
		hpBar.transform.LookAt(Camera.main.transform.position);
	}

	public override void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		//deform
		float deformerForce = damage * damageToForceMultiplier;
		deformerForce = Mathf.Clamp(deformerForce, 0f, maxDeformForce);

		deformer.deformMethod = MeshDeformer.DeformMethod.InstantOnce;
		deformer.Deform(hit, deformerForce);

		//normal take damage
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		currentHP -= damage;
		currentHP = Mathf.Clamp(currentHP, 0f, maxHP);
		hpBar.UpdateHP(damage);
		if(currentHP <= 0f)
		{
			Die();
		}
	}

	void Die()
	{
		isDead = true;

		if(dieSound != null)
		{
			SoundManager.Instance.PlayClip2D(dieSound);
		}
	}
}
