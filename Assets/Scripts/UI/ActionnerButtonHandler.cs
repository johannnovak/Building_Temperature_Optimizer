using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ActionnerButtonHandler : MonoBehaviour {

	private SelectionController m_controller;

	private void Start () 
	{
		GetComponent<Button> ().onClick.AddListener (delegate {ButtonClicked();});
		m_controller = GameObject.Find ("SelectionManager").GetComponent<SelectionController>();
	}


	private void ButtonClicked()
	{
		m_controller.ActionnerButtonSelection (this.gameObject);
	}
}
