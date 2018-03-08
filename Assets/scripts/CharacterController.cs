using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

/*	void OnMouseUp(){
		print ("i am selected:"+this.gameObject.name);
		if (CentralController.inst.currentSelectedChar) {
			print ("last selectd:" + CentralController.inst.currentSelectedChar.name);
			//hideTerrainGrid (CentralController.inst.currentSelectedChar);
		}
		CentralController.inst.currentSelectedChar = this.gameObject;
		showTerrainGrid (CentralController.inst.currentSelectedChar);
		CentralController.inst.state = 100;
	}*/

	void showTerrainGrid(GameObject go){
		CentralController cc = CentralController.inst;
		TerrainGrid tg = cc.getGlobalTerainGrid ();
		tg.inactiveAllCells ();
		Vector2 pos = cc.getPosFromCord (go.transform.position);
		print ("ch pos:" + pos);
		int range = this.GetComponent<Character>().max_move_distance;
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
/*
	void hideTerrainGrid(GameObject go){
		print ("hide grids for :" + go.name);
		TerrainGrid tg = go.GetComponent<TerrainGrid> ();
		if (tg) {
			tg.DestroyGrids ();
			Destroy (tg);
		}
	}

	void showTerrainGrid(GameObject go){
		print ("showTerrainGrid:" + go);
		TerrainGrid tg = go.AddComponent<TerrainGrid> ();
		//tg.terrain = CentralController.inst.terrain;
		//string path = "Materials/white";
		//tg.cellMaterialInvalid = (Material)Resources.Load<Material> (path);
		//path = "Materials/bark";
		//tg.cellMaterialValid = (Material)Resources.Load<Material> (path);
		//print ("material " + tg.cellMaterialInvalid);

	}
*/
}
