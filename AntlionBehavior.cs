﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntlionBehavior : MonoBehaviour {
	private GameObject antlionCharacter;
	private GameObject player;
	private GameObject ground; // for referencing collissions with the ground
	private bool stalled = false;
	private Animator animator;
	private float countDownTimer = 0f;
	private bool gameOver = false;
	private bool start = false;
	private float digDepth;
	private bool digging = false;

	public float speedX = 5.0f;
	public float digSpeedX = 10.0f;
	public float digSpeedY = 10.0f;
	public float eatingTime = 2f;

	void Start () {
		antlionCharacter = transform.GetChild (0).gameObject;
		initializePositionAntlionCharacter ();
		player = GameObject.Find ("Player").GetComponent<Transform> ().gameObject;
		digDepth = player.GetComponent<PlayerController> ().getDigDepth ();

		ground = GameObject.Find ("Ground").GetComponent<Transform>().gameObject;
		initializeIgnoreColliders (); 
		animator = antlionCharacter.GetComponent<Animator> ();
		animator.speed = 1;
	}

	void Update () {
		if (start) {
			if (player.GetComponent<PlayerController> ().allAntsEaten ()) {
				gameOver = true;
			}

			Vector3 playerLastAnt = Vector3.zero;

			if (!stalled && !gameOver) {
				playerLastAnt = player.GetComponent<PlayerController> ().getLastAntPosition ();

				if (player.GetComponent<PlayerController> ().lastAntDigging ()) {
					if (digging == false) {
						digging = true;
						antlionCharacterignoreGroundColliders (true);
					}
					dig ();
				} 

				else {
					if(toSurface()){
						digging = false;
						antlionCharacterignoreGroundColliders (false); 
					}
				}

				if (playerLastAnt [0] > transform.position.x) {
					transform.Translate (new Vector3 (speedX * Time.deltaTime, 0f, 0f));
					moveAntlionCharacter ();
				}
			}

			if (countDownTimer > 0 && !gameOver) {
				countDownTimer = countDownTimer - Time.deltaTime;
			} 

			else {
				stalled = false;
			}
		}
	}

	private void moveAntlionCharacter() {
		float bufferSpace = 0.2f;
		if (antlionCharacter.transform.position.x <= transform.position.x - bufferSpace) {
			antlionCharacter.transform.Translate (new Vector3 (speedX * Time.deltaTime, 0f, 0f));
		}
		else if (antlionCharacter.transform.position.x >= transform.position.x + bufferSpace) {
			antlionCharacter.transform.Translate (new Vector3 (-speedX * Time.deltaTime, 0f, 0f));
		}
	}
		
	private bool toSurface() {
		//Debug.Log ("here");
		if (antlionCharacter.transform.position.y < transform.position.y) {
			antlionCharacter.transform.Translate (new Vector3 (0f, digSpeedY * Time.deltaTime, 0f));
			return false;
		} 
		return true;
	}

	private void dig() {
		float maxDepth = transform.position.y - digDepth;
		float moveY = 0f;
		float bufferSpace = 0.2f;

		if (antlionCharacter.transform.position.y > maxDepth + bufferSpace) {
			moveY = -digSpeedY * Time.deltaTime;
		}

		else if (antlionCharacter.transform.position.y < maxDepth - bufferSpace) {
			moveY = digSpeedY * Time.deltaTime;
		}

		else {
			moveY = 0;
		}

		antlionCharacter.transform.Translate (0f, moveY, 0f);
	}

	private void antlionCharacterignoreGroundColliders (bool ignore) {
		Collider2D[] groundColliders = ground.GetComponentsInChildren<Collider2D> ();

		for (int i = 0; i < groundColliders.Length; i++) {
			Physics2D.IgnoreCollision (antlionCharacter.GetComponent<Collider2D> (), groundColliders [i], ignore);
		}
		Rigidbody2D rb = antlionCharacter.GetComponent<Rigidbody2D> ();
		if (ignore) {
			rb.gravityScale = 0f;
			rb.freezeRotation = true;
		} 

		else {
			rb.gravityScale = 1f;
			rb.freezeRotation = false;
		}
	}

	private void initializeIgnoreColliders () {
		Collider2D coll = GetComponent<Collider2D> ();
		Collider2D collAntlion = antlionCharacter.GetComponent<Collider2D> ();
		Physics2D.IgnoreCollision (collAntlion, coll, true);

		GameObject[] allGameObjects = UnityEngine.Object.FindObjectsOfType<GameObject> ();
		for (int i = 0; i<  allGameObjects.Length; i++) {
			// disable collisions for parent colllider
			if (allGameObjects[i].activeInHierarchy) {
				Physics2D.IgnoreCollision (coll, allGameObjects[i].GetComponent<Collider2D> (), true);
			}
			// disable collision for antlion character collider
			if (allGameObjects[i].activeInHierarchy && (allGameObjects[i].name.Contains("Ant Hill") || !allGameObjects[i].name.Contains("Ant"))) {
				Physics2D.IgnoreCollision (collAntlion, allGameObjects[i].GetComponent<Collider2D>(), true);
			}
		}

		Collider2D[] groundColliders = ground.GetComponentsInChildren<Collider2D> ();
		for (int i = 0; i < groundColliders.Length; i++) {
			Physics2D.IgnoreCollision (coll, groundColliders [i], false);
			Physics2D.IgnoreCollision (collAntlion, groundColliders [i], false);
		}
	}

	public bool isGameOver() {
		return gameOver;
	}

	public void setEating(bool isEating) {
		stalled = isEating;
		countDownTimer = eatingTime;
	}

	public bool isEating() {
		return stalled;
	}

	public void enableAntlionCharacterCollider(GameObject thrownObject) {
		Collider2D antlionColl = antlionCharacter.GetComponent<Collider2D> ();
		Physics2D.IgnoreCollision (antlionColl, thrownObject.GetComponent<Collider2D>(), false);
	}

	private void initializePositionAntlionCharacter() {
		antlionCharacter.transform.position = transform.position;
	}

	public void startMovement() {
		start = true;
	}

	public void stopMovement() {
		start = false;
		stopAntAnimations ();
	}

	private void stopAntAnimations () {
		antlionCharacter.GetComponent<Animator> ().Stop ();
	}

	private void startAntAnimations () {
		//antlionCharacter.GetComponent<Animator> ().Play ("RUN");
	}
}
