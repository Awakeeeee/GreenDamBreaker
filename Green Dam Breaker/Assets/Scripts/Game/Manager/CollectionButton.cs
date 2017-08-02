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

	private bool hasBeenSet = false;	//if the button has connected to gun collection data
	public bool HasBeenSet{get{return hasBeenSet;}}

	void Start()
	{
		
	}

	public void AddGun(Gun gunInfo)
	{
		hasBeenSet = true;
		btnText.text = gunInfo.gunName;
		attachedGunID = gunInfo.ID;
	}

	public void ClearBtn()
	{
		hasBeenSet = false;
		btnText.text = "Empty Btn";
		attachedGunID = -1;
	}

	public void OnPointerClick(PointerEventData e)
	{
		if(!hasBeenSet)
			return;

		PersonalIntelligentMachine.Instance.SelectGunFromCollection(attachedGunID);
	}
}
