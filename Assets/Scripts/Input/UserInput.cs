using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UserInput : MonoBehaviour {

	public float speed=0.1f;

	private GameObject dialogCanvas;
	private DialogController dialogCtrl;

	private float dialogInputDelay = 0.15f;
	private float dialogInputDecay = 0.0f;

	// Use this for initialization
	void Start () {
		dialogCanvas = GameObject.Find("DialogCanvas");
		dialogCtrl = dialogCanvas.GetComponent<DialogController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateDecays(){
		dialogInputDecay -= Time.deltaTime;
	}

	void FixedUpdate(){
		UpdateDecays();
		float hRaw = CrossPlatformInputManager.GetAxisRaw("Horizontal");
		float vRaw = CrossPlatformInputManager.GetAxisRaw("Vertical");
		if (Mathf.Abs (hRaw) > 0 || Mathf.Abs (vRaw) > 0) {
			float rInputMag = 1.0f / Mathf.Sqrt (hRaw * hRaw + vRaw * vRaw);
			Vector3 inputUnit = new Vector3 (hRaw * rInputMag, vRaw * rInputMag, 0);
			transform.Translate (speed*inputUnit);
		}
		if (dialogInputDecay <= 0.0f && Input.GetKey("c")){
			dialogInputDecay = dialogInputDelay;
            dialogCtrl.AlterActiveChoice(-1);
        }
        else if (dialogInputDecay <= 0.0f && Input.GetKey("f")){
        	dialogInputDecay = dialogInputDelay;
        	dialogCtrl.AlterActiveChoice(1);
        }
        
	}
}
