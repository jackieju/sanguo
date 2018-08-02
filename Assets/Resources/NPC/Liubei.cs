using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liubei : CharacterSetting {

	public override void setup(){
		Debug.Log ("===>call setup Huangzhong");
		name = "刘备";
		desc = "";
		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		//prefab_name = "Kyle";
		prefab_name = "bubing";
		kill = new string[]{"Lvbu"};
		bekilled = new string[]{};
		nations = new Dictionary<string, int> (){ 
			{"Shu", 0}
		};
		head_image="Avatar"+prefab_name;

	}



}

