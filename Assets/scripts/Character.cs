using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NJG;


public class Character : MonoBehaviour {
	//主摄像机对象
	private Camera camera;
	//NPC模型高度
	float npcHeight;
	//红色血条贴图
	private Texture2D blood_red;
	//黑色血条贴图
	//private Texture2D blood_black;
	private Texture2D bloodslot;

	public string name="xxxxx";
	//public MBS.tbbPlayerInfo go; // gameobject
	public CharacterSetting setting;


	public int max_move_distance=2;
	private  int hp;

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
	public int	HP;

	/// <summary>
	/// Magic points. Depleted when you cast a spell
	/// </summary>
	public int	MP;

	/// <summary>
	/// Max health this player can have
	/// </summary>
	public int	MaxHP;

	/// <summary>
	/// Max magic points this player can have
	/// </summary>
	public int	MaxMP;

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

		Character ret = Character.loadCharacterPrefab(cs.name);
		ret.setting = cs;
		ret.name = cs.name;
		return ret;

	}

	public static Character loadCharacterPrefab(string name){
		print ("====>ready to load "+name);
		Character go = Instantiate (Resources.Load<Character>("Prefabs/Characters/"+name));
		print ("g " + go.GetType().ToString());
		print ("go " + go.name);
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
		go.gameObject.layer = LayerMask.NameToLayer("Char");
		go.tag = "char";
		go.gameObject.tag = "char";
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
		if (null == model)
		{
			Debug.LogError("No model specified for " + transform.name);
			return;
		}


		_model = (Transform)Instantiate(model);
		_model.parent = transform;
		_model.localPosition = Vector3.zero;
		_model.localRotation = Quaternion.identity;

		hp = setting.maxhp/2;

		//得到摄像机对象
		camera = Camera.main;

		//注解1
		//得到模型原始高度
		float size_y = gameObject.GetComponent<Collider>().bounds.size.y;
		//得到模型缩放比例
		float scal_y = transform.localScale.y;
		//它们的乘积就是高度
		npcHeight = (size_y *scal_y) ;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	void OnGUI(){
		//得到NPC头顶在3D世界中的坐标
		//默认NPC坐标点在脚底下，所以这里加上npcHeight它模型的高度即可
		Vector3 worldPosition = new Vector3 (transform.position.x , transform.position.y + npcHeight ,transform.position.z);
		//根据NPC头顶的3D坐标换算成它在2D屏幕中的坐标
		Vector2 position = camera.WorldToScreenPoint (worldPosition);
		//得到真实NPC头顶的2D坐标
		int padding =  30;
		position = new Vector2 (position.x, Screen.height - position.y  - padding);
		//print ("gui position:" + position);
		//注解2
		//计算出血条的宽高
		Vector2 bloodSize = GUI.skin.label.CalcSize (new GUIContent(blood_red));


		bloodSize.x = 100;
		bloodSize.y = 10;
		//通过血值计算红色血条显示区域
		//int blood_width = blood_red.width * hp/setting.maxhp;
		int blood_width = (int)(bloodSize.x * hp / setting.maxhp);

		//GUI.depth = 10;
			
		//先绘制黑色血条
		//GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2),position.y - bloodSize.y ,bloodSize.x,bloodSize.y),blood_black);
		GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2) - 2, position.y - bloodSize.y -2, bloodSize.x+4, bloodSize.y+4),bloodslot);

		//在绘制红色血条
		GUI.DrawTexture(new Rect(position.x - (bloodSize.x/2),position.y - bloodSize.y ,blood_width,bloodSize.y),blood_red);


		GUIStyle cc=new GUIStyle();
		cc.normal.background = null;    //这是设置背景填充的
		cc.normal.textColor=new Color(1,1,1);   //设置字体颜色的
		cc.fontSize = 20;       //当然，这是字体颜色
		GUI.skin.label.fontSize = 20;

		//注解3
		//计算NPC名称的宽高
		Vector2 nameSize = GUI.skin.label.CalcSize (new GUIContent(name));


		GUIStyle bb=new GUIStyle();
		bb.normal.background = null;    //这是设置背景填充的
		bb.normal.textColor=new Color(1,0,0);   //设置字体颜色的
		bb.fontSize = 40;       //当然，这是字体颜色

//		name= "魏延";
		//print ("nameSize:" + nameSize);
		//设置显示颜色为黄色
		GUI.color  = Color.white;
		//绘制NPC名称
		GUI.Label(new Rect(10, 10 , nameSize.x, nameSize.y), "我方回合", bb);



		GUI.Label(new Rect(position.x - (nameSize.x/2), position.y - nameSize.y - bloodSize.y  ,nameSize.x, nameSize.y), name, cc);
	}
}
