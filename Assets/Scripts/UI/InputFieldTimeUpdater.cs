using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class InputFieldTimeUpdater : MonoBehaviour {

	public InputField m_hourInputField;
	public InputField m_minuteInputField;

	private Text m_hourPlaceHolder;
	private Text m_minutePlaceHolder;

	// Use this for initialization
	void Start () {
		m_hourPlaceHolder = m_hourInputField.transform.GetChild (0).gameObject.GetComponent<Text> ();
		m_minutePlaceHolder = m_minuteInputField.transform.GetChild (0).gameObject.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		string[] currentTime = DateTime.Now.ToString ("HH:mm").Split (':');
		m_hourPlaceHolder.text = currentTime [0];
		m_minutePlaceHolder.text = currentTime [1];
	}
}
