using UnityEngine;
using System.Collections;

public class TimeTempManager : MonoBehaviour {

	public TimeTempListener[] m_listeners;
	
	public void UpdateListeners(float _currentTimeHour, float _currentTimeMinute, float _currentTimeSecond, float _currentTemperature)
	{
		foreach (TimeTempListener listener in m_listeners)
			listener.UpdateCurrentTimeAndTemperature (_currentTimeHour, _currentTimeMinute, _currentTimeSecond, _currentTemperature);
	}
}
