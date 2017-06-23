using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHP : Health 
{
	public float maxHP;

	private float currentHP;

	protected override void OnEnable()
	{
		base.OnEnable();
		currentHP = maxHP;
		GUIManager.Instance.UpdateText(GUIManager.Instance.HealthText, maxHP.ToString());
	}

	public override void TakeDamage(RaycastHit hit, float damage)
	{
		currentHP -= damage;
		if(currentHP <= 0f)
		{
			currentHP = 0f;
			CharacterDie();
		}
		GUIManager.Instance.UpdateText(GUIManager.Instance.HealthText, currentHP.ToString());

		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);
	}

	void CharacterDie()
	{
		Debug.Log("Character Die");
	}
}
