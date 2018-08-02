using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sunquan : CharacterSetting {

	public override void setup(){
		Debug.Log ("===>call setup Huangzhong");
		name = "孙权";
		desc = "";
		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		//prefab_name = "Kyle";
		prefab_name = "xuhuang";
		//kill = new string[]{"Lvbu"};
		bekilled = new string[]{};
		nations = new Dictionary<string, int> (){ 
			{"Wu", 0}
		};
		head_image="Avatar"+prefab_name;

	}



}
