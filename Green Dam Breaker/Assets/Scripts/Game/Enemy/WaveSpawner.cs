using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour 
{
	public Transform[] spawnPoints;

	public Wave[] waves;
	public EnemyHP enemyPrefab;

	private Wave currentWave;
	private int currentWaveNumber;
	private int waveAliveEnemyNumber;	//when wave aline enemy count is 0, start next wave
	private int waveRemainingEnemyNumber;	//the enemy waiting to be spawned in a wave
	private float nextSpawnTime;

	void Start()
	{
		currentWaveNumber = 0;
		NextWave();
	}

	void Update()
	{
		if(waveAliveEnemyNumber > 0)
		{
			if(Time.time >= nextSpawnTime && waveRemainingEnemyNumber > 0)
			{
				EnemyHP newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPos(), Quaternion.identity);
				nextSpawnTime = Time.time + currentWave.spawnInterval;
				waveRemainingEnemyNumber --;
				newEnemy.OnDeath += OnEnemyDeath;	//TODO no where to -= this method, does destroy the enemy do the work?
			}
		}else{
			NextWave();
		}
	}

	void OnEnemyDeath()
	{
		waveAliveEnemyNumber --;
	}

	void NextWave()
	{
		currentWaveNumber ++;
		if(currentWaveNumber <= waves.Length)
		{
			currentWave = waves[currentWaveNumber - 1];
			waveAliveEnemyNumber = currentWave.waveEnemyTotal;
			waveRemainingEnemyNumber = currentWave.waveEnemyTotal;
			nextSpawnTime = Time.time + currentWave.spawnInterval;
		}else{
			Debug.LogWarning("All enemy from this spawner has been spawned.");
		}
	}

	Vector3 GetRandomSpawnPos()
	{
		int x = Random.Range(0, spawnPoints.Length);
		return spawnPoints[x].position;
	}
}

[System.Serializable]
public class Wave
{
	public int waveEnemyTotal;
	public float spawnInterval;
}
