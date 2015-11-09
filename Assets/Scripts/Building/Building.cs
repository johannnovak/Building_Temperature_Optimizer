using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour {

	private Floor[] m_floors;

	// Use this for initialization
	void Start () {
		m_floors = new Floor[transform.childCount];
		int realsize = 0;
		for(int i = 0; i < m_floors.Length; ++i)
			if(transform.GetChild(i).tag.Equals("floor"))
			{
				m_floors[realsize] = transform.GetChild(i).gameObject.GetComponent<Floor>();
				m_floors[realsize]. Initialize();
				++realsize;
			}

		Array.Resize(ref m_floors, realsize);

		Debug.Log (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override string ToString ()
	{
		string display = "";

		display += "\n Building "+name;
		display += "\n Composed of " + m_floors.Length + " floors :";
		display += "\n========================================";
		foreach (Floor f in m_floors)
			display += f.ToString ();
		display += "\n========================================";

		return display;
	}

	public Floor[] GetFloors()
	{
		return m_floors;
	}
}
