using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmoothCamera2D : MonoBehaviour {

	public new Camera camera;
	public GameObject target;
	private float leftBoundary;
	private float rightBoundary;
	private float speed;
//	private float baseSpd = 0.01f;

	static private bool isMovingRight;
	static private bool isMovingLeft;

	static private List<GameObject> playerUnits = new List<GameObject>();
	static private List<GameObject> enemyUnits = new List<GameObject>();

	void Start(){

		GameObject scrollBackground = GameObject.FindGameObjectWithTag ("ScrollBackground");
		BoxCollider2D collider = scrollBackground.GetComponent<BoxCollider2D>();

		// find left and right edge of the camera
		float camDistance = Vector3.Distance(transform.position, camera.transform.position);
		Vector2 bottomLeftCorner = camera.ViewportToWorldPoint(new Vector3(0,0, camDistance));
		Vector2 topRightCorner = camera.ViewportToWorldPoint(new Vector3(1,1, camDistance));
				
		float cameraLeftEdge = bottomLeftCorner.x;
		float cameraRightEdge = topRightCorner.x;
	
		// set camera left and right boundary with respect to camera anchor
		leftBoundary = target.transform.position.x - cameraLeftEdge + collider.bounds.min.x;
		rightBoundary = collider.bounds.max.x - (cameraRightEdge - target.transform.position.x);
	}


	void OnTriggerEnter2D(Collider2D collision){

		// determine the number of enemy and player unit inside the trigger
		if (collision.CompareTag("Enemy")){
			enemyUnits.Add (collision.gameObject);
		}
		if (collision.CompareTag("Player")){
			playerUnits.Add (collision.gameObject);
		}
	}

	void OnTriggerExit2D(Collider2D collision){
		if (collision.CompareTag("Enemy")){
			enemyUnits.Remove (collision.gameObject);
		}
		if (collision.CompareTag("Player")){
			playerUnits.Remove (collision.gameObject);
		}	
	}

	void moveSpeedUpdate(){
		// if both enemy and player units are present do not move the camera
		// if either enemy or player units are present move at the speed of the fastest unit (to be added)
		if (enemyUnits.Count != 0 && playerUnits.Count !=0) {
			isMovingLeft = false;
			isMovingRight = false;

		}else if (enemyUnits.Count != 0 && playerUnits.Count == 0 && isMovingRight == false) {
			Unit enemyUnit = enemyUnits[0].transform.parent.GetComponent<Unit>();
			speed = (float) enemyUnit.speed;

			isMovingLeft = true;
			isMovingRight = false;

		}else if (enemyUnits.Count == 0 && playerUnits.Count !=0 && isMovingLeft == false) {
			Unit playerUnit = playerUnits[0].transform.parent.GetComponent<Unit>();
			speed = (float)playerUnit.speed;
			isMovingRight = true;
			isMovingLeft = false;
		}
	}

	//REMOVE INACTIVE GAMEOBJECTS from array
	void removeInactiveUnits(){
		for (int i = 0; i < enemyUnits.Count; i++) {
			if (!enemyUnits [i].active) {
				enemyUnits.Remove (enemyUnits [i]);
			}
		}
		for (int i = 0; i < playerUnits.Count; i++) {
			if (!playerUnits [i].active) {
				playerUnits.Remove (playerUnits [i]);
			}
		}
	}


	// Update is called once per frame
	void LateUpdate (){
		moveSpeedUpdate ();
		removeInactiveUnits ();
			
		if (isMovingRight == true ) {
		//	target.transform.position = Vector3.MoveTowards (target.transform.position, new Vector3 (100.0f, target.transform.position.y, target.transform.position.z), speed);
			target.transform.position = new Vector3 (target.transform.position.x + speed, target.transform.position.y, target.transform.position.z);

		} else if (isMovingLeft == true) {
		//	target.transform.position = Vector3.MoveTowards (target.transform.position, new Vector3 (-100.0f, target.transform.position.y, target.transform.position.z), speed);
			target.transform.position = new Vector3 (target.transform.position.x - speed, target.transform.position.y, target.transform.position.z);
		}
		Vector3 pos = target.transform.position;

		// Left boundary contraint
		if (pos.x < leftBoundary) {
			pos.x = leftBoundary;
			target.transform.position = new Vector3 (leftBoundary, target.transform.position.y, target.transform.position.z);
		}

		// Right boundary contraint
		if (pos.x > rightBoundary) {
			pos.x = rightBoundary;
			target.transform.position = new Vector3 (rightBoundary, target.transform.position.y, target.transform.position.z);
		}

		transform.position = new Vector3 (pos.x, transform.position.y, transform.position.z);
	}
}