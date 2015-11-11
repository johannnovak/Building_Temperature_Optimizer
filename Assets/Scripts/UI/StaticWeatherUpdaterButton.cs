using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaticWeatherUpdaterButton : MonoBehaviour {

	public InputField m_sunRiseHourInputField;
	public InputField m_sunRiseMinuteInputField;
	public InputField m_sunSetHourInputField;
	public InputField m_sunSetMinuteInputField;
	public InputField m_timeAndTemperatureInputField;
	
	private string m_sunRiseHour;
	private string m_sunRiseMinute;
	private string m_sunSetHour;
	private string m_sunSetMinute;
	private string m_timeAndTemperature;

	public bool m_isSelected;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateInputFields()
	{
		if (!string.IsNullOrEmpty (m_sunRiseHour))
			m_sunRiseHourInputField.text = m_sunRiseHour;
		else
			m_sunRiseHourInputField.text = string.Empty;

		if(!string.IsNullOrEmpty(m_sunRiseMinute))
			m_sunRiseMinuteInputField.text = m_sunRiseMinute;
		else
			m_sunRiseMinuteInputField.text = string.Empty;

		if(!string.IsNullOrEmpty(m_sunSetHour))
			m_sunSetHourInputField.text = m_sunSetHour;
		else
			m_sunSetHourInputField.text = string.Empty;

		if(!string.IsNullOrEmpty(m_sunSetMinute))
			m_sunSetMinuteInputField.text = m_sunSetMinute;
		else
			m_sunSetMinuteInputField.text = string.Empty;

		if(!string.IsNullOrEmpty(m_timeAndTemperature))
			m_timeAndTemperatureInputField.text = m_timeAndTemperature;
		else
			m_timeAndTemperatureInputField.text = string.Empty;
	}
	
	public void SetSunRiseHour(string _sunRiseHour)
	{
		m_sunRiseHour = _sunRiseHour;
		Debug.Log(m_sunRiseHour);
	}

	public string GetSunRiseHour()
	{
		return m_sunRiseHour;
	}
	
	public void SetSunRiseMinute(string _sunRiseMinute)
	{
		m_sunRiseMinute = _sunRiseMinute;
	}
	
	public string GetSunRiseMinute()
	{
		return m_sunRiseMinute;
	}

	public void SetSunSetHour(string _sunSetHour)
	{
		m_sunSetHour = _sunSetHour;
	}
	
	public string GetSunSetHour()
	{
		return m_sunSetHour;
	}

	public void SetSunSetMinute(string _sunSetMinute)
	{
		m_sunSetMinute = _sunSetMinute;
	}
	
	public string GetSunSetMinute()
	{
		return m_sunSetMinute;
	}

	public void SetTimeAndTemperature(string _timeAndTemperature)
	{
		m_timeAndTemperature = _timeAndTemperature;
	}
	
	public string GetTimeAndTemperature()
	{
		return m_timeAndTemperature;
	}

	public bool IsSelected()
	{
		return m_isSelected;
	}

	public void SetSelected()
	{	
		if (!m_isSelected)
			m_isSelected = true;
	}

	public void Deselect()
	{
		m_isSelected = false;
	}
}
