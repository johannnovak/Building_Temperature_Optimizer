using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ConfigurationController : MonoBehaviour {

	private Building m_building;
	public Text m_textConfiguredRoom;
	public Button m_buttonConfigure;
	public Button m_buttonGo;

	private Dictionary<int, List<RoomContainer>> m_remainingRoomContainers;
	private int m_currentSelectedFloor;

	private List<int> m_floorTotalRoomNb;

	// Use this for initialization
	void Start () {
		m_building = GameObject.Find ("building").GetComponent<Building> ();
		m_currentSelectedFloor = -1;
		m_remainingRoomContainers = new Dictionary<int, List<RoomContainer>> ();
		m_floorTotalRoomNb = new List<int> ();
		m_building.Initialize ();
		for(int i = 0; i < m_building.GetFloors().ToArray().Length; ++i)
		{
			m_remainingRoomContainers.Add(i, new List<RoomContainer>(m_building.GetFloors()[i].GetRoomContainers()));
			m_floorTotalRoomNb.Add(m_building.GetFloors()[i].GetRoomContainers().Length);
		}
	}

	private void Update()
	{	
		int totalRCNumber = 0;
		int remainingRooms = 0;

		if(m_currentSelectedFloor != -1)
		{
			List<RoomContainer> containerList;
			m_remainingRoomContainers.TryGetValue (m_currentSelectedFloor, out containerList);

			remainingRooms = containerList.Count;

			totalRCNumber += m_floorTotalRoomNb.ToArray()[m_currentSelectedFloor];
			//m_textConfiguredRoom.text = (m_floorTotalRoomNb.ToArray()[m_currentSelectedFloor]-containerList.Count) + "/"+ m_floorTotalRoomNb.ToArray()[m_currentSelectedFloor];
		}
		else
		{
			foreach(int rcNumber in m_floorTotalRoomNb)
				totalRCNumber += rcNumber;
			
			foreach (KeyValuePair<int, List<RoomContainer>> pair in m_remainingRoomContainers)
			{
				remainingRooms += pair.Value.Count;
			}			
		}

		int configuredRooms = totalRCNumber - remainingRooms;
		m_textConfiguredRoom.text = configuredRooms.ToString() + "/" + totalRCNumber.ToString();

		if(remainingRooms == 0)
			m_buttonConfigure.interactable = true;
	}

	public void SetCurrentSelectedFloor(int _floorNumber)
	{
		m_currentSelectedFloor = _floorNumber;
	}

	public void UpdateConfiguredRoomContainers(RoomContainer _container)
	{
		int currentSelectedFloor = -1;
		if(m_currentSelectedFloor == -1)
		{
			foreach(KeyValuePair<int, List<RoomContainer>> pair in m_remainingRoomContainers)
			{
				if(pair.Value.Contains(_container))
				{
					currentSelectedFloor = pair.Key;
					break;
				}
			}
			if(currentSelectedFloor == -1)
				return;
		}
		else
		{
			currentSelectedFloor = m_currentSelectedFloor;
		}

		List<RoomContainer> roomContainerList;
		m_remainingRoomContainers.TryGetValue (currentSelectedFloor, out roomContainerList);
		roomContainerList.Remove (_container);
	}

	public void ResetConfigurationManager()
	{
		m_currentSelectedFloor = 0;
		m_remainingRoomContainers = new Dictionary<int, List<RoomContainer>> ();
		for(int i = 0; i < m_building.GetFloors().ToArray().Length; ++i)
		{
			m_remainingRoomContainers.Add(i, new List<RoomContainer>(m_building.GetFloors()[i].GetRoomContainers()));
		}
	}

	public Building GetBuilding()
	{
		return m_building;
	}
}
