using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CanvasGUIController : MonoBehaviour {

	private Canvas thisCanvas;
	private Button[] buttonList;
	private bool lastState;

	// Use this for initialization
	void Start () {
		thisCanvas = gameObject.GetComponent<Canvas>();
		buttonList = gameObject.GetComponentsInChildren<Button>();
		lastState = thisCanvas.enabled;

		for(int i = 0; i < buttonList.GetLength(0); i++)
			buttonList[i].enabled = thisCanvas.enabled;
	}
	
	// Update is called once per frame
	void Update () {

		if(thisCanvas.enabled != lastState)
		{
			for(int i = 0; i < buttonList.GetLength(0); i++)
				buttonList[i].enabled = thisCanvas.enabled;

			lastState = thisCanvas.enabled;
		}
	}
}
