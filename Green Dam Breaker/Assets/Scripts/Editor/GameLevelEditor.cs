using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(GameLevel))]
public class GameLevelEditor : EditorWithSubEditors<LevelMissionBaseEditor, LevelMissionBase>
{
	public Type[] missionTypeList;	//why need a type list? Create scriptable object instance requires a type
	public string[] missionNameList;
	private int selection;

	private SerializedProperty missionsProp;

	void OnEnable()
	{
		missionsProp = serializedObject.FindProperty("missions");

		SetupMissionList();
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		DrawMissionPanel();
	}

	void DrawMissionPanel()
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;
	}

	void DrawMissionPopup()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		selection = EditorGUILayout.Popup(selection, missionNameList, GUILayout.Width(200f));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Add Mission", GUILayout.Width(150f)))
		{
			Type missionType = missionTypeList[selection];
			LevelMissionBase mission = CreateInstance(missionType) as LevelMissionBase;
			missionsProp.AddElementToArray(mission);
		}
		if(GUILayout.Button("Clear Mission List", GUILayout.Width(150f)))
		{
			missionsProp.ClearArray();
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}

	void SetupMissionList()
	{
		//get the type I am looking for
		Type targetType = typeof(LevelMissionBase);
		//get the assembly of targetType, then extract all types in it
		Type[] allTypes = targetType.Assembly.GetTypes();

		List<Type> wantedTypes = new List<Type>();

		foreach(Type t in allTypes)
		{
			if(t.IsSubclassOf(targetType) && !t.IsAbstract)	//search type of targetType in all types
			{
				wantedTypes.Add(t);
			}
		}
		missionTypeList = wantedTypes.ToArray();

		//Type list finish, handle name list

		List<string> tempNameList = new List<string>();
		foreach(Type t in wantedTypes)
		{
			tempNameList.Add(t.Name);
		}
		missionNameList = tempNameList.ToArray();
	}

	protected override void InitEachSubEditor (LevelMissionBaseEditor editor)
	{
		return;
	}
}
