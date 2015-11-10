using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SimulationController : MonoBehaviour {

	public GameObject m_menuPanel;
	public GameObject m_simulationPanel;

	private static List<Weather> m_simulationWeathers = new List<Weather>();

	private float m_simulationHour;
	private float m_simulationMinute;

	public GameObject m_world;
	public SunMoonOrbiting m_sunMoonOrbiting;

	// Use this for initialization
	void Start () {
	}

	public void Go()
	{
		m_world.SetActive (true);
		m_menuPanel.SetActive (false);
		m_simulationPanel.SetActive (true);

		m_sunMoonOrbiting.SetCurrentTime(m_simulationHour, m_simulationMinute);
		m_sunMoonOrbiting.ComputeDayTime (m_simulationWeathers.ToArray()[0]);

		Debug.Log ("Starting simulation with time : " + m_simulationHour + ":" + m_simulationMinute);
	}

	// Update is called once per frame
	void Update () {

	}

	public void AddSimulationWeather(Weather _weather)
	{
		m_simulationWeathers.Add (_weather);
		Debug.Log (_weather);
	}

	public void SetCurrentTime(float _simulationHour, float _simulationMinute)
	{
		m_simulationHour = _simulationHour;
		m_simulationMinute = _simulationMinute;
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public static float GetCurrentTemperature(int _currentDay, float _currentHour, float _currentMinute, float _currentSecond)
	{
		return SimulationController.m_simulationWeathers.ToArray()[_currentDay].GetTemperatureWithTime(_currentHour, _currentMinute, _currentSecond);
	}

	public void QuitApplication()
	{
		Application.Quit ();
	}
}
