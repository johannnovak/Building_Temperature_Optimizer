using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour {

	private float m_width;
	private float m_length;
	private float m_height;

	public void Initialize()
	{
		if(transform.childCount == 0)
		{
			m_width = gameObject.GetComponent<MeshRenderer>().bounds.size.x;
			m_length = gameObject.GetComponent<MeshRenderer>().bounds.size.y;
			m_height = gameObject.GetComponent<MeshRenderer>().bounds.size.z;
		}
		else
		{
			
		}
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