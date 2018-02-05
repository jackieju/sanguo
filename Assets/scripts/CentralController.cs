using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class CentralController : MonoBehaviour {
	private Vector3 origial_camera_pos;
	private int start_drag = 0;
	private Vector3 last_mouse_pos;

	public Faction[] factions = new Faction[4];

	// Use this for initialization
	void Start () {
		origial_camera_pos = transform.position;
		for (int i = 0; i < factions.Length; i++) {
			factions [0] = new Faction ();

			// get characters
			Character[] characters;
			characters = new Character[1]; //should be set by caller
			characters[0] = Character.create("Ethan");
			characters[0] = Character.create("RedSamurai");

			factions [0].loadCharacters (characters);
		}
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
		if (Input.GetMouseButton (0)) {
			if (Input.mousePosition.x > 900 && Input.mousePosition.y > 400){
				print ("hello");				
				if (start_drag == 0){
					start_drag = 1;
					last_mouse_pos = Input.mousePosition;
				}
				else {
					Vector3 delta = Input.mousePosition - last_mouse_pos;
					print("dela:"+delta);
					transform.position += new Vector3(delta.x, 0, delta.y);
					last_mouse_pos = Input.mousePosition;
				}

			}
			else{
				
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			print ("mouse up");
			start_drag = 0;
			transform.position = origial_camera_pos;
		}
	}
}
