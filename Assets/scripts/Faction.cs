using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
	public Character[] characters;


	public void loadCharacters(Character[] characters){
		this.characters = characters;

		// put the npc on to the terrain, maybe do this later in another step
		MBS.tbbPlayerInfo c = characters [0].go;
		c.transform.position = new Vector3 (20, 0, 10);
		//c.PlayDieAnimation();
		print ("created character "+c.character_name);

		c = characters [1].go;
		c.transform.position = new Vector3 (30, 0, 10);
		//c.anim.Play ("DamageFront");


		print ("created character "+c.character_name);
	}

	public void getCharacterList(){
		
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
