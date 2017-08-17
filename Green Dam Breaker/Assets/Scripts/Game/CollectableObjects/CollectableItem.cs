using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectableItem : Collectable, ICollectable 
{
	public Item item;

	public void Collect()
	{
		GUIManager.Instance.playerInventory.AddItem(item);
		Destroy(this.transform.parent.gameObject);
	}
}
