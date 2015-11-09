using UnityEngine;
using System.Collections;

public class EnemyBulletMovement : MonoBehaviour {

	public float speedModifier; //Speed Modifier

	void FixedUpdate () {
		if(GameLogic.gameRunning)
		{
			//Move Bullet
			Vector3 newPos = transform.position; //Grab current position
			newPos.y -= speedModifier; //Modify position
			transform.position = newPos; //Apply modified position
		}
	}

	public void Destroy()
	{
		Destroy (gameObject); //Delete object
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.name == "Floor") {
			Destroy ();
		}
		if (coll.gameObject.CompareTag ("Player")) {
			coll.gameObject.GetComponent<PlayerScript>().Destroy(0);
			Destroy ();
		}
	}
}
