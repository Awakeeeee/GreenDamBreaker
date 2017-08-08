using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHP : Health 
{
	public float maxHP;

	private float currentHP;
	private bool isDead;

	protected override void OnEnable()
	{
		base.OnEnable();
		isDead = false;
		currentHP = maxHP;
		GUIManager.Instance.HealthBar.ResetBar(maxHP, 1.0f);
	}

	public override void TakeDamage(float damage, RaycastHit hit = default(RaycastHit))
	{
		if(isDead)
			return;

		currentHP -= damage;
		if(currentHP <= 0f)
		{
			isDead = true;
			currentHP = 0f;
			StartCoroutine(CharacterDie());
		}
		GUIManager.Instance.HealthBar.UpdateHP(damage);

		//sfx
		if(getShootSound != null && audioSource != null)
		{
			audioSource.PlayOneShot(getShootSound);	
		}

		//particle hit effet
		if(hit.transform != null)
		{
			GlobalBulletImpactParticle.Instance.CreateBulletImpactAt(hit.point);
		}
	}

	IEnumerator CharacterDie()
	{
		GetComponent<FPSCharacterController>().enabled = false;

		GameLevel.Instance.SetLevelEndText();

		yield return StartCoroutine(LieBack());
		yield return StartCoroutine(SceneController.Instance.SceneEndFade(true));

		GUIManager.Instance.ShowCursor();
		SceneController.Instance.MyLoadScene(1, false, true, false);	//to main menu

		this.enabled = false;
	}

	IEnumerator LieBack()
	{
		float percent = 0.0f;

		ScreenEffectManager.Instance.enabled = true;
		ScreenEffectManager.Instance.eType = ScreenEffectManager.EffectType.BoxBlur;

		while(percent < 1f)
		{
			float rotateX = Mathf.Lerp(0f, -90f, percent);
			this.transform.rotation = Quaternion.Euler(rotateX, this.transform.eulerAngles.y, this.transform.eulerAngles.z);

			ScreenEffectManager.Instance.blurIterration = (int)Mathf.Lerp(0, 5, percent);

			percent += Time.deltaTime * 2;
			yield return null;
		}

		ScreenEffectManager.Instance.enabled = false;
	}
}
