using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportController : MonoBehaviour {

	private Camera dialogCam;
	private Camera gameCam;
	static float t = 0.0f;

	private float gameCamXSize = 0.5f;

	private float min;
	private float max;

	[SerializeField]
	private float rate = 0.5f;

	[SerializeField]
	private float startSize = 0.618f;

	private Transform target;


	void Start()
	{
		dialogCam = transform.GetChild(0).GetComponent<Camera>();
		gameCam = transform.GetChild(1).GetComponent<Camera>();
		target = GameObject.Find ("Character").transform;
		min = startSize;
		max = startSize;
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
