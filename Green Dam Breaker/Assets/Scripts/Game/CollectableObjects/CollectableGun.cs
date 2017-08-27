using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGun : Collectable, ICollectable
{
	public Gun gun;

	void OnEnable()
	{
		
	}

	public void Collect()
	{
		PersonalIntelligentMachine.Instance.AddGunToCollection(gun);
		GameManager.Instance.ownedGuns.Add(gun);
		GUIManager.Instance.PlayCollectGunPrompt();
		SoundManager.Instance.PlayClip2D(pickSFX);
		Destroy(this.transform.parent.gameObject);
	}
}
