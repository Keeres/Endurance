using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;			   // Create one instance of GameManager, make sure only one exsits

	void Awake () {
		if(instance == null) {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else Destroy(this); // or gameObject
	}

	public static int nRow = 8;                                    // Number of rows in the game board
	public static int nCol = 7;                                    // Number of columns in the game board
	public ReadJson readJson;
	public GameObject tile_1;								   	   // Tile tpye 1
	public GameObject tile_2;									   // Tile tpye 2
	public Block[] blockTypes;                             		   // Number of types of different blocks used 
	public Block[,] blocks = new Block[nCol+1, nRow+1]; 	       // Stores the block currently on the gameboard
	public int [,] gameBoard = new int[nCol, nRow];        		   // Gameboard size
	public Vector3 screenMidPoint;

	void Start () {
		screenMidPoint = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width/2.0f, Screen.height/2.0f, 0.0f));

		parseJsonData ();
		initializeGameBoard ();
		initializeBlocks ();
		AllPossibleSwaps.instance.detectPossibleSwaps();
		TopUpBlocks.instance.topUp ();
	}

	// JSON
	void parseJsonData(){
		JsonData LevelLayout = readJson.getLevel ("Level_1");

		// JsonData [0,0] is top left, reorder so that [0,0] is at bottom left for game board
		for (int i = 0; i < nRow; i++) {
			for (int j = 0; j < nCol; j++) {
				gameBoard[j, i] = (int)LevelLayout["tiles"][nRow-1-i][j];
			}
		}
	}

	// INITIALIZE //
	// Alternates between two different blocks
	void initializeGameBoard(){
		// Every even row starts with tile_1 and alternates with tile_2
		// Every odd row starts with tile_2 and alternates with tile_1

		for (int col = 0; col<GameManager.nCol; col++) {
			for (int row = 0; row<GameManager.nRow; row++){
				if (gameBoard [col, row] == 1) {
					if (col % 2 == 0 && (col * nCol + row) % 2 == 0) {
						Instantiate (tile_1, new Vector3 (col, row, 1), Quaternion.identity);
					} else if (col % 2 == 0 && (col * nCol + row) % 2 != 0) {
						Instantiate (tile_2, new Vector3 (col, row, 1), Quaternion.identity);
					}
					if (col % 2 != 0 && (col * nCol + row) % 2 != 0) {
						Instantiate (tile_2, new Vector3 (col, row, 1), Quaternion.identity);
					} else if (col % 2 != 0 && (col * nCol + row) % 2 == 0) {
						Instantiate (tile_1, new Vector3 (col, row, 1), Quaternion.identity);
					}
				}
			}
		}
	}

	void initializeBlocks(){
		for (int row = 0; row < nRow; row++) {
			for (int col = 0; col < nCol; col++) {
				if (gameBoard [col , row] == 1) {
					spawnAt (col, row);
				}
			}
		}
	}
		
	// Spawn block at x,y //
	void spawnAt(int col, int row) {
		int index = Random.Range (0, blockTypes.Length);

		// If the block chosen to spawn makes a chain with the blocks to the left or down direction, choose a new block
		// Cares only about left and down direction becuase it is being built from right to left and bottom to top
		while ((findTag (col-1, row) == blockTypes[index].tag && findTag (col-2, row) == blockTypes [index].tag)
			|| (findTag (col, row - 1) == blockTypes[index].tag && findTag (col, row-2) == blockTypes [index].tag)) {

			index = Random.Range (0, blockTypes.Length);
		};
			
		blocks[col, row] = (Block)Instantiate (blockTypes [index], new Vector2 (col, row), Quaternion.identity);
	//	Debug.Log (blocks [col, row]);
	}
		
	// Finds the tag (type) of block at (x, y) location
	public string findTag(int col, int row){

		if (col >= 0 && row >= 0){
			if (blocks [col, row]) {
				return blocks [col, row].tag;
			}
		}
		return "no tag";
	}
}

