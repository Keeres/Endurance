using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Chain : MonoBehaviour {

	public static Chain instance; //the instance of our class that will do the work

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}
		
	private int smoothing = 5;              // smooth out the aniamtion
	private int chainCount = 0;  			//number of blocks in the chain, including once that were counted twice
	private List<int[]> chains = new List<int[]>();

	void detectHorizontalChains(){
		
		// loop thorugh the entire gameboard to find chains longer than 3
		for (int row = 0; row < GameManager.nRow ; row++) {
			for (int col = 0; col < GameManager.nCol - 2 ; col++) {  //last two col cannot form a chain of 3
				if (GameManager.instance.findTag (col, row) != "no tag") {

					string chainType = GameManager.instance.blocks [col, row].tag;

					// Find in the horizontal direction if the next to blocks matches the current block
					if (GameManager.instance.findTag (col + 1, row) == chainType &&
					    GameManager.instance.findTag (col + 2, row) == chainType) {

						// save the 3 blocks that forms the chain, there may be blocks being accounted multiple times
						for (int i = 0; i < 3; i++) {
							int[] chain = new int [] { col + i, row };
							chains.Add (chain);
						}
					}
				}
			}
		}
	}
		
	void detectVerticalChains(){
	//	Debug.Log (GameManager.instance.blocks[1,1].tag);
		// loop thorugh the entire gameboard to find chains longer than 3
		for (int col = 0; col < GameManager.nCol; col++) {
			for (int row = 0; row < GameManager.nRow - 2 ; row++) {  //last two col cannot form a chain of 3
				if (GameManager.instance.findTag (col, row) != "no tag") {

					string chainType = GameManager.instance.blocks [col, row].tag;

					// Find in the vertical direction if the next to blocks matches the current block
					if (GameManager.instance.findTag (col, row + 1) == chainType &&
						GameManager.instance.findTag (col, row + 2) == chainType) {

						// save the 3 blocks that forms the chain, there may be blocks being accounted multiple times
						for (int i = 0; i < 3; i++) {
							int[] chain = new int [] { col, row + i };
							chains.Add (chain);
						}
					}
				}
			}
		}
	}

	public void removeChains(){
		chains.Clear();

		detectVerticalChains ();
		detectHorizontalChains ();
		animateChainedBlocks ();
		separateBlockTypes ();
	}

	public void animateChainedBlocks(){
		chainCount = 0;
		if (chains.Count == 0) {
			Debug.Log ("no chains detected");
		} else {
			foreach (int[] chain in chains) {
				Transform transform_1 = GameManager.instance.blocks [chain [0], chain [1]].transform;
				instance.StartCoroutine (instance.transformBlocks (transform_1));
			}
		}
	}
	

	// Problem with transform.scale too slow, change to animation later
	IEnumerator transformBlocks(Transform transform_1){
		Vector3 targetScale = new Vector3 (0.0f, 0.0f, 0.0f);

		// use Vector3.MoveTowards instead of Vector3.Lerp becuase ease scale was too slow
		while (transform_1.localScale != targetScale) {
			
			transform_1.localScale = Vector3.MoveTowards (transform_1.localScale, targetScale, (float)Time.deltaTime * smoothing);
	
			yield return null;
		}

		yield return null;

		chainCount++;

		if (chainCount == chains.Count) {
	//		Debug.Log ("scaling done");
			cleanUpChainedBlocks ();
		}
	}
		
	// destroys the blocks
	void cleanUpChainedBlocks(){
		Debug.Log ("start clean up");
		chainCount = 0;

		foreach (int[] chain in chains) {
			if (GameManager.instance.blocks [chain [0], chain [1]] != null) {
				instance.StartCoroutine (instance.destroyBlock (GameManager.instance.blocks [chain [0], chain [1]]));
			} else {
				chainCount++;
			}
		}
	}
		
	// Count the number of non repeating blocks removed
	// ex. the chains are in groups of three, in case of 4 in a row there will be 2 chains of three but only 4 blocks are not overlapped
	void separateBlockTypes(){
		int length = GameManager.instance.blockTypes.Length;

		List<int[]>[] blockTypes = new List<int[]>[length]; 
		for (int i = 0; i < length; i++) {
			blockTypes[i] = new List<int[]>();
		}

		foreach (int[] chain in chains) {
			for (int i = 0; i <length; i++) {
				if (GameManager.instance.blocks [chain [0], chain [1]].tag == GameManager.instance.blockTypes [i].tag) {
					blockTypes [i].Add (chain);
				}
			}
		}

		for (int i = 0; i < length; i++) {
			removeRepeatingBlocks (blockTypes[i], GameManager.instance.blockTypes[i].tag);
		}
	}

	// loop through all blocks, if repeating block is found, subtract it from the total number of blocks
	void removeRepeatingBlocks(List<int[]> blockType, string tag){
		int count = blockType.Count;
		int length = GameManager.instance.blockTypes.Length;
		if (blockType.Count != 0) {
		//	Debug.Log (tag);

			for (int i = 0; i < blockType.Count; i++) {
				for (int j = i; j < blockType.Count; j++) {
					if (i != j) {
						if (blockType [i] [0] == blockType [j] [0] && blockType [i] [1] == blockType [j] [1]) {
							count--;
						//	Debug.Log (blockType [i] [0] + " " + blockType [j] [0]);
						//	Debug.Log (blockType [i] [1] + " " + blockType [j] [1]);

						//	Debug.Log (count);
						}
					}
				}
			}
			Debug.Log ("number of " + tag +" blocks = " + count);
			for (int i = 0; i <length; i++) {
				if (tag == GameManager.instance.blockTypes [i].tag) {

					Spawner.instance.unitDepolyCount [i] += count;
					Spawner.instance.unitLabel[i].text = Spawner.instance.unitDepolyCount[i] + "/" +Spawner.instance.depolyCondition[i];
				}
			}
		}
	}

	IEnumerator destroyBlock(Block block){
		DestroyImmediate(block.gameObject);
		yield return null;

		chainCount++;

		if (chainCount == chains.Count) {
			Debug.Log ("all blocks destroyed");
			Gap.instance.fillGaps ();
		}
	}

}
