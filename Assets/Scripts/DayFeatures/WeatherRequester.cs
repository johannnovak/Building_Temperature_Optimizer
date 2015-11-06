using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using SimpleJSON;


public class WeatherRequester : MonoBehaviour {

	public string m_town;

	// Use this for initialization
	void Start () {
		RequestWeather ();
	}

	public Weather RequestWeather()
	{
		string json = GetJSONFromWebRequest ("http://api.worldweatheronline.com/free/v2/weather.ashx?q="+m_town+"&format=json&num_of_days=1&date=today&key=b1fbba3aff3f713681d871adf0f99");
		JSONNode document = JSON.Parse (json);
	
		/* sunset/rise */
		string sunRise = document["data"]["weather"][0]["astronomy"][0]["sunrise"].ToString().Split(new char[]{'\"'})[1];
		float sunRiseHour = float.Parse(sunRise.Split(new char[]{':'})[0])+(sunRise.EndsWith("PM")?12:0);
		float sunRiseMinute = float.Parse (sunRise.Split (new char[]{':'})[1].Remove(2));

		string sunSet = document["data"]["weather"][0]["astronomy"][0]["sunset"].ToString().Split(new char[]{'\"'})[1];
		float sunSetHour = float.Parse(sunSet.Split(new char[]{':'})[0])+(sunSet.EndsWith("PM")?12:0);
		float sunSetMinute = float.Parse (sunSet.Split(new char[]{':'})[1].Remove(2));

		Dictionary<float, float> timeAndTemperature = new Dictionary<float, float> ();

		/* hours / temperature */
		for (int i = 0; i < document["data"]["weather"][0]["hourly"].Count; ++i)
			timeAndTemperature.Add(float.Parse(document ["data"] ["weather"] [0] ["hourly"] [i] ["time"])/100, float.Parse (document ["data"] ["weather"] [0] ["hourly"] [i] ["tempC"]));

		Weather weather = new Weather (sunRiseHour, sunRiseMinute, sunSetHour, sunSetMinute, timeAndTemperature);
	
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
