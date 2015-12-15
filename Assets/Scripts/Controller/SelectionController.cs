using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class SelectionController : MonoBehaviour {
	
	public Camera m_camera;

	public ConfigurationController m_configurationController;
	
	public GameObject m_panelRoomInfo;
	public GameObject m_panelActionnerList;
	public GameObject m_panelHeatRangeInfo;

	private GameObject m_actionnerListContentPanel;
	private List<GameObject> m_actionnerButtonList;
	private InputFieldTabManager m_tabManager;
	public InputField m_inputfieldObjectiveTemperature;
	public InputField m_inputfieldMinDeliveredEnergy;
	public InputField m_inputfieldMaxDeliveredEnergy;

	private GameObject m_selectedActionnerButton;

	private List<GameObject> m_selectedObjects;
	private Material m_selectedMaterial;
	private Material m_selectedOldMaterial;

	private bool m_canSelect;

	// Use this for initialization
	void Start () {
		m_canSelect = true;
		m_selectedObjects = new List<GameObject> ();
		m_selectedMaterial = Resources.Load("stripes_mat", typeof(Material)) as Material;

		m_tabManager = GetComponent<InputFieldTabManager> ();

		m_actionnerListContentPanel = m_panelActionnerList.transform.GetChild (1).GetChild (0).GetChild (0).GetChild (0).gameObject;
		Building b = m_configurationController.m_building;
		b.Initialize ();
		m_actionnerButtonList = new List<GameObject> ();
		foreach (Floor f in b.GetFloors())
			foreach (RoomContainer rc in f.GetRoomContainers())
				foreach (Room r in rc.GetRooms())
					foreach (Actionner a in r.GetCommandableActionnerList())
						AddButtonToActionnerList (m_actionnerButtonList.ToArray().Length);
//		RectTransform rectT = m_actionnerListContentPanel.GetComponent<RectTransform> ();
//		rectT.offsetMax = new Vector2(rectT.offsetMax.x, m_actionnerButtonList.ToArray () [0].gameObject.GetComponent<RectTransform> ().rect.height * m_actionnerButtonList.ToArray ().Length);

		m_selectedActionnerButton = null;
		m_panelActionnerList.SetActive (false);
		m_panelRoomInfo.SetActive (false);
		m_panelHeatRangeInfo.SetActive (false);
	}
	
	private void AddButtonToActionnerList(int _index)
	{
		GameObject bo = Instantiate(Resources.Load("Prefabs/actionnerListButton")) as GameObject;
		Vector3 contentPosition = m_actionnerListContentPanel.transform.position;
		Vector3 pos = new Vector3 (contentPosition.x, m_actionnerListContentPanel.transform.parent.position.y - bo.GetComponent<RectTransform>().rect.height - _index*bo.GetComponent<RectTransform>().rect.height, contentPosition.z);
		bo.transform.position = pos;
		bo.transform.SetParent (m_actionnerListContentPanel.transform);
		bo.SetActive (true);
		m_actionnerButtonList.Add (bo);
	}

	// Update is called once per frame
	void Update () {
		
		if (m_canSelect && Input.GetMouseButtonDown(0))
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
						if(m_selectedObjects.Contains(o))
						{
							/* Room inside the selected objects. => deselection. */
							RoomDeselection();
						}
						/* Room not inside => add new room. */
						else
						{
							RoomDeselection();
							RoomSelection(o);
						}
					}
					else /* No room selected => add room. */
					{
						RoomSelection(o);
					}

					/* Shows menu when clicked. */
					if(m_selectedObjects.Count > 0)
					{
						InitializeSimulationMenus(o);
					}
					else
					{
						m_panelRoomInfo.SetActive(false);
						m_panelActionnerList.SetActive(false);
						m_panelHeatRangeInfo.SetActive(false);
					}
				}
			}
		}
	}

	private void InitializeSimulationMenus(GameObject _object)
	{ 
		m_tabManager.ResetIndex();
		m_panelRoomInfo.SetActive(true);
		m_panelRoomInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _object.transform.parent.gameObject.name;
		EventSystem.current.SetSelectedGameObject(m_inputfieldObjectiveTemperature.gameObject, null);
		m_inputfieldObjectiveTemperature.OnPointerClick(new PointerEventData(EventSystem.current));
		
		bool show = false;
		ResetActionnerButtonsPanel();
		m_panelActionnerList.SetActive(true);
		RoomContainer rc = _object.transform.parent.gameObject.GetComponent<RoomContainer>();
		if (rc.ContainsCommandableActionners ()) 
		{
			int index = 0;
			for (int i = 0; i < rc.GetRooms().ToArray().Length; ++i) 
			{
				Room r = rc.GetRooms ().ToArray () [i];
				for (int j = 0; j < r.GetCommandableActionnerList().ToArray().Length; ++j) 
				{
					Actionner actionner = r.GetCommandableActionnerList ().ToArray () [j].GetComponent<Actionner> ();
					GameObject button = m_actionnerButtonList.ToArray () [index];
					button.SetActive (true);
					button.transform.GetChild (0).GetComponent<Text> ().text = actionner.gameObject.name;
					++index;
				}
			}
		}
		else
		{
			m_panelActionnerList.SetActive(false);
			m_selectedActionnerButton = null;
		}

	}
	
	private void ResetActionnerButtonsPanel()
	{
		foreach (GameObject o in m_actionnerButtonList)
			o.SetActive (false);
	}

	private void RoomDeselection()
	{
		foreach(GameObject selectedObject in m_selectedObjects)
			selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];
		m_selectedObjects.Clear();
		
		m_inputfieldObjectiveTemperature.text = "";
		m_panelHeatRangeInfo.SetActive (false);	
		m_selectedActionnerButton = null;

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldObjectiveTemperature};
	}

	private void RoomSelection(GameObject o)
	{
		Transform containerTransform = o.transform.parent;
		for(int i = 0; i < containerTransform.childCount; ++i)
		{
			GameObject roomObject = containerTransform.GetChild(i).gameObject;
			//m_selectedOldMaterial = m_selectedObject.GetComponent<Renderer>().material;
			roomObject.GetComponent<Renderer>().material = m_selectedMaterial;
			m_selectedObjects.Add(roomObject);
		}
		
		/* Updates the inputfields. */
		float temp = containerTransform.gameObject.GetComponent<RoomContainer>().GetObjectiveTemperature();
		m_inputfieldObjectiveTemperature.text = (float.IsNaN(temp) ? "" : temp.ToString());

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldObjectiveTemperature};
	}

	private bool ActionnerReadied(RoomContainer _rc)
	{
		bool actionnersReadied = true;
		foreach (Room r in _rc.GetRooms())
			foreach (Actionner ac in r.GetCommandableActionnerList())
				actionnersReadied &= (!float.IsNaN (ac.GetMinDeliveredEnergy()) && float.IsNaN (ac.GetMinDeliveredEnergy ()));
		
		return actionnersReadied;
	}

	public void CheckRoomConfigured(RoomContainer _rc)
	{
		bool actionnersReadied = ActionnerReadied (_rc);
		
		if (float.IsNaN(_rc.GetObjectiveTemperature()) && actionnersReadied) 
		{
			Debug.Log("oooooooooooooooooooook");
			m_configurationController.UpdateConfiguredRoomContainers(_rc);
		}
	}

	public void UpdateObjectiveTemperature(string _temperature)
	{
		RoomContainer c = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();

		CheckRoomConfigured (c);

		if (!string.IsNullOrEmpty (_temperature)) 
		{
			float temperature;
			float.TryParse (_temperature, out temperature);
			c.SetObjectiveTemperature (temperature);
		}
	}
	
	public void UpdateMinDeliveredEnergy(string _energy)
	{
		RoomContainer c = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();
		
		CheckRoomConfigured (c);

		if (!string.IsNullOrEmpty (_energy)) 
		{
			float energy;
			float.TryParse (_energy, out energy);
			Actionner ac = GameObject.Find(m_selectedActionnerButton.transform.GetChild(0).GetComponent<Text>().text).GetComponent<Actionner>();
			ac.SetMinDeliveredEnergy(energy);
		}
	}
	
	public void UpdateMaxDeliveredEnergy(string _energy)
	{
		RoomContainer c = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();
		
		CheckRoomConfigured (c);
		if (!string.IsNullOrEmpty (_energy)) 
		{
			float energy;
			float.TryParse (_energy, out energy);
			Actionner ac = GameObject.Find(m_selectedActionnerButton.transform.GetChild(0).GetComponent<Text>().text).GetComponent<Actionner>();
			ac.SetMaxDeliveredEnergy(energy);
		}
	}

	public void ResetSelectionController()
	{
		foreach(GameObject selectedObject in m_selectedObjects)
			selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];
		m_selectedObjects.Clear();	
	}

	public void StopSelecting()
	{
		m_canSelect = false;
	}

	public void StartSelecting()
	{
		m_canSelect = true;
	}

	public void ActionnerButtonSelection(GameObject _object)
	{
		m_panelHeatRangeInfo.SetActive (true);
		m_selectedActionnerButton = _object;
		m_panelHeatRangeInfo.transform.GetChild (0).GetChild (0).GetComponent<Text> ().text = "Actionner\n" + (m_selectedActionnerButton.transform.GetChild (0).GetComponent<Text> ().text);

		Actionner ac = GameObject.Find(m_selectedActionnerButton.transform.GetChild(0).GetComponent<Text>().text).GetComponent<Actionner>();
		float minEnergy = ac.GetMinDeliveredEnergy ();
		float maxEnergy = ac.GetMaxDeliveredEnergy ();

		m_inputfieldMinDeliveredEnergy.text = (float.IsNaN(minEnergy) ? "" : minEnergy.ToString());
		m_inputfieldMaxDeliveredEnergy.text = (float.IsNaN(maxEnergy) ? "" : maxEnergy.ToString());

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldMinDeliveredEnergy, m_inputfieldMaxDeliveredEnergy, m_inputfieldObjectiveTemperature};

		EventSystem.current.SetSelectedGameObject(m_inputfieldMinDeliveredEnergy.gameObject, null);
		m_inputfieldMinDeliveredEnergy.OnPointerClick(new PointerEventData(EventSystem.current));

	}
}
