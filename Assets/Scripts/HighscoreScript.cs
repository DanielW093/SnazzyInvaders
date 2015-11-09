using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class HighscoreScript : MonoBehaviour{

	private Text[] highScoreTexts = new Text[10];
	private Text[] highNameTexts = new Text[10];

	private static int[] highscores = new int[10];
	private static string[] hightexts = new string[10];

	// Use this for initialization
	void Start () {
		highScoreTexts = GameObject.Find ("HighscoreTable").GetComponentsInChildren<Text>();
		highNameTexts = GameObject.Find ("HighNameTable").GetComponentsInChildren<Text>();

		for(int i = 0; i < 10; i++)
		{
			highScoreTexts[i].text = highscores[i].ToString();
			if(hightexts[i] != null)
				highNameTexts[i].text = hightexts[i];
			else
				highNameTexts[i].text = "N/A";
		}
	}
	
	public static void updateHighscore(int newScore, string name)
	{
		List<int> tempList = new List<int>(highscores);
		List<string> tempNames = new List<string>(hightexts);

		for(int i = 0; i < 10; i++)
		{
			if(newScore > tempList[i])
			{
				tempList.Insert(i, newScore);
				tempNames.Insert(i, name);

				highscores = tempList.ToArray ();
				hightexts = tempNames.ToArray ();

				SaveHighscores();

				break;
			}
		}
	}

	public static void LoadHighscores()
	{
		BinaryFormatter bf = new BinaryFormatter();

		FileStream highscoreFile = File.Open(Application.persistentDataPath + "/highScores.gd", FileMode.OpenOrCreate);
		if(highscoreFile.Length != 0)
			highscores = (int[])bf.Deserialize(highscoreFile);
		highscoreFile.Close ();

		FileStream highnameFile = File.Open(Application.persistentDataPath + "/highNames.gd", FileMode.OpenOrCreate);
		if(highnameFile.Length != 0)
			hightexts = (string[])bf.Deserialize (highnameFile);
		highnameFile.Close ();
	}

	static void SaveHighscores(){
		BinaryFormatter bf = new BinaryFormatter();

		FileStream highscoreFile = File.Open(Application.persistentDataPath + "/highScores.gd", FileMode.OpenOrCreate);
		bf.Serialize (highscoreFile, highscores);
		highscoreFile.Close ();

		FileStream highnameFile = File.Open(Application.persistentDataPath + "/highNames.gd", FileMode.OpenOrCreate);
		bf.Serialize (highnameFile, hightexts);
		highnameFile.Close ();
	}
}
