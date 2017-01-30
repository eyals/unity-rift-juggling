using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grab : MonoBehaviour {

	private bool isGrabbing = false;
	private GameObject grabbedObject;
	public LayerMask grabLayer;
	public OVRInput.Controller controller;
	public string[] buttons;
	public float grabRadius = 0.1f;
	public AudioClip audioClip;

	// Update is called once per frame
	void Update () {
		bool buttonPressed = false;
		for (int i=0; i<buttons.Length; i++) {
			if(Input.GetAxis(buttons[i])==1) {
				buttonPressed = true;
				break;
			}
		}
		if (!isGrabbing && buttonPressed) GrabObject();
		if (isGrabbing && !buttonPressed) DropObject();

	}

	void GrabObject() {
		RaycastHit[] hits;
		hits = Physics.SphereCastAll(transform.position, grabRadius, transform.forward, 0f, grabLayer);
		if (hits.Length < 1) return;
		int closestObject = 0;
		for(int i=0; i<hits.Length; i++) {
			if (hits[i].distance < hits[closestObject].distance) closestObject = i;
		}
		grabbedObject = hits[closestObject].transform.gameObject;
		grabbedObject.GetComponent<Rigidbody>().isKinematic = true;
		grabbedObject.transform.position = transform.position;
		grabbedObject.transform.parent = transform;

		// HAPTIC FEEDBACK - NOT WORKING
		OVRHapticsClip haptic = new OVRHapticsClip(audioClip);
		OVRHaptics.RightChannel.Preempt(haptic);


		isGrabbing = true;
	}

	void DropObject() {
		if (grabbedObject == null) return;
		grabbedObject.transform.parent = null;
		grabbedObject.GetComponent<Rigidbody>().isKinematic = false;
		grabbedObject.GetComponent<Rigidbody>().velocity = OVRInput.GetLocalControllerVelocity(controller);
		grabbedObject = null;

		isGrabbing = false;
	}
}
