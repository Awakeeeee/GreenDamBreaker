using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LevelMissionBase : ScriptableObject
{
	[Multiline]
	public string missionDescription;

	protected bool bCompleted;
	public bool IsCompleted {get {return bCompleted; }}

	protected void OnEnable()
	{
		InternalOnEnable();
	}

	protected abstract void InternalOnEnable();

	protected void OnDisable()
	{
		InternalOnEnable();
	}

	protected abstract void InternalOnDisable();

	protected abstract bool CheckComplete();
}
