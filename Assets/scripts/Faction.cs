using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
	public FactionSetting fs;
	public Character[] characters;
	public Vector3 pos = new Vector3(30,0,0);
	public Vector3 rot = new Vector3(180,0,0);
	public int factionID = 0; // 0 ~ 3, 0 is player himself

	public Faction(FactionSetting fs){
		this.fs = fs;
	}


	public void setPos(Vector3 position, Vector3 rotate){
		pos = position;
		rot = rotate;
	}

	public void transformAsFaction(Transform t){
		CentralController cc = CentralController.inst;
		float ang = 0;
		t.RotateAround (cc.getTerrainCenterPoint (), new Vector3 (0, 1, 0), factionID*(360/cc.FACTION_NUMBER));

	}


	public void deployCharacters(Character[] characters){
		this.characters = characters;
		//Vector3 tc = CentralController.inst.getTerrainCenterPoint ();
		//print ("tc:" + tc);

		// put the npc on to the terrain, maybe do this later in another step
		Character c = characters [0];
		c.transform.position = new Vector3 (20, 0, 10);
		transformAsFaction (c.transform);

		//c.PlayDieAnimation();
		print ("created character "+c.name);

		c = characters [1];
		c.transform.position = new Vector3 (20, 0, 20);
		transformAsFaction(c.transform);

		//c.anim.Play ("DamageFront");


		print ("created character "+c.name);
	}
	/*
	public void loadCharacters(Character[] characters){
		this.characters = characters;
		//Vector3 tc = CentralController.inst.getTerrainCenterPoint ();
		//print ("tc:" + tc);

		// put the npc on to the terrain, maybe do this later in another step
		MBS.tbbPlayerInfo c = characters [0].go;
		c.transform.position = new Vector3 (20, 0, 10);
		transformAsFaction (c.transform);

		//c.PlayDieAnimation();
		print ("created character "+c.character_name);

		c = characters [1].go;
		c.transform.position = new Vector3 (20, 0, 20);
		transformAsFaction(c.transform);

		//c.anim.Play ("DamageFront");


		print ("created character "+c.character_name);
	}
*/
	public void getCharacterList(){
		
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


}
