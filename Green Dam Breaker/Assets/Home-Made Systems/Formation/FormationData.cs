using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationData : ScriptableObject
{
	public string fName;
	public List<Vector3> formation;

	public void Setup(string name, params Vector3[] nodes)
	{
		formation = new List<Vector3>(nodes);
		fName = name;
	}

	public Vector3 GetNextPosition()
	{
		if(formation == null || formation.Count <= 0)
		{
			Debug.LogError("Formation data is not yet set up.");
			return Vector3.zero;
		}

		Vector3 v;

		//this is using list as a queue
		//i'm not using queue because queue is not automatically serialized by Unity
		v = formation[0];
		formation.RemoveAt(0);
		formation.Add(v);

		return v;
	}
}
