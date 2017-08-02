using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CollectableItem : MonoBehaviour, ICollectable 
{
	public Item item;

	void Update()
	{
		transform.Rotate(new Vector3(60f, 60, 60f) * Time.deltaTime);
	}

	public void Collect()
	{
		GUIManager.Instance.playerInventory.AddItem(item);
		Destroy(this.gameObject);
	}
}
