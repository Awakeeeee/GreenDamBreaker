using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : MonoBehaviour 
{
	public int levelID;
	public string levelName;
	[Multiline]
	public string levelDescription;

	public LevelMissionBase[] missions;

	public AudioClip levelBGM;
}
