using UnityEngine;
using System.Collections;

public class BuildingFloorManager : MonoBehaviour {
	
	public BuildingFloorListener[] m_listeners;
	
	public void UpdateListeners(int _dropdownNumber)
	{
		foreach (BuildingFloorListener listener in m_listeners)
			listener.FilterFloors(_dropdownNumber);
	}
}
