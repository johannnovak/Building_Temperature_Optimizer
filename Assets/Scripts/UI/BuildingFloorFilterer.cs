using UnityEngine;
using System.Collections;

public class BuildingFloorFilterer : BuildingFloorListener {

	private Building m_building;
	private Floor m_displayedFloor;

	private void Start()
	{
		m_building = GetComponent<Building>();
	}

	public override void FilterFloors(int _floorNumber)
	{
		switch(_floorNumber)
		{
			case 0:	
				foreach(Floor f in m_building.GetFloors())
					f.gameObject.SetActive(true);
				m_displayedFloor = null;
				break;
			default:
				if(m_displayedFloor != null)
					m_displayedFloor.gameObject.SetActive(false);
				else
					foreach(Floor f in m_building.GetFloors())
						f.gameObject.SetActive(false);

				m_displayedFloor = m_building.GetFloors()[_floorNumber - 1];
				m_displayedFloor.gameObject.SetActive(true);
				break;
		}
	}
}
