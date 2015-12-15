using UnityEngine;
using System.Collections;

public class Actionner : MonoBehaviour {

	private float m_minDeliveredEnergy;
	private float m_maxDeliveredEnergy;

	private void Start()
	{
		m_minDeliveredEnergy = float.NaN;
		m_maxDeliveredEnergy = float.NaN;
	}

	public float GetMinDeliveredEnergy()
	{
		return m_minDeliveredEnergy;
	}

	public void SetMinDeliveredEnergy(float _energy)
	{
		m_minDeliveredEnergy = _energy;
	}

	public float GetMaxDeliveredEnergy()
	{
		return m_maxDeliveredEnergy;
	}

	public void SetMaxDeliveredEnergy(float _energy)
	{
		m_maxDeliveredEnergy = _energy;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
