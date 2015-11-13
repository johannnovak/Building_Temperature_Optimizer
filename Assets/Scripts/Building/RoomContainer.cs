using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomContainer : MonoBehaviour {

	private Room[] m_rooms;
	private float m_volume;

	private float m_objectiveTemperature;
	private float m_deliveredEnergy;

	// Use this for initialization
	public void Initialize() {
		m_volume = 0;
		m_objectiveTemperature = float.NaN;
		m_deliveredEnergy = float.NaN;

		m_rooms = new Room[transform.childCount];

		for (int i = 0; i < m_rooms.Length; ++i)
		{
			m_rooms[i] = transform.GetChild(i).gameObject.GetComponent<Room> ();
			m_rooms[i].Initialize();

			AddVolume(m_rooms[i]);
		}
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
	
	public void SetDeliveredEnergy(float _energy)
	{
		m_deliveredEnergy = _energy;
	}
	
	public float GetDeliveredEnergy()
	{
		return m_deliveredEnergy;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\nComposed of "+m_rooms.Length+" room spaces.";
		foreach (Room r in m_rooms)
			display += r.ToString ();

		return display;
	}

	public void ResetRoomContainer()
	{
		m_objectiveTemperature = float.NaN;
		m_deliveredEnergy = float.NaN;
		m_volume = 0;
	}
}
