using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeUpdater : MonoBehaviour {

	public Text m_timeDisplayer;

	public void UpdateTime(float _hour, float _minutes)
	{
		string display = "";
		if (_hour < 10)
			display += "0";
		display += _hour;

		display += ":";

		if (_minutes < 10)
			display += "0";
		display += _minutes;

		m_timeDisplayer.text = display;
	}
}