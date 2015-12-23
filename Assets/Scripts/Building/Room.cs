using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Room : MonoBehaviour {

	public float Width { get; private set;}
	public float Height { get; private set;}
	public float Depth { get; private set;}

	public float Surface { get; private set;}
	public float Volume { get; private set;}

	private List<Wall> m_walls;

	private List<Actionner> m_commandableActionners;

	public void Initialize()
	{
		Bounds colliderBoxBounds = GetComponent<BoxCollider> ().bounds;
		Vector3 dimensions = colliderBoxBounds.max - colliderBoxBounds.min;

		Width = dimensions.x;
		Height = dimensions.y;
		Depth = dimensions.z;

		Surface = Width * Height;
		Volume = Surface * Depth;

		m_walls = new List<Wall> ();
		m_commandableActionners = new List<Actionner> ();
		for (int i = 0; i < transform.childCount; ++i)
			if (transform.GetChild (i).tag.Equals ("commandableObject"))
				m_commandableActionners.Add (transform.GetChild(i).gameObject.GetComponent<Actionner>());
	}

	public List<Wall> GetWalls()
	{
		return m_walls;
	}

	public void AddWall(Wall _wall)
	{
		m_walls.Add (_wall);
	}

	public List<Actionner> GetCommandableActionnerList()
	{
		return m_commandableActionners;
	}

	public override string ToString()
	{
		string display = "";

		display += "\nRoom - "+ name;
		display += "\n width  = " + Width;
		display += "\n height = " + Height;
		display += "\n depth  = " + Depth;
		display += "\nComposed of " + m_walls.ToArray ().Length + " walls :";
		foreach (Wall w in m_walls)
			display += w.ToString ();
		display += "\nEndRoom";

		return display;
	}

	public void ResetRoom()
	{
		foreach (Actionner ac in m_commandableActionners)
			ac.ResetActionner ();
	}
}