using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonalIntelligentMachine : SingletonBase<PersonalIntelligentMachine>
{
	//follow order main = 0, armory = 1 TODO state will be 2 and so on
	public CanvasGroup[] panels;
	private Animator anim;

	[Header("Amory Panel")]
	public RectTransform armoryContent;
	private CollectionButton[] gunSelectBtns;

	[SerializeField]private bool isOpenning;

	//int is gun ID, gun is the gameObject
	public Dictionary<int, Gun> gunDictionry;

	public Gun CurrentGun{
		get
		{
			Gun[] existingGuns = FPSCharacterController.Instance.hands.GetComponentsInChildren<Gun>();
			for(int i = 0; i < existingGuns.Length; i++)
			{
				if(existingGuns[i].isActiveAndEnabled)
					return existingGuns[i];
			}
			return null;
		}
	}

	void Awake()
	{
		anim = GetComponent<Animator>();

		gunDictionry = new Dictionary<int, Gun>();														//the dictionary keeps track of owned guns
		gunSelectBtns = armoryContent.GetComponentsInChildren<CollectionButton>();						//the list keeps track of the states of UI gun buttons in armory, the buttons should be all empty at start, then initialized by 'existingGuns'
		Gun[] existingGuns = FPSCharacterController.Instance.hands.GetComponentsInChildren<Gun>(true);	//see what guns the player starts with, used to initialize the dictionary

		if(existingGuns.Length > 0)
		{
			foreach(Gun g in existingGuns)
			{
				//init dictionary
				gunDictionry.Add(g.ID, g);
				//init UI gun buttons
				SetGunButton(g);
			}
		}
	}

	public void AddGunToCollection(Gun gun)			//gun is gun data, newGun is the real gun object created from gun data
	{
		Gun newGun;
		if(AddToHands(gun, out newGun))				//add gun object to hands, this checks if already own this gun
		{
			gunDictionry.Add(newGun.ID, newGun);	//add to dictionary, link to gun gameobject under hands
			SetGunButton(gun);						//update UI
		}
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
		foreach(KeyValuePair<int, Gun> gun in gunDictionry)	//traverse dictionary
		{
			gun.Value.gameObject.SetActive(false);
		}
	}

	void SetGunButton(Gun gunInfo)
	{
		for(int i = 0; i< gunSelectBtns.Length; i++)
		{
			if(!gunSelectBtns[i].HasBeenSet)
			{
				gunSelectBtns[i].AddGun(gunInfo);
				return;
			}
		}
	}

	bool AddToHands(Gun pickGun, out Gun newGunFromPickGun)
	{
		foreach(KeyValuePair<int, Gun> gun in gunDictionry)
		{
			if(gun.Value.ID == pickGun.ID)
			{
				newGunFromPickGun = null;
				return false;
				//TODO already have it , decide what to do
				//collect ammo? just cannot pick up?
			}
		}

		//if not return means currently donnot has this gun, then create a gun from the gun data, and pass the real gun object out
		newGunFromPickGun = Instantiate(pickGun, FPSCharacterController.Instance.hands) as Gun;
		newGunFromPickGun.gameObject.SetActive(false);
		return true;
	}

	//-----Navigate panels------
	public void DeactivateAllPanels()
	{
		foreach(CanvasGroup cg in panels)
		{
			cg.interactable = false;
			cg.blocksRaycasts = false;
			cg.alpha = 0f;
		}
	}

	public void ActivatePanel(CanvasGroup cg)
	{
		DeactivateAllPanels();
		cg.interactable = true;
		cg.blocksRaycasts = true;
		cg.alpha = 1.0f;
	}
		
	public void ToggleIntelligentMachine()
	{
		if(isOpenning)
		{
			isOpenning = false;
			anim.SetTrigger("Deactivate");
			GUIManager.Instance.HideCursor();
		}else{
			ActivatePanel(panels[0]);
			//TODO deactivate other panels
			isOpenning = true;
			anim.SetTrigger("Activate");
			GUIManager.Instance.ShowCursor();
		}
	}
}
