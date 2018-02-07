using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	private Vector3 origial_camera_pos;
	private int start_drag = 0;
	private Vector3 last_mouse_pos;


	// Use this for initialization
	void Start () {
		//mCamera = Camera.main;

		origial_camera_pos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		handle_map_mover ();
	}


	void handle_map_mover(){
		if (Input.GetMouseButtonDown (0)) {
			print ("mouse down");
			print (Input.mousePosition);

			RaycastHit hit = new RaycastHit ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				print (hit.collider.gameObject);
				print (hit.transform.gameObject);
			}
		}
		if (Input.GetMouseButton (0)) {
			if (Input.mousePosition.x > 900 && Input.mousePosition.y > 400) {
				print ("hello");				
				if (start_drag == 0) {
					start_drag = 1;
					last_mouse_pos = Input.mousePosition;
				} else {
					Vector3 delta = Input.mousePosition - last_mouse_pos;
					print ("dela:" + delta);
					transform.position += new Vector3 (delta.x, 0, delta.y);
					last_mouse_pos = Input.mousePosition;
				}

			} else {

			}
		}
		if (Input.GetMouseButtonUp (0)) {
			print ("mouse up");
			start_drag = 0;
			transform.position = origial_camera_pos;
		}
	}
}
