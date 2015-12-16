using UnityEngine;
using System.Collections;

public class Actionner : MonoBehaviour {

	public float MinDeliveredEnergy{ get; set;}
	public float MaxDeliveredEnergy{ get; set;}

	public bool Prepared { get; set;}

	private void Start()
	{
		MinDeliveredEnergy = float.NaN;
		MaxDeliveredEnergy = float.NaN;
		Prepared = false;
	}
}