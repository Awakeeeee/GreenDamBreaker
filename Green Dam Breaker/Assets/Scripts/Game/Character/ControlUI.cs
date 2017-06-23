using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUI : CharacterAbility
{
	protected override void Update ()
	{
		base.Update ();

		if(Input.GetButtonDown("IntelligentMachineUI"))
		{
			GUIManager.Instance.ToggleIntelligentMachine();
		}
	}
}
