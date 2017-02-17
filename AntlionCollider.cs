﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionCollider : MonoBehaviour {
	private AntlionBehavior antlion;
	private Collider2D blockingObstacleColl;
	private GameObject player;
	void Start() {
		antlion = GameObject.Find("Antlion").GetComponent<AntlionBehavior>();
		player = GameObject.Find ("Player").GetComponent<Transform> ().gameObject;
		blockingObstacleColl = null;
	}

	private void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("here");
		bool gameOver = antlion.isGameOver (); 
		if (!antlion.isEating() && !gameOver && coll.gameObject.name.Contains ("Ant")) {
			if (coll.gameObject.name.Contains ("Fire Ant") && player.GetComponent<PlayerController> ().holdingThrowableObject ()) {
				player.GetComponent<PlayerController> ().destroyThrowableObject ();
				antlion.setEating (true);
				Debug.Log ("thrown object destroyed");
			}

			else {
				Debug.Log ("ant eaten");
				GameObject ant = player.GetComponent<PlayerController> ().lastAntEaten (coll.gameObject);
				Destroy (ant);
				antlion.setEating (true);
			}
		} 
	
		else if (!gameOver && coll.gameObject.name.Contains ("Throwable Obstacle")) {
			if (coll.gameObject.GetComponent<Rigidbody2D> ().velocity.x <= 0) {
				antlion.setEating (true);
			}
			Destroy (coll.gameObject);
		}
	}


	private void OnCollisionStay2D(Collision2D coll) {
		bool gameOver = antlion.isGameOver (); 
		if (!antlion.isEating() && !gameOver && coll.gameObject.name.Contains ("Ant")) {
			if (coll.gameObject.name.Contains ("Fire Ant") && player.GetComponent<PlayerController> ().holdingThrowableObject ()) {
				player.GetComponent<PlayerController> ().destroyThrowableObject ();
				antlion.setEating (true);
				Debug.Log ("thrown object destroyed");
			}

			else {
				Debug.Log ("ant eaten");
				GameObject ant = player.GetComponent<PlayerController> ().lastAntEaten (coll.gameObject);
				Destroy (ant);
				antlion.setEating (true);
			}
		} 
	}
}
