using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevel : SingletonBase<GameLevel> 
{
	public int levelID;
	public string levelName;
	[Multiline]
	public string levelDescription;
	public bool isLastLevel = false;

	public LevelMissionBase[] missions = new LevelMissionBase[0];

	public AudioClip levelBGM;

	bool levelSuccess = false;

	void OnEnable()
	{
		levelSuccess = false;

		if(SceneController.Instance != null)
		{
			SceneController.Instance.LoadSceneEvent += SetLevelStartText;
		}
	}

	void OnDisable()
	{
		if(SceneController.Instance != null)
		{
			SceneController.Instance.LoadSceneEvent -= SetLevelStartText;
		}
	}

	void Update()
	{
		//if no mission set in this level, don't check
		if(missions.Length <= 0)
			return;

		//TEST
		if(Input.GetKeyDown(KeyCode.L))
		{
			if(!levelSuccess)
			{
				StartCoroutine(LevelSuccess());
			}
		}

		//complete everyone to complete this level
		for(int i = 0; i < missions.Length; i++)
		{
			missions[i].CheckComplete();

			if(!missions[i].IsCompleted)
				return;
		}

		if(!levelSuccess)
		{
			StartCoroutine(LevelSuccess());
		}
	}

	IEnumerator LevelSuccess()
	{
		levelSuccess = true;

		//TODO maybe there are others things need to be disabled
		FPSCharacterController.Instance.enabled = false;
		SetLevelEndText();

		yield return StartCoroutine(SceneController.Instance.SceneEndFade(false));
		GUIManager.Instance.ShowCursor();
		//level success also lead to next level, if all levels are donw, go to main menu
		if(!isLastLevel)
		{
			SceneController.Instance.LoadNextScene(true, true, true);	
		}else{
			SceneController.Instance.LoadNextScene(true, true, false);
		}
	}

	void SetLevelStartText()
	{
		SceneController.Instance.SetOverallPropmpt("Level " + levelID + " - " + levelName, Color.white);
	}

	public void SetLevelEndText()
	{
		if(levelSuccess)
		{
			SceneController.Instance.SetOverallPropmpt("Mission Accomplished", Color.green);
		}else{
			SceneController.Instance.SetOverallPropmpt("You Die", Color.red);
		}
	}
}
