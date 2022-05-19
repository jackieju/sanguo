using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NJG;
using UnityEngine.UI;


public class Character : MonoBehaviour {

	/* for gui draw, not used now */
	//主摄像机对象
	private Camera camera;
	//NPC模型高度
	float npcHeight;
	//红色血条贴图
	private Texture2D blood_red;
	//黑色血条贴图
	//private Texture2D blood_black;
	private Texture2D bloodslot;
	/********             ********/


	public Texture head_image;
	public string name="xxxxx";
	//public MBS.tbbPlayerInfo go; // gameobject
	public CharacterSetting setting;
	//private Canvas namebar;
	private GameObject namebar;
	public Material goMaterial;
	public float cellSize=1;

	public int max_move_distance=2;
	public int attack;
	public int knowledge;
	public int hp;
	public int maxhp;
	public int mp;
	public int maxmp;
	public Faction faction;



	public void destory(){
		Destroy (namebar);
		Destroy (this.gameObject);
	}
	public Faction getFaction ()
	{
		return faction;
	}

	public void setFaction(Faction f){
		faction = f;
		namebar.GetComponent<Text> ().color = faction.color;
	}
	public int getEffectMaxMoveDistance(){
		return max_move_distance;
	}


	/// <summary>
	/// The name of your character
	/// </summary>
	public string	character_name;

	/// <summary>
	/// The name of the run animation to play
	/// </summary>
	public string	run_animation_name = "Run";

	/// <summary>
	/// TThe name of the run backwards animation to play
	/// </summary>
	public string	run_back_animation_name = "RunBack";

	/// <summary>
	/// The name of the dying animation to play
	/// </summary>
	public string	die_animation_name = "Die";

	/// <summary>
	/// The name of the animation to play when taking daamage
	/// </summary>
	public string	damage_front_animation = "DamageFront";

	/// <summary>
	/// Not currently implmented, here I want to make the character vary their idle animation.
	/// This is where you list the names of the animations to be played
	/// </summary>
	public string[]
	idle_animation_names = new string[]{"Idle"};



	/// <summary>
	/// audio to play when taking damage
	/// </summary>
	public AudioClip	hurt_audio;

	/// <summary>
	/// Audio to play when dying
	/// </summary>
	public AudioClip	die_audio;

	/// <summary>
	/// The actual character model to spawn for this character
	/// </summary>
	public Transform 
	model;

	/// <summary>
	/// Not implemented in this version, this indicates wether a character in real-time mode is ready to attack or wether his cooldown timer is still in effect
	/// </summary>
	public bool 
	needs_recharge =  false;

	/// <summary>
	/// The image you will see during character select screens
	/// </summary>
	public Texture2D
	avatar;


	/// <summary>
	/// How long should the timer take to fill up
	/// </summary>
	public float	recharge_time = 5f;

	/// <summary>
	/// How fast should the timer be updated. Keep at 1 for real time or modify to apply boosts or curses
	/// </summary>
	public float	recharge_speed = 1f; 

	//How many tiles does this character require on the battle field?
	public Vector2
	tiles_required = new Vector2(1,1);


	/// <summary>
	/// Health points. Die when this reahes 0
	/// </summary>
	//public int	HP;

	/// <summary>
	/// Magic points. Depleted when you cast a spell
	/// </summary>
	//public int	MP;

	/// <summary>
	/// Max health this player can have
	/// </summary>
	//public int	MaxHP;

	/// <summary>
	/// Max magic points this player can have
	/// </summary>
	//public int	MaxMP;

	/// <summary>
	/// The player's level. In the basic damage system included with this kit, damage is multiplied by the player's level when attacking
	/// </summary>
	public int	Level = 1;


	//Future use...
	//		public tbbeAlignments[]
	//			alignments;

	//		public tbbeSkillsets[]
	//			skills;

	//		public tbbeModifiers[]
	//			modifiers;

	/// <summary>
	/// What action is this character going to perform? Melee, magic, flee, what?
	/// </summary>
	[System.NonSerialized]
	public int 
	selected_action;


	/// <summary>
	/// A reference to this character's animator component that plays the relevant animations on the character
	/// </summary>
	[System.NonSerialized] public Animator			controller;

