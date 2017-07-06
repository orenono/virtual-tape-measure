using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ModeSwitcher : MonoBehaviour {

	public GameObject ballMake;
	public GameObject ballMove;
	//private BallMaker ballMaker;
	//float dist;

	//public Text txt;


	private int appMode = 0;
	// Use this for initialization
	void Start () {

		//txt.text = "wrong number";
		//txt.enabled = true;

		//ballMaker = GetComponent<BallMaker>();


	}
	
	// Update is called once per frame
	void Update () {

		//dist = 2.34f;

	}

	void EnableBallCreation(bool enable)
	{
		ballMake.SetActive (enable);
		ballMove.SetActive (!enable);
		
	}

	void OnGUI()
	{
		string modeString = appMode == 0 ? "MAKE" : "BREAK";
		if (GUI.Button(new Rect(Screen.width -150.0f, 0.0f, 150.0f, 100.0f), modeString))
		{
			appMode = (appMode + 1) % 2;
			EnableBallCreation (appMode == 0);
		}

		//GUI.Label(new Rect(10, 10, 100, 20), "The distance is " + dist.ToString() );
			

	}

}
