using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;

public class Knight : Unit {

	public ReadJson readJson;
	Unit knight; 
	public bool attacking = true;
	// Use this for initialization
	new void Start () {
		base.Start ();

		parseJsonData ();
	}

	// Loads data for unit from json file
	// To be changed according to netcode
	void parseJsonData(){
		JsonData knightUnitData = readJson.getUnits ("Units");

		HP = (double)knightUnitData ["Knight"] ["Health"];
		Attk = (double)knightUnitData ["Knight"] ["Attack"];
		Def = (double)knightUnitData ["Knight"] ["Defense"];
		Spd = (double)knightUnitData ["Knight"] ["Speed"];
		AttkSpd = (double)knightUnitData ["Knight"] ["AttackSpeed"];
		AttkRange = (double)knightUnitData ["Knight"] ["AttackRange"];
		SensorRange = (double)knightUnitData ["Knight"] ["SensorRange"];
		FullHP = HP;

		speed = (float)Spd;

		animator.SetTrigger ("KnightRun");
	}

	// Update is called once per frame
 	override public void FixedUpdate ()
	{
		base.FixedUpdate ();
		base.nearestTarget ();
		base.movementCondition ();
		base.attackCondition ();
		base.updateHealthBar ();
	}
}