	/// <summary>
	/// A reference to this character's legacy animations if present
	/// </summary>
	[System.NonSerialized] public Animation			anim;

	/// <summary>
	/// An internal reference to help locate the character in the battlefield array
	/// </summary>
	[System.NonSerialized] public Vector2			tile_index;


	static int __character_id = 0;
	int
	character_id;

	Transform
	_model;

	float 
	recharge_timer;

	/// <summary>
	/// Is this character currently facing away from the attacker
	/// </summary>
	[System.NonSerialized]
	public bool 
	facing_away = false;

	AudioSource _audio;





	public static Character load(CharacterSetting cs){
        print("===>load char from cs:" + cs);
		Character ret = Character.loadCharacterPrefab(cs);
		ret.setting = cs;
		ret.name = cs.name;
        print("===>char name:" + cs.name);
		ret.max_move_distance = cs.movability;
		ret.hp =  ret.maxhp = cs.maxhp;
		ret.mp = ret.maxmp = cs.maxmp;
		print("===>texture:"+"Textures/avatars/"+cs.head_image);
		ret.head_image = Resources.Load<Texture2D> ("Textures/avatars/"+cs.head_image);
		//print ("===>head_image:" + ret.head_image.GetType());
		return ret;

	}

	/** function used for loading 2d mesh character **/
	//
	public  Vector3 MeshVertex(int x, int z) {
		return new Vector3(x * cellSize, 2, z * cellSize);
	}

	public void UpdateMesh(int x, int z) {
		Mesh mesh = this.gameObject.GetComponent<MeshFilter> ().mesh;
		mesh.vertices = new Vector3[] {
			MeshVertex(x, z),
			MeshVertex(x, z + 1),
			MeshVertex(x + 1, z),
			MeshVertex(x + 1, z + 1),
		};
		mesh.RecalculateBounds (); // otherwise mesh will not be visible without in break mode
	}


	public static Character createGridCharGO(){
		GameObject go = new GameObject();

		go.AddComponent<Character> ();
		Character char1 = go.GetComponent<Character> ();
		char1.cellSize = CentralController.gridCellSize;

		Material goMaterial = (Material)Resources.Load<Material> ("Materials/char");

		go.name = "chargo";
		//go.transform.parent = CentralController.inst.getGlobalTerainGrid().transform;

	//	if (!globalGrid)
		//	go.transform.localPosition = gridOrigin;

		//go.transform.localPosition = Vector3.zero;
		go.AddComponent<MeshRenderer>();
		Mesh mesh = new Mesh();

		mesh.vertices = new Vector3[] { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero };
		mesh.triangles = new int[] { 0, 1, 2, 2, 1, 3 };
		mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		mesh.uv = new Vector2[] { new Vector2(1, 1), new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 0) };
		go.AddComponent<MeshFilter> ().mesh = mesh;
		char1.UpdateMesh (0, 0);
		go.AddComponent<MeshCollider>();

		MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();

		meshRenderer.material = goMaterial;






