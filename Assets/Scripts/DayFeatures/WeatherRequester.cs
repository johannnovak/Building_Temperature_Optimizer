using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using SimpleJSON;


public class WeatherRequester : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RequestWeather ();
	}

	/*
	 * { 'data': { 'current_condition': [ {'cloudcover': '0', 'FeelsLikeC': '18', 'FeelsLikeF': '64', 'humidity': '88', 'observation_time': '01:53 PM', 'precipMM': '0.3', 'pressure': '1022', 'temp_C': '18', 'temp_F': '64', 'visibility': '10', 'weatherCode': '296',  'weatherDesc': [ {'value': 'Light Rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0017_cloudy_with_light_rain.png' } ], 'winddir16Point': 'SW', 'winddirDegree': '220', 'windspeedKmph': '17', 'windspeedMiles': '11' } ],  'request': [ {'query': 'Paris, France', 'type': 'City' } ],  'weather': [ { 'astronomy': [ {'moonrise': '02:14 AM', 'moonset': '03:19 PM', 'sunrise': '07:45 AM', 'sunset': '05:23 PM' } ], 'date': '2015-11-06',  'hourly': [ {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '72', 'chanceofrain': '26', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '7', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '48', 'DewPointC': '13', 'DewPointF': '55', 'FeelsLikeC': '14', 'FeelsLikeF': '56', 'HeatIndexC': '15', 'HeatIndexF': '59', 'humidity': '86', 'precipMM': '0.2', 'pressure': '1020', 'tempC': '16', 'tempF': '60', 'time': '100', 'visibility': '5', 'weatherCode': '263',  'weatherDesc': [ {'value': 'Patchy light drizzle' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0025_light_rain_showers_night.png' } ], 'WindChillC': '14', 'WindChillF': '56', 'winddir16Point': 'SSW', 'winddirDegree': '203', 'WindGustKmph': '30', 'WindGustMiles': '19', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '13', 'chanceofrain': '0', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '7', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '77', 'DewPointC': '12', 'DewPointF': '54', 'FeelsLikeC': '12', 'FeelsLikeF': '53', 'HeatIndexC': '13', 'HeatIndexF': '56', 'humidity': '93', 'precipMM': '0.0', 'pressure': '1020', 'tempC': '15', 'tempF': '58', 'time': '400', 'visibility': '10', 'weatherCode': '119',  'weatherDesc': [ {'value': 'Cloudy' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '12', 'WindChillF': '53', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '30', 'WindGustMiles': '19', 'windspeedKmph': '17', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '7', 'chanceofrain': '7', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '4', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '13', 'DewPointF': '56', 'FeelsLikeC': '13', 'FeelsLikeF': '56', 'HeatIndexC': '14', 'HeatIndexF': '58', 'humidity': '94', 'precipMM': '0.1', 'pressure': '1021', 'tempC': '14', 'tempF': '57', 'time': '700', 'visibility': '10', 'weatherCode': '122',  'weatherDesc': [ {'value': 'Overcast' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '13', 'WindChillF': '56', 'winddir16Point': 'SSW', 'winddirDegree': '197', 'WindGustKmph': '26', 'WindGustMiles': '16', 'windspeedKmph': '17', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '79', 'chanceofrain': '0', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '0', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '92', 'DewPointC': '15', 'DewPointF': '58', 'FeelsLikeC': '16', 'FeelsLikeF': '61', 'HeatIndexC': '16', 'HeatIndexF': '61', 'humidity': '91', 'precipMM': '0.1', 'pressure': '1022', 'tempC': '14', 'tempF': '58', 'time': '1000', 'visibility': '10', 'weatherCode': '122',  'weatherDesc': [ {'value': 'Overcast' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '16', 'WindChillF': '61', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '25', 'WindGustMiles': '16', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '77', 'chanceofrain': '35', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '0', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '19', 'FeelsLikeF': '66', 'HeatIndexC': '19', 'HeatIndexF': '66', 'humidity': '82', 'precipMM': '0.1', 'pressure': '1022', 'tempC': '18', 'tempF': '64', 'time': '1300', 'visibility': '10', 'weatherCode': '176',  'weatherDesc': [ {'value': 'Patchy rain nearby' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0009_light_rain_showers.png' } ], 'WindChillC': '19', 'WindChillF': '66', 'winddir16Point': 'SW', 'winddirDegree': '217', 'WindGustKmph': '34', 'WindGustMiles': '21', 'windspeedKmph': '20', 'windspeedMiles': '13' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '90', 'chanceofrain': '91', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '1', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '17', 'DewPointF': '62', 'FeelsLikeC': '18', 'FeelsLikeF': '65', 'HeatIndexC': '18', 'HeatIndexF': '65', 'humidity': '89', 'precipMM': '1.1', 'pressure': '1022', 'tempC': '19', 'tempF': '66', 'time': '1600', 'visibility': '9', 'weatherCode': '296',  'weatherDesc': [ {'value': 'Light rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0017_cloudy_with_light_rain.png' } ], 'WindChillC': '18', 'WindChillF': '65', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '36', 'WindGustMiles': '22', 'windspeedKmph': '18', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '18', 'chanceofrain': '96', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '2', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '17', 'FeelsLikeF': '63', 'HeatIndexC': '17', 'HeatIndexF': '63', 'humidity': '91', 'precipMM': '4.6', 'pressure': '1023', 'tempC': '17', 'tempF': '63', 'time': '1900', 'visibility': '7', 'weatherCode': '302',  'weatherDesc': [ {'value': 'Moderate rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0034_cloudy_with_heavy_rain_night.png' } ], 'WindChillC': '17', 'WindChillF': '63', 'winddir16Point': 'SSW', 'winddirDegree': '198', 'WindGustKmph': '40', 'WindGustMiles': '25', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '0', 'chanceofrain': '66', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '18', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '95', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '17', 'FeelsLikeF': '62', 'HeatIndexC': '17', 'HeatIndexF': '62', 'humidity': '91', 'precipMM': '0.4', 'pressure': '1024', 'tempC': '16', 'tempF': '62', 'time': '2200', 'visibility': '2', 'weatherCode': '266',  'weatherDesc': [ {'value': 'Light drizzle' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0033_cloudy_with_light_rain_night.png' } ], 'WindChillC': '17', 'WindChillF': '62', 'winddir16Point': 'SSW', 'winddirDegree': '198', 'WindGustKmph': '37', 'WindGustMiles': '23', 'windspeedKmph': '18', 'windspeedMiles': '11' } ], 'maxtempC': '19', 'maxtempF': '66', 'mintempC': '15', 'mintempF': '59', 'uvIndex': '1' } ] }}
	  */

	public static Weather RequestWeather()
	{
		Weather weather = new Weather ();

		//string json = GetJSONFromWebRequest ('http://api.worldweatheronline.com/free/v2/weather.ashx?q=Paris&format=json&num_of_days=1&date=today&key=b1fbba3aff3f713681d871adf0f99\');
		string json = "'data': { 'current_condition': [ {'cloudcover': '0', 'FeelsLikeC': '18', 'FeelsLikeF': '64', 'humidity': '88', 'observation_time': '01:53 PM', 'precipMM': '0.3', 'pressure': '1022', 'temp_C': '18', 'temp_F': '64', 'visibility': '10', 'weatherCode': '296',  'weatherDesc': [ {'value': 'Light Rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0017_cloudy_with_light_rain.png' } ], 'winddir16Point': 'SW', 'winddirDegree': '220', 'windspeedKmph': '17', 'windspeedMiles': '11' } ],  'request': [ {'query': 'Paris, France', 'type': 'City' } ],  'weather': [ { 'astronomy': [ {'moonrise': '02:14 AM', 'moonset': '03:19 PM', 'sunrise': '07:45 AM', 'sunset': '05:23 PM' } ], 'date': '2015-11-06',  'hourly': [ {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '72', 'chanceofrain': '26', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '7', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '48', 'DewPointC': '13', 'DewPointF': '55', 'FeelsLikeC': '14', 'FeelsLikeF': '56', 'HeatIndexC': '15', 'HeatIndexF': '59', 'humidity': '86', 'precipMM': '0.2', 'pressure': '1020', 'tempC': '16', 'tempF': '60', 'time': '100', 'visibility': '5', 'weatherCode': '263',  'weatherDesc': [ {'value': 'Patchy light drizzle' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0025_light_rain_showers_night.png' } ], 'WindChillC': '14', 'WindChillF': '56', 'winddir16Point': 'SSW', 'winddirDegree': '203', 'WindGustKmph': '30', 'WindGustMiles': '19', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '13', 'chanceofrain': '0', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '7', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '77', 'DewPointC': '12', 'DewPointF': '54', 'FeelsLikeC': '12', 'FeelsLikeF': '53', 'HeatIndexC': '13', 'HeatIndexF': '56', 'humidity': '93', 'precipMM': '0.0', 'pressure': '1020', 'tempC': '15', 'tempF': '58', 'time': '400', 'visibility': '10', 'weatherCode': '119',  'weatherDesc': [ {'value': 'Cloudy' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '12', 'WindChillF': '53', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '30', 'WindGustMiles': '19', 'windspeedKmph': '17', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '7', 'chanceofrain': '7', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '4', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '13', 'DewPointF': '56', 'FeelsLikeC': '13', 'FeelsLikeF': '56', 'HeatIndexC': '14', 'HeatIndexF': '58', 'humidity': '94', 'precipMM': '0.1', 'pressure': '1021', 'tempC': '14', 'tempF': '57', 'time': '700', 'visibility': '10', 'weatherCode': '122',  'weatherDesc': [ {'value': 'Overcast' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '13', 'WindChillF': '56', 'winddir16Point': 'SSW', 'winddirDegree': '197', 'WindGustKmph': '26', 'WindGustMiles': '16', 'windspeedKmph': '17', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '79', 'chanceofrain': '0', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '0', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '92', 'DewPointC': '15', 'DewPointF': '58', 'FeelsLikeC': '16', 'FeelsLikeF': '61', 'HeatIndexC': '16', 'HeatIndexF': '61', 'humidity': '91', 'precipMM': '0.1', 'pressure': '1022', 'tempC': '14', 'tempF': '58', 'time': '1000', 'visibility': '10', 'weatherCode': '122',  'weatherDesc': [ {'value': 'Overcast' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0004_black_low_cloud.png' } ], 'WindChillC': '16', 'WindChillF': '61', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '25', 'WindGustMiles': '16', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '77', 'chanceofrain': '35', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '0', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '19', 'FeelsLikeF': '66', 'HeatIndexC': '19', 'HeatIndexF': '66', 'humidity': '82', 'precipMM': '0.1', 'pressure': '1022', 'tempC': '18', 'tempF': '64', 'time': '1300', 'visibility': '10', 'weatherCode': '176',  'weatherDesc': [ {'value': 'Patchy rain nearby' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0009_light_rain_showers.png' } ], 'WindChillC': '19', 'WindChillF': '66', 'winddir16Point': 'SW', 'winddirDegree': '217', 'WindGustKmph': '34', 'WindGustMiles': '21', 'windspeedKmph': '20', 'windspeedMiles': '13' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '90', 'chanceofrain': '91', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '1', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '17', 'DewPointF': '62', 'FeelsLikeC': '18', 'FeelsLikeF': '65', 'HeatIndexC': '18', 'HeatIndexF': '65', 'humidity': '89', 'precipMM': '1.1', 'pressure': '1022', 'tempC': '19', 'tempF': '66', 'time': '1600', 'visibility': '9', 'weatherCode': '296',  'weatherDesc': [ {'value': 'Light rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0017_cloudy_with_light_rain.png' } ], 'WindChillC': '18', 'WindChillF': '65', 'winddir16Point': 'SSW', 'winddirDegree': '204', 'WindGustKmph': '36', 'WindGustMiles': '22', 'windspeedKmph': '18', 'windspeedMiles': '11' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '18', 'chanceofrain': '96', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '2', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '100', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '17', 'FeelsLikeF': '63', 'HeatIndexC': '17', 'HeatIndexF': '63', 'humidity': '91', 'precipMM': '4.6', 'pressure': '1023', 'tempC': '17', 'tempF': '63', 'time': '1900', 'visibility': '7', 'weatherCode': '302',  'weatherDesc': [ {'value': 'Moderate rain' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0034_cloudy_with_heavy_rain_night.png' } ], 'WindChillC': '17', 'WindChillF': '63', 'winddir16Point': 'SSW', 'winddirDegree': '198', 'WindGustKmph': '40', 'WindGustMiles': '25', 'windspeedKmph': '19', 'windspeedMiles': '12' }, {'chanceoffog': '0', 'chanceoffrost': '0', 'chanceofhightemp': '0', 'chanceofovercast': '0', 'chanceofrain': '66', 'chanceofremdry': '0', 'chanceofsnow': '0', 'chanceofsunshine': '18', 'chanceofthunder': '0', 'chanceofwindy': '0', 'cloudcover': '95', 'DewPointC': '16', 'DewPointF': '60', 'FeelsLikeC': '17', 'FeelsLikeF': '62', 'HeatIndexC': '17', 'HeatIndexF': '62', 'humidity': '91', 'precipMM': '0.4', 'pressure': '1024', 'tempC': '16', 'tempF': '62', 'time': '2200', 'visibility': '2', 'weatherCode': '266',  'weatherDesc': [ {'value': 'Light drizzle' } ],  'weatherIconUrl': [ {'value': 'http://cdn.worldweatheronline.net/images/wsymbols01_png_64/wsymbol_0033_cloudy_with_light_rain_night.png' } ], 'WindChillC': '17', 'WindChillF': '62', 'winddir16Point': 'SSW', 'winddirDegree': '198', 'WindGustKmph': '37', 'WindGustMiles': '23', 'windspeedKmph': '18', 'windspeedMiles': '11' } ], 'maxtempC': '19', 'maxtempF': '66', 'mintempC': '15', 'mintempF': '59', 'uvIndex': '1' } ] }";
		JSONNode document = JSON.Parse (json);
		IEnumerator dataEnumerator = document.Childs.GetEnumerator();
		
		// Current temperature.
		dataEnumerator.MoveNext ();

		// City.
		dataEnumerator.MoveNext ();
		
		// Overall weather data.
		dataEnumerator.MoveNext ();
		JSONNode weatherArray = (JSONNode)dataEnumerator.Current;
		IEnumerator weatherEnumerator = weatherArray.Childs.GetEnumerator();

		// sunset/rise
		weatherEnumerator.MoveNext ();
		string astronomyValues = ((JSONNode) weatherEnumerator.Current).ToString();
		string[] splitAstronomyValues = astronomyValues.Split (new char[]{'\''});

		string sunRise = (splitAstronomyValues [7]);
		float sunRiseHour = float.Parse(sunRise.Split(new char[]{'\"'})[0])+(sunRise.EndsWith("PM")?12:0);
		float sunRiseMinute = float.Parse (sunRise.Split (new char[]{'\"'}) [2].Remove (2));
		Debug.Log (sunRiseHour + ":" + sunRiseMinute);

		string sunSet = splitAstronomyValues [9];
		float sunSetHour = float.Parse(sunSet.Split(new char[]{'\"'})[0])+(sunSet.EndsWith("PM")?12:0);
		float sunSetMinute = float.Parse (sunSet.Split (new char[]{'\"'}) [2].Remove (2));
		Debug.Log (sunSetHour + ":" + sunSetMinute);

		// hourly

		return weather;
	}

	private static string GetJSONFromWebRequest(string _url)
	{
		WebRequest request = WebRequest.Create (_url);
		WebResponse response = request.GetResponse ();
		Stream data = response.GetResponseStream ();
		string json = "";
		using (StreamReader sr = new StreamReader(data)) {
			json = sr.ReadToEnd ();
		}
		return json;
	}
}

