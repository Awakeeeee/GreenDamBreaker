using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor 
{
	MapGenerator self;

	void OnEnable()
	{
		self = target as MapGenerator;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if(GUILayout.Button("Generate Map"))
		{
			self.GenerateMap();
		}

		if(GUILayout.Button("Clear Map"))
		{
			self.ClearMap();
		}
	}
}
