using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	private float m_width;
	private float m_length;
	private float m_height;

	private List<Actionner> m_commandableActionners;

	public void Initialize()
	{
		m_width = gameObject.GetComponent<MeshRenderer>().bounds.size.x;
		m_length = gameObject.GetComponent<MeshRenderer>().bounds.size.y;
		m_height = gameObject.GetComponent<MeshRenderer>().bounds.size.z;

		m_commandableActionners = new List<Actionner> ();
		for (int i = 0; i < transform.childCount; ++i)
			if (transform.GetChild (i).tag.Equals ("commandableActionner"))
				m_commandableActionners.Add (transform.GetChild(i).gameObject.GetComponent<Actionner>());
	}

	private void Start()
	{

	}

	public float GetWidth()
	{
		return m_width;
	}

	public float GetLength()
	{
		return m_length;
	}

	public float GetHeight()
	{
		return m_height;
	}

	public override string ToString()
	{
		string display = "";

		display += "\n     Room - "+ name;
		display += "\n width  = " + m_width;
		display += "\n length = " + m_length;
		display += "\n height = " + m_height;

		return display;
	}
}