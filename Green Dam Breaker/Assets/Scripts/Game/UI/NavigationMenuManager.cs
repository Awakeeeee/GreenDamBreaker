using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationMenuManager : MonoBehaviour 
{
	public CanvasGroup[] allPanels;

	void Start()
	{
		if(allPanels.Length <= 0)
		{
			Debug.LogWarning("Dont forget to set up menu panels in inspector.");
			return;
		}

		DeactiveAllPanels();
		ActivePanel(0);
	}

	public void ActivePanel(int index)
	{
		allPanels[index].alpha = 1;
		allPanels[index].gameObject.SetActive(true);
	}

	public void DeactiveAllPanels()
	{
		foreach(CanvasGroup cg in allPanels)
		{
			cg.alpha = 0f;
			cg.gameObject.SetActive(false);
		}
	}

	public void LoadScene(int sceneID)
	{
		if(SceneController.Instance == null)
		{
			Debug.LogWarning("Game should start at Persistent scene, check if you have persistent scene loaded.");
		}
		SceneController.Instance.MyLoadScene(sceneID, true, true, true);
	}

	public void OnClickExit()
	{
		Application.Quit();
	}
}
