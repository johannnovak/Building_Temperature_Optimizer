using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class SimulationController : MonoBehaviour {

	public GameObject m_menuPanel;
	public GameObject m_simulationPanel;
	public GameObject m_timeManipulationPanel;
	public GameObject m_timeTempPanel;
	public GameObject m_selectionController;
	public GameObject m_configurationController;

	private static List<Weather> m_simulationWeathers = new List<Weather>();

	private float m_simulationHour;
	private float m_simulationMinute;

	public GameObject m_world;
	public SunMoonOrbiting m_sunMoonOrbiting;

	public bool m_testEnabled;

	// Use this for initialization
	void Start () {
		if (m_testEnabled)
		{
			m_simulationHour = 12;
			m_simulationMinute = 0;

			Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();
			timeAndTemperature.Add (0, 10);
			timeAndTemperature.Add (5, 12);
			timeAndTemperature.Add (8, 13);
			timeAndTemperature.Add (12, 15);
			timeAndTemperature.Add (18, 14);
			timeAndTemperature.Add (24, 9);
			
			Weather w = new Weather (6, 0, 18, 0, timeAndTemperature);
			AddSimulationWeather(w);

			timeAndTemperature = new Dictionary<float, float> ();
			timeAndTemperature.Add (0, 10);
			timeAndTemperature.Add (5, 12);
			timeAndTemperature.Add (8, 13);
			timeAndTemperature.Add (12, 15);
			timeAndTemperature.Add (18, 14);
			timeAndTemperature.Add (24, 9);
			
			w = new Weather (6, 0, 18, 0, timeAndTemperature);
			AddSimulationWeather(w);
			
			timeAndTemperature = new Dictionary<float, float> ();
			timeAndTemperature.Add (0, 10);
			timeAndTemperature.Add (5, 12);
			timeAndTemperature.Add (8, 13);
			timeAndTemperature.Add (12, 15);
			timeAndTemperature.Add (18, 14);
			timeAndTemperature.Add (24, 9);
			
			w = new Weather (6, 0, 18, 0, timeAndTemperature);
			AddSimulationWeather(w);
			
			timeAndTemperature = new Dictionary<float, float> ();
			timeAndTemperature.Add (0, 10);
			timeAndTemperature.Add (5, 12);
			timeAndTemperature.Add (8, 13);
			timeAndTemperature.Add (12, 15);
			timeAndTemperature.Add (18, 14);
			timeAndTemperature.Add (24, 9);
			
			w = new Weather (6, 0, 18, 0, timeAndTemperature);
			AddSimulationWeather(w);

			GoConfig();
		}
		else
		{
			m_configurationController.SetActive(false);
			m_selectionController.SetActive(false);
		}
	}

	public void GoConfig()
	{
		m_world.SetActive (true);
		m_menuPanel.SetActive (false);
		m_simulationPanel.SetActive (true);
		m_configurationController.SetActive(true);
		m_selectionController.SetActive(true);
		m_timeManipulationPanel.SetActive (false);
		m_timeTempPanel.SetActive (false);

		m_configurationController.GetComponent<ConfigurationController>().SetCurrentSelectedFloor(-1);

		m_sunMoonOrbiting.SetCurrentTime(m_simulationHour, m_simulationMinute);
		m_sunMoonOrbiting.ComputeDayTime (m_simulationWeathers.ToArray()[0]);

		Debug.Log ("Starting simulation with time : " + m_simulationHour + ":" + m_simulationMinute);
	}

	public void AddSimulationWeather(Weather _weather)
	{
		m_simulationWeathers.Add (_weather);
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

	public List<Weather> GetWeatherList()
	{
		return m_simulationWeathers;
	}

	public void QuitApplication()
	{
		Application.Quit ();
	}

	public void ResetSimulation()
	{
		m_simulationWeathers.Clear ();
	}
}
