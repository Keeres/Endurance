using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Unit : MonoBehaviour {

	public double FullHP;
	public double HP;
	public double Attk;
	public double Def;
	public double Spd;
	public double AttkSpd;
	public double AttkRange;
	public double SensorRange;

	public float speed; 

	GameObject [] array;
	List <GameObject> targets;
	bool isAttacking = false;
	private static bool isEnemyDead = false;
	public Animator animator;
	GameObject healthBar;

	BoxCollider2D [] colliders;
	BoxCollider2D attacKCollider;

	// Use this for initialization
	public void Start () {
		animator = GetComponent<Animator> ();
	}

	public virtual void nearestTarget() {
		// find all target objects, if object is an enemy unit find all player unit, if object is a player unit find all enemy unit
		if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Enemy") {
			array = GameObject.FindGameObjectsWithTag ("Player");
		} else if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Player") {
			array = GameObject.FindGameObjectsWithTag ("Enemy");
		}
		targets = new List<GameObject> (array);

		// Find attack collider - attack collider is attatched to the weapons and follows the attack animations
		colliders = gameObject.GetComponentsInChildren <BoxCollider2D>();

		foreach (BoxCollider2D collider in colliders) {
			if (collider.tag == "AttackCollider") {
				attacKCollider = collider;
			}
		}

		// sort enemies by distance to the unit
		// the closest target is at index 0 after sorting
		targets = targets.OrderBy(
			x => Vector2.Distance(this.transform.position, x.transform.position)
		).ToList();
	}

	public virtual void movementCondition(){
		if (targets.Count != 0 && Vector3.Distance (targets [0].transform.position, transform.position) <= SensorRange && isAttacking == false) {
			// move towards the nearest enemy unit if it is within range
			if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Player") {
				speed = (float)Spd;
			//	transform.position = Vector3.MoveTowards (transform.position, new Vector3 (100.0f, transform.position.y, transform.position.z), speed);
				transform.position = new Vector3 (transform.position.x + speed, transform.position.y, transform.position.z);

			} else if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Enemy") {
				speed = (float)Spd;
			//	transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-1-0.0f,transform.position.y, transform.position.z), speed);
				transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);

			}


			//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (transform.position.x, targets [0].transform.position.y, transform.position.z), speed*0.5f);

		} else if (isAttacking == false) { 	
			// move in a straight line if no enemy is in range
			speed = (float)Spd;

			if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Player") {
				transform.position = new Vector3 (transform.position.x + speed, transform.position.y, transform.position.z);
				//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (20.0f, transform.position.y, transform.position.z), speed);
			} else if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Enemy") {
				transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);

				//transform.position = new Vector3 (transform.position.x + baseSpd * Spd, transform.position.y, transform.position.z);
				//transform.position = Vector3.MoveTowards (transform.position, new Vector3 (-20.0f, transform.position.y, transform.position.z), speed);
			}
		}
	}

	public virtual void attackCondition(){

		if (targets.Count != 0 && targets[0].transform.GetComponentInParent<Unit>().HP > 0.0f && isAttacking == false) {

			// if target is inside attack range, begin attacking
			if (Vector3.Distance (targets [0].transform.position, transform.position) <= AttkRange) {
				attackAnimation ();
				isAttacking = true;
			}
			if (Vector3.Distance (targets [0].transform.position, transform.position) > AttkRange) {
				// if target is outside attack range, stop attacking
				isAttacking = false;
			}
		}
	}

 	public virtual void updateHealthBar(){
		if (HP >= 0.0f) {
			// find the healthbar according to tag
			if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Enemy") {
				healthBar = gameObject.transform.FindChild ("EnemyHealthBar").gameObject;
			} else if (gameObject.GetComponentInChildren<Collider2D> ().tag == "Player") {
				healthBar = gameObject.transform.FindChild ("PlayerHealthBar").gameObject;
			}

			// adjust x scale according to health left
			if (healthBar) {
				float newXScale = (float)(HP / FullHP);
				healthBar.transform.localScale = new Vector3 (newXScale, 1.0f, 1.0f);
			}

			// if HP is less or equal to zero play dead animation
			if ((float)HP <= 0.0f) {
				deadAnimation ();
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider){

		// if colliding with a player or enemy
		if (collider.CompareTag("Enemy") || collider.CompareTag("Player" ) ) {

			GameObject temp = collider.gameObject;
			Debug.Log (collider.tag);
			// the closest target is at index 0 after sorting
			if (temp.GetInstanceID () == targets [0].GetInstanceID ()) {
				Unit targetUnit = targets [0].transform.GetComponentInParent<Unit> ();

				// disable collider to prevent hitting the target twice
				attacKCollider.enabled = false;

				// apply damage to target based on attacj power
				targetUnit.HP = targetUnit.HP - Attk;
				if (targetUnit.HP <= 0.0f) {
					isEnemyDead = true;

					// disables all enemy colliders
					BoxCollider2D [] enemyColliders = targetUnit.GetComponentsInChildren <BoxCollider2D>();

					foreach (BoxCollider2D enemyCollider in enemyColliders) {
						enemyCollider.enabled = false;
					}		
				}
			}
		}
	}

	// Update is called once per frame
	public virtual void FixedUpdate () {

	}

	// animation switches
	void runAnimation(){
		switch (transform.tag) {
		case "Knight":
			animator.SetTrigger ("KnightRun");
			break;
		}
	}

	void attackAnimation(){
		switch (transform.tag) {
		case "Knight":
			animator.SetTrigger ("KnightAttack");
			break;
		}
	}

	void deadAnimation(){
		switch (transform.tag) {
		case "Knight":
			animator.SetTrigger ("KnightDead");
			break;
		}
	}

	// executes at the end of attack animation
	void attackAnimationFunction(){
		if (isEnemyDead == true) {
			isAttacking = false;
			isEnemyDead = false;
			runAnimation ();
		}
		attacKCollider.enabled = isActiveAndEnabled;
	}

	// execites at the end of dead animation
	void clearAniamtion (){
		// disables the gameobject so it can be used again 
		gameObject.SetActive (false);
	}
}
