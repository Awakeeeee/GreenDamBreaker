using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDatabase : MonoBehaviour
{
	private static FormationDatabase instance;

	public static FormationDatabase Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<FormationDatabase>();

				if(instance == null)
				{
					Debug.LogError("Singleton error: No FormationDatabase in scene.");
				}

				return instance;
			}

			return instance;	
		}
	}

	public List<FormationData> database;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}else if(instance != this)
		{
			Destroy(this.gameObject);
		}

		DontDestroyOnLoad(this.gameObject);
	}

	public FormationData PickRandomFormation(int enemyNum)
	{
		List<FormationData> validFormations = new List<FormationData>();

		for(int i = 0; i < database.Count; i++)
		{
			if(database[i].formation.Count < enemyNum)
				continue;

			validFormations.Add(database[i]);
		}

		int x = Random.Range(0, validFormations.Count);
		return validFormations[x];
	}
}
