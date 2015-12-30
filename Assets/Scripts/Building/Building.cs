using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class Building : MonoBehaviour {

	private List<Floor> m_floors;
	public Text m_textRoomConfiguredCount;
	private bool m_initialized = false;


	// Use this for initialization
	void Start () {
		Initialize();
	}

	public void Initialize()
	{
		if (!m_initialized) 
		{
			m_floors = new List<Floor> ();
			int realsize = 0;
			for (int i = 0; i < transform.childCount; ++i)
			{
				if (transform.GetChild (i).tag.Equals ("floor")) 
				{
					m_floors.Add(transform.GetChild (i).gameObject.GetComponent<Floor>());
					m_floors.ToArray()[realsize].Initialize ();
					++realsize;
				}
			}

			foreach (Floor f in m_floors)
				f.InitializeWalls ();

			int count = 0;
			foreach (Floor f in m_floors)
				foreach (RoomContainer rc in f.GetRoomContainers())
					++count;
	
			m_textRoomConfiguredCount.text = "0/" + count;
				
			m_initialized = true;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public override string ToString ()
	{
		string display = "";

		display += "\n Building "+name;
		display += "\n Composed of " + m_floors.ToArray().Length + " floors :";
		display += "\n========================================";
		foreach (Floor f in m_floors)
			display += f.ToString ();
		display += "\n========================================";

		return display;
	}

	public List<Floor> GetFloors()
	{
		return m_floors;
	}

	public void ResetBuilding()
	{
		foreach (Floor f in m_floors)
			f.ResetFloor ();
	}
}
