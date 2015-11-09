﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingFloorListener : MonoBehaviour {

	public Building m_building;
	private Dropdown m_dropdown;

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
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
