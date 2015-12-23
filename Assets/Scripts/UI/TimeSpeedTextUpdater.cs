using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeSpeedTextUpdater : MonoBehaviour {

	private float m_timeSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_timeSpeed == 0)
			GetComponent<Text> ().text = "PAUSE";
		else
			GetComponent<Text> ().text = "x" + m_timeSpeed.ToString ();
	}

	public void SetTimeSpeed(float _speed)
	{
		m_timeSpeed = _speed;
	}
}
