using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SunMoonOrbiting : MonoBehaviour {

	public bool m_mockTest;
	public float m_currentTimeHour;
	public float m_currentTimeMinute;
	private float m_currentTimeSeconds;

	public float m_sunRiseHour;
	public float m_sunRiseMinute;
	public float m_sunSetHour;
	public float m_sunSetMinute;

	public float m_timeSpeed;

	private float m_currentAngle;

	private float m_dayTimeSecond;
	private float m_nightTimeSecond;

	void Start () {
		if (m_mockTest) 
		{
			Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();
			timeAndTemperature.Add (0, 10);
			timeAndTemperature.Add (5, 12);
			timeAndTemperature.Add (8, 13);
			timeAndTemperature.Add (12, 15);
			timeAndTemperature.Add (18, 14);
			timeAndTemperature.Add (23, 9);

			Weather w = new Weather (m_sunRiseHour, m_sunRiseMinute, m_sunSetHour, m_sunSetMinute, timeAndTemperature);
			ComputeDayTime (w);
		}
	}

	public void ComputeDayTime(Weather _weather)
	{
		float dayTimeHour = (_weather.GetSunSetHour()+_weather.GetSunSetMinute()/60) - (_weather.GetSunRiseHour()+_weather.GetSunRiseMinute()/60);
		m_dayTimeSecond = dayTimeHour * 3600;
		m_nightTimeSecond = (24 - dayTimeHour) * 3600;
		
		ComputeFirstOrbitRotation ();
		
		GetComponent<TimeUpdater> ().UpdateTime (m_currentTimeHour, m_currentTimeMinute);
	}

	private void ComputeFirstOrbitRotation()
	{
		Debug.Log (m_currentTimeHour+":"+m_currentTimeMinute);

		float deltaAngle = 0;
		/* sunrise < time < sunset */
		if ((m_currentTimeHour+m_currentTimeMinute/60) > (m_sunRiseHour+m_sunRiseMinute/60) && (m_currentTimeHour+m_currentTimeMinute/60) < (m_sunSetHour+m_sunSetMinute/60)) 
		{
			float elapsedTime = ((m_currentTimeHour*3600+m_currentTimeMinute*60) - (m_sunRiseHour*3600+m_sunRiseMinute*60)) * 3600;
			deltaAngle = (elapsedTime * 180F)/ m_dayTimeSecond;
		}
		else /* time > sunset && time < sunrise */
		{
			float elapsedTime = 0;

			/* time > sunset */
			if(m_currentTimeHour >= 12)
				elapsedTime = (m_currentTimeHour*3600+m_currentTimeMinute*60) - (m_sunSetHour*3600+m_sunSetMinute*60);
			else /* time < sunrise */
				elapsedTime = (m_currentTimeHour*3600+m_currentTimeMinute*60) - (m_sunRiseHour*3600+m_sunRiseMinute*60);
			deltaAngle = (elapsedTime * 180F)/ m_nightTimeSecond;
		}

		float modAngle = deltaAngle%360;
		transform.RotateAround(Vector3.zero, Vector3.right, modAngle);
		transform.GetChild (0).LookAt (Vector3.zero);
		transform.GetChild (1).LookAt (Vector3.zero);
	}


	void Update () {
		Debug.Log ("********************");
		float updatedTime = Time.deltaTime * m_timeSpeed;
		m_currentTimeSeconds += updatedTime;
		if(m_currentTimeSeconds >= 60)
		{
			m_currentTimeMinute += 1;
			m_currentTimeSeconds %= 60;
			
			if (m_currentTimeMinute == 60) 
			{
				m_currentTimeHour = (m_currentTimeHour + 1) % 24;
				m_currentTimeMinute = 0;
			}

			GetComponent<TimeUpdater>().UpdateTime(m_currentTimeHour,m_currentTimeMinute);
		}

		float deltaAngle = 0;
		if(transform.rotation.eulerAngles.x < 180)
			deltaAngle = (updatedTime * 180F)/ m_dayTimeSecond;
		else
			deltaAngle = (updatedTime * 180F)/ m_nightTimeSecond;
		Debug.Log (deltaAngle);

		float modAngle = deltaAngle%360;
		Debug.Log (modAngle);

		transform.RotateAround(Vector3.zero, Vector3.right, modAngle);
		Debug.Log (transform.rotation.eulerAngles.ToString("F8"));

		transform.GetChild (0).LookAt (Vector3.zero);
		transform.GetChild (1).LookAt (Vector3.zero);
	}

	public void SetCurrentTime(float _currentTimeHour, float _currentTimeMinute)
	{
		m_currentTimeHour = _currentTimeHour;
		m_currentTimeMinute = _currentTimeMinute;
	}

	public void DoubleTimeSpeed()
	{
		m_timeSpeed *= 2;
	}

	public void HalfTimeSpeed()
	{
		m_timeSpeed /= 2;
	}
}
