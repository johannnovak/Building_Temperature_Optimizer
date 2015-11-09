using UnityEngine;
using System.Collections;

public class Floor : MonoBehaviour {

	private Room[] m_rooms;

	// Use this for initialization
	void Start () {
		m_rooms = new Room[transform.childCount];
		for (int i = 0; i < m_rooms.Length; ++i)
			m_rooms [i] = transform.GetChild (i).gameObject.GetComponent<Room> ();
	}
}
