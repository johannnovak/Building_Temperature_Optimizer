using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

class ActionnerPropertiesAttr: Attribute
{
	internal ActionnerPropertiesAttr(float _wattPerHourCost)
	{
		WattPerHourCost = _wattPerHourCost;
	}
	
	public float WattPerHourCost { get; private set;}
}

public enum ActionnerProperties 
{
	[ActionnerPropertiesAttr(1.05F)] RadiativeHeater = 0,
	[ActionnerPropertiesAttr(5.5F)] GazHeater = 1,
	[ActionnerPropertiesAttr(3.8F)] AirConditionner = 2
}

public class Actionner : MonoBehaviour {

	public float MinDeliveredEnergy{ get; set;}
	public float MaxDeliveredEnergy{ get; set;}

	public bool Prepared { get; set;}

	public ActionnerProperties m_properties;

	private void Start()
	{
		MinDeliveredEnergy = float.NaN;
		MaxDeliveredEnergy = float.NaN;
		Prepared = false;
	}	
	
	private static ActionnerPropertiesAttr GetAttr(ActionnerProperties _acProps)
	{
		return (ActionnerPropertiesAttr)Attribute.GetCustomAttribute(ForValue(_acProps), typeof(ActionnerPropertiesAttr));
	}
	
	private static MemberInfo ForValue(ActionnerProperties _acProps)
	{
		return typeof(ActionnerProperties).GetField(Enum.GetName(typeof(ActionnerProperties), _acProps));
	}

	public void ResetActionner()
	{
		MinDeliveredEnergy = float.NaN;
		MaxDeliveredEnergy = float.NaN;

		Prepared = false;
	}
}