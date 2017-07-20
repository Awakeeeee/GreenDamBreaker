using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FormationViewer : MonoBehaviour 
{
	public FormationData checkData;
	[HideInInspector]public List<Transform> nodes;

	void AddSpecificNode(Vector3 pos)
	{
		GameObject newNode = new GameObject();
		newNode.transform.SetParent(this.transform);
		newNode.transform.position = pos;
		newNode.name = "Node";
	}

	void ClearNodes()
	{
		Transform[] childs = this.GetComponentsInChildren<Transform>(true);
		for(int i = 1; i < childs.Length; i++)
		{
			DestroyImmediate(childs[i].gameObject);
		}

		nodes.Clear();
	}

	public void CheckFormation()
	{
		ClearNodes();

		if(checkData == null)
		{
			Debug.LogWarning("Checking formation is null!!!");
			return;
		}

		for(int i = 0; i < checkData.formation.Count; i++)
		{
			AddSpecificNode(checkData.formation[i]);
		}
	}

	//----------
	void OnDrawGizmos()
	{
		nodes = new List<Transform>(this.GetComponentsInChildren<Transform>(false));
		nodes.RemoveAt(0);

		for(int i = 0; i < nodes.Count; i++)
		{
			//draw points
			Gizmos.color = Color.black;
			Gizmos.DrawSphere(nodes[i].position, 1.5f);

			//connect points
			Gizmos.color = Color.blue;
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
[CustomEditor(typeof(FormationViewer))]
public class FormationViewerEditor : Editor
{
	FormationViewer self;

	void OnEnable()
	{
		self = target as FormationViewer;
	}

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.BeginVertical(GUI.skin.box);
		EditorGUI.indentLevel++;
		EditorGUILayout.LabelField("- 点击查看当前的阵型数据");
		EditorGUI.indentLevel--;
		EditorGUILayout.EndVertical();

		if(GUILayout.Button("Check Formation"))
		{
			self.CheckFormation();
		}
	}
}
#endif
