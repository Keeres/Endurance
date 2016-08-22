using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyObjectPooler : MonoBehaviour {


	public static EnemyObjectPooler instance;
	public GameObject [] pooledObjectPrefabs;
	public int[] pooledAmount;
	public bool willGrow = true;
	public List <GameObject> [] pooledObjects;	

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {
		pooledObjects = new List<GameObject>[pooledObjectPrefabs.Length];

		int i = 0;
		foreach (GameObject pooledObjectPrefab in pooledObjectPrefabs){

			pooledObjects[i] = new List<GameObject>(); 

			for (int n = 0; n < pooledAmount[i]; n++) {
				GameObject obj = (GameObject) Instantiate(pooledObjectPrefab);
				obj.name = (string)pooledObjectPrefab.tag;
				//Debug.Log (obj.name);

				poolObject (obj);
			}
			i++;
		}
	}

	public void poolObject (GameObject obj){
		for ( int i=0; i<pooledObjectPrefabs.Length; i++){
			if(pooledObjectPrefabs[i].tag == obj.name){
				obj.gameObject.SetActive(false);
				pooledObjects[i].Add(obj);
				return;
			}
		}
	}

	public GameObject getPooledObjectWithType(string objectType){

		for(int i=0; i<pooledObjectPrefabs.Length; i++){
			GameObject prefab = pooledObjectPrefabs[i];
			//	Debug.Log (prefab.name);
			if (prefab.name == objectType) {

				for (int n = 0; n < pooledObjects[i].Count; n++) {
					if (!pooledObjects [i][n].gameObject.activeInHierarchy) {
						return pooledObjects [i][n];
					}
				}

				// if all pooled objects are in use, create new ones and add to pooledObjects
				if (willGrow) {
					GameObject obj = (GameObject)Instantiate (pooledObjectPrefabs [i]);
					pooledObjects [i].Add (obj);
					return obj;
				}
			}
		}
		return null;
	}
}