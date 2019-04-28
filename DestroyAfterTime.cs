using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{


	public GameObject effect;
	public float Time;

	// Initialisierung der Explosion
	void Start()
	{
		Invoke("Explode", 5);
	}
	void Explode()
	{
		Instantiate(effect, gameObject.transform.position, gameObject.transform.rotation);
		Destroy(gameObject);
	}
}
