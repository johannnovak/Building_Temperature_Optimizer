using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weather{
	
	private float m_sunRiseHour;
	private float m_sunRiseMinute;
	
	private float m_sunSetHour;
	private float m_sunSetMinute;

	private Dictionary<float, float> m_timeAndTemperature;

	public Weather(float _sunRiseHour, float _sunRiseMinute, float _sunSetHour, float _sunSetMinute, Dictionary<float, float> _timeAndTemperature)
	{
		m_sunRiseHour = _sunRiseHour;
		m_sunRiseMinute = _sunRiseMinute;
		m_sunSetHour = _sunSetHour;
		m_sunSetMinute = _sunSetMinute;
		m_timeAndTemperature = _timeAndTemperature;
	}
	
	public float GetSunRiseHour()
	{
		return m_sunRiseHour;
	}
	
	public float GetSunRiseMinute()
	{
		return m_sunRiseMinute;
	}

	public float GetSunSetHour()
	{
		return m_sunSetHour;
	}

	public float GetSunSetMinute()
	{
		return m_sunSetMinute;
	}

	public float GetTemperatureWithTime(float _timeHour, float _timeMinute, float _timeSecond)
	{
		bool firstPassed = false;
		KeyValuePair<float, float> lastPair = new KeyValuePair<float, float>(0,0);
		float time = _timeHour + _timeMinute / 60 + _timeSecond / 3600;
		foreach(KeyValuePair<float, float> pair in m_timeAndTemperature)
		{
			if(firstPassed && time <= pair.Key && time >= lastPair.Key)
			{
				float offset = lastPair.Value;
				float slope = (pair.Value - lastPair.Value)/(pair.Key - lastPair.Key);
				float normalizedTime = (time - lastPair.Key);

				/* f(t) = lastTemp + slope(lastTemp->CurrentTemp) * (Time elapsed since lastTime) */
				return (offset + slope * normalizedTime);
			}

			lastPair = pair;
			firstPassed = true;
		}
		return 0F;
	}

	public Dictionary<float, float> GetTimeAndTemperatures()
	{
		return m_timeAndTemperature;
	}

	public override string ToString ()
	{
		string display = "";
		display += "\nWeather";
		display += "\nSunrise : " + (m_sunRiseHour < 10 ? "0" : "") + m_sunRiseHour + ":" + (m_sunRiseMinute < 10 ? "0" : "") + m_sunRiseMinute;
		display += "\nSunset : " + (m_sunSetHour < 10 ? "0" : "") + m_sunSetHour + ":" + (m_sunSetMinute < 10 ? "0" : "") + m_sunSetMinute;
		foreach (KeyValuePair<float, float> pair in m_timeAndTemperature)
			display += "\nHour : " + pair.Key + "/ Temperature : " + pair.Value;
		return display;
	}
}
