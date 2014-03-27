using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	SystemGenerator sysGenInstance = null;

	float lineWidth = 0.02f;

	// Use this for initialization
	void Start () 
	{
		sysGenInstance = SystemGenerator.Instance;

		for (int i = 0; i < sysGenInstance.StarConnectionList.Count; i++) 
		{
			RenderStarConnections(sysGenInstance.StarConnectionList[i]);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		sysGenInstance.Update ();
	}

	private void RenderStarConnections(StarConnection connection)
	{
		GameObject connectionObj = GameObject.Instantiate (Resources.Load ("Prefabs/StarConnection")) as GameObject;
		LineRenderer starConnectionRenderer = connectionObj.GetComponent<LineRenderer> ();

		starConnectionRenderer.SetWidth (lineWidth, lineWidth);
		starConnectionRenderer.renderer.material = Resources.Load ("LineMaterial") as Material;

		starConnectionRenderer.SetVertexCount (2);

		starConnectionRenderer.SetPosition (0, connection.startPoint);
		starConnectionRenderer.SetPosition (1, connection.endPoint);
	}
}
