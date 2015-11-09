using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuScript : MonoBehaviour {

	public GameObject gameManager;

	void Start()
	{
		gameManager = GameObject.Find ("GameManager");
	}

    public void StartGame() {
		Application.LoadLevel (1);
	}

	public void LoadMenu(){
		Application.LoadLevel (0);
	}

	public void LoadHighscoreFromMenu(){
		Application.LoadLevel (2);
	}

	public void SubmitHighscore(){
		HighscoreScript.updateHighscore(GameLogic.score, GameObject.Find ("NameInputText").GetComponentInChildren<Text>().text);
		Application.LoadLevel (2);
	}

	public void RespawnButton(){
		gameManager.GetComponent<GameLogic>().playerRespawn();
	}

	public void NextLevelButton(){
		gameManager.GetComponent<GameLogic>().NextLevel();
	}
}
