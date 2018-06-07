using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Huangzhong : CharacterSetting {

	public override void setup(){
		Debug.Log ("===>call setup Huangzhong");
		name = "黄忠";
		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		prefab_name = "Ethan";
	
	}

	// Use this for initialization
	void Start () {

	}
	// Update is called once per frame
	void Update () {
		
	}
}
