using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : PersistentSingletonBase<GUIManager>
{
	public CanvasGroup HealthPanel, AmmoPanel;

	public Text HealthText, CurrentAmmoText, FullAmmoText;

	[Header("Intelligent Machine UI")]
	public CanvasGroup imPanel;
	public Animator imAnimator;
	public CanvasGroup mainPanel;
	public CanvasGroup armoryPanel;
	private bool imOpen = false;

	public void UpdateText(Text UIText, string content)
	{
		UIText.text = content;
	}

	public void ShowText(Text t)
	{
		t.gameObject.SetActive(true);
		t.enabled = true;
	}

	public void DeactivateCanvasGroup(CanvasGroup cg)
	{
		cg.interactable = false;
		cg.blocksRaycasts = false;
		cg.alpha = 0f;
	}

	public void ActivateCanvasGroup(CanvasGroup cg)
	{
		cg.interactable = true;
		cg.blocksRaycasts = true;
		cg.alpha = 1.0f;
	}

	public void ToggleIntelligentMachine()
	{
		if(imOpen)
		{
			imOpen = false;
			imAnimator.SetTrigger("Deactivate");
			HideCursor();
		}else{
			imOpen = true;
			imAnimator.SetTrigger("Activate");
			ShowCursor();
		}
	}

	public void ShowCursor()
	{
		Cursor.lockState = CursorLockMode.None;	//lock to center of screen
		Cursor.visible = true;
	}
	public void HideCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
