using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KillTarget : LevelMissionBase
{
	public Health target;

	protected override void InternalOnEnable ()
	{
		
	}

	protected override void InternalOnDisable ()
	{
		
	}

	public override void CheckComplete ()
	{
		if(bCompleted)
			return;
		
		if(target.IsDead)
		{
			bCompleted = true;
		}
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