using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Huangzhong : CharacterSetting {

	public override void setup(){
		Debug.Log ("===>call setup Huangzhong");
		name = "黄忠";
		desc = "";

		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		//prefab_name = "Ethan";
		prefab_name = "z1";
		kill = new string[]{};
		bekilled = new string[]{};
		nations = new Dictionary<string, int> (){ 
			{"Shu", 3}
		};
		head_image="Avatar"+prefab_name;

	}


}
