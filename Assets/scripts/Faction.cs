using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction : MonoBehaviour {
	public FactionSetting fs;
	public Character[] characters;
	public Vector3 pos = new Vector3(30,0,0);
	public Vector3 rot = new Vector3(180,0,0);
	public int factionID = 0; // 0 ~ 3, 0 is player himself
	public Color color; // character name color
	public int remaining_operation_number = 1;
	public Faction(FactionSetting fs){
		this.fs = fs;
		remaining_operation_number = fs.max_operation_number;
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


	public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
	{
		
		Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
		Vector3 resultVec3 = center + point;
		print ("before rotate:" + position+  ",after:"+ resultVec3);
		return resultVec3;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles) {
		Vector3 dir = point - pivot; // get point direction relative to pivot
		dir = Quaternion.Euler(angles) * dir; // rotate it
		point = dir + pivot; // calculate rotated point
		return point; // return it
	}

	public Vector3 transformPosAsFaction(Vector3 t){
		CentralController cc = CentralController.inst;
		float ang = factionID * (360 / cc.FACTION_NUMBER);
		//print ("ang:" + ang);
		//Vector3 pivot = cc.getTerrainCenterPoint () + new Vector3 (0, 1, 0) - cc.getTerrainCenterPoint () ;
		//print ("axis:" + pivot);
		//return RotatePointAroundPivot (t, cc.getTerrainCenterPoint () + new Vector3 (0, 1, 0), new Vector3(0, ang, 0));
		return RotateRound(t, cc.getTerrainCenterPoint (), Vector3.up,  ang);
	}


	public void deployCharacters(Character[] characters){
		CentralController cc = CentralController.inst;
		Vector2 start_pos = new Vector2 (8, 8);
		Vector2 cur_pos = start_pos;
		int row_npc_number = 8;
		int gap = 2; // distance between npc
		cur_pos.x -= gap*row_npc_number/2;
		cur_pos.y += gap*row_npc_number/2;

		this.characters = characters;
		//Vector3 tc = CentralController.inst.getTerrainCenterPoint ();
		//print ("tc:" + tc);

		// put the npc on to the terrain
		int j = 0;
		for (int i = 0; i< characters.Length; i++){


			Character c = characters [i];
			c.setFaction(this);
			print ("current pos:" + cur_pos);

			Vector3 trans_pos = transformPosAsFaction ( CentralController.getCordFromPos(cur_pos)  );
			print ("current pos2:" + trans_pos);
			CentralController.inst._move_char (c, CentralController.getPosFromCord(trans_pos));
			//c.transform.position = cc.getCordFromPos((int)cur_pos.x, (int)cur_pos.y);
			print ("pos:" + c.transform.position);
			//transformAsFaction (c.transform);

			//c.PlayDieAnimation();
			print ("created character "+c.name);


			//c.anim.Play ("DamageFront");
			if (j >= row_npc_number) {
				cur_pos.x -= (row_npc_number-1)*gap;
				cur_pos.y += (row_npc_number+1)*gap;
				j = 0;
			} else {
				cur_pos.x += gap;
				cur_pos.y -= gap;
				j += 1;
			}

		}


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
