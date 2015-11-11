using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class StaticWeatherUIManager : MonoBehaviour {

	private List<StaticWeatherUpdaterButton> m_weatherButtons;

	private int m_currentWeatherIndex;

	private bool m_checkSelected;
	private bool m_isInitialized;

	// Use this for initialization
	void Start () {
		m_currentWeatherIndex = 0;
		m_checkSelected = false;
		m_isInitialized = false;
	}

	public void Initialize()
	{
		m_weatherButtons = new List<StaticWeatherUpdaterButton> ();
		for (int i = 0; i < transform.childCount; ++i)
			m_weatherButtons.Add (transform.GetChild(i).gameObject.GetComponent<StaticWeatherUpdaterButton>());
		m_isInitialized = true;
	}

	// Update is called once per frame
	void Update () 
	{
		if (!m_isInitialized)
			Initialize ();
		if(m_checkSelected)
		{
			int i = 0;
			bool stop = false;
			while(!stop && i < m_weatherButtons.Count)
			{
				StaticWeatherUpdaterButton button = m_weatherButtons.ToArray()[i];

				if(button.IsSelected())
				{
					m_weatherButtons.ToArray()[m_currentWeatherIndex].gameObject.GetComponent<Button>().interactable = true;
					stop = true;
					m_currentWeatherIndex = i;
					m_checkSelected = false;
					button.gameObject.GetComponent<Button>().interactable = false;
					button.Deselect();
					button.UpdateInputFields();
				}
				++i;
			}
		}
	}

	public void WeatherButtonClicked()
	{
		m_checkSelected = true;
	}
	
	public void InputFieldSunRiseHourUpdate(string _sunRiseHour)
	{
		m_weatherButtons.ToArray () [m_currentWeatherIndex].SetSunRiseHour (_sunRiseHour);
	}
	
	public void InputFieldSunRiseMinuteUpdate(string _sunRiseMinute)
	{
		m_weatherButtons.ToArray () [m_currentWeatherIndex].SetSunRiseMinute (_sunRiseMinute);
	}
	
	public void InputFieldSunSetHourUpdate(string _sunSetHour)
	{
		m_weatherButtons.ToArray () [m_currentWeatherIndex].SetSunSetHour (_sunSetHour);
	}
	
	public void InputFieldSunSetMinuteUpdate(string _sunSetMinute)
	{
		m_weatherButtons.ToArray () [m_currentWeatherIndex].SetSunSetMinute (_sunSetMinute);
	}
	
	public void InputFieldTimeAndTemperatureUpdate(string _timeAndTemperature)
	{
		m_weatherButtons.ToArray () [m_currentWeatherIndex].SetTimeAndTemperature (_timeAndTemperature);
	}
}
