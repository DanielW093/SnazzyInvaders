using UnityEngine;
using System.Collections;

public class BonusUFO : MonoBehaviour {

	public int startScoreValue;
	private int scoreValue;

	public float minDelay;
	public float maxDelay;

	public GameObject UFO;
	float delay; //Delay in seconds
	public float lastUFOTime; //Last time that the UFO appeared


	// Use this for initialization
	void Start () {
		GenerateDelay ();
		lastUFOTime = Time.time;

		ResetScoreValue();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - lastUFOTime > delay)
		{
			GameObject newUFO = (GameObject)(Instantiate(UFO));
			newUFO.transform.position = new Vector3(12, 11, 2);
			newUFO.GetComponent<BonusUFOMovement>().scoreValue = scoreValue;

			lastUFOTime = 10000;
		}
	}

	public void GenerateDelay()
	{
		delay = Random.Range(minDelay, maxDelay);
	}

	public void SubtractScoreValue()
	{	
		if(scoreValue > 50)
			scoreValue -= 50;
	}

	public void ResetScoreValue()
	{
		scoreValue = startScoreValue;
	}
}
