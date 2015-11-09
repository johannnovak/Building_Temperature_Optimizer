using UnityEngine;
using System.Collections;

public class CameraHandling : MonoBehaviour {

	private float m_rotationX;
	private float m_rotationY;


	// Update is called once per frame
	void Update () {
		/* Computes the X and Y rotation from the current device used. */
		m_rotationY  = Input.GetAxis ("Mouse X");
		m_rotationX += Input.GetAxis ("Mouse Y");
		
		m_rotationY += transform.localEulerAngles.y;

		transform.localEulerAngles = new Vector3(-m_rotationX, m_rotationY, 0);
	}
}
