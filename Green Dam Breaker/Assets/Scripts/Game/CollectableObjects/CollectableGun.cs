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
			g.gameObject.SetActive(false);

			if(g.ID == gunPrefab.ID)
			{
				checkHave = true;
				g.gameObject.SetActive(true);
				//TODO ammo? anim?
			}
		}

		if(!checkHave)
		{
			gunPrefab = Instantiate(gunPrefab, playerHands);
		}

		checkHave = false;
	}
}