		return char1;

	}
	//
	/*****       end             ****/



	// create 3d Character Object from prefab
	public static Character createGO(string name, string prefab_name){

		print("load prefab Prefabs/Characters/CharacterPrefabs/"+prefab_name);
		Character go = Instantiate (Resources.Load<Character>("Prefabs/Characters/CharacterPrefabs/"+prefab_name));

		//Character go = Resources.Load<Character>("Prefabs/Characters/"+name);

		//GameObject go = Instantiate (Resources.Load<GameObject>("Prefabs/Characters/"+name));
		//go.AddComponent<Character> ();

		print ("g " + go.GetType().ToString());
		print ("go " + go.name);
		//print ("go1" + go.gameObject.gameObject.GetInstanceID());
		//print ("go11" + go.gameObject.gameObject.gameObject.GetInstanceID());
		//print ("go11" + go.gameObject.GetInstanceID());

		go.gameObject.name = name+go.gameObject.GetInstanceID();
		print ("go2 " + GameObject.Find("Ethan(Clone)"));
		/*GameObject g = GameObject.Find ("Ethan(Clone)");
		if (g != null)
			g.transform.localScale += new Vector3(9,9,9);*/
		//go.transform.position = new Vector3 (10, 10, 10);

		// attach script for mapitem
		//We need to fetch the Type
		//System.Type MyScriptType = System.Type.GetType ("MapItem" + ",Assembly-CSharp");
		//print ("myscripttype:" + MyScriptType.GetType().ToString());
		//Now that we have the Type we can use it to Add Component
		MapItem mi = go.gameObject.AddComponent<MapItem>();
		//MapItem mi = go.gameObject.GetComponent<MapItem>;
		mi.type = 1;

	
		BoxCollider boxCol = go.gameObject.GetComponent<BoxCollider> ();
		if (boxCol == null)
			boxCol = go.gameObject.AddComponent<BoxCollider> ();
		/*Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
		Renderer thisRenderer = go.GetComponent<Renderer>();
		bounds.Encapsulate(thisRenderer.bounds);
		//boxCol.offset = bounds.center - transform.position;
		boxCol.size = bounds.size;*/
		/*Bounds newbound = go.gameObject.GetComponent<MeshFilter>().mesh.bounds;
		boxCol.bounds.Encapsulate (newbound);
		boxCol.size = newbound.size;
		*/







		go.gameObject.AddComponent<CharacterController> ();
		go.gameObject.layer = LayerMask.NameToLayer("Char");
		go.tag = "char";
		go.gameObject.tag = "char";


		//
		// scale it
		//

		//		c.transform.GetChild(0).localScale += new Vector3 (5, 5, 5);

		//		c.gameObject.transform.GetChild(0).localScale += new Vector3 (5, 5, 5);
		/*foreach (Transform child in go.gameObject.transform) {
			print("=======>child "+ child.gameObject);
			child.transform.localScale += new Vector3(5, 2, 2);
		}
		foreach (Transform child in go.transform) {
			print("=======>child "+ child.gameObject);
			child.localScale += new Vector3(5, 2, 2);
		}*/
		//go.gameObject.gameObject.gameObject.transform.localScale += new Vector3 (5, 5, 5);

		//go.gameObject.gameObject.transform.localScale += new Vector3 (5, 5, 5);
		//		c.gameObject.GetComponent<GameObject>().transform.localScale += new Vector3 (5, 5, 5);
		//		c.GetComponent<GameObject>().transform.localScale += new Vector3 (5, 5, 5);
		//go.transform.localScale += new Vector3(2, 2, 2);
		//go.gameObject.transform.localScale  += new Vector3(1, 1, 1);
		//print("child count:"+go.gameObject.transform.childCount);
		//print("child count:"+go.transform.childCount);
		//go.transform.SetParent(CentralController.inst.terrain.transform);

		/* this is the right way, but now we dont scale it*/
		//go.gameObject.transform.localScale += new Vector3 (1, 1, 1);


		// set npcHeight

		float size_y = boxCol.bounds.size.y;
		//得到模型缩放比例
		//float scal_y = go.gameObject.transform.localScale.y;
		//它们的乘积就是高度
		go.setNPCHeight(size_y) ;
		//print ("npc scale:" + scal_y);
		//print ("npc size_y:" + size_y);
		print ("npc height:" + size_y );
		return go;
	}

	public static Character loadCharacterPrefab(CharacterSetting cs){
		string name = cs.name;
		string prefab_name = cs.prefab_name;

		print ("====>ready to load "+name); 

		// *** way1: create 2d char game object on a grid mesh
		//Character go = createGridCharGO ();

		// *** way2: create 3d character game object
		Character go = createGO(name, prefab_name);



		//
		// add name bar
		//
		Canvas can = CentralController.inst.canvas;
		GameObject newGO = new GameObject("Text_namebar");
		newGO.transform.SetParent(can.transform);
		//Vector2 v2 = go.getHeadPositionInScreen (Camera.main);
		//newGO.transform.position = new Vector3(v2.x, v2.y, 0);
		Text text = newGO.AddComponent<Text>();
		Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
		text.font = ArialFont;
		text.fontSize = 24;
		text.material = ArialFont.material;
		text.text = name;
		//text.color = Color.blue;
		//text.color = go.faction.color;
		text.alignment = TextAnchor.MiddleCenter;
		RectTransform rt = newGO.GetComponent<RectTransform> ();
		rt.sizeDelta =  new Vector2 (200, rt.sizeDelta.y);
		go.namebar = newGO;

		go.namebar.transform.localScale = new Vector3 (1, 1, 1);

		// make sure it's render under panel
		go.namebar.transform.SetSiblingIndex (0);



		/*go.namebar = Instantiate(Resources.Load<Canvas>("Prefabs/CharName"));
		print ("namebar " + go.namebar);

		//Text nametext = go.namebar.GetComponent<Text>();
		Text nametext = go.namebar.GetComponentInChildren<Text>();

		print ("nametext " + nametext);

		nametext.text = name; 
		*/
		//go.gameObject.AddComponent<Canvas> (go.namebar);
		//namebar.position = new Vector3(player.position.x, player.position.y * SomeYOffset, player.position.z);
		//namebar.transform.position = Camera.main.WorldToViewportPoint(go.transform.position) + new Vector3(0f, 0.05f, 0f); // Change the 0.05f value to some other value for desired height


		//go.AddComponent<Character> ();
		//return go.GetComponent<Character>();
		return go;


	}
	void Awake(){
		blood_red = Resources.Load<Texture2D> ("Textures/blood");
		bloodslot = Resources.Load<Texture2D> ("Textures/bloodslot");
	}

	/*public static Character create(CharacterSetting cs){

		Character ret;
		ret = new Character();
		ret.setting = cs;
		ret.name = cs.name;
		ret.loadPrefab(cs.name);
		return ret;
	
	}


	public MBS.tbbPlayerInfo loadPrefab(string name){

		go = Instantiate (Resources.Load<MBS.tbbPlayerInfo>("Prefabs/Characters/"+name));
		print ("g " + go.GetType().ToString());
		print ("go " + go.character_name);
	//	go.transform.position = new Vector3 (10, 10, 10);

		// attach script for mapitem
		//We need to fetch the Type
		//System.Type MyScriptType = System.Type.GetType ("MapItem" + ",Assembly-CSharp");
		//print ("myscripttype:" + MyScriptType.GetType().ToString());
		//Now that we have the Type we can use it to Add Component
		MapItem mi = go.gameObject.AddComponent<MapItem>();
		//MapItem mi = go.gameObject.GetComponent<MapItem>;
		mi.type = 1;
		go.gameObject.AddComponent<BoxCollider> ();
		go.gameObject.AddComponent<CharacterController> ();
		return go;
	}*/

