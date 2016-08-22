using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Spawner : MonoBehaviour {

	public static Spawner instance; //the instance of our class that will do the work

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}

	public Text[] unitLabel;
	public int[] unitDepolyCount; 
	public int [] depolyCondition;                 // (future) To be read in based on type of units 

	// Use this for initialization
	void Start () {
		int length = GameManager.instance.blockTypes.Length;

		unitLabel = new Text[length];
		unitDepolyCount = new int [length];
		depolyCondition = new int [length];

		depolymentConditionInit ();

		for (int i = 0; i < length; i++) {
			unitLabel [i] = GameObject.Find ("Canvas/Unit" + i + "Label").GetComponent<UnityEngine.UI.Text> ();
			unitLabel [i].text = "0/" + depolyCondition[i];
			unitDepolyCount [i] = 0;
		}
	}

	void depolymentConditionInit(){
		depolyCondition [0] = 3;
		depolyCondition [1] = 5;
		depolyCondition [2] = 5;
		depolyCondition [3] = 5;
		depolyCondition [4] = 8;
		depolyCondition [5] = 8;
	}

	void depolyUnit(string tag){
		GameObject unit = ObjectPoolerScript.instance.getPooledObjectWithType ((string)tag);
		unit.transform.position = transform.position;
		unit.gameObject.transform.FindChild ("PlayerHealthBar").gameObject.SetActive (true);
		unit.gameObject.SetActive(true);
	}


	// Update is called once per frame
	void Update () {
		int length = 6;

		for (int i = 0; i < length; i++) {
			if (unitDepolyCount [i] >= depolyCondition [i]) {
				unitDepolyCount [i] = unitDepolyCount [i] - depolyCondition [i];

				depolyUnit (GameManager.instance.blockTypes [i].tag);
			}
		}
	}
}
