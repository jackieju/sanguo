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

	void OnMouseDown(){
		print ("i am selected");
		hideTerrainGrid (CentralController.inst.currentSelectedChar);
		CentralController.inst.currentSelectedChar = this.gameObject;
		showTerrainGrid (CentralController.inst.currentSelectedChar);
		CentralController.inst.state = 100;
	}

	void hideTerrainGrid(GameObject go){
		Destroy (gameObject.GetComponent<TerrainGrid> ());
	}

	void showTerrainGrid(GameObject go){
		print ("showTerrainGrid:" + gameObject);
		TerrainGrid tg = gameObject.AddComponent<TerrainGrid> ();
		//tg.terrain = CentralController.inst.terrain;
		//string path = "Materials/white";
		//tg.cellMaterialInvalid = (Material)Resources.Load<Material> (path);
		//path = "Materials/bark";
		//tg.cellMaterialValid = (Material)Resources.Load<Material> (path);
		//print ("material " + tg.cellMaterialInvalid);

	}
}
