using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoints : MonoBehaviour 
{
	public Transform[] spawnPoints;

	public bool respawn;

	private FPSCharacterController character;

	void Start()
	{
		respawn = false;
		character = FindObjectOfType<FPSCharacterController>();
	}

	void Update()
	{
		Respawn();
	}

	void Respawn()
	{
		if(respawn)
		{
			int x = Random.Range(0, spawnPoints.Length);
			character.transform.position = spawnPoints[x].position;
			respawn = false;
		}
	}
}
