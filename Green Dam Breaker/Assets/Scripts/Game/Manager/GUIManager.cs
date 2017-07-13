using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : SingletonBase<GUIManager>
{
	[Header("Health UI")]
	public CanvasGroup HealthPanel;
	public Text HealthText;

	[Header("Ammo UI")]
	public CanvasGroup AmmoPanel;
	public Text CurrentAmmoText, FullAmmoText;

	[Header("Intelligent Machine UI")]
	public CanvasGroup imPanel;
	public Animator imAnimator;
	public CanvasGroup mainPanel;
	public CanvasGroup armoryPanel;
	public RectTransform armoryContent;
	private CollectionButton[] gunCollection;
	private bool imOpen = false;

	[Header("Inventory")]
	public Inventory playerInventory;
	public CanvasGroup tooltip;
	public Text tipTitle;
	public Text tipDescription;

	[Header("Game Play")]
	public CanvasGroup sniperSightMask;

	void Start()
	{
		sniperSightMask.alpha = 0f;

		HideToolTip();
	}

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
			ActivateCanvasGroup(mainPanel);
			DeactivateCanvasGroup(armoryPanel);
			//TODO deactivate other panels
			imOpen = true;
			imAnimator.SetTrigger("Activate");
			ShowCursor();
		}
	}

	public void SetupCollectionUI()
	{
		gunCollection = armoryContent.GetComponentsInChildren<CollectionButton>();
	}

	public void SetCollectionButton(Gun gunInfo)
	{
		for(int i = 0; i< gunCollection.Length; i++)
		{
			if(!gunCollection[i].TogglePointerDown)
			{
				gunCollection[i].AddGun(gunInfo);
				return;
			}
		}
	}

	public void ShowToolTip(string tip, string desc)
	{
		tipTitle.text = tip;
		tipDescription.text = desc;

		tooltip.alpha = 1.0f;
	}

	public void HideToolTip()
	{
		tooltip.alpha = 0.0f;
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
