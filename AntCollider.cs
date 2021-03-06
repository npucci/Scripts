﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntCollider : MonoBehaviour {
	private PlayerController player;
	private ScoreControl scoreKeeper;
	private Collider2D blockingObstacleColl;

	void Start() {
		player = GameObject.Find("Player").GetComponent<PlayerController>();
		scoreKeeper = GameObject.Find("Level Manager").GetComponent<ScoreControl>();
		blockingObstacleColl = null;
	}
		
	private void OnCollisionEnter2D(Collision2D coll)
	{
		string collName = coll.gameObject.name;

		if (collName.Contains("Item")) {
			scoreKeeper.incrementScore (coll.gameObject.name);
			Destroy (coll.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D coll) {
		string collName = coll.gameObject.name;
		if (collName.Contains ("Rock Obstacle") || collName.Contains ("Throwable Obstacle")) {
			if (transform.position.y >= coll.gameObject.transform.position.y - 2f) {
				blockingObstacleColl = coll.gameObject.GetComponent<Collider2D> ();
			}
		} 

		else if (collName.Contains ("Tree Obstacle")) {
			blockingObstacleColl = coll.gameObject.GetComponent<Collider2D> ();
		}
	}

	private void OnCollisionExit2D(Collision2D coll) {
		string collName = coll.gameObject.name;

		if (collName.Contains("Tree Obstacle") || collName.Contains("Rock Obstacle") || collName.Contains("Throwable Obstacle")) {
			blockingObstacleColl = null;
		}

		else if (blockingObstacleColl != null && blockingObstacleColl.Equals(coll)) {
			blockingObstacleColl = null;
		}
	}

	public bool isStuck() {
		return blockingObstacleColl != null;
	}
		
	public Collider2D getBlocingObstacleColl() {
		return blockingObstacleColl;
	}

	public void receiveBlocingObstacleColl(Collider2D coll) {
		blockingObstacleColl = coll;
	}

	public void clearBlockingObstacleColl() {
		blockingObstacleColl = null;
	}
}