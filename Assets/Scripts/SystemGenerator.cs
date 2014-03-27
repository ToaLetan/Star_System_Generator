using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct StarConnection
{
	public Vector3 startPoint;
	public Vector3 endPoint;
}

public class SystemGenerator
{
	#region Fields
	private static SystemGenerator instance = null;

	public List<GameObject> StarList = new List<GameObject>();
	//public List<Vector3> ConnectionList = new List<Vector3> ();

	public List<StarConnection> StarConnectionList = new List<StarConnection> ();

	private int numOfSystems = 0;
	private float minStarDistance = 1.5f;
	private float maxConnectDistance = 3.0f;

	private Vector2 minSystemPos = new Vector2(-2.5f, -2.5f);
	private Vector2 maxSystemPos = new Vector2(2.5f, 2.5f);

	#endregion

	#region Properties
	public static SystemGenerator Instance
	{
		get
		{
			if(instance == null)
				instance = new SystemGenerator();
			return instance;
		}
	}

	#endregion

	private SystemGenerator()
	{
		GenerateSystems (10);
		PopulateConnections ();
	}

	#region Methods
	// Update is called once per frame
	public void Update () 
	{

	}

	private void GenerateSystems(int numSystems)
	{
		numOfSystems = numSystems;

		for(int i = 0; i < numSystems; i++)
		{
			GameObject newStar = GameObject.Instantiate(Resources.Load("Prefabs/Point") ) as GameObject;
			newStar.transform.position = GenerateSystemPosition(minSystemPos, maxSystemPos);

			//Space out stars so they aren't too close to each other.
			SpaceOutSystems(newStar);

			StarList.Add(newStar);
		}
	}

	private Vector2 GenerateSystemPosition(Vector2 minPos, Vector2 maxPos)
	{
		float randXPos = Random.Range (minPos.x, maxPos.x);
		float randYPos = Random.Range (minPos.y, maxPos.y);

		return new Vector2(randXPos, randYPos);
	}

	public void PopulateConnections()
		//Each star searches all other stars, if it's within the maximum distance, create a new StarConnection between the two points and add it to the list.
	{
		for (int i = 0; i < StarList.Count; i++) 
		{
			for(int j = 0; j < StarList.Count; j++)
			{
				if(StarList[i] != StarList[j] && GetDistance(StarList[i].transform.position, StarList[j].transform.position) < maxConnectDistance)
				{
					StarConnection newConnection = new StarConnection();
					newConnection.startPoint = StarList[i].transform.position;
					newConnection.endPoint = StarList[j].transform.position;

					if(StarConnectionList.Contains(newConnection) == false)
						StarConnectionList.Add(newConnection);
				}
			}
		}
	}

	public void PopulateConnectionsByNum()
		//Each star generates an amount of connections to have, searches for closest stars, create a new StarConnection between the two points and add it to the list.
	{
		int randNumofConnections = 0;
		
		for (int i = 0; i < StarList.Count; i++) 
		{
			randNumofConnections = Random.Range (1, 20);
			List<GameObject> closestStar = GetNearestStars(StarList[i], randNumofConnections);
			
			for(int j = 0; j < closestStar.Count; j++)
			{
				StarConnection newConnection = new StarConnection();
				newConnection.startPoint = StarList[i].transform.position;
				newConnection.endPoint = closestStar[j].transform.position;
				
				if(StarConnectionList.Contains(newConnection) == false)
					StarConnectionList.Add(newConnection);
			}
		}

		/*int randNumofConnections = 0;

		for (int i = 0; i < StarList.Count; i++) 
		{
			randNumofConnections = Random.Range (1, 5);
			List<GameObject> closestStar = GetNearestStars(StarList[i], randNumofConnections);

			for(int j = 0; j < closestStar.Count; j++)
			{
				Debug.DrawLine(StarList[i].transform.position, closestStar[j].transform.position, Color.red);
			}
		}*/
	}



	private void SpaceOutSystems(GameObject newStar, bool allowForExpansion = false) //Recursive function used to prevent stars from being clustered together.
	{
		int numFarAwayStars = 0;
		Vector2 expansionRange = Vector2.zero;

		if(StarList.Count != 0 && numFarAwayStars < StarList.Count)
		{
			for(int c = 0; c < StarList.Count; c++)
			{

				//If the star is too close to another one, generate a new position
				if(GetDistance(newStar.transform.position, StarList[c].transform.position) < minStarDistance)
				{
					if(allowForExpansion)
					{
						expansionRange = new Vector2(minStarDistance, minStarDistance) * StarList.Count/10;
					}

					newStar.transform.position = GenerateSystemPosition(minSystemPos - expansionRange, maxSystemPos + expansionRange);
				}
				else
				{
					numFarAwayStars += 1;
				}
			}
		}

		if (numFarAwayStars != StarList.Count)
			SpaceOutSystems (newStar, allowForExpansion);
	}

	private List<GameObject> GetNearestStars(GameObject targetStar, int numNearestStars)
	{
		float shortestDist = 0;
		List<GameObject> returnStars = new List<GameObject>();

		for(int i = 0; i < numNearestStars; i++)
		{
			for (int j = 0; j < StarList.Count; j++) 
			{
				if(StarList[j] != targetStar && returnStars.Contains(StarList[j]) == false &&
				   (shortestDist == 0 || GetDistance(targetStar.transform.position, StarList[j].transform.position) < shortestDist ) )
				{
					shortestDist = GetDistance(targetStar.transform.position, StarList[j].transform.position);
					returnStars.Add(StarList[j]);
				}
			}
		}

		return returnStars;
	}

	private void ConnectSystemsByAmount() //Currently handles rendering of lines, will improve this later.
	{
		int randNumofConnections = 0;

		for (int i = 0; i < StarList.Count; i++) 
		{
			randNumofConnections = Random.Range (1, 5);
			List<GameObject> closestStar = GetNearestStars(StarList[i], randNumofConnections);

			for(int j = 0; j < closestStar.Count; j++)
			{
				Debug.DrawLine(StarList[i].transform.position, closestStar[j].transform.position, Color.red);
			}
		}
	}

	public float GetDistance(float point1, float point2)
	{
		return Mathf.Sqrt( (point1 - point2) * (point1 - point2) );
	}
	
	public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return Mathf.Sqrt( (point1.x - point2.x) * (point1.x - point2.x) + (point1.y - point2.y) * (point1.y - point2.y) );
	}

	#endregion
}
