using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TurnTimer : MonoBehaviour {
	public float timeout = 10;
	public float time;
	public Text[] _texts;
	public Image[] images;
	public int faction_number=2;
	public bool _next = false;

	//public int current = 0;

	// Use this for initialization
	void Start () {
		faction_number = images.Length;
		for (int i = 0; i < _texts.Length; i++) {
			_texts [i].text = timeout.ToString("0");
		}
	}

	public void setNext(){
		_next = true;
	}

	// change turn to next player
	public void next(){
		CentralController cc = CentralController.inst;

		if (cc.current_operating_faction + 1 >= faction_number) {
			cc.current_operating_faction = 0;
		} else {
			cc.current_operating_faction += 1;
		}


	}

	// Update is called once per frame
	void Update () {
		
		CentralController cc = CentralController.inst;
		if (cc.gameworld_ready == false)
			return;



		float delta = Time.deltaTime;

		if (_next) {
			int current = cc.current_operating_faction;
			Image image = images [current];
			Text text = _texts [current];
			image.fillAmount = 1;
			Color c = image.color;
			c.r = 0.6f;
			c.g = 1;
			c.b = 0.8f;
			image.color = c;
			text.text = 0.ToString ("0");
			next ();
			_next = false;

		}


		if ( cc.current_operating_faction < faction_number ) {
			time += delta;

			Image image = images [cc.current_operating_faction];
			Text text = _texts [cc.current_operating_faction];
			image.fillAmount = (float) time / timeout;
			Color c = image.color;
			c.r = 0.6f;
			c.g = 1;
			c.b = 0.8f;
			image.color = c;
			text.text = ((int)(timeout - time)).ToString ("0");

			//print ("time:" + time);
			if (time > timeout) {
				time = 0;

				next ();

			} 


		}

	}
}
