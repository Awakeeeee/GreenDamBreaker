using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KillTarget : LevelMissionBase
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
[CustomEditor(typeof(KillTarget))]
public class KillTargetEditor : LevelMissionBaseEditor
{
	protected override void OnEnableChild ()
	{

	}

	protected override string GetMissionName ()
	{
		return "杀死指定目标";
	}
}
#endif