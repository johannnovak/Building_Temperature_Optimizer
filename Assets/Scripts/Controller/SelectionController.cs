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
	public Text m_deliveredEnergyRangeInterval;
	public Text m_deliveredEnergyValue;

	private GameObject m_selectedActionnerButton;

	private List<GameObject> m_selectedObjects;
	private Material m_selectedMaterial;
	private Material m_selectedOldMaterial;

	private bool m_canSelect;

	public GameObject m_buttonGo;
	public GameObject m_buttonConfigure;

	// Use this for initialization
	public void Start () {
		m_canSelect = true;
		m_selectedObjects = new List<GameObject> ();
		m_selectedMaterial = Resources.Load("stripes_mat", typeof(Material)) as Material;

		m_tabManager = GetComponent<InputFieldTabManager> ();

		m_actionnerListContentPanel = m_panelActionnerList.transform.GetChild (1).GetChild (0).GetChild (0).GetChild (0).gameObject;
		Building b = m_configurationController.GetBuilding();
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

		m_deliveredEnergyRangeInterval.text = "[Nan ; Nan]";
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
			Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
			bool hit = Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, m_camera.cullingMask);
			if (hit) 
			{
				if (hitInfo.transform.gameObject.tag == "room")
				{
					GameObject roomSelected = hitInfo.transform.gameObject;

					/* Room already selected. */
					if(m_selectedObjects.Count != 0)
					{
						if(m_selectedObjects.Contains(roomSelected))
						{
							/* Room inside the selected objects. => deselection. */
							RoomDeselection();
						}
						/* Room not inside => add new room. */
						else
						{
							RoomDeselection();
							RoomSelection(roomSelected);
						}
					}
					else /* No room selected => add room. */
					{
						RoomSelection(roomSelected);
					}

					/* Shows menu when clicked. */
					if(m_selectedObjects.Count > 0)
					{
						InitializeSimulationMenus(roomSelected);
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

	private void InitializeSimulationMenus(GameObject _roomSelected)
	{ 
		m_tabManager.ResetIndex();
		m_panelRoomInfo.SetActive(true);
		m_panelRoomInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = _roomSelected.transform.parent.gameObject.name;
		EventSystem.current.SetSelectedGameObject(m_inputfieldObjectiveTemperature.gameObject, null);
		m_inputfieldObjectiveTemperature.OnPointerClick(new PointerEventData(EventSystem.current));

		ResetActionnerButtonsPanel();
		m_panelActionnerList.SetActive(true);
		RoomContainer rc = _roomSelected.transform.parent.gameObject.GetComponent<RoomContainer>();
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
			UpdateHeatIntervalText(rc.MinDeliveredEnergy, rc.MaxDeliveredEnergy);
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

	public void RoomDeselection()
	{
		foreach(GameObject selectedObject in m_selectedObjects)
			selectedObject.GetComponent<MeshRenderer>().materials = new Material[0];
		m_selectedObjects.Clear();

		m_inputfieldObjectiveTemperature.text = "";
		m_panelHeatRangeInfo.SetActive (false);	
		m_panelActionnerList.SetActive (false);
		m_panelRoomInfo.SetActive (false);
		m_selectedActionnerButton = null;

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldObjectiveTemperature};
	}

	private void RoomSelection(GameObject o)
	{
		Transform roomContainerTransform = o.transform.parent;
		for(int i = 0; i < roomContainerTransform.childCount; ++i)
		{
			GameObject roomObject = roomContainerTransform.GetChild(i).gameObject;
			//m_selectedOldMaterial = m_selectedObject.GetComponent<Renderer>().material;
			roomObject.GetComponent<Renderer>().material = m_selectedMaterial;
			m_selectedObjects.Add(roomObject);
		}

		/* Updates the inputfields. */
		float temp = roomContainerTransform.gameObject.GetComponent<RoomContainer>().ObjectiveTemperature;
		float minDeliveredEnergy = roomContainerTransform.gameObject.GetComponent<RoomContainer> ().MinDeliveredEnergy;
		float maxDeliveredEnergy = roomContainerTransform.gameObject.GetComponent<RoomContainer> ().MaxDeliveredEnergy;
		float currentDeliveredEnergy = roomContainerTransform.gameObject.GetComponent<RoomContainer> ().CurrentDeliveredEnergy;
		m_inputfieldObjectiveTemperature.text = (float.IsNaN(temp) ? "" : temp.ToString());
		UpdateHeatIntervalText (minDeliveredEnergy, maxDeliveredEnergy);
		m_deliveredEnergyValue.text = currentDeliveredEnergy.ToString () + " °C";

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldObjectiveTemperature};
	}

	private bool ActionnerReadied(RoomContainer _rc)
	{
		bool actionnersReadied = true;
		foreach (Room r in _rc.GetRooms())
			foreach (Actionner ac in r.GetCommandableActionnerList())
				actionnersReadied &= ac.Prepared;

		return actionnersReadied;
	}

	public void CheckRoomConfigured(RoomContainer _rc)
	{
		bool actionnersReadied = ActionnerReadied (_rc);

		if (!float.IsNaN(_rc.ObjectiveTemperature) && actionnersReadied) 
		{
			m_configurationController.UpdateConfiguredRoomContainers(_rc);
			_rc.Prepared = true;
		}
		else
		{
			_rc.Prepared = false;
		}
	}

	public void UpdateObjectiveTemperature(string _temperature)
	{
		RoomContainer c = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();

		if (!string.IsNullOrEmpty (_temperature)) 
		{
			float temperature;
			float.TryParse (_temperature, out temperature);
			c.ObjectiveTemperature = temperature;

			UpdateButtonSimulationLaunch();
		}
		else
		{
			m_inputfieldObjectiveTemperature.text = float.IsNaN(c.ObjectiveTemperature)?"":c.ObjectiveTemperature.ToString();
		}
		CheckRoomConfigured (c);
	}

	public void UpdateMinDeliveredEnergyFromInputField(string _energy)
	{
		UpdateMinDeliveredEnergy (_energy, null);
	}

	public void UpdateMinDeliveredEnergy(string _energy, Actionner _ac)
	{
		RoomContainer rc = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();
		Actionner ac;

		if (_ac == null)
			ac = GameObject.Find (m_selectedActionnerButton.transform.GetChild (0).GetComponent<Text> ().text).GetComponent<Actionner> ();
		else
			ac = _ac;

		if (!string.IsNullOrEmpty (_energy)) 
		{
			float energy;
			float.TryParse (_energy, out energy);

			if(float.IsNaN(ac.MaxDeliveredEnergy) || energy < ac.MaxDeliveredEnergy)
			{
				if(!float.IsNaN(rc.MinDeliveredEnergy) && !float.IsNaN(ac.MinDeliveredEnergy))
					rc.MinDeliveredEnergy = rc.MinDeliveredEnergy - ac.MinDeliveredEnergy;

				ac.MinDeliveredEnergy = energy;

				if(float.IsNaN(rc.MinDeliveredEnergy))
					rc.MinDeliveredEnergy = ac.MinDeliveredEnergy;
				else
					rc.MinDeliveredEnergy = rc.MinDeliveredEnergy + ac.MinDeliveredEnergy;

				UpdateHeatIntervalText(rc.MinDeliveredEnergy, rc.MaxDeliveredEnergy);

				if(!float.IsNaN(ac.MinDeliveredEnergy) && !float.IsNaN(ac.MaxDeliveredEnergy))
					ac.Prepared = true;

				UpdateButtonSimulationLaunch();
			}
			else
			{
				m_inputfieldMinDeliveredEnergy.text = (float.IsNaN(ac.MinDeliveredEnergy))?"":ac.MinDeliveredEnergy.ToString();
			}
		}
		else
		{
			m_inputfieldMinDeliveredEnergy.text = float.IsNaN(ac.MinDeliveredEnergy)?"":ac.MinDeliveredEnergy.ToString();
		}
		CheckRoomConfigured (rc);
	}

	public void UpdateMaxDeliveredEnergyFromInputField(string _energy)
	{
		UpdateMaxDeliveredEnergy (_energy, null);
	}

	private void UpdateMaxDeliveredEnergy(string _energy, Actionner _ac)
	{
		RoomContainer rc = m_selectedObjects.ToArray () [0].transform.parent.gameObject.GetComponent<RoomContainer> ();
		Actionner ac;

		if (_ac == null)
			ac = GameObject.Find (m_selectedActionnerButton.transform.GetChild (0).GetComponent<Text> ().text).GetComponent<Actionner> ();
		else
			ac = _ac;


		if (!string.IsNullOrEmpty (_energy)) 
		{
			float energy;
			float.TryParse (_energy, out energy);

			if(float.IsNaN(ac.MinDeliveredEnergy) || energy > ac.MinDeliveredEnergy)
			{
				if(!float.IsNaN(rc.MaxDeliveredEnergy) && !float.IsNaN(ac.MaxDeliveredEnergy))
					rc.MaxDeliveredEnergy = rc.MaxDeliveredEnergy - ac.MaxDeliveredEnergy;

				ac.MaxDeliveredEnergy = energy;

				if(float.IsNaN(rc.MaxDeliveredEnergy))
					rc.MaxDeliveredEnergy = ac.MaxDeliveredEnergy;
				else
					rc.MaxDeliveredEnergy = rc.MaxDeliveredEnergy + ac.MaxDeliveredEnergy;

				UpdateHeatIntervalText(rc.MinDeliveredEnergy, rc.MaxDeliveredEnergy);

				if(!float.IsNaN(ac.MinDeliveredEnergy) && !float.IsNaN(ac.MaxDeliveredEnergy))
					ac.Prepared = true;

				UpdateButtonSimulationLaunch();
			}
			else
			{
				m_inputfieldMaxDeliveredEnergy.text = (float.IsNaN(ac.MaxDeliveredEnergy))?"":ac.MaxDeliveredEnergy.ToString();
			}
		}
		else
		{
			m_inputfieldMaxDeliveredEnergy.text = float.IsNaN(ac.MaxDeliveredEnergy)?"":ac.MaxDeliveredEnergy.ToString();
		}
		CheckRoomConfigured (rc);
	}

	private void UpdateHeatIntervalText(float _minEnergy, float _maxEnergy)
	{
		m_deliveredEnergyRangeInterval.text = "[ "+_minEnergy+" ; "+_maxEnergy+" ]";
	}

	private void UpdateButtonSimulationLaunch()
	{
		if(m_buttonGo.activeInHierarchy)
		{
			m_buttonGo.SetActive(false);
			m_buttonConfigure.SetActive(true);
			m_buttonConfigure.GetComponent<Button>().interactable = true;
		}
	}

	public void RandomConfiguration()
	{
		RoomDeselection ();

		System.Random rd = new System.Random ();
		for(int i = 0; i < m_configurationController.GetBuilding().GetFloors().ToArray().Length; ++i)
		{
			Floor f = m_configurationController.GetBuilding().GetFloors().ToArray()[i];
			m_configurationController.SetCurrentSelectedFloor(i);
			foreach(RoomContainer rc in f.GetRoomContainers())
			{
				m_selectedObjects.Clear();
				m_selectedObjects.Add(rc.transform.GetChild(0).gameObject);
				UpdateObjectiveTemperature(rd.Next(12, 25).ToString());
				m_selectedObjects.Clear();

				foreach(Room r in rc.GetRooms())
				{
					m_selectedObjects.Clear();
					m_selectedObjects.Add(r.gameObject);

					foreach(Actionner ac in r.GetCommandableActionnerList())
					{
						switch(ac.m_properties)				
						{
						case ActionnerProperties.AirConditionner:
							UpdateMinDeliveredEnergy(rd.Next(-4200, -2500).ToString(), ac);
							UpdateMaxDeliveredEnergy(rd.Next(3000, 6000).ToString(), ac);
							break;
						case ActionnerProperties.GazHeater:	
							UpdateMinDeliveredEnergy("0", ac);
							UpdateMaxDeliveredEnergy(rd.Next(700, 3100).ToString(), ac);
							break;
						case ActionnerProperties.RadiativeHeater:
							UpdateMinDeliveredEnergy("0", ac);
							UpdateMaxDeliveredEnergy(rd.Next(500, 2000).ToString(), ac);
							break;
						}
					}
				}
			}
		}
		m_configurationController.SetCurrentSelectedFloor(-1);
		m_selectedObjects.Clear();
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
		float minEnergy = ac.MinDeliveredEnergy;
		float maxEnergy = ac.MaxDeliveredEnergy;

		m_inputfieldMinDeliveredEnergy.text = (float.IsNaN(minEnergy) ? "" : minEnergy.ToString());
		m_inputfieldMaxDeliveredEnergy.text = (float.IsNaN(maxEnergy) ? "" : maxEnergy.ToString());

		m_tabManager.ResetIndex ();
		m_tabManager.m_inputFields = new InputField[]{m_inputfieldMinDeliveredEnergy, m_inputfieldMaxDeliveredEnergy, m_inputfieldObjectiveTemperature};

		EventSystem.current.SetSelectedGameObject(m_inputfieldMinDeliveredEnergy.gameObject, null);
		m_inputfieldMinDeliveredEnergy.OnPointerClick(new PointerEventData(EventSystem.current));
	}
}
