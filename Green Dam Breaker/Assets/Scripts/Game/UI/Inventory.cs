using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
	public SlotUI[] slots;
	public ItemUI[] items;

	void Awake()
	{
		slots = GetComponentsInChildren<SlotUI>(true);
		items = GetComponentsInChildren<ItemUI>(true);

		for(int i = 0; i < slots.Length; i++)
		{
			slots[i].Init(i, this);
			items[i].Init(i, this, null, true);
		}
	}

	//TODO stackable item is not yet implemented
	public bool AddItem(Item itemData)
	{
		for(int i = 0; i < items.Length; i++)
		{
			if(items[i].isEmpty)
			{
				items[i].itemData = itemData;
				items[i].imageUI.sprite = itemData.sp;
				items[i].isEmpty = false;

				return true;
			}
		}

		Debug.LogWarning("Inventory is full.");
		return false;
	}

	public bool SearchItemByID(int id)
	{
		for(int i = 0; i < items.Length; i++)
		{
			if(items[i].isEmpty)
				continue;
			
			if(items[i].itemData.itemID == id)
				return true;
		}

		return false;
	}

	void ClearSlot(int pos)
	{
		items[pos].itemData = null;
		items[pos].isEmpty = true;
		items[pos].imageUI.sprite = null;
	}
}
