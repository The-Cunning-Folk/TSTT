using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportController : MonoBehaviour {

	private Camera dialogCam;
	private Camera gameCam;
	static float t = 0.0f;

	private float gameCamXSize = 0.5f;

	private float min = 0.5F;
	private float max =  0.5F;

	public float rate = 0.5f;

	private Transform target;


	void Start()
	{
		dialogCam = transform.GetChild(0).GetComponent<Camera>();
		gameCam = transform.GetChild(1).GetComponent<Camera>();
		target = GameObject.Find ("Character").transform;
	}

	void ChangeGameCamSize(float newSize){
		min = gameCamXSize;
		max = newSize;
		t = 0.0f;
	}

	void Update()
	{
		if (Input.GetKey ("space")) {
			ChangeGameCamSize (Random.Range (0.0f, 1.0f));
		}

		gameCamXSize = Mathf.Lerp (min, max, t);

		t += rate * Time.deltaTime;

		// setup the rectangle
		dialogCam.rect = new Rect (gameCamXSize, 0.0f, 1.0f - gameCamXSize, 1.0f);
		gameCam.rect = new Rect (0.0f, 0.0f, gameCamXSize, 1.0f);

		gameCam.transform.position = target.position - new Vector3(0.0f,0.0f,5.0f);

	}
}
