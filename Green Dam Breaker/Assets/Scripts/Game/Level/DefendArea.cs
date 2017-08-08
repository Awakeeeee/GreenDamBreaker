using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DefendArea : LevelMissionBase
{
	public int enemyNumber;
	public AIStateMachine enemyPrefab;

	List<AIStateMachine> enemies;

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
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(DefendArea))]
public class DefendAreaEditor : LevelMissionBaseEditor
{
	protected override void OnEnableChild ()
	{
		
	}

	protected override string GetMissionName ()
	{
		return "坚守阵地，杀死来犯的敌人！";
	}
}
#endif
