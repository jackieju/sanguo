using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	private Vector3 origial_camera_pos=Vector3.zero;
	private int start_drag = 0;
	private Vector3 last_mouse_pos;
	private bool moving = false;

	// Use this for initialization
	void Start () {
		//mCamera = Camera.main;

		origial_camera_pos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		print ("st:"+CentralController.inst.state);

		if (CentralController.inst.state == 100) {
			print ("mouse up222");
			if (Input.GetMouseButtonUp (0)) {
				CentralController cc = CentralController.inst;
				print ("mouse up111");
				print (Input.mousePosition);

				RaycastHit hit = new RaycastHit ();
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				/*if (Physics.Raycast (ray, out hit)) {
					print (hit.collider.gameObject);
					print (hit.transform.gameObject);
				}*/

				if (cc.terrain.GetComponent<Collider> ().Raycast (ray, out hit, Mathf.Infinity)) {
					Vector2 pos = cc.getPosFromCord (hit.point);
					print ("click cell " + pos);

					// move player's hero here
					if (validatePos (pos)) {
						cc.currentSelectedChar.transform.position = cc.getCordFromPos (pos);
						cc.state = 0;
						print ("hero moved");
						TerrainGrid tg = cc.getGlobalTerainGrid ();
						tg.inactiveAllCells ();
					}
				}
			}
		} else {
			handleSelectChar ();
		}


		handle_map_mover ();
	}

	void handleSelectChar(){
		if (Input.GetMouseButtonUp (0)) {
			CentralController cc = CentralController.inst;

			RaycastHit hit = new RaycastHit ();
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				print (hit.collider.gameObject);
				print (hit.transform.gameObject);
				if (hit.collider.gameObject.tag == "char") {
					
					print ("i am selected:" + hit.transform.gameObject);
					if (CentralController.inst.currentSelectedChar) {
						print ("last selectd:" + CentralController.inst.currentSelectedChar.name);
						//hideTerrainGrid (CentralController.inst.currentSelectedChar);
					}
					CentralController.inst.currentSelectedChar = hit.transform.gameObject;
					showTerrainGrid (CentralController.inst.currentSelectedChar);
					cc.state = 100;
				}
			}
		}

	}

	void showTerrainGrid(GameObject go){
		CentralController cc = CentralController.inst;
		TerrainGrid tg = cc.getGlobalTerainGrid ();
		tg.inactiveAllCells ();
		Vector2 pos = cc.getPosFromCord (go.transform.position);
		print ("ch pos:" + pos);
		int range = go.GetComponent<Character>().max_move_distance;
		int start_x = (int)(pos.x - range);
		int x = start_x;
		int start_y = (int)(pos.y + range);
		int y = start_y;

		for (int k = 0; k < range * 2 ; k++) {
			for (int i = 0; i < range * 2 ; i++) {
				tg.activeCell (x, y);
				y -= 1;
			}
			y = start_y;	
			x += 1;
		}
	}
	bool validatePos(Vector2 pos){
		return true;	
	}

	void handle_map_mover(){

		if (Input.GetMouseButton (0)) {
			//if (Input.mousePosition.x > 900 && Input.mousePosition.y > 400) {
			if (true){
								
				if (start_drag == 0 && last_mouse_pos == Vector3.zero) {
					print ("hello");
					start_drag = 1;
					last_mouse_pos = Input.mousePosition;
				} else {
					Vector3 delta = Input.mousePosition - last_mouse_pos;
					print ("dela:" + delta);
					transform.position += new Vector3 (delta.x, 0, delta.y);
					last_mouse_pos = Input.mousePosition;
					moving = true;
				}

			} else {

			}
		}
		if (start_drag == 1 && Input.GetMouseButtonUp (0)) {
			print ("mouse up");
			start_drag = 0;
			transform.position = origial_camera_pos;
			last_mouse_pos = Vector3.zero;
			moving = false;
		}
	}
}
