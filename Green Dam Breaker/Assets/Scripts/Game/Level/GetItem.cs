using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GetItem : LevelMissionBase 
{
	public Transform target;

	protected override void InternalOnEnable ()
	{
		
	}

	protected override void InternalOnDisable ()
	{
		
	}

	protected override bool CheckComplete ()
	{
		return false;
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