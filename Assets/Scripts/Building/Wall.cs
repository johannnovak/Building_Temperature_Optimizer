using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

class WallMaterialAttr: Attribute
{
	internal WallMaterialAttr(float _conductivity)
	{
		Conductivity = _conductivity;
	}

	public float Conductivity { get; private set;}
}

public enum WallMaterial 
{
	[WallMaterialAttr(1.02356F)] Concrete = 0,
	[WallMaterialAttr(0.36987F)] Wood = 1,
	[WallMaterialAttr(0.00025F)] Glass = 2
}

public class Wall : MonoBehaviour {

	private RoomContainer m_roomContainer1;
	public Room m_room1;
	private RoomContainer m_roomContainer2;
	public Room m_room2;

	public WallMaterial m_wallMaterial;

	public float Width { get; private set;}
	public float Height { get; private set;}
	public float Depth { get; private set;}
	public float Surface { get; private set;}
	public float Volume { get; private set;}

	public void Initialize()
	{
		m_room1.AddWall(GetComponent<Wall>());
		m_roomContainer1 = m_room1.gameObject.transform.parent.gameObject.GetComponent<RoomContainer> ();

		if (m_room2 != null)
		{
			m_room2.AddWall(GetComponent<Wall>());
			m_roomContainer2 = m_room2.gameObject.transform.parent.gameObject.GetComponent<RoomContainer> ();
		}

		Bounds colliderBounds = GetComponent<BoxCollider> ().bounds;
		Vector3 dimensions = colliderBounds.max - colliderBounds.min;

		Width = dimensions.x;
		Height = dimensions.y;
		Depth = dimensions.z;

		Surface = Width * Height;
		Volume = Surface * Depth;
	}

	public RoomContainer GetRoomContainer1()
	{
		return m_roomContainer1;
	}

	public void SetRoomContainer1(RoomContainer _roomContainer1)
	{
		m_roomContainer1 = _roomContainer1;
	}

	public RoomContainer GetRoomContainer2()
	{
		return m_roomContainer2;
	}
	
	public void SetRoomContainer2(RoomContainer _roomContainer2)
	{
		m_roomContainer2 = _roomContainer2;
	}

	public WallMaterial GetWallMaterial()
	{
		return m_wallMaterial;
	}

	public override string ToString ()
	{
		string display = "";

		display += "\nWall : '"+name+"'";
		display += "\nWall Material  = " + m_wallMaterial + " (" + GetAttr(m_wallMaterial).Conductivity+")";
		display += "\n -> Width    = " + Width;
		display += "\n -> Height   = " + Height;
		display += "\n -> Depth    = " + Depth;
		display += "\n -> Surface  = " + Surface;
		display += "\n -> Volume   = " + Volume;
		display += "\n -> Between '" + ((m_room1 == null)?"outside":m_room1.name) + "' <-> '" + ((m_room2 == null)?"outside":m_room2.name) + "'";
		display += "\nEndWall";

		return display;
	}
	
	private static WallMaterialAttr GetAttr(WallMaterial _wallMaterial)
	{
		return (WallMaterialAttr)Attribute.GetCustomAttribute(ForValue(_wallMaterial), typeof(WallMaterialAttr));
	}
	
	private static MemberInfo ForValue(WallMaterial _w)
	{
		return typeof(WallMaterial).GetField(Enum.GetName(typeof(WallMaterial), _w));
	}
}
