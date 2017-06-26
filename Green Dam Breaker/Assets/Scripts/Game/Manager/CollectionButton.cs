using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CollectionButton : MonoBehaviour, IPointerClickHandler
{
	public Text btnText;
	public Image btnImage;
	public int attachedGunID = -1;

	private bool togglePointerDown = false;
	public bool TogglePointerDown{get{return togglePointerDown;}}

	void Start()
	{
		
	}

	public void AddGun(Gun gunInfo)
	{
		togglePointerDown = true;
		btnText.text = gunInfo.gunName;
		attachedGunID = gunInfo.ID;
	}

	public void ClearBtn()
	{
		togglePointerDown = false;
		btnText.text = "Empty Btn";
		attachedGunID = -1;
	}

	public void OnPointerClick(PointerEventData e)
	{
		if(!togglePointerDown)
			return;

		PersonalIntelligentMachine.Instance.SelectGunFromCollection(attachedGunID);
	}
}
