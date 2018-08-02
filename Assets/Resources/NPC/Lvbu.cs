using System.Collections.Generic;


public class Lvbu : CharacterSetting {

	public override void setup(){
		name = "吕布";
		desc = "三国名将，武力超群\n被曹操打败斩首";

		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		prefab_name = "Troll";
		//prefab_name = "nv";
		//kill = new string[]{"Dongzhuo", "Dingyuan"};
		nations = new Dictionary<string, int> (){ 
			{"Lvbu", 0},

		};
		head_image="Avatar"+prefab_name;

	}


}
