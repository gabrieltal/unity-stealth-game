/***************************************************************
* file: Patrol.cs
* author: Gabriel Talavera
* class: CS 470.01 - Game Development
*
* assignment: Quarter Project
* date last modified: 5/29/2017
*
* purpose: Allows enemy to follow a patrol path and to follow the player if they are spotted
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {
	//Transform array sets up points for the agent to navigate to
	public Transform[] points;
	RaycastHit hit;
	//destination sets up next destination point
	private int destination = 0;
	private UnityEngine.AI.NavMeshAgent agent;
	public float visionDistance = 20;
	private bool playerSpotted = false;
	public int widthOfVision = 20;
	private float curTime = 0;
	public float pauseDuration = 4;
	public int heightOfVision = 20;
	private int lastPlayerLocation;
	Animator anim;
	void Start() {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
		agent.autoBraking = true;
		anim = agent.GetComponent <Animator> ();
		GotoNextPoint ();
	}

	void GotoNextPoint() {
		if (!playerSpotted) {
			//if no more points to navigate to it does nothing
			if (points.Length == 0)
				return;
			//Sets agent to move to the selected destination
			agent.destination = points [destination].position;
			curTime = 0;
			//The next point in the array is set up as the destination, cycles to start if needed
			destination = (destination + 1) % points.Length;
		} else {
			agent.destination = hit.point;
		}
	}

	void Update(){
		if ( (Physics.Raycast (transform.position, transform.forward, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.right/widthOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward + (transform.up/heightOfVision)).normalized, out hit, visionDistance)) ||
			(Physics.Raycast (transform.position, (transform.forward - (transform.up/heightOfVision)).normalized, out hit, visionDistance))) {
			if (hit.collider.tag == "Player") {
				//Player is found here so enemy will do something
				agent.destination = hit.point;
				playerSpotted = true;
			}
		}
		else {
			playerSpotted = false;
		}
		//as the agent approaches its destination it finds its next destination
		if (agent.remainingDistance < 0.5f) {
			if (curTime == 0) {
				curTime = Time.time;
				//Currently only works for RobotEnemy 
				//To pause for other enemies set the "RobotIdle_001 to what their
				//Idle animation is called
				anim.SetTrigger ("RobotIdle_001");
			}
			if ((Time.time - curTime) >= pauseDuration) {
				GotoNextPoint ();
			}
		}
	}

}