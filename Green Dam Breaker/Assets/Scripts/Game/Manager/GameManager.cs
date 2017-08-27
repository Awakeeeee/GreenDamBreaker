using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//I want to save global data
public class GameManager : PersistentSingletonBase<GameManager>
{
	public List<Gun> ownedGuns;

	void Start()
	{
		ownedGuns = new List<Gun>();
	}

	void OnEnable()
	{
		if(SceneController.Instance != null)
			SceneController.Instance.LoadSceneEvent += ReadData;
	}

	void OnDisable()
	{
		if(SceneController.Instance != null)
			SceneController.Instance.LoadSceneEvent += ReadData;
	}

	void ReadData()
	{
		for(int i = 0; i < ownedGuns.Count; i++)
		{
			PersonalIntelligentMachine.Instance.AddGunToCollection(ownedGuns[i]);
		}
	}

	public void ClearData()
	{
		ownedGuns.Clear();
	}
}
