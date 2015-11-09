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

	public bool RequestOnlineWeather(string _city, float _simulationTimeHour, float _simulationTimeMinute, float _simulationDurationHour, float _simulationDurationMinute)
	{
		string json = GetJSONFromWebRequest ("http://api.worldweatheronline.com/free/v2/weather.ashx?q=" + _city + "&format=json&num_of_days=1&date=today&key=b1fbba3aff3f713681d871adf0f99");

		Debug.Log (json);
		Debug.Log (string.IsNullOrEmpty (json));

		if (string.IsNullOrEmpty (json))
			return false;

		JSONNode document = JSON.Parse (json);

		if(document["data"]["error"].Count > 0)
			return false;
		else
		{
			/* sunset/rise */
			string sunRise = document ["data"] ["weather"] [0] ["astronomy"] [0] ["sunrise"].ToString ().Split (new char[]{'\"'}) [1];
			float sunRiseHour = float.Parse (sunRise.Split (new char[]{':'}) [0]) + (sunRise.EndsWith ("PM") ? 12 : 0);
			float sunRiseMinute = float.Parse (sunRise.Split (new char[]{':'}) [1].Remove (2));

			string sunSet = document ["data"] ["weather"] [0] ["astronomy"] [0] ["sunset"].ToString ().Split (new char[]{'\"'}) [1];
			float sunSetHour = float.Parse (sunSet.Split (new char[]{':'}) [0]) + (sunSet.EndsWith ("PM") ? 12 : 0);
			float sunSetMinute = float.Parse (sunSet.Split (new char[]{':'}) [1].Remove (2));

			Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();

			/* hours / temperature */
			for (int i = 0; i < document["data"]["weather"][0]["hourly"].Count; ++i)
				timeAndTemperature.Add (float.Parse (document ["data"] ["weather"] [0] ["hourly"] [i] ["time"]) / 100, float.Parse (document ["data"] ["weather"] [0] ["hourly"] [i] ["tempC"]));

			m_controller.SetSimulationWeather(new Weather (sunRiseHour, sunRiseMinute, sunSetHour, sunSetMinute, timeAndTemperature));
			m_controller.SetCurrentTime(_simulationTimeHour, _simulationTimeMinute);

			return true;
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
		catch(WebException e)
		{
			Debug.Log("exception");
			return null;
		}
	}

	public void CreateOfflineWeather(float _simulationTimeHour, float _simulationTimeMinute, float _simulationDurationHour, float _simulationDurationMinute, float _simulationSunriseHour, float _simulationSunriseMinute, float _simulationSunsetHour, float _simulationSunsetMinute, Dictionary<float,float> _timeAndTemperature)
	{
		m_controller.SetSimulationWeather(new Weather (_simulationSunriseHour, _simulationSunriseMinute, _simulationSunsetHour, _simulationSunsetMinute, _timeAndTemperature));
		m_controller.SetCurrentTime(_simulationTimeHour, _simulationTimeMinute);
	}
}