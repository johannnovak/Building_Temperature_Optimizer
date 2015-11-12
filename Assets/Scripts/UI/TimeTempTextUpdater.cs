using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class TimeTempTextUpdater : TimeTempListener {

	private Text m_timeDisplayer;

	private void Start()
	{
		m_timeDisplayer = GetComponent<Text> ();
	}

	private void Update()
	{
		string display = "";
	
		/* displays time. */
		if (m_timeHour < 10)
			display += "0";
		display += m_timeHour;

		display += ":";

		if (m_timeMinute < 10)
			display += "0";
		display += m_timeMinute;

		/* displays temperature. */
		display += "\n";
		display += m_temperature.ToString("F1") + "°C";

		m_timeDisplayer.text = display;
	}
}