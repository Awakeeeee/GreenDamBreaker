using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoundCubeMesh))]
public class RoundCubeMeshEditor : Editor 
{
	RoundCubeMesh self;

	void OnEnable()
	{
		self = target as RoundCubeMesh;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		if(GUILayout.Button("Create Mesh"))
		{
			self.GenerateRoundCube();
			string assetName = string.Format("{0}{1}{2}{3}_R_Cube", self.sizeX.ToString(), self.sizeY.ToString(), self.sizeZ.ToString(), self.roundness.ToString());
			AssetDatabase.CreateAsset(self.MeshAsset, "Assets/Models/ProceduralMeshes/" + assetName + ".asset");
			AssetDatabase.SaveAssets();
		}

		if(GUILayout.Button("Clear Mesh"))
		{
			self.ClearMesh();
		}

		EditorGUILayout.LabelField("My notes:");
		EditorGUILayout.LabelField("Press \"Create Mesh\" to generate the");
		EditorGUILayout.LabelField("mesh asset at Assets/Models/ProceduralMeshes");

		EditorGUILayout.LabelField("-------------------------------------------");

		EditorGUILayout.LabelField("Press \"Clear Mesh\" to clear the settings on this generator.");
		EditorGUILayout.LabelField("REMEMEBER to delete the generated mesh asset manually.");
	}
}
