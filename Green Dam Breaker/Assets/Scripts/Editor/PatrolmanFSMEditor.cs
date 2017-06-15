using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatrolmanFSM))]
public class PatrolmanFSMEditor : Editor 
{
	PatrolmanFSM self;

	GUIStyle labelStyle = new GUIStyle();

	void OnEnable()
	{
		self = target as PatrolmanFSM;

		labelStyle.normal.textColor = Color.blue;
		labelStyle.fontSize = 15;
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.LabelField("Current AI State:", labelStyle);
		if(self.currentState != null)
		{
			EditorGUILayout.LabelField(self.currentState.ToString(), labelStyle);
		}else{
			EditorGUILayout.LabelField("No State | Game not started", labelStyle);
		}
		base.OnInspectorGUI ();
	}
}
