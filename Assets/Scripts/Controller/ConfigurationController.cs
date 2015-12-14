using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ConfigurationController : MonoBehaviour {

	public Building m_building;
	public Text m_textConfiguredRoom;
	public Button m_buttonGo;

	private Dictionary<int, List<RoomContainer>> m_remainingRoomContainers;
	private int m_currentSelectedFloor;

	private List<int> m_floorTotalRoomNb;

	// Use this for initialization
	void Start () {
		m_currentSelectedFloor = 0;
		m_remainingRoomContainers = new Dictionary<int, List<RoomContainer>> ();
		m_floorTotalRoomNb = new List<int> ();
		m_building.Initialize ();
		for(int i = 0; i < m_building.GetFloors().Length; ++i)
		{
			m_remainingRoomContainers.Add(i, new List<RoomContainer>(m_building.GetFloors()[i].GetRoomContainers()));
			m_floorTotalRoomNb.Add(m_building.GetFloors()[i].GetRoomContainers().Length);
		}

	}

	private void Update()
	{
		if(m_currentSelectedFloor != -1)
		{
			List<RoomContainer> containerList;
			m_remainingRoomContainers.TryGetValue (m_currentSelectedFloor, out containerList);
			m_textConfiguredRoom.text = (m_floorTotalRoomNb.ToArray()[m_currentSelectedFloor]-containerList.Count) + "/"+ m_floorTotalRoomNb.ToArray()[m_currentSelectedFloor];
		}

		int remainingRooms = 0;
		foreach (KeyValuePair<int, List<RoomContainer>> pair in m_remainingRoomContainers)
			remainingRooms += pair.Value.Count;

		if(remainingRooms == 0)
			m_buttonGo.interactable = true;
	}

	public void SetCurrentSelectedFloor(int _floorNumber)
	{
		m_currentSelectedFloor = _floorNumber;
	}

	public void UpdateConfiguredRoomContainers(RoomContainer _container)
	{
		List<RoomContainer> roomContainerList;
		m_remainingRoomContainers.TryGetValue (m_currentSelectedFloor, out roomContainerList);

		roomContainerList.Remove (_container);
	}

	public void ResetConfigurationManager()
	{
		m_currentSelectedFloor = 0;
		m_remainingRoomContainers = new Dictionary<int, List<RoomContainer>> ();
		for(int i = 0; i < m_building.GetFloors().Length; ++i)
		{
			m_remainingRoomContainers.Add(i, new List<RoomContainer>(m_building.GetFloors()[i].GetRoomContainers()));
		}
	}
}
