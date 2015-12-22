using UnityEngine;
using System.Collections;

public class BuildingFloorManager : MonoBehaviour {
	
	public BuildingFloorListener[] m_listeners;
	public SelectionController m_selectionController;

	public void UpdateListeners(int _dropdownNumber)
	{
		m_selectionController.RoomDeselection ();
		foreach (BuildingFloorListener listener in m_listeners)
			listener.FilterFloors(_dropdownNumber);
	}
}
