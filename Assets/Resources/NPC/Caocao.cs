using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caocao : CharacterSetting {

	public override void setup(){
		Debug.Log ("===>call setup Huangzhong");
		name = "曹操";
		desc = "魏武帝";
		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		//prefab_name = "Kyle";
		prefab_name = "bubing";
		kill = new string[]{"Lvbu", "Zhugeliang"};
		bekilled = new string[]{};
		nations = new Dictionary<string, int> (){ 
			{"Wei", 0}
		};
		head_image="Avatar"+prefab_name;
	}





}
