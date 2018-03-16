using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnTimer : MonoBehaviour {
	public float timeout = 10;
	public float waittime = 3;
	public float[] times;
	public Text[] _texts;
	public Image[] images;
	public int faction_number=2;
	public int[] state;

	public int current = 0;
	// Use this for initialization
	void Start () {
		faction_number = images.Length;
		state = new int[faction_number];
		for (int i = 0; i < state.Length; i++) {
			state [i] = 0;
		}
		state [0] = 1;

		times = new float[faction_number];
		for (int i = 0; i < times.Length; i++) {
			times [i] = 0;
		}

	}

	// Update is called once per frame
	void Update () {
		float delta = Time.deltaTime;
		bool timeup = false;
		float time;
		if ( current < faction_number ) {
			times [current] += delta;
			time = times [current];

			//print ("time:" + time);
			if (time > timeout) {
				times [current] = 0;
				timeup = true;
			} 
			
			Image image = images [current];
			Text text = _texts [current];
			image.fillAmount = (float) time / timeout;
			Color c = image.color;
			c.r = 0.6f;
			c.g = 1;
			c.b = 0.8f;
			image.color = c;
			text.text = ((int)(timeout - time)).ToString ("0");
		}

		for (int i = 0; i < faction_number; i++) {
			if (state [i] == 0)
				continue;
			if (i == current && current < faction_number)
				continue;
			
			times [i] += delta;

			if (state [i] == 2) {
				if (times [i] > waittime) {
					times [i] = 0;
					state [i] = 1; // operating

				} else {
					Image image2 = images [i];
					Text text2 = _texts [i];
					image2.fillAmount = (float)times [i] / waittime;
					Color c2 = image2.color;
					c2.r = 1;
					c2.g =	0.8f;
					c2.b = 0.8f;
					image2.color = c2;
					//image.color.a = 0.5;
					text2.text = ((int)(waittime - times [i])).ToString ("0");

				}
			} else if (state [i] == 1) {
				if (times [i] > timeout) {
					times [i] = 0;
					state [i] = 2; // waiting

				} else {
					Image image2 = images [i];
					Text text2 = _texts [i];
					image2.fillAmount = (float)times [i] / timeout;
					Color c2 = image2.color;
					c2.r = 0.8f;
					c2.g =1;
					c2.b = 0.8f;

					image2.color = c2;
					//image.color.a = 0.5;
					text2.text = ((int)(timeout - times [i])).ToString ("0");

				}
			}
		}

		if (current < faction_number ) {
			if (timeup) {
				state [current] = 2; // waiting
				current += 1;
				if (current < faction_number)
					state [current] = 1;
			}
		}

	}
}
