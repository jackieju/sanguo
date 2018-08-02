using System.Collections.Generic;


public class Zhaoyun : CharacterSetting {

	public override void setup(){
		name = "赵云";
		desc = "";

		clonable = true;
		movability = 20;
		attack = 10;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		//prefab_name = "RedSamurai";
		prefab_name = "bubing";

		nations = new Dictionary<string, int> (){ 
			{"Shu", 3}
		};
		head_image="Avatar"+prefab_name;

	}


}
