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

	public RoomContainer m_room1;
	public RoomContainer m_room2;

	public WallMaterial m_wallMaterial;

	public float Width { get; private set;}
	public float Height { get; private set;}
	public float Surface { get; private set;}

	private void Start()
	{
		Width = GetComponent<RectTransform> ().rect.width;
		Height = GetComponent<RectTransform> ().rect.height;
		Surface = Width * Height;
	}

	public RoomContainer GetRoomContainer1()
	{
		return m_room1;
	}

	public void SetRoomContainer1(RoomContainer _room1)
	{
		m_room1 = _room1;
	}

	public RoomContainer GetRoomContainer2()
	{
		return m_room2;
	}
	
	public void SetRoomContainer2(RoomContainer _room2)
	{
		m_room2 = _room2;
	}

	public WallMaterial GetWallMaterial()
	{
		return m_wallMaterial;
	}
}
