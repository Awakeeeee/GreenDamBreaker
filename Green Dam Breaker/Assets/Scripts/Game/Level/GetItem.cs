using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GetItem : LevelMissionBase 
{
	public Item target;

	protected override void InternalOnEnable ()
	{
		InspectObject.pickEvent += OnPickupKeyItem;
	}

	protected override void InternalOnDisable ()
	{
		InspectObject.pickEvent -= OnPickupKeyItem;
	}

	public override void CheckComplete ()
	{
		//this mission use the event to check complete
	}

	void OnPickupKeyItem()
	{
		if(GUIManager.Instance.playerInventory.SearchItemByID(target.itemID))
		{
			bCompleted = true;
		}
	}
}


#if UNITY_EDITOR
[CustomEditor(typeof(GetItem))]
public class GetItemEditor : LevelMissionBaseEditor
{
	protected override void OnEnableChild ()
	{

	}

	protected override string GetMissionName ()
	{
		return "取得目标物品"; 
	}
}
#endif