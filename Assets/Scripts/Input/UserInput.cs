using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UserInput : MonoBehaviour {

	public float speed=0.1f;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate(){
		float hRaw = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float vRaw = CrossPlatformInputManager.GetAxisRaw("Vertical");
		if (Mathf.Abs (hRaw) > 0 || Mathf.Abs (vRaw) > 0) {
			float rInputMag = 1.0f / Mathf.Sqrt (hRaw * hRaw + vRaw * vRaw);
			Vector3 inputUnit = new Vector3 (hRaw * rInputMag, vRaw * rInputMag, 0);
			transform.Translate (speed*inputUnit);
		}
	}
}
