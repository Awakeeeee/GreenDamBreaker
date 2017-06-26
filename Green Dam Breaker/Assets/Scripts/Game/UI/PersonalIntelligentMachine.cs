using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalIntelligentMachine : SingletonBase<PersonalIntelligentMachine>
{
	//int is gun ID, gun is the gameObject
	public Dictionary<int, Gun> gunDictionry;

	void Awake()
	{
		gunDictionry = new Dictionary<int, Gun>();

		Gun[] existingGuns = FPSCharacterController.Instance.hands.GetComponentsInChildren<Gun>(true);
		GUIManager.Instance.SetupCollectionUI();

		if(existingGuns.Length > 0)
		{
			foreach(Gun g in existingGuns)
			{
				gunDictionry.Add(g.ID, g);	//init dictionary
				GUIManager.Instance.SetCollectionButton(g);	//init UI
			}
		}
	}

	public void AddGunToCollection(Gun gun)
	{
		gunDictionry.Add(gun.ID, gun);	//add to dictionary, link to gun gameobject under hands
		GUIManager.Instance.SetCollectionButton(gun);	//update UI
	}

	public void SelectGunFromCollection(int ID)
	{
		Gun theGun;

		if(gunDictionry.TryGetValue(ID, out theGun))
		{
			BareHands();
			theGun.gameObject.SetActive(true);
		}
	}

	public void BareHands()
	{
		Gun[] existingGuns = FPSCharacterController.Instance.hands.GetComponentsInChildren<Gun>();
		for(int i = 0; i < existingGuns.Length; i++)
		{
			existingGuns[i].gameObject.SetActive(false);
		}
	}
}
