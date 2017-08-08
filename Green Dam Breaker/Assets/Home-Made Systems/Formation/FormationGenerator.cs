using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FormationGenerator : MonoBehaviour
{
	[Header("Name tis formation:")]
	public string formationName = "Default Name";
	[HideInInspector]public List<Transform> nodes;

	public void AddNode()
	{
		GameObject newNode = new GameObject();
		newNode.transform.SetParent(this.transform);
		newNode.transform.localPosition = Vector3.zero + Vector3.up * 0.5f;
		newNode.name = "Node";
	}

	public void ClearNodes()
	{
		Transform[] childs = this.GetComponentsInChildren<Transform>(true);
		for(int i = 1; i < childs.Length; i++)
		{
			DestroyImmediate(childs[i].gameObject);
		}

		nodes.Clear();
		SetDefaultName();
	}

	public void SetDefaultName()
	{
		formationName = "Defualt Name" + Random.Range(0, 1000).ToString();
	}

	//-----------------------------

	void OnDrawGizmos()
	{
		nodes = new List<Transform>(this.GetComponentsInChildren<Transform>(false));
		nodes.RemoveAt(0);

		for(int i = 0; i < nodes.Count; i++)
		{
			//draw points
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(nodes[i].position, 1f);

			//connect points
			Gizmos.color = Color.red;
			if(i + 1 >= nodes.Count)
			{
				Gizmos.DrawLine(nodes[i].position, nodes[0].position);
			}else
			{
				Gizmos.DrawLine(nodes[i].position, nodes[i + 1].position);
			}
		}
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(FormationGenerator))]
public class FormationGeneratorEditor : Editor
{
	FormationGenerator self;

	void OnEnable()
	{
		self = target as FormationGenerator;
	}

	public override void OnInspectorGUI ()
	{
		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("阵型编辑器:");
		EditorGUILayout.LabelField("在任意场景使用这个东西编辑一个阵型，");
		EditorGUILayout.LabelField("然后将阵型保存为数据文件");
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

		EditorGUILayout.Space();

		if(GUILayout.Button("Add Node"))
		{
			self.AddNode();
		}

		EditorGUILayout.Space();

		base.OnInspectorGUI ();

		EditorGUILayout.Space();

		if(GUILayout.Button("Reset"))
		{
			self.ClearNodes();
		}

		EditorGUILayout.Space();

		EditorGUILayout.Space();

		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("- 点击\"Save Current Formation\"将把当前阵型保存为位置数据");
		EditorGUILayout.LabelField("- 保存的数据文件在: Assets-Set it when you use this system");
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

		if(GUILayout.Button("Save Current Formation"))
		{
			FormationData formation = ScriptableObject.CreateInstance("FormationData") as FormationData;

			Vector3[] data = new Vector3[self.nodes.Count];
			for(int i = 0; i < self.nodes.Count; i++)
			{
				data[i] = self.nodes[i].position;	//save world position
			}

			formation.Setup(self.formationName, data);

			AssetDatabase.CreateAsset(formation, "Assets/Prefabs/FormationData/" + self.formationName.ToString() + ".asset");
			AssetDatabase.SaveAssets();

			self.SetDefaultName();
		}
	}
}
#endif
