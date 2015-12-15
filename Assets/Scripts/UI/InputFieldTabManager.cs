using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InputFieldTabManager : MonoBehaviour {

	public InputField[] m_inputFields;

	int m_currentIndex;

	// Use this for initialization
	private void Start () {
		m_currentIndex = 0;
	}

	private void OnGUI () {
		//Debug.Log (Input.GetKeyDown(KeyCode.Tab) + " / " + m_currentIndex+ " / "+m_inputFields[m_currentIndex].isFocused);


		if (Input.GetKeyDown(KeyCode.Tab) && m_inputFields[m_currentIndex].isFocused)
		{
			m_currentIndex = (m_currentIndex + 1)%m_inputFields.Length;
			EventSystem.current.SetSelectedGameObject(m_inputFields[m_currentIndex].gameObject, null);
		}				
	}

	public void ResetIndex()
	{
		m_currentIndex = 0;
	}
}
	