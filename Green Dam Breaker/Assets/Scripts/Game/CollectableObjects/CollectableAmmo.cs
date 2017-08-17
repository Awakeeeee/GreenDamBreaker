using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAmmo : Collectable, ICollectable
{
	public int ammo = 20;

	public void Collect()
	{
		PersonalIntelligentMachine.Instance.CurrentGun.CollectAmmo(ammo);
		Destroy(parent.gameObject);
	}
}
