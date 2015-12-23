using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SunMoonOrbiting : MonoBehaviour {

	private int m_currentTimeDay;
	private float m_currentTimeHour;
	private float m_currentTimeMinute;
	private float m_currentTimeSeconds;

	public float m_timeSpeed;
	private float m_savedTimeSpeed;
	public TimeSpeedTextUpdater m_simulationTimeSpeedText;
	private float m_currentAngle;

	private float m_dayTimeSecond;
	private float m_nightTimeSecond;

	private bool m_orbiting;

	void Start () {
		m_orbiting = false;
	}

	public void ComputeDayTime(Weather _weather)
	{
		float dayTimeHour = (_weather.GetSunSetHour()+_weather.GetSunSetMinute()/60) - (_weather.GetSunRiseHour()+_weather.GetSunRiseMinute()/60);
		m_dayTimeSecond = dayTimeHour * 3600;
		m_nightTimeSecond = (24 - dayTimeHour) * 3600;
		
		ComputeFirstOrbitRotation (_weather);

		GetComponent<TimeTempManager>().UpdateListeners(m_currentTimeHour, m_currentTimeMinute, m_currentTimeSeconds, SimulationController.GetCurrentTemperature(m_currentTimeDay, m_currentTimeHour, m_currentTimeMinute, m_currentTimeSeconds));
	}

	private void ComputeFirstOrbitRotation(Weather _weather)
	{
		float deltaAngle = 0;
		/* sunrise < time < sunset */
		if ((m_currentTimeHour+m_currentTimeMinute/60) > (_weather.GetSunRiseHour()+_weather.GetSunRiseMinute()/60) && (m_currentTimeHour+m_currentTimeMinute/60) < (_weather.GetSunSetHour()+_weather.GetSunSetMinute()/60)) 
		{
			float elapsedTime = ((m_currentTimeHour*3600+m_currentTimeMinute*60) - (_weather.GetSunRiseHour()*3600+_weather.GetSunRiseMinute()*60));
			deltaAngle = (elapsedTime * 180F)/ m_dayTimeSecond;
		}
		else /* time > sunset && time < sunrise */
		{
			float elapsedTime = 0;

			/* time > sunset */
			if(m_currentTimeHour >= _weather.GetSunSetHour()+_weather.GetSunSetMinute()/60)
				elapsedTime = m_dayTimeSecond + (m_currentTimeHour*3600+m_currentTimeMinute*60) - (_weather.GetSunSetHour()*3600+_weather.GetSunSetMinute()*60);
			else /* time < sunrise */
				elapsedTime = (m_currentTimeHour*3600+m_currentTimeMinute*60) - (_weather.GetSunRiseHour()*3600+_weather.GetSunRiseMinute()*60);
			deltaAngle = (elapsedTime * 180F)/ m_nightTimeSecond;
		}
		float modAngle = deltaAngle%360;

		transform.RotateAround(Vector3.zero, Vector3.right, modAngle);
		transform.GetChild (0).LookAt (Vector3.zero);
		transform.GetChild (1).LookAt (Vector3.zero);

		m_simulationTimeSpeedText.SetTimeSpeed(m_timeSpeed);
	}
	
	public void StartOrbiting()
	{
		m_orbiting = true;
	}
	
	public void StopOrbiting()
	{
		m_orbiting = false;
	}

	public void ResetOrbit()
	{
		Quaternion q = new Quaternion ();
		q.eulerAngles = new Vector3 (0, 0, 0);
		transform.rotation = q;

		m_currentTimeDay = 0;
	}

	void Update () {
		if (m_orbiting) {
			float updatedTime = Time.deltaTime * m_timeSpeed;
			m_currentTimeSeconds += updatedTime;
			if (m_currentTimeSeconds >= 60) {
				++m_currentTimeMinute;
				m_currentTimeSeconds %= 60;
				
				if (m_currentTimeMinute == 60) {
					++m_currentTimeHour;
					m_currentTimeMinute = 0;
				}

				if (m_currentTimeHour == 24) {
					++m_currentTimeDay;
					m_currentTimeHour = 0;
				}
				GetComponent<TimeTempManager> ().UpdateListeners (m_currentTimeHour, m_currentTimeMinute, m_currentTimeSeconds, SimulationController.GetCurrentTemperature (m_currentTimeDay, m_currentTimeHour, m_currentTimeMinute, m_currentTimeSeconds));
			}

			float deltaAngle = 0;
			if (transform.rotation.eulerAngles.x < 180)
				deltaAngle = (updatedTime * 180F) / m_dayTimeSecond;
			else
				deltaAngle = (updatedTime * 180F) / m_nightTimeSecond;

			float modAngle = deltaAngle % 360;

			transform.RotateAround (Vector3.zero, Vector3.right, modAngle);

			transform.GetChild (0).LookAt (Vector3.zero);
			transform.GetChild (1).LookAt (Vector3.zero);
		}
	}

	public void SetCurrentTime(float _currentTimeHour, float _currentTimeMinute)
	{
		m_currentTimeHour = _currentTimeHour;
		m_currentTimeMinute = _currentTimeMinute;
	}

	public void DoubleTimeSpeed()
	{
		m_timeSpeed *= 2.0F;
		m_simulationTimeSpeedText.SetTimeSpeed (m_timeSpeed);
	}

	public void PauseTimeSpeed()
	{
		if(m_timeSpeed != 0.0F)
		{
			m_savedTimeSpeed = m_timeSpeed;
			m_timeSpeed = 0.0F;
		}	
		m_simulationTimeSpeedText.SetTimeSpeed (m_timeSpeed);
	}

	public void ResumeTimeSpeed()
	{
		if(m_savedTimeSpeed != 0.0F)
		{
			m_timeSpeed = m_savedTimeSpeed;
			m_savedTimeSpeed = 0.0F;
		}
		
		m_simulationTimeSpeedText.SetTimeSpeed (m_timeSpeed);
		Debug.Log (m_timeSpeed);
	}

	public void HalfTimeSpeed()
	{
		m_timeSpeed /= 2;
		m_simulationTimeSpeedText.SetTimeSpeed (m_timeSpeed);
	}
	
	public float GetSunAngle()
	{
		if(transform.GetChild(0).position.y > 0)
		   return (transform.GetChild(0).position.z < 0) ? transform.rotation.eulerAngles.x : 180 - transform.rotation.eulerAngles.x;
		return 0F;
	}	
	
	public float GetMoonAngle()
	{
		if(transform.GetChild(1).position.y > 0)
			return (transform.GetChild(1).position.z < 0) ? 360 - transform.rotation.eulerAngles.x : (180F - transform.rotation.eulerAngles.x)*(-1);
		return 0F;
	}	
}
