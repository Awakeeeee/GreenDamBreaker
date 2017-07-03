using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour 
{
	public GameObject tile;
	public GameObject obstacle;
	public int sizeX, sizeY;
	public Transform tileParent;
	[Range(0f, 1.0f)]
	public float edgePercentage;
	public int obstaclePosRandomSeed;

	private List<Coord> originalCoords;
	private Queue<Coord> shuffledCoords; 

	void Start()
	{
		GenerateMap();
	}

	public void GenerateMap()
	{
		ClearMap();
		SetupCoords();

		//map center at wpos (0,0,0), tile scale is (1,1,1)
		for(int i = 0; i < sizeX; i++)
		{
			for(int j = 0; j < sizeY; j++)
			{
				Vector3 pos = CoordToPosition(i,j);
				GameObject newTile = Instantiate(tile, pos, Quaternion.Euler(Vector3.right * 90f));
				newTile.transform.localScale *= (1 - edgePercentage);
				newTile.transform.SetParent(tileParent);
			}
		}

		//generate obstacles
		for(int i = 0; i < 10; i++)
		{
			Coord c = GetRandomCoord();
			Vector3 pos = CoordToPosition(c.x, c.y);
			GameObject obs = Instantiate(obstacle, pos, Quaternion.identity);
			obs.transform.position += Vector3.up * 0.5f;
			obs.transform.SetParent(tileParent);
		}
	}

	public void ClearMap()
	{
		if(tileParent.childCount > 0)
		{
			Transform[] children = tileParent.GetComponentsInChildren<Transform>();	// this will get itself and all its children
			//start from 1, exclude self
			for(int i = 1; i < children.Length; i++)
			{
				DestroyImmediate(children[i].gameObject);
			}
		}
	}

	void SetupCoords()
	{
		originalCoords = new List<Coord>();
		for(int x = 0; x < sizeX; x++)
		{
			for(int y = 0; y < sizeY; y++)
			{
				originalCoords.Add(new Coord(x, y));
			}
		}

		shuffledCoords = new Queue<Coord>(Utility.FisherYatesShuffle(originalCoords.ToArray(), obstaclePosRandomSeed));
	}

	//Queue cannot use Queue[i] to get each element, so use this method
	Coord GetRandomCoord()
	{
		Coord c = shuffledCoords.Dequeue();
		shuffledCoords.Enqueue(c);
		return c;
	}

	///up direction is not calculated and will always be 0
	Vector3 CoordToPosition(int x, int y)
	{
		Vector3 pos = new Vector3(-sizeX/2 + x + 0.5f, 0f, -sizeY/2 + y + 0.5f);
		return pos;
	}
}

public struct Coord
{
	public int x;
	public int y;

	public Coord(int _x, int _y)
	{
		x = _x;
		y = _y;
	}
}
