using UnityEngine;
using System.Collections;

public class ExplosionLifetime : MonoBehaviour {

	private ParticleSystem pSystem;

	// Use this for initialization
	void Start () {
		pSystem = gameObject.GetComponent<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		if (pSystem.isStopped)
		{
			Destroy (gameObject);
		}
	}
}
