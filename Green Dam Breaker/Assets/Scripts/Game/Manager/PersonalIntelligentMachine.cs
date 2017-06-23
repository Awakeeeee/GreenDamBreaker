using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalIntelligentMachine : SingletonBase<PersonalIntelligentMachine>
{
	//int is gun ID, gun is the gameObject
	public Dictionary<int, Gun> gunCollection;

	void Awake()
	{
		gunCollection = new Dictionary<int, Gun>();

		Gun[] existingGuns = FPSCharacterController.Instance.hands.GetComponentsInChildren<Gun>(true);
		if(existingGuns.Length > 0)
		{
			foreach(Gun g in existingGuns)
			{
				gunCollection.Add(g.ID, g);
			}
		}
	}

	public void AddGunToCollection(Gun gun)
	{
		gunCollection.Add(gun.ID, gun);

		//Update ui
	}

	public void SelectGunFromCollection(int ID)
	{
		Gun theGun;

		if(gunCollection.TryGetValue(ID, out theGun))
		{
			theGun.gameObject.SetActive(true);
		}
	}
}
