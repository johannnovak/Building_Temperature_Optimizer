using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class OptionController : MonoBehaviour {

	public Dropdown m_dropdownConfigurationStyle;

	public Text m_textConfigureDescription;
	public Button m_buttonConfigure;
	public Button m_buttonLaunch;

	public InputField m_simulationTimeHourInputField;
	public InputField m_simulationTimeMinuteInputField;
	
	public InputField m_cityInputField;

	public StaticWeatherUpdaterButton[] m_weatherButtons;

	private void Start()
	{
	}

	public void UpdateUIFromDropDownValue(int _value)
	{
		if(_value == 0)
		{
			m_cityInputField.transform.parent.gameObject.SetActive(true);
			m_weatherButtons[0].transform.parent.parent.gameObject.SetActive(false);
		}
		else if(_value == 1)
		{
			m_cityInputField.transform.parent.gameObject.SetActive(false);
			m_weatherButtons[0].transform.parent.parent.gameObject.SetActive(true);
		}
	}

	public void Configure()
	{
		bool configurationOK = true;
		string problemDesc = "";

		string[] currentTime = DateTime.Now.ToString ("HH:mm").Split (':');
		float simulationTimeHour = string.IsNullOrEmpty (m_simulationTimeHourInputField.text) ? float.Parse(currentTime[0]) : float.Parse (m_simulationTimeHourInputField.text);
		float simulationTimeMinute = string.IsNullOrEmpty (m_simulationTimeMinuteInputField.text) ? float.Parse(currentTime[1]) : float.Parse (m_simulationTimeMinuteInputField.text);

		KeyValuePair<bool, string> pair;
		/* Dynamic search. */
		if (m_dropdownConfigurationStyle.value == 0) 
		{
			string city = string.IsNullOrEmpty (m_cityInputField.text) ? "Belfort" : m_cityInputField.text;
			pair = GetComponent<WeatherConfigurator> ().RequestOnlineWeather (city, simulationTimeHour, simulationTimeMinute, ref m_textConfigureDescription);

		}
		else /* Static input of values. */
		{
			pair = GetComponent<WeatherConfigurator>().CreateOfflineWeather(simulationTimeHour, simulationTimeMinute, m_weatherButtons);
		}
		configurationOK &= pair.Key;
		problemDesc = pair.Value;

		if(configurationOK)
		{
			m_textConfigureDescription.text = "Configuration OK !";
			m_buttonLaunch.interactable = true;
		}
		else
		{
			m_textConfigureDescription.text = problemDesc;
		}
	}
}
