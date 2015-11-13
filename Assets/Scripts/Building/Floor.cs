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
			if(buildingObject.tag.Equals("room"))
			{
				RoomContainer roomContainer = buildingObject.GetComponent<RoomContainer> ();
				roomContainer.Initialize();
				roomContainers.Add(roomContainer);
			}
		}
		m_roomContainers = roomContainers.ToArray();
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
}
