using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomContainer : MonoBehaviour {

	private List<Room> m_rooms;
	private float m_volume;

	private float m_objectiveTemperature;
	private float m_minDeliveredEnergy;
	private float m_maxDeliveredEnergy;
	private float m_currentDeliveredEnergy;

	private bool m_containsCommandableActionners;

	// Use this for initialization
	public void Initialize() {
		m_rooms = new List<Room> ();
		m_volume = 0;
		m_objectiveTemperature = float.NaN;
		m_minDeliveredEnergy = float.NaN;
		m_maxDeliveredEnergy = float.NaN;
		m_currentDeliveredEnergy = float.NaN;
		
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
		m_volume += (_room.GetLength()*_room.GetWidth()*_room.GetHeight());
	}

	public float GetRoomVolume()
	{
		return m_volume;
	}
	
	public void SetObjectiveTemperature(float _temperature)
	{
		m_objectiveTemperature = _temperature;
	}

	public float GetObjectiveTemperature()
	{
		return m_objectiveTemperature;
	}

	public float GetMinDeliveredEnergy()
	{
		return m_minDeliveredEnergy;
	}

	public float GetMaxDeliveredEnergy()
	{
		return m_maxDeliveredEnergy;
	}
	
	public void SetCurrentDeliveredEnergy(float _energy)
	{
		m_currentDeliveredEnergy = _energy;
	}
	
	public float GetCurrentDeliveredEnergy()
	{
		return m_currentDeliveredEnergy;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\nComposed of "+m_rooms.ToArray().Length+" room spaces.";
		foreach (Room r in m_rooms)
			display += r.ToString ();

		return display;
	}

	public void ResetRoomContainer()
	{
		m_objectiveTemperature = float.NaN;
		m_currentDeliveredEnergy = float.NaN;
		m_minDeliveredEnergy = float.NaN;
		m_maxDeliveredEnergy = float.NaN;
		m_volume = 0;
	}
}
