using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour
{

	// Initialisierung
	void Start()
	{

	}

	// Mit jedem Frame updaten
	void Update()
	{
		transform.RotateAround(Vector3.zero, Vector3.left, 1.1f * Time.deltaTime);
		transform.LookAt(Vector3.zero);
	}
}
