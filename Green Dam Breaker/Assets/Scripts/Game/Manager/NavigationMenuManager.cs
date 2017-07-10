using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMenuManager : MonoBehaviour 
{
	public void LoadScene(string sceneName)
	{
		if(SceneController.Instance == null)
		{
			Debug.LogWarning("Game should start at Persistent scene, check if you have persistent scene loaded.");
		}
		SceneController.Instance.MyLoadScene(sceneName);
	}
}