// Use this for initialization
	void Start () {
		/*if (null == model)
		{
			Debug.LogError("No model specified for " + transform.name);
			return;
		}


		_model = (Transform)Instantiate(model);
		_model.parent = transform;
		_model.localPosition = Vector3.zero;
		_model.localRotation = Quaternion.identity;
*/
		print ("start of " + name);
		print ("setting = " + setting);
		hp = setting.maxhp/2;

		//得到摄像机对象
		camera = Camera.main;

		//initNPCHeight ();

	}

	void setNPCHeight(float h){
		npcHeight = h;
	}

	void initNPCHeight(){
		//注解1
		//得到模型原始高度
		float size_y = gameObject.GetComponent<Collider>().bounds.size.y;
		//得到模型缩放比例
		//float scal_y = transform.localScale.y;
		//它们的乘积就是高度
		npcHeight = (size_y ) ;
	}
	
	// Update is called once per frame
	void Update () {
		//Vector3 v3 = camera.WorldToViewportPoint(transform.position);
		RectTransform canvas = CentralController.inst.canvas.GetComponent<RectTransform>();
		Vector2 uiOffset = new Vector2((float)canvas.sizeDelta.x / 2f, (float)canvas.sizeDelta.y / 2f);

		//Vector2 position = camera.WorldToScreenPoint (transform.position);
		Vector3 pos = new Vector3(transform.position.x, transform.position.y + npcHeight + 0.3f, transform.position.z);
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(pos);
		Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvas.sizeDelta.x, ViewportPosition.y * canvas.sizeDelta.y );
		namebar.transform.localPosition = proportionalPosition - uiOffset;
		//namebar.transform.localPosition = new Vector3 (position.x, position.y, 0);
		//namebar.transform.localPosition = new Vector3 (v3.x, v3.y, 0);
	/*	
		//Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y + npcHeight ,transform.position.z);

		//namebar.transform.position = Camera.main.WorldToViewportPoint(transform.position) + new Vector3(0f, 1.05f, 0f); // Change the 0.05f value to some other value for desired height
		Text t = namebar.GetComponentInChildren<Text>();
		TextGenerator textGen = new TextGenerator();
		TextGenerationSettings generationSettings = t.GetGenerationSettings(t.rectTransform.rect.size); 
		float width = textGen.GetPreferredWidth(t.text, generationSettings);
		float height = textGen.GetPreferredHeight(t.text, generationSettings);
		print ("width:" + width);
		//namebar.transform.position = transform.position + new Vector3(0-width*namebar.transform.localScale.x/200, npcHeight+1.05f, 0); // Change the 0.05f value to some other value for desired height
		//namebar.transform.position = transform.position + new Vector3(0, npcHeight+1.05f, 0); // Change the 0.05f value to some other value for desired height

		Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y + npcHeight ,transform.position.z);
		Vector2 position = camera.WorldToScreenPoint (worldPosition);
		namebar.transform.position = worldPosition;
		namebar.transform.rotation = Camera.main.transform.rotation;*/
	}

	Vector2 getHeadPositionInScreen(Camera camera){
		//得到NPC头顶在3D世界中的坐标
		//默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
		Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y + npcHeight ,transform.position.z);
		//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
		Vector2 position = camera.WorldToScreenPoint (worldPosition);
		return position;
	}
	
	void drawCharInfo(){
		
		Vector2 position = getHeadPositionInScreen (camera);

		//得到真实NPC头顶的2D坐标
		int padding =  30;
		position = new Vector2 (position.x, Screen.height - position.y  - padding);
		//print ("gui position:" + position);


		//
		// draw blood bar
		//
		//注解2
		//计算出血条的宽高
		Vector2 bloodSize = GUI.skin.label.CalcSize (new GUIContent(blood_red));
		bloodSize.x = 100;
		bloodSize.y = 10;
		//通过血值计算红色血条显示区域
		//int blood_width = blood_red.width * hp/setting.maxhp;
		int blood_width = (int)(bloodSize.x * hp / setting.maxhp);

		//GUI.depth = 10;
		/*
		//先绘制黑色血条
		//GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2),position.y - bloodSize.y ,bloodSize.x,bloodSize.y),blood_black);
		GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2) - 2, position.y - bloodSize.y -2, bloodSize.x+4, bloodSize.y+4),bloodslot);

		//在绘制红色血条
		GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2),position.y - bloodSize.y ,blood_width,bloodSize.y),blood_red);

		*/

		//
		// draw text 
		//

		GUIStyle cc=new GUIStyle();
		cc.normal.background = null;    //这是设置背景填充的
		cc.normal.textColor=new Color(1,1,1);   //设置字体颜色的
		cc.fontSize = 20;       //当然，这是字体颜色
		GUI.skin.label.fontSize = 20;
		//Font myFont = (Font)Resources.Load("Fonts/Arial", typeof(Font));
		//cc.font = myFont;


		//注解3
		//计算NPC名称的宽高
		Vector2 nameSize = GUI.skin.label.CalcSize (new GUIContent(name));


		GUIStyle bb=new GUIStyle();
		bb.normal.background = null;    //这是设置背景填充的
		bb.normal.textColor=new Color(1,0,0);   //设置字体颜色的
		bb.fontSize = 40;       //当然，这是字体颜色
		//		name= "魏延";
		//print ("nameSize:" + nameSize);
		//设置显示颜色

		//print ("faction:" + faction);
		GUI.color = faction.color;
		//绘制NPC名称
		GUI.Label(new Rect(10, 10 , nameSize.x, nameSize.y), "我方回合", bb);

		GUI.Label(new Rect(position.x - (nameSize.x/2), position.y - nameSize.y - bloodSize.y  ,nameSize.x, nameSize.y), name, cc);
	}
	void OnGUI(){
		//drawCharInfo ();
	}
}
