using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : PersistentSingletonBase<GUIManager>
{
	public CanvasGroup HealthPanel, AmmoPanel;

	public Text HealthText, CurrentAmmoText, FullAmmoText;

	public void UpdateText(Text UIText, string content)
	{
		UIText.text = content;
	}
}
