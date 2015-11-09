using UnityEngine;
using System.Collections;

public class BulletMovement : MonoBehaviour {

	public float speedModifier; //Speed Modifier

	void FixedUpdate () {
		if(GameLogic.gameRunning)
		{
			//Move Bullet
			Vector3 newPos = transform.position; //Grab current position
			newPos.y += PlayerScript.bulletSpeed * speedModifier; //Modify position
			transform.position = newPos; //Apply modified position
		}
	}

	void Update()
	{
		//DO THIS A BETTER WAY WHEN YOU FIND A SECOND
		if (transform.position.y > 11.95) //Is bullet currently on screen?
		{
			Destroy ();
		}
	}

	public void Destroy()
	{
		Destroy (gameObject); //Delete object
		int IID = transform.GetInstanceID(); //Find unique instance ID of this bullet
		for(int i = 0; i < PlayerScript.bulletList.Count; i++) //Cycle through bullet list
		{
			int LID = PlayerScript.bulletList[i].transform.GetInstanceID(); //Find unique instance ID of current bullet in list
			if(LID == IID) //Are the two unique ID's the same?
			{
				PlayerScript.bulletList.RemoveAt(i); //Remove from list
				break;
			}
		}
	}
}
