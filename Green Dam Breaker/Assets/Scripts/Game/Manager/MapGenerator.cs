using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour 
{
	public GameObject tile;
	public GameObject obstacle;
	public Vector2 obstacleHeightRange;
	public Color foreGroundColor;
	public Color backGroundColor;
	public float scaler = 1;
	public int sizeX, sizeY;
	public Coord startCoord;
	public Transform tileParent;
	[Range(0f, 1.0f)]
	public float edgePercentage;
	[Range(0f, 1.0f)]
	public float obstaclePercentage;
	public int randomSeed;

	private Transform[,] tileMap;
	private List<Coord> originalCoords;
	private Queue<Coord> shuffledCoords;
	private List<Coord> openCoords;
	private Queue<Coord> shuffledOpenCoords;

	//---test---
	CustomStruct[] testStruct;
	CustomClass[] testClasses;
	Coord[] testCoords;

	void Start()
	{
		GenerateMap();
	}

	public void GenerateMap()
	{
		ClearMap();
		SetupCoords();

		//map center at wpos (0,0,0), tile scale is (1,1,1)
		tileMap = new Transform[sizeX, sizeY];
		for(int i = 0; i < sizeX; i++)
		{
			for(int j = 0; j < sizeY; j++)
			{
				Coord c = new Coord(i, j);
				Vector3 pos = CoordToPosition(c);
				GameObject newTile = Instantiate(tile, pos, Quaternion.Euler(Vector3.right * 90f));
				newTile.transform.localScale *= scaler;
				newTile.transform.localScale *= (1 - edgePercentage);
				newTile.transform.SetParent(tileParent);

				tileMap[i,j] = newTile.transform;
				openCoords.Add(c);
			}
		}

		//generate obstacles
		System.Random sr = new System.Random(randomSeed);
		int obstacleNum = (int)(sizeX * sizeY * obstaclePercentage);
		int currentObstacleCount = 0;
		bool[,] obstacleMap = new bool[sizeX, sizeY];	//mark if a coordinate stands an obstacle

		for(int i = 0; i < obstacleNum; i++)
		{
			//before start, pretend to spawn this obstacle, and then check validity by the accessible algorithm
			Coord c = GetRandomCoord();
			obstacleMap[c.x, c.y] = true;
			currentObstacleCount++;

			if(c != startCoord && IsMapFullyAccessible(obstacleMap, currentObstacleCount) == true)	//after check this obstacle is indeed OK to spawn
			{
				float height = Mathf.Lerp(obstacleHeightRange.x, obstacleHeightRange.y, (float)sr.NextDouble());
				Vector3 pos = CoordToPosition(c);
				GameObject obs = Instantiate(obstacle, pos, Quaternion.identity);
				obs.transform.localScale = new Vector3(obs.transform.localScale.x * scaler * (1 - edgePercentage), height, obs.transform.localScale.z * scaler * (1 - edgePercentage));
				obs.transform.position += Vector3.up * 0.5f * height;
				obs.transform.SetParent(tileParent);

				Renderer obsRenderer = obs.GetComponent<Renderer>();
				Material newMat = new Material(obsRenderer.sharedMaterial);	//create a new material, which is a copy of obs's shared mat
				float distPercent = (float)c.y / (float)sizeY;
				newMat.color = Color.Lerp(foreGroundColor, backGroundColor, distPercent);	//modify the new mat
				obsRenderer.material = newMat;	//now actually every obstacle has its own material
			
				openCoords.Remove(c);
			}
			else   //after check, this obstacle turns to make map inaccessible, cancel spawning
			{
				obstacleMap[c.x, c.y] = false;
				currentObstacleCount--;
			}
		}

		shuffledOpenCoords = new Queue<Coord>(Utility.FisherYatesShuffle(openCoords.ToArray(), randomSeed));
	}

	//Next time when you want to understand this method, try work it out in a tic-tac-toe 9-grid map
	//this method exucetes one time when each obstacle is trying to spawn
	bool IsMapFullyAccessible(bool[,] obstacleMap, int currentObstacleCount)
	{
		bool[,] checkBook = new bool[obstacleMap.GetLength(0), obstacleMap.GetLength(1)];	//mark if a coordinate has already been looked

		Queue<Coord> envaluationQueue = new Queue<Coord>();	//when coord is in queque, it has been looked and it must be a path tile. After check its neighbours it is removed from queue

		int calculatedPathTileCount = 1;	//1 since startCoord is always path tile

		envaluationQueue.Enqueue(startCoord);	//always from this coordinate, and start a radiation style check(pretty much like A* path finding)
		checkBook[startCoord.x, startCoord.y] = true;

		while(envaluationQueue.Count > 0)
		{
			Coord thisCoord = envaluationQueue.Dequeue();

			for(int x = -1; x < 2; x++)
			{
				for(int y = -1; y < 2; y++)	//loop through the 8 neighboours of current coordinate
				{
					Coord neighbour = new Coord(thisCoord.x + x, thisCoord.y + y);

					if(x != 0 && y != 0)	//ignore the 4 diagonal coord, because 4 diagonal obstacles won't block map path
						continue;

					if(neighbour.x < 0 || neighbour.x > obstacleMap.GetLength(0) - 1 || neighbour.y < 0 || neighbour.y > obstacleMap.GetLength(1) - 1)	//check if the neighbour coord is not in bound of map
						continue;
					
					if(checkBook[neighbour.x, neighbour.y] == true)	//check if this coord has already been looked
						continue;

					if(obstacleMap[neighbour.x, neighbour.y] == false)	//there is no obstacle at this coord
					{
						calculatedPathTileCount++;
						envaluationQueue.Enqueue(neighbour);	//within next several while loop, this neight will be envaluated as thisCoord
						checkBook[neighbour.x, neighbour.y] = true;
					}
				}
			}
		}

		int targetPathTileCount = sizeX * sizeY - currentObstacleCount;
		return calculatedPathTileCount == targetPathTileCount;
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

		shuffledCoords = new Queue<Coord>(Utility.FisherYatesShuffle(originalCoords.ToArray(), randomSeed));

		openCoords = new List<Coord>();
	}

	//Queue cannot use Queue[i] to get each element, so use this method
	Coord GetRandomCoord()
	{
		Coord c = shuffledCoords.Dequeue();
		shuffledCoords.Enqueue(c);
		return c;
	}

	public Transform GetRandomOpenTile()
	{
		Coord c = shuffledOpenCoords.Dequeue();
		shuffledOpenCoords.Enqueue(c);
		return tileMap[c.x, c.y];
	}

	///up direction is not calculated and will always be 0
	Vector3 CoordToPosition(Coord coord)
	{
		Vector3 pos = new Vector3(-sizeX/2f + (coord.x + 0.5f) * scaler, 0f, -sizeY/2f + (coord.y + 0.5f) * scaler);
		return pos;
	}

	void LogOut()
	{
		testStruct = new CustomStruct[]{new CustomStruct(tile.gameObject, null), new CustomStruct(obstacle.gameObject, null), new CustomStruct(obstacle.gameObject, null)};
		Debug.Log("Struct with different reference type fields:");
		foreach(CustomStruct c in testStruct)
		{
			Debug.Log(c.GetHashCode());
		}

		testCoords = new Coord[]{new Coord(0, 0), new Coord(1,1), new Coord(2,2)};
		Debug.Log("Struct with different value type fields:");
		foreach(Coord c in testCoords)
		{
			Debug.Log(c.GetHashCode());
		}

		testClasses = new CustomClass[]{new CustomClass(1, 1), new CustomClass(1,1), new CustomClass(2,2)};
		Debug.Log("Class with different value type fields:");
		foreach(CustomClass c in testClasses)
		{
			Debug.Log(c.GetHashCode());
		}
	}
}

[System.Serializable]
public struct Coord
{
	public int x;
	public int y;
 
	public Coord(int _x, int _y)
	{
		x = _x;
		y = _y;
	}

	public static bool operator ==(Coord a, Coord b)
	{
		return a.x == b.x && a.y == b.y;
	}

	public static bool operator !=(Coord a, Coord b)
	{
		return !(a == b);
	}

	public override bool Equals (object o)
	{
		Coord c = (Coord)o;
		return this == c;
	}

	public override int GetHashCode ()	//TODO you should use the fields in the struct, to produce an unique ID, this is what GetHashCode does
	{
		return x^y;	//xor-ing(01 gets 1, 10 gets 1, 11 or 00 gets 0)
	}

	public override string ToString ()
	{
		return string.Format ("[Coord({0},{1})]", x, y);
	}
}

public class CustomClass
{
	int x;
	int y;

	public CustomClass(int _x, int _y)
	{
		x = _x;
		y = _y;
	}
}

public struct CustomStruct
{
	GameObject gg;
	GameObject hh;

	public CustomStruct(GameObject _g, GameObject _h)
	{
		gg = _g;
		hh = _h;
	}
}
