using UnityEngine;
using System.Collections;

public class SystemGenerator
{
	#region Fields
	private static SystemGenerator instance = null;

	private int numOfSystems = 0;

	private Vector2 minSystemPos = new Vector2(-2, -2);
	private Vector2 maxSystemPos = new Vector2(2, 2);

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
		}
	}

	private Vector2 GenerateSystemPosition(Vector2 minPos, Vector2 maxPos)
	{
		float randXPos = Random.Range (minPos.x, maxPos.x);
		float randYPos = Random.Range (minPos.y, maxPos.y);

		return new Vector2(randXPos, randYPos);
	}

	private void ConnectSystems()
	{

	}

	#endregion
}
