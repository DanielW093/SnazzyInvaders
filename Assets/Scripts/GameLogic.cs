using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	static public bool gameRunning; //Is the game running

	static public int score; //The players score
	static public int lives; //The players lives
	static public int coins; //The players coins
	
	private Text scoreText; //The text that displays the score
	private Text livesText; //The text that displays the lives
	private Text currentCoinsText; //The text that displays the players coins
	private Text collectedCoinsText; //The Text that displays the collected coins

	private Canvas gameOverScreen; //The canvas with the gameover screen on it
	private Canvas respawnScreen; //The canvas with the respawn screen on it
	private Canvas powerupScreen; //The canvas with the powerup screen on it
	private GameObject player; //The player gameobject
	private GameObject enemyCluster; //The enemy cluster gameobject

	private int initialScore; //Score at the beginning of the level
	private int collectedCoins;	//Coins collected during the level

	//Powerups enabled
	private bool pauseAlienEnabled;
	private bool ceasefireEnabled;
	private bool increaseBulletsEnabled;
	//private bool renewShieldsEnabled;
	//Time powerups activated
	private float pauseAlienActivated;
	private float ceasefireActivated;
	private float increaseBulletsActivated;
	//private float renewShieldsActivated; 
	//Amount of powerups
	private int amountPA;
	private int amountCF;
	private int amountIB;
	private int amountRS;
	//Powerup buttons
	private Button PAButton;
	private Button CFButton;
	private Button IBButton;
	private Button RSButton;
	//Powerup Cooldowns
	public int PAC;
	public int CFC;
	public int IBC;
	public int RSC;
	
	AudioSource buySound;
	AudioSource powerupSound;

	void Start () {
		gameRunning = true;

		score = 0;
		scoreText = GameObject.Find ("ScoreText").GetComponent<Text>(); //Find scoretext
		lives = 3;
		livesText = GameObject.Find ("LivesText").GetComponent<Text>(); //Find lives text
		coins = 0;
		currentCoinsText = GameObject.Find ("CurrentCoinsText").GetComponent<Text>(); //Find coins text
		initialScore = score;
		collectedCoins = 0;
		collectedCoinsText = GameObject.Find ("CollectedCoinsText").GetComponent<Text>(); //Find collected coins text
		collectedCoinsText.enabled = false;

		gameOverScreen = GameObject.Find ("GameOverCanvas").GetComponent<Canvas>(); //Find Game Over Screen canvas
		gameOverScreen.enabled = false;
		respawnScreen = GameObject.Find ("RespawnCanvas").GetComponent<Canvas>(); //Find Respawn Screen canvas
		respawnScreen.enabled = false;
		powerupScreen = GameObject.Find ("PowerupCanvas").GetComponent<Canvas>(); //Find Powerup Screen canvas
		powerupScreen.enabled = false;

		player = GameObject.FindGameObjectWithTag("Player");
		enemyCluster = GameObject.Find ("EnemyCluster");

		//Disable all powerups
		pauseAlienEnabled = false;
		ceasefireEnabled = false;
		increaseBulletsEnabled = false;
		//renewShieldsEnabled = false;
		//Set powerup times
		pauseAlienActivated = Time.time;
		ceasefireActivated = Time.time;
		increaseBulletsActivated = Time.time;
		//renewShieldsActivated = Time.time; 
		//Set powerup amounts
		amountPA = 0;
		amountCF = 0;
		amountIB = 0;
		amountRS = 0;
		//Find Buttons
		PAButton = GameObject.Find ("PAButton").GetComponent<Button>();
		CFButton = GameObject.Find ("CFButton").GetComponent<Button>();
		IBButton = GameObject.Find ("IBButton").GetComponent<Button>();
		RSButton = GameObject.Find ("RSButton").GetComponent<Button>();
		//Set text
		PAButton.GetComponentInChildren<Text>().text = amountPA.ToString();
		CFButton.GetComponentInChildren<Text>().text = amountCF.ToString();
		IBButton.GetComponentInChildren<Text>().text = amountIB.ToString();
		RSButton.GetComponentInChildren<Text>().text = amountRS.ToString();

		buySound = gameObject.GetComponent<AudioSource>();
		powerupSound = GameObject.Find("PowerupSound").GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreText.text = "Score:\n" + score; //Update score text
		livesText.text = "Lives:\n" + lives; //Update lives text
		currentCoinsText.text = "C: " + coins; //Update coins text
		collectedCoinsText.text = "+" + collectedCoins; //Update collected coins text

		//Update powerups
		UpdatePowerups();

		//Are all the aliens destroyed?
		if(ClusterControl.alienList.Count == 0 && gameRunning){
			gameRunning = false;
			collectedCoins = (int)((score - initialScore) / 4);
			powerupScreen.enabled = true;

			collectedCoinsText.enabled = true;

			StartCoroutine(AddCoins());
		}

		//Has the player clicked the mouse to skip the coin collection?
		if(Input.GetMouseButtonDown(0) && !gameRunning && collectedCoins != 0)
		{
			coins += collectedCoins;
			collectedCoins = 0;
			collectedCoinsText.enabled = false;
			StopAllCoroutines();
		}
	}

	IEnumerator AddCoins()
	{
		while(collectedCoins != 0)
		{
			coins++;
			collectedCoins --;
			currentCoinsText.text = "C: " + coins;
			collectedCoinsText.text = "+" + collectedCoins;

			yield return new WaitForSeconds(0.02f);
		}

		collectedCoinsText.enabled = false;
	}

	public void playerDeath()
	{
		lives--;
		gameRunning = false;

		if(lives > 0)
		{
			respawnScreen.enabled = true;
		}
		else 
		{
			gameOverScreen.enabled = true;
		}
	}

	public void playerRespawn()
	{
		GameObject[] bullets;
		bullets = (GameObject.FindGameObjectsWithTag("EnemyProjectile"));
		for(int i = 0; i < bullets.GetLength(0); i++)
			Destroy(bullets[i]);
		for(int i = 0; i < PlayerScript.bulletList.Count; i++)
			Destroy (PlayerScript.bulletList[i]);
		PlayerScript.bulletList.Clear ();

		respawnScreen.enabled = false;
		player.GetComponent<PlayerScript>().Spawn();
		gameRunning = true;
	}

	public void NextLevel()
	{
		if(collectedCoins > 0)
		{
			coins += collectedCoins;
		}

		collectedCoins = 0;

		initialScore = score;

		GameObject.Find ("GameManager").GetComponent<BonusUFO>().ResetScoreValue();
		lives++;

		powerupScreen.enabled = false;

		ClusterControl.alienList.Clear ();

		GameObject[] bullets;
		bullets = (GameObject.FindGameObjectsWithTag("EnemyProjectile"));
		for(int i = 0; i < bullets.GetLength(0); i++)
			Destroy(bullets[i]);
		for(int i = 0; i < PlayerScript.bulletList.Count; i++)
			Destroy (PlayerScript.bulletList[i]);
		PlayerScript.bulletList.Clear ();

		if(enemyCluster.GetComponent<ClusterControl>().movementSpeed < 0.1)
			enemyCluster.GetComponent<ClusterControl>().increaseSpeed(0.005f);

		if(enemyCluster.GetComponent<ClusterControl>().minDelay > 0.5f)
			enemyCluster.GetComponent<ClusterControl>().minDelay -= 0.1f;

		if(enemyCluster.GetComponent<ClusterControl>().maxDelay > 2f)
			enemyCluster.GetComponent<ClusterControl>().maxDelay -= 0.1f;

		enemyCluster.GetComponent<ClusterControl>().SpawnCluster();

		gameRunning = true;
	}

	private void UpdatePowerups()
	{
		//Pause Alien Button
		if(pauseAlienEnabled && Time.time-pauseAlienActivated < PAC)
		{
			//Update cooldown timer
			Text buttonText = PAButton.GetComponentInChildren<Text>();
			buttonText.text = ((int)(PAC+1 - (Time.time-pauseAlienActivated))).ToString();
			//Disable button
			PAButton.interactable = false;
		}
		else if (pauseAlienEnabled && Time.time-pauseAlienActivated >= PAC)
		{
			//Update text to amount remaining
			Text buttonText = PAButton.GetComponentInChildren<Text>();
			buttonText.text = amountPA.ToString();
			//Disable powerup
			pauseAlienEnabled = false;
			enemyCluster.GetComponent<ClusterControl>().aliensMoving = true;
			//Re-enable button, if there are any remaining
			if(amountPA > 0)
				PAButton.interactable = true;
		}
		else if(!pauseAlienEnabled && amountPA > 0)
		{
			Text buttonText = PAButton.GetComponentInChildren<Text>();
			PAButton.interactable = true;
			buttonText.text = amountPA.ToString();
		}
		//Ceasefire Button
		if(ceasefireEnabled && Time.time-ceasefireActivated < CFC)
		{
			//Update cooldown timer
			Text buttonText = CFButton.GetComponentInChildren<Text>();
			buttonText.text = ((int)(CFC+1 - (Time.time-ceasefireActivated))).ToString();
			//Disable button
			CFButton.interactable = false;
		}
		else if (ceasefireEnabled && Time.time-ceasefireActivated >= CFC)
		{
			//Update text to amount remaining
			Text buttonText = CFButton.GetComponentInChildren<Text>();
			buttonText.text = amountCF.ToString();
			//Disable powerup
			ceasefireEnabled = false;	
			enemyCluster.GetComponent<ClusterControl>().isFiring = true;
			//Re-enable button, if there are any remaining
			if(amountCF > 0)
				CFButton.interactable = true;
		}
		else if(!pauseAlienEnabled && amountCF > 0)
		{
			Text buttonText = CFButton.GetComponentInChildren<Text>();
			CFButton.interactable = true;
			buttonText.text = amountCF.ToString();
		}
		//Increase max bullets button
		if(increaseBulletsEnabled && Time.time-increaseBulletsActivated < IBC)
		{
			//Update cooldown timer
			Text buttonText = IBButton.GetComponentInChildren<Text>();
			buttonText.text = ((int)(IBC+1 - (Time.time-increaseBulletsActivated))).ToString();
			//Disable button
			IBButton.interactable = false;
		}
		else if(increaseBulletsEnabled && Time.time-increaseBulletsActivated >= IBC)
		{
			//Update text to amount remaining
			Text buttonText = IBButton.GetComponentInChildren<Text>();
			buttonText.text = amountIB.ToString();
			//Disable powerup
			increaseBulletsEnabled = false;
			PlayerScript.maxBullets = 1;
			//Re-enable button, if there are any remaining
			if(amountIB > 0)
				IBButton.interactable = true;
		}
		else if(!pauseAlienEnabled && amountIB > 0)
		{
			Text buttonText = IBButton.GetComponentInChildren<Text>();
			IBButton.interactable = true;
			buttonText.text = amountIB.ToString();
		}
//		//Renew shields button
//		if(renewShieldsEnabled && Time.time-renewShieldsActivated < RSC)
//		{
//			//Update cooldown timer
//			Text buttonText = RSButton.GetComponentInChildren<Text>();
//			buttonText.text = ((int)(RSC+1 - (Time.time-renewShieldsActivated))).ToString();
//			//Disable button
//			RSButton.interactable = false;
//		}
//		else if(renewShieldsEnabled && Time.time-renewShieldsActivated >= RSC)
//		{
//			//Update text to amount remaining
//			Text buttonText = RSButton.GetComponentInChildren<Text>();
//			buttonText.text = amountRS.ToString();
//			//Disable powerup
//			renewShieldsEnabled = false;	
//			//Re-enable button, if there are any remaining
//			if(amountRS > 0)
//				RSButton.interactable = true;
//		}
//		else if(!pauseAlienEnabled && amountRS > 0)
//			RSButton.interactable = true;

		Button pauseBuyButton = GameObject.Find ("PauseAlienPanel").GetComponentInChildren<Button>();
		if(coins >= 400 && pauseBuyButton.interactable == false)
			pauseBuyButton.interactable = true;
		else if (coins < 400)
			pauseBuyButton.interactable = false;

		Button ceasefireBuyButton = GameObject.Find ("CeasefirePanel").GetComponentInChildren<Button>();
		if(coins >= 300 && ceasefireBuyButton.interactable == false)
			ceasefireBuyButton.interactable = true;
		else if (coins < 300)
			ceasefireBuyButton.interactable = false;

		Button increaseFireButton = GameObject.Find ("IncreaseBulletsPanel").GetComponentInChildren<Button>();
		if(coins >= 250 && increaseFireButton.interactable == false)
			increaseFireButton.interactable = true;
		else if (coins < 250)
			increaseFireButton.interactable = false;

		Button extraLifeButton = GameObject.Find ("ExtraLifePanel").GetComponentInChildren<Button>();
		if(coins >= 700 && extraLifeButton.interactable == false)
			extraLifeButton.interactable = true;
		else if (coins < 700)
			extraLifeButton.interactable = false;
	}

	public void PauseAliens(){
		if(gameRunning)
		{
			powerupSound.Play ();
			pauseAlienActivated = Time.time;
			pauseAlienEnabled = true;
			amountPA--;
			//Pause aliens
			enemyCluster.GetComponent<ClusterControl>().aliensMoving = false;
		}
	}

	public void Ceasefire(){
		if(gameRunning)
		{
			powerupSound.Play ();
			ceasefireActivated = Time.time;
			ceasefireEnabled = true;
			amountCF--;
			//Ceasefire aliens
			enemyCluster.GetComponent<ClusterControl>().isFiring = false;
		}
	}

	public void IncreaseMaxBullets(){
		if(gameRunning)
		{
			powerupSound.Play ();
			increaseBulletsActivated = Time.time;
			increaseBulletsEnabled = true;
			amountIB--;
			//Increase max bullets
			PlayerScript.maxBullets = 3;
		}
	}

//	public void RenewShields(){
//		if(gameRunning)
//		{
//			renewShieldsActivated = Time.time;
//			renewShieldsEnabled = true;
//			amountRS--;
//			//TODO: Implement shields and renewal powerup
//		}
//	}

	public void buyPowerup(int type)
	{ 
		buySound.Play ();
		switch (type)
		{
		case 0:
			//FREEZE
			coins -= 400;
			amountPA++;
			break;
		case 1:
			//CEASEFIRE
			coins -= 300;
			amountCF++;
			break;
		case 2:
			//RAPID FIRE
			coins -= 250;
			amountIB++;
			break;
		case 3:
			//EXTRA LIFE
			coins -= 700;
			lives++;
			break;
		}
	}
}
