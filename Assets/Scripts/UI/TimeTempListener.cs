using UnityEngine;
using System.Collections;

public abstract class TimeTempListener : MonoBehaviour {

	protected float m_timeHour;
	protected float m_timeMinute;
	protected float m_timeSecond;

	protected float m_temperature;

	public void UpdateCurrentTimeAndTemperature(float _currentTimeHour, float _currentTimeMinute, float _currentTimeSecond, float _currentTemperature)
	{
		m_timeHour = _currentTimeHour;
		m_timeMinute = _currentTimeMinute;
		m_timeSecond = _currentTimeSecond;

		m_temperature = _currentTemperature;
	}
}
