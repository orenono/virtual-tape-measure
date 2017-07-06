using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void Default() {
		SceneManager.LoadScene ("LDCDefault");
	}

	public void FarMode() {
		SceneManager.LoadScene ("LDCFarMode");
	}

	public void ContinuousMode() {
		SceneManager.LoadScene ("LDCContinuousMode");
	}
}
