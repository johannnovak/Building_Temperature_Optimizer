using UnityEngine;
using System.Collections;

public class BuildingFloorRoomContainerUpdater : BuildingFloorListener {

	private ConfigurationController m_controller;

	private void Start()
	{
		m_controller = GetComponent<ConfigurationController> ();
	}

	public override void FilterFloors (int _floorNumber)
	{
		m_controller.SetCurrentSelectedFloor (_floorNumber - 1);
	}
}
