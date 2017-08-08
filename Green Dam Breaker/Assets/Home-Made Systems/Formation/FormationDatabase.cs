using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationDatabase : SingletonBase<FormationDatabase>
{
	public List<FormationData> database;

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
