using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimulationController : MonoBehaviour {

	public GameObject m_menuPanel;
	public GameObject m_simulationPanel;

	private Weather m_simulationWeather;

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
		m_sunMoonOrbiting.ComputeDayTime (m_simulationWeather);

		Debug.Log ("Starting simulation with time : " + m_simulationHour + ":" + m_simulationMinute);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetSimulationWeather(Weather _weather)
	{
		m_simulationWeather = _weather;
		Debug.Log (m_simulationWeather);
	}

	public void SetCurrentTime(float _simulationHour, float _simulationMinute)
	{
		m_simulationHour = _simulationHour;
		m_simulationMinute = _simulationMinute;
	}

	public void QuitApplication()
	{
		Application.Quit ();
	}
}
