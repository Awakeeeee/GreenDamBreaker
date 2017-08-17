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
		Destroy(this.transform.parent.gameObject);
	}
}
