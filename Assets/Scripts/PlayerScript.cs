using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class PlayerScript : MonoBehaviour {

	public GameObject bullet; //Bullet template
	public float movementMod; //Movement Speed Modifier

	static public int maxBullets; //Maximum Bullets Allowed
	static public float bulletSpeed; //Bullet movement speed
	static public List<GameObject> bulletList = new List<GameObject> (); //List of bullets

	private Vector2 startPosition;
	private float startTime;

	private GameObject gameManager;
	public ParticleSystem explosion;
	
	private Vector3 moveLocation;
	private int screenHeight;

	private AudioSource myAudio;

	// Use this for initialization
	void Start () {
		bulletList.Clear();
		maxBullets = 1; //Set current bullet max value
		bulletSpeed = 1; //Set current bullet speed

		gameManager = GameObject.Find ("GameManager");

		moveLocation = transform.position;

		screenHeight = Screen.currentResolution.height;

		myAudio = gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(GameLogic.gameRunning)
		{
			if (Application.platform == RuntimePlatform.Android) {
				int noTouches = Input.touchCount;
				if(noTouches > 0)
				{
					for(int i = 0; i < noTouches; i++)
					{
						Touch myTouch = Input.GetTouch (i);

						TouchPhase phase = myTouch.phase;

						switch(phase)
						{
						case TouchPhase.Began:
							if(myTouch.position.y > screenHeight/2)
							{
								//Shoot
								Shoot ();
							}
							else
							{
								//Move
								float dist = 15f;
								Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
								moveLocation.x = ray.GetPoint(dist).x;
							}
							break;
						case TouchPhase.Moved:
							if(myTouch.position.y > screenHeight/2)
							{
								//Shoot
								Shoot ();
							}
							else
							{
								//Move
								float dist = 15f;
								Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
								moveLocation.x = ray.GetPoint(dist).x;
							}
							break;
						case TouchPhase.Stationary:
							if(myTouch.position.y > screenHeight/2)
							{
								//Shoot
								Shoot ();
							}
							else
							{
								//Move
								float dist = 15f;
								Ray ray = Camera.main.ScreenPointToRay(myTouch.position);
								moveLocation.x = ray.GetPoint(dist).x;
							}
							break;
						case TouchPhase.Ended:
							moveLocation.x = transform.position.x;
							break;	
						}
					}
				}
			} else {
				//Mouse Controls
				if (Input.GetMouseButton(0)) 
				{ 
						Shoot (); //Shoot bullet!
				}
				if(Input.GetMouseButton(1))
				{
					float dist = 15f;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					moveLocation.x = ray.GetPoint(dist).x;
				}
				if(Input.GetMouseButtonUp(1))
				{
					moveLocation.x = transform.position.x;
				}
			}
		}
	}

	void FixedUpdate() {
		if(GameLogic.gameRunning)
		{
			if(transform.position.x < moveLocation.x)
			{
				Vector3 newPos = transform.position;
				newPos.x += movementMod;
				transform.position = newPos;
			}
			else if (transform.position.x > moveLocation.x)
			{
				Vector3 newPos = transform.position;
				newPos.x -= movementMod;
				transform.position = newPos;
			}
		}
	}

	void Shoot() {
		if (bulletList.Count < maxBullets) { //Do we have the maximum bullets allowed?
			GameObject newBullet = (GameObject)Instantiate (bullet); //Create new bullet
			newBullet.transform.position = transform.position;	//Set start position to player position
			bulletList.Add (newBullet); //Add new bullet to bullet list
			myAudio.Play ();
		} else {
			Debug.Log ("LIMIT REACHED");
		}	
	}

	public void Destroy(int type)
	{
		if (type > 0)
			GameLogic.lives = 1;
		GameObject.Find("ExplosionSound").GetComponent<AudioSource>().Play();
		Instantiate(explosion, transform.position, Quaternion.identity); //Creates explosion
		gameManager.GetComponent<GameLogic>().playerDeath();
		gameObject.transform.position = new Vector3(0, -20, 2);
	}

	public void Spawn()
	{
		gameObject.transform.position = new Vector3(0, 0, 2);
	}
	
}
