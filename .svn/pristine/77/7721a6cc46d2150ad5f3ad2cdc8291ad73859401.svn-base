using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gap : MonoBehaviour {
	public static Gap instance; //the instance of our class that will do the work

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}

	private static List<int[]> columns = new List<int[]>();
	private int smoothing = 8;
	private int arrayCount = 0;
	private float delayTime = 0;
	private int currentColumn = 0; 

	public void fillGaps(){
		findGaps();
		animateFallingBlocks ();
	}

	// Returns an array of columns. In each column is the blocks that needs to be moved to fill the holes
	void findGaps(){
		columns.Clear ();

		for (int col = 0; col<GameManager.nCol; col++){
			for (int row = 0; row < GameManager.nRow; row++) {
				
				// If a tile exists and no block is occupying it
				if (GameManager.instance.gameBoard [col, row] == 1 && GameManager.instance.blocks [col, row] == null) {		
					//Debug.Log (GameManager.blocks [column, row] + " " +column+ " " + row);

					for (int lookUp = row + 1; lookUp < GameManager.nRow; lookUp++) {
						if (GameManager.instance.blocks [col, lookUp] != null) {
							// assign current block with the one above it

							int [] array = new int[] {-1, -1};

							// set the current block as the block above it, and the one above it to null
							// this only modified the col and row of the blocks in the grid but does not actually transfrom the position of the blocks
							GameManager.instance.blocks [col, row] = GameManager.instance.blocks [col, lookUp];
							GameManager.instance.blocks [col, lookUp] = null;

							array [0] = col;
							array [1] = row;

							columns.Add(array);

							break;
						}
					}

				}
			}
		}
	}


	void animateFallingBlocks(){

		Debug.Log ("falling");
		arrayCount = 0;
		delayTime = 0;
		// Introduce a delay between each block falling in the same column to produce a more natural animation
		foreach (int[] array in columns) {
			if (array [0] == currentColumn) {
				delayTime = delayTime + 0.05f;
			} else {
				delayTime = 0;
			}

			// The col, row of the block is set, but still need to transform the block sprite to the right coordinate
			Block block = GameManager.instance.blocks [array [0], array [1]];
			Vector3 newPosition = new Vector3 (array [0], array [1], 0);
	//		Debug.Log (block.tag);
	//		Debug.Log (delayTime);
			instance.StartCoroutine (instance.animate (block, newPosition, delayTime));

			currentColumn = array [0];
		}

		instance.StartCoroutine (instance.buffer (delayTime));
	}

	IEnumerator animate(Block block, Vector3 newPosition, float delay){
		yield return new WaitForSeconds (delay);

		// transform each block to it's new position
		while (block.transform.position != newPosition) {
			block.transform.position = Vector3.MoveTowards (block.transform.position, newPosition, (float)Time.deltaTime * smoothing);

			yield return null;
		}
		yield return null;

		// counts to ensure all blocks has moved to the right position before continue
		arrayCount++;

		if (arrayCount == columns.Count) {
			Debug.Log ("finishing falling");
		}
	}

	IEnumerator buffer(float delay){
		//Debug.Log ("delay" + " " +delay);
		yield return new WaitForSeconds (delay);

		TopUpBlocks.instance.topUp();
	}
}
