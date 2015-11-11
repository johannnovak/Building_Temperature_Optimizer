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
	public InputField m_simulationDurationHourInputField;
	public InputField m_simulationDurationMinuteInputField;
	
	public InputField m_cityInputField;

	public InputField m_simulationSunRiseHourInputField;
	public InputField m_simulationSunRiseMinuteInputField;
	public InputField m_simulationSunSetHourInputField;
	public InputField m_simulationSunSetMinuteInputField;
	public InputField m_simulationTemperatures;
	
	//public StaticWeatherUpdaterButton[] m_weatherButtons;

	private void Start()
	{
	}

	public void UpdateUIFromDropDownValue(int _value)
	{
		if(_value == 0)
		{
			m_cityInputField.transform.parent.gameObject.SetActive(true);
			m_simulationSunRiseHourInputField.transform.parent.gameObject.SetActive(false);
			m_simulationSunSetHourInputField.transform.parent.gameObject.SetActive(false);
			m_simulationTemperatures.transform.parent.gameObject.SetActive(false);
		}
		else if(_value == 1)
		{
			m_cityInputField.transform.parent.gameObject.SetActive(false);
			m_simulationSunRiseHourInputField.transform.parent.gameObject.SetActive(true);
			m_simulationSunSetHourInputField.transform.parent.gameObject.SetActive(true);
			m_simulationTemperatures.transform.parent.gameObject.SetActive(true);
		}
	}

	public void Configure()
	{
		bool configurationOK = true;
		string problemDesc = "";

		string[] currentTime = DateTime.Now.ToString ("HH:mm").Split (':');
		float simulationTimeHour = string.IsNullOrEmpty (m_simulationTimeHourInputField.text) ? float.Parse(currentTime[0]) : float.Parse (m_simulationTimeHourInputField.text);
		float simulationTimeMinute = string.IsNullOrEmpty (m_simulationTimeMinuteInputField.text) ? float.Parse(currentTime[1]) : float.Parse (m_simulationTimeMinuteInputField.text);
		
		float simulationDurationHour = string.IsNullOrEmpty (m_simulationDurationHourInputField.text) ? 0F : float.Parse (m_simulationDurationHourInputField.text);
		float simulationDurationMinute = string.IsNullOrEmpty (m_simulationDurationMinuteInputField.text) ? 0F : float.Parse (m_simulationDurationMinuteInputField.text);

		/* Dynamic search. */
		if (m_dropdownConfigurationStyle.value == 0) 
		{
			string city = string.IsNullOrEmpty (m_cityInputField.text) ? "Belfort" : m_cityInputField.text;
			KeyValuePair<bool, string> pair = GetComponent<WeatherConfigurator> ().RequestOnlineWeather (city, simulationTimeHour, simulationTimeMinute, simulationDurationHour, simulationDurationMinute);
			configurationOK &= pair.Key;
			problemDesc = pair.Value;
		}
		else /* Static input of values. */
		{
			float simulationSunriseHour = string.IsNullOrEmpty (m_simulationSunRiseHourInputField.text) ? 0F : float.Parse (m_simulationSunRiseHourInputField.text);
			float simulationSunriseMinute = string.IsNullOrEmpty (m_simulationSunRiseMinuteInputField.text) ? 0F : float.Parse (m_simulationSunRiseMinuteInputField.text);

			float simulationSunsetHour = string.IsNullOrEmpty (m_simulationSunSetHourInputField.text) ? 0F : float.Parse (m_simulationSunSetHourInputField.text);
			float simulationSunsetMinute = string.IsNullOrEmpty (m_simulationSunSetMinuteInputField.text) ? 0F : float.Parse (m_simulationSunSetMinuteInputField.text);

			if(simulationSunriseHour+simulationSunriseMinute/60 == simulationSunsetHour+simulationSunsetMinute/60)
			{
				configurationOK = true;
				problemDesc = "Sunrise cannot be equal to Sunset.";
			}

			string[] timeAndTemperaturesStrings = m_simulationTemperatures.text.Split(';');
			Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();
			for (int i = 0; i < timeAndTemperaturesStrings.Length; ++i)
			{
				float hour = float.Parse (timeAndTemperaturesStrings[i].Split(':')[0]);
				float temperature = float.Parse (timeAndTemperaturesStrings[i].Split(':')[1]);
				if(i == 0 && (simulationTimeHour+simulationTimeMinute/60) < hour)
				{
					configurationOK = false;
					problemDesc = "Simulation time happening before first time/temperature pair.";
				}
				timeAndTemperature.Add (hour, temperature);
			}

			if(configurationOK)
				GetComponent<WeatherConfigurator>().CreateOfflineWeather(simulationTimeHour, simulationTimeMinute, simulationDurationHour, simulationDurationMinute, simulationSunriseHour, simulationSunriseMinute, simulationSunsetHour, simulationSunsetMinute, timeAndTemperature);
		}

		if(configurationOK)
		{
			m_textConfigureDescription.gameObject.SetActive(true);
			m_textConfigureDescription.text = "Configuration OK !";
			m_buttonLaunch.interactable = true;
		}
		else
		{
			m_textConfigureDescription.gameObject.SetActive(true);
			m_textConfigureDescription.text = problemDesc;
		}
	}
}
