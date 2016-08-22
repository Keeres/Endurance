using UnityEngine;
using System.Collections;

public class SpriteAlpha : MonoBehaviour {
	public float alpha;
	//public GameObject gameobject;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Color temp = this.gameObject.GetComponent<SpriteRenderer>().color;
		temp.a = alpha;
		GetComponent<SpriteRenderer>().color = temp;

	}
}
