using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

	float timer;
	float time = 1;
	bool deploy = true;

	void Start(){
		//depolyUnit ("Knight");

	}

	void spawnTimer(){
		if (timer > time && deploy == true) {
			depolyUnit ("Knight");	
			deploy = false;
			Debug.Log ("deploy");
		} else {
			timer += Time.deltaTime;
		}
	}


	void depolyUnit(string tag){
		GameObject unit = EnemyObjectPooler.instance.getPooledObjectWithType ((string)tag);
		unit.transform.position = transform.position;
		Vector3 newScale = unit.gameObject.transform.localScale;
		newScale.x = -1;
		unit.gameObject.transform.localScale = newScale;
		unit.gameObject.SetActive(true);
		unit.gameObject.GetComponentInChildren<Collider2D>().tag= "Enemy";
		unit.gameObject.transform.FindChild ("EnemyHealthBar").gameObject.SetActive (true);
	}

	public void depolyRandomUnit(){
		int random = Random.Range (0, 3);
		string randomTag = GameManager.instance.blockTypes [random].tag;
		GameObject unit = EnemyObjectPooler.instance.getPooledObjectWithType ((string)randomTag);
		float randomYPos = Random.Range (-0.3f, 0.3f); // 2 decimal rounding


		unit.transform.position = new Vector3(transform.position.x, transform.position.y + randomYPos, transform.position.z);
		Vector3 newScale = unit.gameObject.transform.localScale;
		newScale.x = -1;
		unit.gameObject.transform.localScale = newScale;
		unit.gameObject.SetActive(true);
		unit.gameObject.GetComponentInChildren<Collider2D>().tag= "Enemy";
		unit.gameObject.transform.FindChild ("EnemyHealthBar").gameObject.SetActive (true);
	}

	
	// Update is called once per frame
	void Update () {
		spawnTimer ();
	}
}
