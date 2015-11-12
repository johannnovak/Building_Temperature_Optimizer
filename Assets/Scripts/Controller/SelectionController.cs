using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
	
	public Camera m_camera;

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
					if(m_selectedObjects.Count != 0)
					{
						//m_selectedObject.GetComponent<Renderer>().material = m_selectedOldMaterial;
						if(m_selectedObjects.Contains(o))
						{
							foreach(GameObject selectedObject in m_selectedObjects)
								selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];

							m_selectedObjects.Clear();
						}
						else
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
						}
					}
					else
					{
						Transform containerTransform = o.transform.parent;
						for(int i = 0; i < containerTransform.childCount; ++i)
						{
							GameObject roomObject = containerTransform.GetChild(i).gameObject;
							//m_selectedOldMaterial = m_selectedObject.GetComponent<Renderer>().material;
							roomObject.GetComponent<Renderer>().material = m_selectedMaterial;
							m_selectedObjects.Add(roomObject);
						}
					}
				}
			}
		}
	}
}
