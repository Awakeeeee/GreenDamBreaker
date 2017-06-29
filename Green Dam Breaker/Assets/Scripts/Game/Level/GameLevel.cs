using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour 
{
	public int levelID;
	public string levelName;
	[Multiline]
	public string levelDescription;

	public LevelMissionBase[] missions = new LevelMissionBase[0];

	public AudioClip levelBGM;

	void Update()
	{
		//complete everyone to complete this level
		for(int i = 0; i < missions.Length; i++)
		{
			if(!missions[i].IsCompleted)
				return;
		}

		Debug.Log("Level is completed.");
	}
}
