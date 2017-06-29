using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
	public int inventoryItemID;
	public bool isEmpty;
	public Item itemData;
	public Inventory parentInventory;

	public Image imageUI;
	public CanvasGroup cgUI;

	public void Init(int listID, Inventory inven, Item data = null, bool empty = true)
	{
		inventoryItemID = listID;
		parentInventory = inven;
		itemData = data;
		isEmpty = empty;
	}

	public void OnBeginDrag(PointerEventData e)
	{
		this.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
		cgUI.blocksRaycasts = false;
	}

	public void OnDrag(PointerEventData e)
	{
		this.transform.position = e.position;
	}

	public void OnEndDrag(PointerEventData e)
	{
		Debug.Log("Item on end drag is fired");
		this.transform.localScale = Vector3.one;
		this.transform.localPosition = Vector3.zero;
		cgUI.blocksRaycasts = true;
	}

	public void OnPointerEnter(PointerEventData e)
	{
		if(isEmpty)
			return;

		GUIManager.Instance.ShowToolTip(itemData.itemName, itemData.infomation);
		GUIManager.Instance.tooltip.transform.position = this.transform.position;
	}

	public void OnPointerExit(PointerEventData e)
	{
		if(isEmpty)
			return;

		GUIManager.Instance.HideToolTip();
	}
}
