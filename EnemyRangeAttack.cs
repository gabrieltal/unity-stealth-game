/***************************************************************
* file: EnemyRangeAttack.cs
* author: Gabriel Talavera
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Gives enemy a range attack. Only fires if player is in sight
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangeAttack : MonoBehaviour {
	public float damage;
	public float bulletVelocity = 100;
	GameObject player;
	PlayerHealth playerHP;
	private bool playerInRange = false;
	public int visionDistance = 20;
	//Lower the widthOfVision the wider the field of view
	//Same with heightOfVision
	//The larger the number the narrower the field of view
	public float widthOfVision = 15;
	public float heightOfVision = 15;
	private float nextDamage = 3.0f;
	public float fireRate = 3.0f;
	RaycastHit hit;
	public Rigidbody projectile;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHP = player.GetComponent<PlayerHealth>();
	}

	//Attack method
	//Shoots a projectile at the player that self-destructs after a short period
	void Attack() {
		if (nextDamage <= Time.time) {
			playerHP.addDamage (damage);
			nextDamage = Time.time + fireRate;
	
			var bullet = Instantiate (projectile, transform.position, transform.rotation);
			bullet.velocity = transform.TransformDirection (Vector3.forward * bulletVelocity);
			Destroy (bullet.gameObject, 2);
		}
	}

	// Update is called once per frame
	void Update () {
		//If player is in range then attacks
		//Checks if Player is in range, if so turns red and attacks, else vision stays blue 
		if (playerInRange) {
			/*
			Debug.DrawRay (transform.position, transform.forward * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward + (transform.up / heightOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward - (transform.up / heightOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward + (transform.right / widthOfVision)).normalized * visionDistance, Color.red);
			Debug.DrawRay (transform.position, (transform.forward - (transform.right / widthOfVision)).normalized * visionDistance, Color.red);
		*/
			Attack ();
		} else {
			//Else player isn't in range and sets the bool to false
			playerInRange = false;
			/*
			Debug.DrawRay (transform.position, transform.forward * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward + (transform.up / heightOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward - (transform.up / heightOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward + (transform.right / widthOfVision)).normalized * visionDistance, Color.blue);
			Debug.DrawRay (transform.position, (transform.forward - (transform.right / widthOfVision)).normalized * visionDistance, Color.blue);
*/
		}
	
		
		//Uses ray casts to determine if player is in enemy's line of sight
		if ( (Physics.Raycast (transform.position, transform.forward, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.up/heightOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.up/heightOfVision)).normalized, out hit, visionDistance))) {
			if (hit.collider.tag == "Player") {
				playerInRange = true;
			}
		}
		else {
			playerInRange = false;
		}
	}

}
