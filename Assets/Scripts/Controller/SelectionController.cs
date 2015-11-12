using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
	
	public Camera m_camera;

	public GameObject m_panelRoomInfo;
	public InputField m_inputfieldObjectiveTemperature;
	public InputField m_inputfieldDeliveredEnergy;

	private List<GameObject> m_selectedObjects;
	private Material m_selectedMaterial;
	private Material m_selectedOldMaterial;

	// Use this for initialization
	void Start () {
		m_selectedObjects = new List<GameObject> ();
		m_selectedMaterial = Resources.Load("stripes_mat", typeof(Material)) as Material;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown(0))
		{
			/* Left click */
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(m_camera.ScreenPointToRay(Input.mousePosition), out hitInfo);
			if (hit) 
			{
				if (hitInfo.transform.gameObject.tag == "room")
				{
					GameObject o = hitInfo.transform.gameObject.transform.gameObject;

					/* Room already selected. */
					if(m_selectedObjects.Count != 0)
					{
						/* Room inside the selected objects. => deselection. */
						if(m_selectedObjects.Contains(o))
						{
							foreach(GameObject selectedObject in m_selectedObjects)
								selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];
							m_selectedObjects.Clear();
							
							m_inputfieldObjectiveTemperature.text = string.Empty;
							m_inputfieldDeliveredEnergy.text = string.Empty;
						}
						else /* Room not inside => list cleared + add new room. */
						{
							foreach(GameObject selectedObject in m_selectedObjects)
								selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];
							
							m_selectedObjects.Clear();

							Transform containerTransform = o.transform.parent;
							for(int i = 0; i < containerTransform.childCount; ++i)
							{
								GameObject roomObject = containerTransform.GetChild(i).gameObject;
								//m_selectedOldMaterial = m_selectedObject.GetComponent<Renderer>().material;
								roomObject.GetComponent<Renderer>().material = m_selectedMaterial;
								m_selectedObjects.Add(roomObject);
							}

							m_inputfieldObjectiveTemperature.text = containerTransform.gameObject.GetComponent<RoomContainer>().GetObjectiveTemperature().ToString();
							m_inputfieldDeliveredEnergy.text = containerTransform.gameObject.GetComponent<RoomContainer>().GetDeliveredEnergy().ToString();
						}
					}
					else /* No room selected => add room. */
					{
						Transform containerTransform = o.transform.parent;
						Debug.Log(containerTransform.gameObject.GetComponent<RoomContainer>().GetObjectiveTemperature()+"/"+containerTransform.gameObject.GetComponent<RoomContainer>().GetDeliveredEnergy());
						for(int i = 0; i < containerTransform.childCount; ++i)
						{
							GameObject roomObject = containerTransform.GetChild(i).gameObject;
							//m_selectedOldMaterial = m_selectedObject.GetComponent<Renderer>().material;
							roomObject.GetComponent<Renderer>().material = m_selectedMaterial;
							m_selectedObjects.Add(roomObject);
						}
						
						m_inputfieldObjectiveTemperature.text = containerTransform.gameObject.GetComponent<RoomContainer>().GetObjectiveTemperature().ToString();
						m_inputfieldDeliveredEnergy.text = containerTransform.gameObject.GetComponent<RoomContainer>().GetDeliveredEnergy().ToString();
					}

					/* Shows menu when clicked. */
					if(m_selectedObjects.Count > 0)
						m_panelRoomInfo.SetActive(true);
					else
						m_panelRoomInfo.SetActive(false);
				}
			}
		}
	}
	
	public void UpdateObjectiveTemperature(string _temperature)
	{
		m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ().SetObjectiveTemperature (float.Parse(_temperature));
	}
	
	public void UpdateDeliveredEnergy(string _energy)
	{
		m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ().SetDeliveredEnergy(float.Parse(_energy));
	}
}
