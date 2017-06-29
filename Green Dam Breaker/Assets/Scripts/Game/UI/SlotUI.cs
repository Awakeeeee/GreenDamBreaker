using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IDropHandler
{
	public int inventorySlotID;
	public Inventory parentInventory;

	public void Init(int listID, Inventory inven)
	{
		inventorySlotID = listID;
		parentInventory = inven;
	}

	public void OnDrop(PointerEventData e)
	{
		Debug.Log("Slot on drop is fired");

		ItemUI draggingItem = e.pointerDrag.GetComponent<ItemUI>();
		if(draggingItem == null)
		{
			Debug.LogError("Dragging item doesnot have an itemUI component, I didnt handle this condition.");
			return;
		}
		ItemUI childItem = this.GetComponentInChildren<ItemUI>();

		int saveID = childItem.inventoryItemID;
		Inventory saveInven = childItem.parentInventory;

		childItem.transform.SetParent(e.pointerDrag.transform.parent);
		childItem.transform.localPosition = Vector3.zero;
		childItem.inventoryItemID = draggingItem.inventoryItemID;
		childItem.parentInventory = draggingItem.parentInventory;

		draggingItem.transform.SetParent(this.transform);
		draggingItem.inventoryItemID = saveID;
		draggingItem.parentInventory = saveInven;
	}
}
