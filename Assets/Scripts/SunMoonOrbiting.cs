using UnityEngine;
using System.Collections;

public class SunMoonOrbiting : MonoBehaviour {
	
	public float m_currentTimeHour;
	public float m_currentTimeMinute;
	private float m_currentTimeSeconds;

	public float m_sunRiseHour;
	public float m_sunRiseMinute;
	public float m_sunSetHour;
	public float m_sunSetMinute;

	public float m_timeSpeed;

	private float m_currentAngle;

	private float m_dayTimeRatio;
	private float m_nightTimeRatio;
	private float m_dayTimeSecond;
	private float m_nightTimeSecond;

	void Start () {
		float dayTimeHour = (m_sunSetHour+m_sunSetMinute/60) - (m_sunRiseHour+m_sunRiseMinute/60);
		m_dayTimeRatio = (24 - dayTimeHour)/24;
		m_nightTimeRatio = 1 - m_dayTimeRatio;
		m_dayTimeSecond = (dayTimeHour) * 3600;
		m_nightTimeSecond = (24 - dayTimeHour) * 3600;

		ComputeFirstOrbitRotation ();

		GetComponent<TimeUpdater> ().UpdateTime (m_currentTimeHour, m_currentTimeMinute);
	}

	private void ComputeFirstOrbitRotation()
	{
		float deltaAngle = 0;
		if ((m_currentTimeHour+m_currentTimeMinute/60) > (m_sunRiseHour+m_sunRiseMinute/60) && (m_currentTimeHour+m_currentTimeMinute/60) < (m_sunSetHour+m_sunSetMinute/60)) 
		{
			float elapsedTime = ((m_currentTimeHour+m_currentTimeMinute/60) - (m_sunRiseHour+m_sunRiseMinute/60)) * 3600;
			deltaAngle = (elapsedTime * 180F)/ m_dayTimeSecond;
			Debug.Log(deltaAngle);
		}
		else
		{
			float elapsedTime = 0;
			if(m_currentTimeHour >= 12)
				elapsedTime = (m_currentTimeHour+m_currentTimeMinute/60) - (m_sunSetHour+m_sunSetMinute/60);
			else
				elapsedTime = (24 - (m_sunSetHour+m_sunSetMinute/60) + (m_currentTimeHour+m_currentTimeMinute/60));
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

		Debug.Log (updatedTime);

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

	public void DoubleTimeSpeed()
	{
		m_timeSpeed *= 2;
	}

	public void HalfTimeSpeed()
	{
		m_timeSpeed /= 2;
	}
}
