using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationWaveSpawner : MonoBehaviour {

	public FWave[] waves;
	public EnemyHP enemyPrefab;

	public bool spawnerFinish = false;

	private FWave currentWave;
	private int currentWaveNumber;
	private int waveAliveEnemyNumber;	//when wave aline enemy count is 0, start next wave
	private int waveRemainingEnemyNumber;	//the enemy waiting to be spawned in a wave
	private float nextSpawnTime;

	void Start()
	{
		currentWaveNumber = 0;
		spawnerFinish = false;

		for(int i = 0; i < waves.Length; i++)
		{
			waves[i].Setup(3);
		}

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
		//If something should happen to tell player that an enemy is spawnning, do it here, and within currentWave.spawningTime
		yield return new WaitForSeconds(currentWave.spawningTime);

		EnemyHP newEnemy = Instantiate(enemyPrefab, currentWave.formationData.GetNextPosition(), Quaternion.identity) as EnemyHP;
		newEnemy.transform.localRotation = Quaternion.Euler(0, 180, 0);

		newEnemy.OnDeath += OnEnemyDeath;
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
public class FWave
{
	public float spawnInterval = 0f;
	public float spawningTime = 0f;
	public FormationData formationData;
	public int waveEnemyTotal;

	public void Setup(int enemyNum)
	{
		waveEnemyTotal = enemyNum;
		formationData = FormationDatabase.Instance.PickRandomFormation(enemyNum);
	}
}
