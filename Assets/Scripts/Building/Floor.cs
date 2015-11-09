using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	private Room[] m_rooms;

	public void Initialize()
	{	
		m_rooms = new Room[transform.childCount];
		for (int i = 0; i < m_rooms.Length; ++i)
		{
			m_rooms[i] = transform.GetChild (i).gameObject.GetComponent<Room> ();
			m_rooms[i].Initialize();
		}
	}

	// Use this for initialization
	void Start () {
	}

	public Room[] GetRooms()
	{
		return m_rooms;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\n   Floor " + name;
		display += "\n Composed of "+m_rooms.Length+" rooms :";
		display += "\n---------------------------------------";
		foreach (Room r in m_rooms)
			display += r.ToString();
		display += "\n---------------------------------------";

		return display;
	}
}
