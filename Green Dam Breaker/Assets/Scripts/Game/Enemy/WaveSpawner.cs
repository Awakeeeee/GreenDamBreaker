using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : EnemySpawnerBase
{
	public Wave[] waves;
	public EnemyHP enemyPrefab;

	private Wave currentWave;
	private int currentWaveNumber;
	private int waveAliveEnemyNumber;	//when wave aline enemy count is 0, start next wave
	private int waveRemainingEnemyNumber;	//the enemy waiting to be spawned in a wave
	private float nextSpawnTime;

	private MapGenerator mapG;

	public bool IsSpawningFinish{ get {return spawnerFinish; }}

	void Start()
	{
		mapG = FindObjectOfType<MapGenerator>();
		currentWaveNumber = 0;
		spawnerFinish = false;
		NextWave();
	}

	void Update()
	{
		if(waveAliveEnemyNumber > 0)
		{
			if(Time.time >= nextSpawnTime && waveRemainingEnemyNumber > 0)
			{
				nextSpawnTime = Time.time + currentWave.spawnInterval;
				waveRemainingEnemyNumber --;
				StartCoroutine(SpawnEnemy());
			}
		}else{
			NextWave();
		}
	}

	IEnumerator SpawnEnemy()
	{
		float timer = 0.0f;

		Renderer tile = mapG.GetRandomOpenTile().GetComponent<Renderer>();

		Color fromColor = tile.material.color;
		Color toColor = Color.red;

		while(timer < currentWave.spawnTime)
		{
			Color frameColor = Color.Lerp(fromColor, toColor, Utility.MyMathPingPoing(timer * currentWave.flashSpeed, 1f));

			tile.material.color = frameColor;

			timer += Time.deltaTime;
			yield return null;
		}

		tile.material.color = fromColor;
		EnemyHP newEnemy = Instantiate(enemyPrefab, tile.transform.position, Quaternion.identity);
		newEnemy.OnDeath += OnEnemyDeath;	//TODO no where to -= this method, does destroy the enemy do the work?
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
			spawnerFinish = true;
		}
	}
}

[System.Serializable]
public class Wave
{
	public int waveEnemyTotal;
	public float spawnInterval;
	public float spawnTime = 1.0f;
	public int flashSpeed = 4;	//when spawnTime is 1, flash 4 times
}
