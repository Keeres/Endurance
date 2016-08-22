using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TopUpBlocks : MonoBehaviour {

	public static TopUpBlocks instance; //the instance of our class that will do the work

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}

	// Randomly create a new cookie type. It can’t be equal to the type of the last new cookie, to prevent too many “freebie” matches.
	// Create the new Cookie object and add it to the array for this column.
	// if a column does not have any holes, you don’t add it to the final array.

	private List<int[]> columns = new List<int[]>();
	private int smoothing = 8;
	private float delayTime = 0;
	private int currentColumn = 0; 
	private float alpha = 0;
	private int arrayCount = 0;

	public void topUp(){
		columns.Clear ();
		int blockType = -1;
		// Loops through the column from top to bottom. This for loop ends when cookies[column, row] is not nil—that is, when it has found a cookie.
		for (int col = 0; col<GameManager.nCol; col ++ ) {
			for (int row = 0; row < GameManager.nRow; row++) {

				// Only need to fill up grid squares that have a tile.
				if (GameManager.instance.gameBoard [col, row] == 1 && GameManager.instance.blocks [col, row] == null) {		
					// 3
					int newBlockType;
					int[] array = new int[] { -1, -1, -1};

					// Randomly create a new cookie type. It can’t be equal to the type of the last new cookie, to prevent too many “freebie” matches.
					do {
						newBlockType = Random.Range (0, GameManager.instance.blockTypes.Length);
					} while (newBlockType == blockType);
					blockType = newBlockType;
				
					array [0] = col;
					array [1] = row;
					array [2] = blockType;

					columns.Add (array);
				}
			}
		}
		animateNewBlocks ();
	}

	void animateNewBlocks() {
		arrayCount = 0;
		delayTime = 0;
	//	var longestDuration: NSTimeInterval = 0
		foreach (int[] array in columns) {
			if (array [0] == currentColumn) {
				delayTime = delayTime + 0.25f;
			} else {
				delayTime = 0;
			}

			// The new block sprite should start out just above the first tile in this column. Since row starts with 0, nRow will be the above the top most row
			int startRow = GameManager.nRow;
			//Debug.Log(array[2]);
			// Assign block to the col, row it belongs, but the sprite is at top of the row 
			GameManager.instance.blocks[array[0], array[1]] = (Block)Instantiate (GameManager.instance.blockTypes [array[2]], new Vector2 (array[0], startRow), Quaternion.identity);
			GameManager.instance.blocks [array [0], array [1]].GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, 0);
			Block block = GameManager.instance.blocks [array [0], array [1]];
			Vector3 newPosition = new Vector3 (array [0], array [1], 0);
						
			instance.StartCoroutine(instance.fallingAndFadeIn(block, newPosition, delayTime));

			currentColumn = array [0];
		}
	}

	IEnumerator fallingAndFadeIn(Block block, Vector3 newPosition, float delay){
		yield return new WaitForSeconds (delay);
		alpha = 0;
		// transform each block to it's new position
		while (block.transform.position != newPosition) {
			alpha = alpha + 0.1f;
			block.GetComponent<SpriteRenderer>().color = new Color (1f, 1f, 1f, alpha);

			block.transform.position = Vector3.MoveTowards (block.transform.position, newPosition, (float)Time.deltaTime * smoothing);

			yield return null;
		}

		yield return null;

		arrayCount++;

		if (arrayCount == columns.Count) {
			Debug.Log ("finishing filling up");

			// find all possible swaps and detect chains after finished filling the gap
			AllPossibleSwaps.instance.detectPossibleSwaps ();
			Chain.instance.removeChains ();
		}
	}
}
