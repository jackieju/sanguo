using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	private Vector3 origial_camera_pos=Vector3.zero;
	private int start_drag = 0; // start drag view on smal area
	private Vector3 last_mouse_pos;
      	private bool moving = false; // moving view in any way
	private int moving_view = 0; // moving view slowly via other area
	public int view_moving_speed = 10; // (1~100) 100 is fastest
	private bool stop_propagate = false;

	private float holding_time = 0;
	private float movable_time = 0.5f; // if hold down mouse button longer than this time, enter moving model
	private Vector3 pos_on_button_down = Vector3.zero;

	// Use this for initialization
	void Start () {
		//mCamera = Camera.main;

		origial_camera_pos = transform.position;

	}
	
	// Update is called once per frame
	void Update () {
		//print ("st:"+CentralController.inst.state);

		stop_propagate = false;

		if (Input.GetMouseButtonDown (0))
			pos_on_button_down = Input.mousePosition;
		
		if (!moving && 0 == moving_view) {
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
						if (validatePos (cc.currentSelectedChar.GetComponent<Character>(), (int)(pos.x), (int)(pos.y) )) {
							cc.currentSelectedChar.transform.position = cc.getCordFromPos (pos);
							cc.state = 0;
							print ("hero moved");
							TerrainGrid tg = cc.getGlobalTerainGrid ();
							tg.inactiveAllCells ();
							stop_propagate = true;
						}
					}
				}
			} else {
				print ("moving:" + moving);
				print ("moveview:" + moving_view);

				handleSelectChar ();
			}
		}
		if (!stop_propagate)
			handle_map_mover ();

		if (Input.GetMouseButtonUp (0))
			pos_on_button_down = Vector3.zero;
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
					stop_propagate = true;
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
		Character hero = go.GetComponent<Character> ();
		int range = hero.getEffectMaxMoveDistance();
		int start_x = (int)(pos.x - range);
		int x = start_x;
		int start_y = (int)(pos.y - range);
		int y = start_y;

		for (int k = 0; k < range * 2+1 ; k++) {
			for (int i = 0; i < range * 2+1 ; i++) {
				tg.activeCell (x, y);
				y += 1;
			}
			y = start_y;	
			x += 1;
		}
	}

	bool validatePos(Character hero, int x, int y){
		
		// check if occupied
		CentralController cc = CentralController.inst;
		TerrainGrid tg = cc.getGlobalTerainGrid ();

		if (!tg.IsCellValid(x,y))
			return false;


		GameObject cell = tg.getCell (x, y);



		// check if it out of range
		Vector2 pos = cc.getPosFromCord (hero.gameObject.transform.position);
		int dist_x = (int)pos.x - x;
		int dist_y = (int)pos.y - y;
		print ("dist:" + dist_x + "," + dist_y);
		int max = hero.getEffectMaxMoveDistance();
		if (dist_x > max || dist_x < -max
			|| dist_y > max || dist_y < -max)
			return false;
		return true;	
	}

	void handle_map_mover(){

		if (Input.GetMouseButton (0)) {
			holding_time += Time.deltaTime;
			Vector3 dis = Input.mousePosition - pos_on_button_down;
			print ("dis:" + dis);
			if (holding_time > movable_time || dis.x > 10 || dis.y> 10 || dis.x < -10 || dis.y < -10) {
				if (moving_view == 0 && Input.mousePosition.x > 900 && Input.mousePosition.y > 400) {
								
					if (start_drag == 0 && last_mouse_pos == Vector3.zero) {
						print ("hello");
						start_drag = 1;
						last_mouse_pos = Input.mousePosition;
					} else {
						Vector3 delta = Input.mousePosition - last_mouse_pos;
						//print ("dela:" + delta);
						transform.position += new Vector3 (delta.x, 0, delta.y);
						last_mouse_pos = Input.mousePosition;
						moving = true;
					}

				} else if (start_drag == 0){
					if (moving_view == 0 && last_mouse_pos == Vector3.zero) {
						print ("start moving view");
						moving_view = 1;
						last_mouse_pos = Input.mousePosition;
					} else {
						Vector3 delta = Input.mousePosition - last_mouse_pos;
						//print ("dela:" + delta);
						transform.position -= new Vector3 (delta.x * view_moving_speed / 1000, 0, delta.y * view_moving_speed / 1000);
						last_mouse_pos = Input.mousePosition;
						moving = true;
					}
				}
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			print ("mouse up");
			if (start_drag == 1) {
				start_drag = 0;
				transform.position = origial_camera_pos;
			}
			last_mouse_pos = Vector3.zero;
			moving = false;
			moving_view = 0;
			holding_time = 0;
		}
	}
}
