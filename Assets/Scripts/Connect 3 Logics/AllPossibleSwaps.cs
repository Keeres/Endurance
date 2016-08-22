using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllPossibleSwaps : MonoBehaviour {

	public static AllPossibleSwaps instance;			   // Create one instance of GameManager, make sure only one exsits

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}

	// Working from bottom up, swap with block to the right, checks for chain then swap back, do the same for up diresction. 
	public List<int[]> swaps = new List<int[]>();

	// Detect if chain exists based on given block
	bool detectPossibleChains(int col, int row, string tag){
		var hoizontalLength = 1;
		var verticalLength = 1;
	
		for (int i = col - 1; i >= 0 && GameManager.instance.findTag(i, row) == tag; --i, ++hoizontalLength){	}
		for (int i = col + 1; i >= 0 && GameManager.instance.findTag(i, row) == tag; ++i, ++hoizontalLength){}

		if (hoizontalLength >= 3) {
			return true;
		}

		for (int i = row - 1; i >= 0 && GameManager.instance.findTag(col, i) == tag; --i, ++verticalLength){	}
		for (int i = row + 1; i >= 0 && GameManager.instance.findTag(col, i) == tag; ++i, ++verticalLength){}

		if (verticalLength >= 3) {
			return true;
		}
		return false;
	}

	public void detectPossibleSwaps(){
		swaps.Clear ();

		// Loops through the gameboard
		for (int row = 0; row < GameManager.nRow; row++) {
			for (int col = 0; col < GameManager.nCol; col++) {
	
				// Check in Horizontal direction
				if (GameManager.instance.blocks [col, row] && GameManager.instance.blocks [col + 1, row]) {
					// swap in horizontal driection
					swapHorizontal (col, row);
			
					if (detectPossibleChains (col, row, GameManager.instance.blocks [col , row].tag) 
						|| detectPossibleChains (col + 1, row, GameManager.instance.blocks [col + 1, row].tag)) {
					
						// if chains detected, add the bock pair to the swap list
						int[] blockPair = new int [] { col, row, col + 1, row };
					//	Debug.Log ("horizontal " + blockPair[0] +" "+ blockPair[1]+" "+blockPair[2]+" "+blockPair[3]);

						swaps.Add (blockPair);
					}
					// Swap back
					swapHorizontal (col, row);
				}
		
				// Check in Vertical direction
				if (GameManager.instance.blocks [col, row] && GameManager.instance	.blocks [col, row + 1]) {

					// swap in verical driection
					swapVertical (col, row);

					if (detectPossibleChains (col, row, GameManager.instance.blocks [col , row].tag) 
						|| detectPossibleChains (col, row + 1, GameManager.instance.blocks [col, row + 1].tag)) {

						// if chains detected, add the bock pair to the swap list
						int[] blockPair = new int [] { col, row, col, row+1};
					//	Debug.Log ("vertical " + blockPair[0] +" "+ blockPair[1]+" "+blockPair[2]+" "+blockPair[3]);


						swaps.Add (blockPair);
					}
					// Swap back
					swapVertical (col, row);
				}
			}
		}
		Debug.Log ("finish detecting all swaps");
	}
		
	private void swapHorizontal(int col, int row){
		Block tempBlock = GameManager.instance.blocks [col, row];
		GameManager.instance.blocks [col, row] = GameManager.instance.blocks [col + 1, row];
		GameManager.instance.blocks [col + 1, row] = tempBlock;
	}

	private void swapVertical(int col, int row){
		Block tempBlock = GameManager.instance.blocks [col, row];
		GameManager.instance.blocks [col, row] = GameManager.instance.blocks [col, row + 1];
		GameManager.instance.blocks [col, row + 1] = tempBlock;
	}
}
