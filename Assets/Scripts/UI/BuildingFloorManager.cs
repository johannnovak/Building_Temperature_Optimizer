using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingFloorManager : MonoBehaviour {
	
	public BuildingFloorListener[] m_listeners;
	public SelectionController m_selectionController;
	private List<BuildingFloorListener> m_listListener;

	private void Start()
	{
		BuildingFloorListener building = GameObject.Find ("building").GetComponent<BuildingFloorListener>();
		m_listListener = new List<BuildingFloorListener>(m_listeners);
		m_listListener.Add (building);
	}

	public void UpdateListeners(int _dropdownNumber)
	{
		m_selectionController.RoomDeselection ();
		foreach (BuildingFloorListener listener in m_listListener)
			listener.FilterFloors(_dropdownNumber);
	}
}
