using UnityEngine;
using System.Collections;

public class BonusUFOMovement : MonoBehaviour {
	
	public int scoreValue;
	public float movementSpeed;
	public ParticleSystem explosion;

	private GameObject UFOSpawner;

	// Use this for initialization
	void Start () {
		UFOSpawner = GameObject.Find ("GameManager");
	}

	void Update()
	{
		if(transform.position.x <= -12)
		{
			UFOSpawner.GetComponent<BonusUFO>().lastUFOTime = Time.time;
			UFOSpawner.GetComponent<BonusUFO>().GenerateDelay ();
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if(GameLogic.gameRunning)
		{
			//Move
			Vector3 newPos = transform.position;
			newPos.x -= (movementSpeed);
			transform.position = newPos;
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.CompareTag("Bullet")) //If enemy has collided with a bullet
		{
			GameObject.Find("ExplosionSound").GetComponent<AudioSource>().Play();
			Instantiate(explosion, transform.position, Quaternion.identity); //Creates explosion
			coll.GetComponent<BulletMovement>().Destroy(); //Destroy the bullet that hit it
			GameLogic.score += scoreValue;	//Add to score

			UFOSpawner.GetComponent<BonusUFO>().lastUFOTime = Time.time;
			UFOSpawner.GetComponent<BonusUFO>().GenerateDelay ();

			GameObject.Find ("GameManager").GetComponent<BonusUFO>().SubtractScoreValue();

			Destroy (gameObject); //Destroy the enemy		
		}
	}
}
