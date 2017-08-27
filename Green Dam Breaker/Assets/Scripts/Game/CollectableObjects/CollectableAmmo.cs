using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAmmo : Collectable, ICollectable
{
	public int ammo = 20;

	public void Collect()
	{
		if(PersonalIntelligentMachine.Instance.CurrentGun == null)
			return;
		
		PersonalIntelligentMachine.Instance.CurrentGun.CollectAmmo(ammo);
		SoundManager.Instance.PlayClip2D(pickSFX);
		Destroy(parent.gameObject);
	}
}
