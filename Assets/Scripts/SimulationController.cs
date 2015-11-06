using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SimulationController : MonoBehaviour {

	private Weather m_simulationWeather;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetSimulationWeather(Weather _weather)
	{
		m_simulationWeather = _weather;
	}

	public void ShowMessage()
	{
		Debug.Log( GameObject.Find ("text_input_field_menu_option_city").GetComponent<Text>().text);
	}
}
