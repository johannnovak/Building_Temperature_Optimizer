using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldFix : MonoBehaviour {

	private InputField m_inputField;

	private void Start()
	{
		m_inputField = GetComponent<InputField> ();
	}

	public void UpdateValue(string _value)
	{
		m_inputField.text += _value;
		m_inputField.textComponent.text += _value;
	}

	public void OnValueChanged(string _value)
	{
		UpdateValue (_value);
	}
}
