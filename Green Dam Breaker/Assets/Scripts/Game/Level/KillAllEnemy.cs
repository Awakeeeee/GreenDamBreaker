using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class KillAllEnemy : LevelMissionBase
{
	public EnemySpawnerBase levelSpawner;

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

		if(levelSpawner.spawnerFinish)
		{
			bCompleted = true;
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(KillAllEnemy))]
public class KillAllEnemyEditor : LevelMissionBaseEditor
{
	protected override void OnEnableChild ()
	{

	}

	protected override string GetMissionName ()
	{
		return "消灭所有敌人";
	}
}
#endif
