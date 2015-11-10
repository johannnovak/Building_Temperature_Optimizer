using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingFloorListener : MonoBehaviour {

	public Building m_building;
	private Dropdown m_dropdown;
	private Floor m_displayedFloor;

	// Use this for initialization
	void Start () {
		m_dropdown = GetComponent<Dropdown> ();
		Dropdown.OptionData option = new Dropdown.OptionData ();
		option.text = "All floors";
		m_dropdown.options.Add (option);

		m_dropdown.transform.GetChild (0).GetComponent<Text> ().text = option.text;

		foreach(Floor f in m_building.GetFloors())
		{
			option = new Dropdown.OptionData ();
			option.text = f.name;
			m_dropdown.options.Add (option);
		}

		m_displayedFloor = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FilterFloors(int _floorNumber)
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
				m_displayedFloor = m_building.GetFloors()[_floorNumber - 1];
				m_displayedFloor.gameObject.SetActive(true);
				break;
		}
	}
}
