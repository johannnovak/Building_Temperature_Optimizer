using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Floor : MonoBehaviour {

	private RoomContainer[] m_roomContainers;

	public void Initialize()
	{	
		List<RoomContainer> roomContainers = new List<RoomContainer> ();
		for (int i = 0; i < transform.GetChild(0).childCount; ++i)
		{
			GameObject buildingObject = transform.GetChild(0).GetChild(i).gameObject;
			if(buildingObject.tag.Equals("roomContainer"))
			{
				RoomContainer roomContainer = buildingObject.GetComponent<RoomContainer> ();
				roomContainer.Initialize();
				roomContainers.Add(roomContainer);
			}
		}
		m_roomContainers = roomContainers.ToArray();
	}

	public void InitializeWalls()
	{
		GameObject wallsContainer = transform.GetChild (1).gameObject;
		for(int i = 0; i < wallsContainer.transform.childCount; ++i)
		{
			Wall w = wallsContainer.transform.GetChild(i).GetComponent<Wall>();
			w.Initialize();
		}
	}

	// Use this for initialization
	void Start () {
	}

	public RoomContainer[] GetRoomContainers()
	{
		return m_roomContainers;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\n   Floor " + name;
		display += "\n Composed of "+m_roomContainers.Length+" rooms :";
		display += "\n---------------------------------------";
		foreach (RoomContainer r in m_roomContainers)
			display += r.ToString();
		display += "\n---------------------------------------";

		return display;
	}

	public void ResetFloor()
	{
		foreach (RoomContainer c in m_roomContainers)
			c.ResetRoomContainer ();
	}
}
