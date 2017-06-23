using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableGun : MonoBehaviour, ICollectable
{
	public Gun gunPrefab;

	Gun[] existingGuns = new Gun[0];
	Transform playerHands;
	bool checkHave;

	void OnEnable()
	{
		playerHands = FPSCharacterController.Instance.hands;
		checkHave = false;
	}

	public void Collect()
	{
		existingGuns = playerHands.GetComponentsInChildren<Gun>(true);
		foreach(Gun g in existingGuns)
		{
			if(g.ID == gunPrefab.ID)
			{
				checkHave = true;
				//TODO ammo? anim?
			}
		}

		if(!checkHave)
		{
			Gun newGun = Instantiate(gunPrefab, playerHands) as Gun;
			newGun.gameObject.SetActive(false);
			PersonalIntelligentMachine.Instance.AddGunToCollection(newGun);
			Destroy(this.gameObject);
		}else{
			//already have it , decide what to do
			//collect ammo? just cannot pick up?
		}

		checkHave = false;
	}
}
