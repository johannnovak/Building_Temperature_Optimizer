using UnityEngine;
using System.Collections;

public class BuildingFloorFilterer : BuildingFloorListener {

	private Building m_building;
	private Floor m_displayedFloor;
	public Camera m_mainCamera;
	private int m_cullingMaskNoFloors;
	private int m_cullingMaskAllFloors;

	private void Start()
	{
		m_building = GetComponent<Building>();
		m_building.Initialize ();
		m_cullingMaskAllFloors = m_mainCamera.cullingMask;
		m_cullingMaskNoFloors = m_cullingMaskAllFloors;

		for(int i = 0; i < m_building.GetFloors().ToArray().Length; ++i)
		{
			string layerName = "floor_"+i;
			int layerMask = 1 << LayerMask.NameToLayer(layerName);
			m_cullingMaskNoFloors &= ~layerMask;
		}
	}

	public override void FilterFloors(int _floorNumber)
	{
		switch(_floorNumber)
		{
			case 0:	
				m_mainCamera.cullingMask = m_cullingMaskAllFloors;
				break;
			default:
				int layerMask = 1 << LayerMask.NameToLayer("floor_"+(_floorNumber-1));
				m_mainCamera.cullingMask = m_cullingMaskNoFloors | layerMask;
				break;
		}
	}
}
