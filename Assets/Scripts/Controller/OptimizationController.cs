using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class OptimizationController : MonoBehaviour {

	private string m_buildingConstraintFilePath;
	private string m_optimizationConstraintFilePath;

	public Button m_buttonGo;
	private Building m_building;
	public SimulationController m_simulationController;
	private List<Weather> m_weatherList;

	// Use this for initialization
	void Start () {
		m_building = GameObject.Find ("building").GetComponent<Building> ();
		m_weatherList = m_simulationController.GetWeatherList ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CreateConfigurationFiles()
	{
		CreateBuildingOptimizationFile ();
		
		m_buttonGo.interactable = true;
	}
	
	private void CreateBuildingOptimizationFile()
	{
	}
}
