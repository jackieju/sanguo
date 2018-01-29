using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentralController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			print ("mouse down");
			print (Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				print (hit.collider.gameObject);
				print (hit.transform.gameObject);
			}
		}
	}
}
