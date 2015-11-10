using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SimulationTimeTempUpdater : MonoBehaviour {

	public Text m_timeDisplayer;
	
	public void UpdateTimeAndTemperature(float _hour, float _minutes, float _temperature)
	{
		string display = "";
	
		/* displays time. */
		if (_hour < 10)
			display += "0";
		display += _hour;

		display += ":";

		if (_minutes < 10)
			display += "0";
		display += _minutes;

		/* displays temperature. */
		display += "\n";
		display += _temperature.ToString("F1") + "°C";

		m_timeDisplayer.text = display;
	}
}