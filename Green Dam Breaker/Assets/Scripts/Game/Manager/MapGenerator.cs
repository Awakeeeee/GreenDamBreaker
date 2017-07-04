using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour 
{
	public GameObject tile;
	public GameObject obstacle;
	public float scaler = 1;
	public int sizeX, sizeY;
	public Coord startCoord;
	public Coord[] testCoords;
	public Transform tileParent;
	[Range(0f, 1.0f)]
	public float edgePercentage;
	[Range(0f, 1.0f)]
	public float obstaclePercentage;
	public int obstaclePosRandomSeed;

	private List<Coord> originalCoords;
	private Queue<Coord> shuffledCoords; 

	void Start()
	{
		GenerateMap();
		LogOut();
	}

	void LogOut()
	{
		foreach(Coord c in testCoords)
		{
			Debug.Log(c.ToString() + " gethascode : " + c.GetHashCode());
		}
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
				newTile.transform.localScale *= scaler;
				newTile.transform.localScale *= (1 - edgePercentage);
				newTile.transform.SetParent(tileParent);
			}
		}

		//generate obstacles
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
				Vector3 pos = CoordToPosition(c.x, c.y);
				GameObject obs = Instantiate(obstacle, pos, Quaternion.identity);
				obs.transform.localScale *= scaler;
				obs.transform.position += Vector3.up * 0.5f * scaler;
				obs.transform.SetParent(tileParent);
			}
			else   //after check, this obstacle turns to make map inaccessible, cancel spawning
			{
				obstacleMap[c.x, c.y] = false;
				currentObstacleCount--;
			}
		}
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
		Vector3 pos = new Vector3(-sizeX/2 + (x + 0.5f) * scaler, 0f, -sizeY/2 + (y + 0.5f) * scaler);
		return pos;
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

	public override int GetHashCode ()	//TODO what is the hash code return of a struct?
	{
		return base.GetHashCode();
	}

	public override string ToString ()
	{
		return string.Format ("[Coord({0},{1})]", x, y);
	}
}
