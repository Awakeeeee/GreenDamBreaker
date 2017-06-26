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

	private SerializedProperty levelIDProp;
	private SerializedProperty levelNameProp;
	private SerializedProperty levelDescriptionProp;
	private SerializedProperty levelBGMProp;
	private SerializedProperty missionsProp;

	GameLevel self;

	GUIStyle labelStyle1;

	void OnEnable()
	{
		missionsProp = serializedObject.FindProperty("missions");
		levelIDProp = serializedObject.FindProperty("levelID");
		levelNameProp = serializedObject.FindProperty("levelName");
		levelDescriptionProp = serializedObject.FindProperty("levelDescription");
		levelBGMProp = serializedObject.FindProperty("levelBGM");

		self = target as GameLevel;

		SetupMissionList();

		labelStyle1 = new GUIStyle();
		labelStyle1.fontStyle = FontStyle.Italic;
		labelStyle1.normal.textColor = Color.cyan;
		labelStyle1.fontSize = 20;
	}

	public override void OnInspectorGUI ()
	{
		DrawLevelInfoPanel();
		DrawMusicPanel();
		DrawMissionPanel();
	}

	void DrawLevelInfoPanel()
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;

		EditorGUILayout.LabelField("Level Infomation", labelStyle1);
		GUILayout.Space(10f);

		EditorGUILayout.PropertyField(levelIDProp);
		EditorGUILayout.PropertyField(levelNameProp);

		GUILayout.Space(5f);

		EditorGUILayout.PropertyField(levelDescriptionProp);

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	void DrawMusicPanel()
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;

		EditorGUILayout.LabelField("Level Music", labelStyle1);
		GUILayout.Space(10f);

		EditorGUILayout.PropertyField(levelBGMProp);

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	void DrawMissionPanel()
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;

		EditorGUILayout.LabelField("Level Missions", labelStyle1);

		GUILayout.Space(10f);

		DrawMissionPopup();

		if(missionsProp.arraySize > 0)
		{
			for(int i = 0; i < missionsProp.arraySize; i++)
			{
				EditorGUILayout.PropertyField(missionsProp.GetArrayElementAtIndex(i));
			}
		}

		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();
	}

	void DrawMissionPopup()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		selection = EditorGUILayout.Popup(selection, missionNameList, GUILayout.Width(200f));
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();

		GUILayout.Space(5f);

		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if(GUILayout.Button("Add Mission", GUILayout.Width(120f)))
		{
			Type missionType = missionTypeList[selection];
			LevelMissionBase mission = CreateInstance(missionType) as LevelMissionBase;
			missionsProp.AddElementToArray(mission);
		}
		if(GUILayout.Button("Remove Mission", GUILayout.Width(120f)))
		{
			missionsProp.RemoveElementAtIndex(missionsProp.arraySize - 1);
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
