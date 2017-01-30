using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreWhenFar : MonoBehaviour {

	private Vector3 originalPosition;
	private Vector3 playerPosition;
	public float maxDistance = 2f;

	private void Start()
	{
		originalPosition = transform.position;
		playerPosition = GameObject.Find("Player").transform.position;
	}

	void Update()
	{
		float distance = Vector3.Distance(transform.position, playerPosition);
		if (distance>maxDistance) {
			GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
			transform.position = originalPosition;
		}
	}
}
