using UnityEngine;
using System.Collections;

public class AlienControl : MonoBehaviour {

	public GameObject projectile;
	public ParticleSystem explosion;
	public int scoreValue;

	void OnTriggerEnter(Collider coll)
	{
		if(coll.gameObject.CompareTag("Bullet")) //If enemy has collided with a bullet
		{
			Instantiate(explosion, transform.position, Quaternion.identity); //Creates explosion
			GameObject.Find("ExplosionSound").GetComponent<AudioSource>().Play();
			coll.GetComponent<BulletMovement>().Destroy(); //Destroy the bullet that hit it
			GameLogic.score += scoreValue;	//Add to score
			Destroy (gameObject); //Destroy the enemy
		}
	}

	public void Shoot()
	{
		GameObject newProjectile = (GameObject)Instantiate (projectile); //Create new alien projectile
		newProjectile.transform.position = transform.position;
	}
}
