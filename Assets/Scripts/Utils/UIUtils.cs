using UnityEngine;
using System.Collections;

public class UIUtils : MonoBehaviour {

	public void UpdateInputField(string _inputfieldName)
	{
		GameObject inputField = GameObject.Find (_inputfieldName);
		inputField.SetActive (false);
		inputField.SetActive (true);
	}
}
