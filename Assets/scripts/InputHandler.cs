using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour {
	private Vector3 origial_camera_pos=Vector3.zero;
	private int start_drag = 0; // start drag view on smal area
	private Vector3 last_mouse_pos;
    private bool moving = false; // moving view in any way
	private int moving_view = 0; // moving view slowly via other area
	public int view_moving_speed = 50; // (1~100) 100 is fastest
	private bool stop_propagate = false;

	private float holding_time = 0;
	private float movable_time = 0.5f; // if hold down mouse button longer than this time, enter moving model
	private Vector3 pos_on_button_down = Vector3.zero;

	private bool click_nowhere = true;
	private Vector3 last_terrain_pos = Vector3.zero; // terrain pos at center of camera
	private Vector3 camera_target = Vector3.zero;  // target position of terrain to center camera

	// Use this for initialization
	void Start () {
		//mCamera = Camera.main;

		origial_camera_pos = transform.position;
		getScreenCenterPoint ();

	}

	Vector3 getScreenCenterPoint(){
		RaycastHit hit = new RaycastHit ();
		Ray ray = Camera.main.ScreenPointToRay (new Vector2(Screen.width/2, Screen.height/2));
		if (CentralController.inst.terrain.GetComponent<Collider> ().Raycast (ray, out hit, Mathf.Infinity)) {
			//	Vector2 pos = cc.getPosFromCord (hit.point);
			last_terrain_pos = hit.point;
		}
		//print ("center_post:" + last_terrain_pos);
		return last_terrain_pos;
	}

	// Update is called once per frame
	void Update () {

		if (camera_target != Vector3.zero ) {
			getScreenCenterPoint ();
			//print ("last_terrain_pos:" + last_terrain_pos);
			//print ("target:" + camera_target);
			Vector3 offset = camera_target - last_terrain_pos ;
			//print ("offset:" + offset);

			if (Mathf.Abs(offset.x) > 1 || Mathf.Abs(offset.z) > 1){
				offset.y = 0;
				if (offset.x > 2 )
					offset.x = 2;
				if (offset.z > 2 )
					offset.z = 2;
				if (offset.x < -2 )
					offset.x = -2;
				if (offset.z < -2 )
					offset.z = -2;
				transform.position += offset;
				last_terrain_pos += offset;
			}else{
				camera_target = Vector3.zero;
			}
		}


		//print ("st:"+CentralController.inst.state);

		stop_propagate = false;
		click_nowhere = true;

		if (Input.GetMouseButtonDown (0))
			pos_on_button_down = Input.mousePosition;
		
		if (!moving && 0 == moving_view) { // not in moving
			if (CentralController.inst.state == 100) { // character selected
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

						//
						// try to move player's hero here
						//
						bool can_move = false;
						Character me = cc.currentSelectedChar.GetComponent<Character> ();
						Character enemy = check_enemy_on_pos (pos);
						print ("enemy is " + enemy);
						int kill_ret = 0;
						if (enemy != null) {
							kill_ret = kill (me, enemy);
							print ("kill_ret is " + kill_ret);

							if (kill_ret == 1) {
								//Destroy (enemy.gameObject);
								can_move = true;
							}
						} else {
							int r = validatePos (me, (int)(pos.x), (int)(pos.y));
							print ("validatePos is " + r);

							if (r != -2) // not in range
								click_nowhere = false;
							if (r == 0) { // valid
								can_move = true;
								print ("can move");


							} else {
								print ("can move2");

							}
						}
						if (can_move) {
							print ("ok move hero");
							cc._move_char (cc.currentSelectedChar.GetComponent<Character> (), pos);
							//cc.currentSelectedChar.transform.position = cc.getCordFromPos (pos);
							cc.state = 0;
							print ("hero moved");
							TerrainGrid tg = cc.getGlobalTerainGrid ();
							tg.inactiveAllCells ();
							stop_propagate = true;
							// fight if enemy is nearby in real-time strategy mode
							// check_fight (cc.currentSelectedChar.GetComponent<Character>(), pos);
						}
					}
				}
			} else {
				//print ("moving:" + moving);
				//print ("moveview:" + moving_view);

				if (handleSelectChar ()){
					click_nowhere = false;
				}
			}
		}
		if (!stop_propagate)
			handle_map_mover ();
		


		if (Input.GetMouseButtonUp (0)) {

			if (!stop_propagate && click_nowhere) {
				CentralController.inst.char_panel.SetActive (false);
				hideTerrainGrid ();
				CentralController.inst.state = 0;
			}
			pos_on_button_down = Vector3.zero;
		}
	}

	int kill(Character c1, Character c2){
		Destroy (c2.gameObject);
		return 1;
	}

	void start_fight (Character ch, Character enemy){
		print ("start fight!");
	}

	void check_fight2(Character character, Vector2 pos){
		print ("check_fight");
		TerrainGrid tg = CentralController.inst.getGlobalTerainGrid ();
		Character enemy = null;

		Vector2 _pos = new Vector2(pos.x, pos.y);

		_pos.x = pos.x + 1;
		enemy = tg.getCell (_pos).GetComponent<Cell>().character ;
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.x = pos.x - 1;
		enemy = tg.getCell (_pos).GetComponent<Cell>().character ;
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.y = pos.y + 1;
		enemy = tg.getCell (_pos).GetComponent<Cell>().character ;
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.y = pos.y - 1;
		enemy = tg.getCell (_pos).GetComponent<Cell>().character ;
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

	}

	/*Character check_enemy_on_pos(Vector2 pos){
		TerrainGrid tg = CentralController.inst.getGlobalTerainGrid ();
		Terrain terrain = CentralController.inst.terrain;
		Vector3 origin = CentralController.inst.getCordFromPos (pos);
		origin.y = 200;
		RaycastHit hitInfo;


		print ("test pos:" + pos + "on cord:"+origin);
		int layerMask =  LayerMask.GetMask("Char");
		print ("ray from:" + terrain.transform.TransformPoint (tg.transform.TransformPoint (origin)));
		Physics.Raycast(terrain.transform.TransformPoint(tg.transform.TransformPoint(origin)), Vector3.down, out hitInfo, Mathf.Infinity, layerMask);
		bool a = hitInfo.collider == null;
		print ("hitINfo:" + hitInfo.collider);
		if (hitInfo.collider) {
			print ("find enemy collider:" + hitInfo.collider);
			if (hitInfo.collider.gameObject.tag == "char") {

				print ("find enemy:" + hitInfo.transform.gameObject);
				return hitInfo.collider.gameObject.GetComponent<Character> ();
			}
		}
		return null;

	}*/


	Character check_enemy_on_pos(Vector2 pos){
		TerrainGrid tg = CentralController.inst.getGlobalTerainGrid ();
		GameObject o  = tg.getObjectOnCell ((int)pos.x, (int)pos.y, LayerMask.GetMask("Char"));
		if (o) {
			print ("find enemy collider:" + o);
			if (o.tag == "char") {

				//print ("find enemy:" + hitInfo.transform.gameObject);
				Character enemy = o.GetComponent<Character> ();
				if (enemy != null && enemy.getFaction().factionID != 0) 
					return enemy;
			}
		}
		return null;

	}
	void check_fight(Character character, Vector2 pos){
		print ("check_fight");
		Character enemy = null;

		Vector2 _pos = new Vector2(pos.x, pos.y);

		_pos.x = pos.x + 1;
		enemy = check_enemy_on_pos (_pos);
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.x = pos.x - 1;
		enemy = check_enemy_on_pos (_pos);
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.x = pos.x;
		_pos.y = pos.y + 1;
		enemy = check_enemy_on_pos (_pos);
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}

		_pos.y = pos.y - 1;
		enemy = check_enemy_on_pos (_pos);
		print ("enemy:" + enemy);
		if (enemy != null)
			print ("enemy faction:" + enemy.getFaction());
		if (enemy != null && enemy.getFaction().factionID != 0) {
			start_fight (character, enemy);
		}
	}
	bool handleSelectChar(){
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
					CentralController.inst.char_panel.SetActive(true);

					cc.state = 100;
					stop_propagate = true;


					// move camera center to this object
					camera_target  = hit.point;
					/*print ("hitpoint:" + hit.point);
					Vector3 offset = hit.point - last_terrain_pos;
					offset.y = 0;
					offset.x /= 4;
					offset.z /= 4;
					transform.position += offset;
					last_terrain_pos = last_terrain_pos + offset;*/

					return true;
				}
			}
		}
		return false;

	}

	void hideTerrainGrid(){
		CentralController cc = CentralController.inst;
		TerrainGrid tg = cc.getGlobalTerainGrid ();
		tg.inactiveAllCells ();

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
		//if (x < 0)
		//	x = 0;
		//if (x >= tg.gridWidth)
		//	x = tg.gridWidth-1;
		int start_y = (int)(pos.y - range);
		int y = start_y;
		//if (y < 0)
		//	y = 0;
		//if (y >= tg.gridHeight)
		//	y = tg.gridHeight-1;
		
		for (int k = 0; k < range * 2+1 ; k++) {
			for (int i = 0; i < range * 2+1 ; i++) {
				tg.activeCell (x, y);
				y += 1;
			}
			y = start_y;	
			x += 1;
		}
	}

	// 0: valid
	// -1: cell not valid
	// -2: not in range

	int validatePos(Character hero, int x, int y){
		CentralController cc = CentralController.inst;
		TerrainGrid tg = cc.getGlobalTerainGrid ();

		// check if it out of range
		Vector2 pos = cc.getPosFromCord (hero.gameObject.transform.position);
		int dist_x = (int)pos.x - x;
		int dist_y = (int)pos.y - y;
		//print ("dist:" + dist_x + "," + dist_y);
		int max = hero.getEffectMaxMoveDistance();
		if (dist_x > max || dist_x < -max
			|| dist_y > max || dist_y < -max)
			return -2;

		// check if is shown as a valid cell


		if (!tg.IsCellValid(x,y))
			return -1;


		//GameObject cell = tg.getCell (x, y);




		return 0;	
	}

	void handle_map_mover(){

		if (Input.GetMouseButton (0)) {
			holding_time += Time.deltaTime;
			Vector3 dis = Input.mousePosition - pos_on_button_down;
			//print ("dis:" + dis);
			if (holding_time > movable_time || dis.x > 10 || dis.y> 10 || dis.x < -10 || dis.y < -10) {
				if (moving_view == 0 && Input.mousePosition.x > 900 && Input.mousePosition.y > 400) {
					// fast move view in particular screen area
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
					// slow move camera
					if (moving_view == 0 && last_mouse_pos == Vector3.zero) {
						print ("start moving view");
						moving_view = 1;
						last_mouse_pos = Input.mousePosition;
					} else {
						Vector3 delta = Input.mousePosition - last_mouse_pos;

						Vector3 delta2 = new Vector3 (delta.x * view_moving_speed / 100, 0, delta.y * view_moving_speed / 100);
						delta2 = Quaternion.Euler (0, 45, 0) * delta2;

						//print ("dela:" + delta);
						transform.position -= delta2;
						last_mouse_pos = Input.mousePosition;
						moving = true;
					}
				}
			}
		}
		
		if (start_drag == 1 || moving || moving_view == 1 ) {
			if (Input.GetMouseButtonUp (0)) {
				print ("mouse up");
				if (start_drag == 1) {
					start_drag = 0;
					transform.position = origial_camera_pos;
				} else {
					getScreenCenterPoint ();
				}
				last_mouse_pos = Vector3.zero;
				moving = false;
				moving_view = 0;
				holding_time = 0;

			}
			stop_propagate = true;
		}
	}
}
