using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomContainer : MonoBehaviour {

	private List<Room> m_rooms;

	public int number { get; set;}

	public float Volume { get; private set;}

	public float ObjectiveTemperature { get; set;}
	public float MinDeliveredEnergy {get; set;}
	public float MaxDeliveredEnergy { get; set;}
	public float CurrentDeliveredEnergy { get; set;}

	public bool Prepared { get; set;}

	private bool m_containsCommandableActionners;

	// Use this for initialization
	public void Initialize() {
		m_rooms = new List<Room> ();
		Volume = 0;
		ObjectiveTemperature = float.NaN;
		MinDeliveredEnergy = float.NaN;
		MaxDeliveredEnergy = float.NaN;
		CurrentDeliveredEnergy = float.NaN;
		Prepared = false;
		
		m_containsCommandableActionners = false;

		for (int i = 0; i < transform.childCount; ++i)
		{
			m_rooms.Add(transform.GetChild(i).gameObject.GetComponent<Room> ());
			m_rooms.ToArray()[i].Initialize();

			AddVolume(m_rooms.ToArray()[i]);
			
			m_containsCommandableActionners |= (m_rooms.ToArray()[i].GetCommandableActionnerList().ToArray().Length > 0);
		}
	}

	public bool ContainsCommandableActionners()
	{
		return m_containsCommandableActionners;
	}

	public List<Room> GetRooms()
	{
		return m_rooms;
	}

	private void AddVolume(Room _room)
	{
		Volume += _room.Volume;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\nRoom container : " + name;
		display += "\n Composed of "+m_rooms.ToArray().Length+" room spaces.";
		foreach (Room r in m_rooms)
			display += r.ToString ();
		display += "\nEndRoomContainer";

		return display;
	}

	public void ResetRoomContainer()
	{
		ObjectiveTemperature = float.NaN;
		CurrentDeliveredEnergy = float.NaN;
		MinDeliveredEnergy = float.NaN;
		MaxDeliveredEnergy = float.NaN;

		Prepared = false;
	}
}
