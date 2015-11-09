using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ClusterControl : MonoBehaviour {

	public float movementSpeed; //Speed Modifer
	private int movementDirection; //Movement direction can either be 1 or -1
	public bool aliensMoving; //Are the aliens moving

	private Vector3 startPosition = new Vector3(0, 8, 2); //The start position for the cluster
	public GameObject Alien; //Alien prefab
	
	public int columns; //How many columns?
	public int rows;	//How many rows?

	private float leftStartPos; //Left start position of cluster
	private float topStartPos;	//Top start position of cluster

	public static List<GameObject> alienList = new List<GameObject>(); //List of Aliens

	private float outerX; //Outermost alien x position on relevant side
	public float outerBounds; //Outer bounds aliens can reach

	private float lowerY; //Lowest y position of any alien
	public float lowerBounds; //Lowest point aliens are allowed to reach

	public float minDelay; //Minimum amount of time to pass between enemy shots (seconds)
	public float maxDelay; //Maximum amount of time to pass between enemy shots (seconds)
	private float lastFireTime; //Time when enemy cluster last fired
	private float fireDelay; //Delay between shots
	public bool isFiring; //Are the aliens firing

	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		alienList.Clear (); //Clear all aliens

		SpawnCluster (); //Spawns enemy cluster

		movementDirection = 1; //Set positive direction
		aliensMoving = true; //Aliens should move

		outerX = 0; //Set default outer x value to centre of cluster
		lowerY = 12; //Set default lower y value to above level

		lastFireTime = Time.time; //Set last fire time
		fireDelay = Random.Range (minDelay, maxDelay); //Calculates an initial random delay
		isFiring = true; //Aliens should fire

		myAudio = gameObject.GetComponent<AudioSource>();
	}

	//Update is run every frame
	void Update(){
		if(GameLogic.gameRunning)
		{
			//Find outermost and lowermost alien
			for(int i = 0; i < alienList.Count; i++)
			{
				if(alienList[i] != null)
				{
					float currentY = alienList[i].transform.position.y;
					float currentX = alienList[i].transform.position.x;
					if(currentY < lowerY)
						lowerY = currentY;
					if(movementDirection == 1 && currentX > outerX)
						outerX = currentX;
					else if (movementDirection == -1 && currentX < outerX)
						outerX = currentX;
				}
				else
				{
					alienList.RemoveAt(i);
				}
			}

			//Has cluster gone too far down?
			if(lowerY <= lowerBounds)
			{
				GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().Destroy (1);
			}
			//Change direction at edge of screen
			if(movementDirection == 1 && outerX > outerBounds || movementDirection == -1 && outerX < -outerBounds)
			{
				movementDirection = -movementDirection;
				Vector3 newPos = transform.position;
				newPos.y -= 1;
				transform.position = newPos;
			}
			//Make an enemy shoot
			if(isFiring)
			{
				if (Time.time - lastFireTime > fireDelay) {
					//Fire
					int i = Random.Range (0, alienList.Count - 1); //Select random alien
					alienList[i].GetComponent<AlienControl>().Shoot ();
					myAudio.Play ();
					//Set last fire time, create new random delay
					lastFireTime = Time.time;
					fireDelay = Random.Range (minDelay, maxDelay);
				}
			}
		}
	}

	// Fixed Update is framerate independent
	void FixedUpdate () {
		if(GameLogic.gameRunning)
		{
			if(aliensMoving)
			{
				//Move
				Vector3 newPos = transform.position;
				newPos.x += (movementSpeed*movementDirection);
				transform.position = newPos;
			}
		}
	}

	public void increaseSpeed(float s)
	{
		movementSpeed += s;
	}

	public void decreaseSpeed(float s)
	{
		movementSpeed -= s;
	}

	public void SpawnCluster() {

		gameObject.transform.position = startPosition;

		leftStartPos = 0 - (columns/2); //Calculates left start position
		topStartPos = 0 + (rows/2); //Calulcates top start position

		for(int r = 0; r < rows; r++) //Increment through rows
		{
			for(int c = 0; c < columns; c++) //Increment through columns
			{
				Vector3 relativePosition = new Vector3(leftStartPos + c, topStartPos - r, 0f); //Calculates relative position
				GameObject newAlien = (GameObject)Instantiate(Alien); //Instantiates new alien
				if(r == 0)
					newAlien.GetComponent<AlienControl>().scoreValue = 40;	//Set score value of this alien
				else if (r > 0 && r < 3)
					newAlien.GetComponent<AlienControl>().scoreValue = 20;	//Set score value of this alien
				else
					newAlien.GetComponent<AlienControl>().scoreValue = 10;	//Set score value of this alien
				newAlien.transform.parent = gameObject.transform; //Sets new alien to child of cluster
				newAlien.transform.localPosition = relativePosition; //Sets position of new alien relative to cluster
				alienList.Add (newAlien); //Add to list of aliens
			}
		}
	}
}
