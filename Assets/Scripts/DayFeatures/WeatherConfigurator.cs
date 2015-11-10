using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using SimpleJSON;


public class WeatherConfigurator : MonoBehaviour {

	public SimulationController m_controller;
	
	private void Start()
	{
	//	RequestWeather ("besancin", 0F, 0F, 0F, 0F);
	}

	public KeyValuePair<bool,string> RequestOnlineWeather(string _city, float _simulationTimeHour, float _simulationTimeMinute, float _simulationDurationHour, float _simulationDurationMinute)
	{
		string json = GetJSONFromWebRequest ("http://api.worldweatheronline.com/free/v2/weather.ashx?q=" + _city + "&format=json&num_of_days=10&date=today&key=b1fbba3aff3f713681d871adf0f99");

		Debug.Log (json);

		if (string.IsNullOrEmpty (json))
			return new KeyValuePair<bool, string> (false, "Could not connect to worldweatheronline.com.");

		JSONNode document = JSON.Parse (json);

		if(document["data"]["error"].Count > 0)
			return new KeyValuePair<bool, string> (false, "City not found.");
		else
		{
			/* for each day requested. */
			int dayNumber = 0;
			JSONArray weatherDays = document ["data"] ["weather"].AsArray;
			Weather previousWeather = null;
			KeyValuePair<float, float> lastPair = new KeyValuePair<float, float>();
			foreach(JSONNode weather in weatherDays.Childs)
			{
				/* sunset/rise */
				string sunRise = weather["astronomy"] [0] ["sunrise"].ToString ().Split (new char[]{'\"'}) [1];
				float sunRiseHour = float.Parse (sunRise.Split (new char[]{':'}) [0]) + (sunRise.EndsWith ("PM") ? 12 : 0);
				float sunRiseMinute = float.Parse (sunRise.Split (new char[]{':'}) [1].Remove (2));
				
				string sunSet = weather["astronomy"] [0] ["sunset"].ToString ().Split (new char[]{'\"'}) [1];
				float sunSetHour = float.Parse (sunSet.Split (new char[]{':'}) [0]) + (sunSet.EndsWith ("PM") ? 12 : 0);
				float sunSetMinute = float.Parse (sunSet.Split (new char[]{':'}) [1].Remove (2));
				
				Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();
				
				/* hours / temperature */
				for (int i = 0; i < weather["hourly"].Count; ++i)
				{
					float hour = float.Parse (weather["hourly"] [i] ["time"]) / 100;
					float temperature = float.Parse (weather["hourly"] [i] ["tempC"]);

					if(i == 0 && dayNumber == 0 && _simulationTimeHour+_simulationTimeMinute/60 < hour)
						return new KeyValuePair<bool, string> (false, "Current simulation time happening before first time/temperature pair ("+hour+"h).");
					else if(i == 0 && dayNumber != 0)
					{
						float time = 24F;
						float offset = lastPair.Value;
						float slope = (temperature - lastPair.Value)/(time + hour - lastPair.Key);
						float normalizedTime = (time - lastPair.Key);
						previousWeather.GetTimeAndTemperatures().Add(time, (offset + slope * normalizedTime));

						timeAndTemperature.Add(0, (offset + slope * normalizedTime));

					}

					lastPair = new KeyValuePair<float, float>(hour, temperature);
					timeAndTemperature.Add (lastPair.Key, lastPair.Value);
				}

				previousWeather = new Weather (sunRiseHour, sunRiseMinute, sunSetHour, sunSetMinute, timeAndTemperature);
				m_controller.AddSimulationWeather(previousWeather);
				m_controller.SetCurrentTime(_simulationTimeHour, _simulationTimeMinute);
				++dayNumber;
			}
			
			return new KeyValuePair<bool, string> (true, "");
		}
	}

	private static string GetJSONFromWebRequest(string _url)
	{
		string json = string.Empty;
		WebRequest request = WebRequest.Create (_url);
		try
		{
			WebResponse response = request.GetResponse ();
			Stream data = response.GetResponseStream ();
			using (StreamReader sr = new StreamReader(data))
				json = sr.ReadToEnd ();
			
			return json;
		}
		catch(WebException)
		{
			return null;
		}
	}

	public void CreateOfflineWeather(float _simulationTimeHour, float _simulationTimeMinute, float _simulationDurationHour, float _simulationDurationMinute, float _simulationSunriseHour, float _simulationSunriseMinute, float _simulationSunsetHour, float _simulationSunsetMinute, Dictionary<float,float> _timeAndTemperature)
	{
		m_controller.AddSimulationWeather(new Weather (_simulationSunriseHour, _simulationSunriseMinute, _simulationSunsetHour, _simulationSunsetMinute, _timeAndTemperature));
		m_controller.SetCurrentTime(_simulationTimeHour, _simulationTimeMinute);
	}
}