/*
 * var N = JSON.Parse(the_JSON_string);
var versionString = N['version'].Value;        // versionString will be a string containing '1.0'
var versionNumber = N['version'].AsFloat;      // versionNumber will be a float containing 1.0
var name = N['data']['sampleArray'][2]['name'];// name will be a string containing 'sub object'
 
//C#
string val = N['data']['sampleArray'][0];      // val contains 'string value'
 
//UnityScript
var val : String = N['data']['sampleArray'][0];// val contains 'string value'
 
var i = N['data']['sampleArray'][1].AsInt;     // i will be an integer containing 5
N['data']['sampleArray'][1].AsInt = i+6;       // the second value in sampleArray will contain '11'
 
N['additional']['second']['name'] = 'FooBar';  // this will create a new object named 'additional' in this object create another
                                               //object 'second' in this object add a string variable 'name'
 
var mCount = N['countries']['germany']['moronCount'].AsInt; // this will return 0 and create all the required objects and
                                                            // initialize 'moronCount' with 0.
 
if (N['wrong'] != null)                        // this won't execute the if-statement since 'wrong' doesn't exist
{}
if (N['wrong'].AsInt == 0)                     // this will execute the if-statement and in addition add the 'wrong' value.
{}
 
N['data']['sampleArray'][-1] = 'Test';         // this will add another string to the end of the array
N['data']['sampleArray'][-1]['name'] = 'FooBar'; // this will add another object to the end of the array which contains a string named 'name'
 
N['data'] = 'erased';                          // this will replace the object stored in data with the string 'erased'
 */
