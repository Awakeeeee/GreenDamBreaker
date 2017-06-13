using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The method I want to call on this event is not on animator's object, so I made this script.
//Seek for other solution.
public class AnimationEventHandler : MonoBehaviour 
{
	public void EventReloadingFinish()
	{
		SendMessageUpwards("ReloadingFinish");
	}

	public void EventReloadingSFX()
	{
		SendMessageUpwards("ReloadingSFX");
	}
}
