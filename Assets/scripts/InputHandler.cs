using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {

	bool touching = false;
	Vector2 lastPos;
	Camera mCamera;

	// Use this for initialization
	void Start () {
		mCamera = Camera.main;
		print("touch handling");
	}
	
	// Update is called once per frame
	void Update () {
		print("update");

		handleTouch();
	}

	void handleTouch(){
		print ("handleTouch:"+Input.touchCount);
		Touch t;
		if(!touching && Input.touchCount >= 1) // pressed
		{
			print("touch down");
			touching = true;
			t = Input.GetTouch(0);
			lastPos = t.position;
			//        yourObject = Instantiate(someObjectPrefab, lastPos, Quaternion.Identity)
			Vector3 tp_w1 = mCamera.ScreenToWorldPoint(t.position);

		}
		else if(touching && Input.touchCount >= 1) // finger down
		{
			t = Input.GetTouch(0);
			lastPos = t.position;
			//        yourObject.transform.Position = Camera.main.ViewportToWorldPoint(lastPos);
			//trackTouch(t);
		}
		else if(touching && Input.touchCount == 0) // released
		{
			//        Vector3 direction = Input.touches[0].position - lastPos;
			//        yourObject.Rigidbody.velocity = direction.normalized*someSpeedValue;



			touching = false;
			print("touch released");

		}
	}
}
