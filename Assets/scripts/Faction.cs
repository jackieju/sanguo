using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
	//public Character[] characters; // prefabs


	public void loadCharacters(Character[] characters){

		//characters [0].transform.position = new Vector3 (10, 0, 5);
		MBS.tbbPlayerInfo c = characters [0].go;
		c.transform.position = new Vector3 (5, 0, 0);
		//c.anim.Play ("DamageFront");
		print ("created character "+c.character_name);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
