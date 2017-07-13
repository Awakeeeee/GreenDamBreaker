using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

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

	public abstract void CheckComplete();
}

#if UNITY_EDITOR
[CustomEditor(typeof(LevelMissionBase))]
public abstract class LevelMissionBaseEditor : Editor
{
	LevelMissionBase self;
	bool expandFoldOut = true;

	void OnEnable()
	{
		self = target as LevelMissionBase;
		OnEnableChild();
	}

	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;

		expandFoldOut = EditorGUILayout.Foldout(expandFoldOut, GetMissionName());
		if(expandFoldOut)
		{
			OnInspectorGUIChild();
		}

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

		serializedObject.ApplyModifiedProperties();
	}

	protected virtual void OnInspectorGUIChild()
	{
		DrawDefaultInspector();
	}

	protected abstract void OnEnableChild();
	protected abstract string GetMissionName();

	public static LevelMissionBase CreateMission(Type t)
	{
		return ScriptableObject.CreateInstance(t) as LevelMissionBase;
	}
}
#endif