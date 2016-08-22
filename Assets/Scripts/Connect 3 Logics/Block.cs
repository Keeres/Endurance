﻿using UnityEngine;
using System.Collections;

public class Block: MonoBehaviour {

	public static int col;
	public static int row;

	//return true if the sprites of the two blocks are the same, return false if not the same
	public bool sameType(Block other) {
		return GetComponent<SpriteRenderer>().sprite == other.GetComponent<SpriteRenderer>().sprite;
	}

	// Update is called once per frame
	void Update () {

	}
}