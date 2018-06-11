using System.Collections.Generic;


public class Zhugeliang : CharacterSetting {

	public override void setup(){
		name = "诸葛亮";
		desc = "";

		clonable = true;
		movability = 20;
		attack = 1;
		knowledge = 1;
		maxhp = 100;
		maxmp = 100;
		prefab_name = "RedSamurai";
		kill = new string[]{"Huangzhong"};
		nations = new Dictionary<string, int> (){ 
			{"Shu", 2}
		};
		head_image="AvatarSamurai";


	}


}
