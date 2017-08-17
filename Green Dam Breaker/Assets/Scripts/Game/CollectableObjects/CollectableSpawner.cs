using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSpawner : MonoBehaviour 
{
	[Header("Overall Setting")]
	public GameObject collectable;
	public float spawnInterval;
	public float springOutHeight;

	[Header("Mode")]
	public bool randomSpawn;
	public Vector3 specificSpawnPos;

	private MapGenerator mapG;
	private float spawnTimer;

	void Start()
	{
		mapG = FindObjectOfType<MapGenerator>();
		spawnTimer = 0.0f;
	}

	void Update()
	{
		spawnTimer += Time.deltaTime;

		if(spawnTimer > spawnInterval)
		{
			spawnTimer = 0.0f;
			if(randomSpawn)
			{
				StartCoroutine(SpawnCollectableRandom());
			}else
			{
				StartCoroutine(SpawnCollectableAt());
			}
		}
	}

	IEnumerator SpawnCollectableAt()
	{
		GameObject newC = Instantiate(collectable, specificSpawnPos, Quaternion.identity);
		yield return StartCoroutine(CollectableSpringOut(newC, specificSpawnPos));
	}

	IEnumerator SpawnCollectableRandom()
	{
		float timer = 0.0f;
		Renderer tile = mapG.GetRandomOpenTile().GetComponent<Renderer>();

		Color fromColor = tile.material.color;
		Color toColor = Color.green;

		while(timer < 1)
		{
			Color frameColor = Color.Lerp(fromColor, toColor, Utility.MyMathPingPoing(timer * 4, 1f));

			tile.material.color = frameColor;

			timer += Time.deltaTime;
			yield return null;
		}

		tile.material.color = fromColor;
		GameObject newC = Instantiate(collectable, tile.transform.position, Quaternion.identity);
		yield return StartCoroutine(CollectableSpringOut(newC, tile.transform.position));
	}

	IEnumerator CollectableSpringOut(GameObject c, Vector3 spawnPos)
	{
		c.transform.SetParent(this.transform);
		c.transform.position = spawnPos;

		float timer = 0.0f;
		//go up
		while(timer < 0.5f)
		{
			Vector3 newPos = Vector3.Lerp(c.transform.position, spawnPos + Vector3.up * springOutHeight, timer / 0.5f);
			c.transform.position = newPos;
			timer += Time.deltaTime;
			yield return null;
		}

		timer = 0.0f;
		//drop
		while(timer < 0.5f)
		{
			Vector3 newPos = Vector3.Lerp(c.transform.position, spawnPos + Vector3.up * springOutHeight * 0.75f, timer / 0.5f);
			c.transform.position = newPos;
			timer += Time.deltaTime;
			yield return null;
		}
	}
}
