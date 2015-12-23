using UnityEngine;
using System.Collections;

public class OptimizationController : MonoBehaviour {

	private string m_buildingConstraintFilePath;
	private string m_optimizationConstraintFilePath;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void SetBuildingConstraintFilePath(string _path)
	{
		m_buildingConstraintFilePath = _path;
	}
	
	public void SetOptimizationConstraintFilePath(string _path)
	{
		m_optimizationConstraintFilePath = _path;
	}
}
