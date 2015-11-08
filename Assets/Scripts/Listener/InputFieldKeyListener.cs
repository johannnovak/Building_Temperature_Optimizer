using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InputFieldKeyListener : MonoBehaviour {

	private InputField m_inputField;
	public InputField m_nextInputField;

	// Use this for initialization
	private void Start () {
		m_inputField = GetComponent<InputField> ();
	}

	private void OnGUI () {
		if (Input.GetKeyDown(KeyCode.Tab) && m_inputField.isFocused)
		{
			EventSystem.current.SetSelectedGameObject(m_nextInputField.gameObject, null);
		}				
	}
}
	