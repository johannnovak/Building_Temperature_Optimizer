using UnityEngine;
using System.Collections;

public class CameraHandling : MonoBehaviour {

	private Camera m_camera;
	
	private Vector3 mouseOrigin;
	private bool m_isRotating;
	private bool m_isTranslating;
	public float m_xyTranslationSpeed;
	public float m_zTranslationSpeed;
	public float m_rotatingSpeed;

	public bool m_canUseCamera;

	private void Start()
	{
		m_camera = GetComponent<Camera> ();
		m_canUseCamera = true;
	}

	// Update is called once per frame
	void Update () {
		if (m_canUseCamera) 
		{
			/* Computes the X and Y rotation from the current device used. */
			float xAxisValue = Input.GetAxis ("Horizontal");
			float zAxisValue = Input.GetAxis ("Vertical");
			float mouseWheelValue = Input.GetAxis ("Mouse ScrollWheel");
			
			if (Input.GetMouseButtonDown (1)) 
			{
				/* Right click */
				m_isRotating = true;
				mouseOrigin = Input.mousePosition;
			}
		
			if (Input.GetMouseButtonDown (2)) 
			{
				/* Middle click */
				m_isTranslating = true;
				mouseOrigin = Input.mousePosition;
			}
		
			if (!Input.GetMouseButton (1))
				m_isRotating = false;
			if (!Input.GetMouseButton (2))
				m_isTranslating = false;
		
			/* Translation with keys */
			transform.Translate (new Vector3 (xAxisValue, zAxisValue, 0.0f));
		
			/* Translation with mouse */
			TranslateIfPossible (m_isTranslating, mouseOrigin);

			/* Perspective y management */
			transform.Translate (new Vector3 (0, 0, m_zTranslationSpeed * mouseWheelValue));
			RotateCameraIfPossible (m_isRotating, mouseOrigin);	
		}
	}
	
	private void TranslateIfPossible(bool _isTranslating, Vector3 _mouseOrigin)
	{
		if (_isTranslating)
			TranslateCamera (_mouseOrigin);
	}
	
	private void TranslateCamera(Vector3 _mouseOrigin)
	{
		Vector3 pos = m_camera.ScreenToViewportPoint(-(Input.mousePosition - _mouseOrigin));
		Vector3 move = new Vector3(pos.x * m_xyTranslationSpeed, pos.y * m_xyTranslationSpeed, 0);
		
		transform.Translate(move, Space.Self);	
	}

	private void RotateCameraIfPossible(bool _isRotating, Vector3 _mouseOrigin)
	{
		if (_isRotating)
			RotateCamera(_mouseOrigin);
	}

	private void RotateCamera(Vector3 _mouseOrigin)
	{
		Vector3 pos = m_camera.ScreenToViewportPoint((Input.mousePosition - _mouseOrigin));
		
		transform.RotateAround(transform.position, transform.right, -pos.y * m_rotatingSpeed);
		transform.RotateAround(transform.position, Vector3.up, pos.x * m_rotatingSpeed);
	}
	
	public void StopUsingCamera()
	{
		m_canUseCamera = false;
	}
	
	public void StartUsingCamera()
	{
		m_canUseCamera = true;
	}

}
