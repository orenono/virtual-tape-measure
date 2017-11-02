using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatePointsContinuously : MonoBehaviour {

	public GameObject ballPrefab;
	public float createHeight;
	public float penDistance = 0.2f;
	Matrix4x4 currentMatrix;
	public LineRenderer linePrefab;
	public TextMesh textMeshPrefab;
	public Color[] colors;// = {Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.blue};
	Vector3 pos4;
	float toInches = 39.3701f;
	public Button resetButton;
	public GameObject [] balls = new GameObject[20];
	int ballsCount = 0;
	float[] distance = new float[20];
	string diString;            
	public Text distText;
	float sumOfDistance;
	int colorIndex;

	//enable for testing:
	/*
	Vector3 pos1;
	Vector3 pos2;
	Vector3 pos3;
	Vector3 pos5;
	*/

	// Use this for initialization
	void Start () {
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
		distText.text = "Total Distance:                       " + 0 + " Inches "; 
		distText.enabled= true;
		pos4 = new Vector3 (0, 0, 0);

		//enable for testing:
		/*
		pos1 = new Vector3(0.0955f,-0.0543f,1);
		pos2 = new Vector3 (0.01795f, -0.0448f, 1);
		pos3 = new Vector3 (0.06f, -0.09f, 1);
		pos5 = new Vector3 (0.04f, -0.01f, 1);
		CreateBall(pos1);
		CreateBall (pos2);
		CreateBall (pos3);
		CreateBall (pos5);
		*/
	}
		public void ARFrameUpdated(UnityARCamera camera)
		{
			Matrix4x4 matrix = new Matrix4x4();
			matrix.SetColumn(3, camera.worldTransform.column3);
		currentMatrix = matrix;

		}

	void CreateBall(Vector3 atPosition)
	{
		GameObject ballGO = Instantiate (ballPrefab, atPosition, Quaternion.identity);
		balls [ballsCount] = ballGO;
		if(ballsCount > 0)
		{
			//save distance between last ball and the previous
			distance[ballsCount - 1] = Vector3.Distance (balls[ballsCount].transform.position, balls[ballsCount - 1].transform.position);
			//create line between the last two objects
			CreateLine(balls[ballsCount].transform.position, balls[ballsCount -1].transform.position, colorIndex, distance[ballsCount-1] );
			//display total distance on screen
			diString =  SumOfDistances (distance);
			distText.text =  "Total Distance:                       " + diString + " Inches ";
			//change to different color next time
			colorIndex++;
		}
		ballsCount++;
	}

	void CreateLine(Vector3 vertex1, Vector3 vertex2, int colorIndex, float dis) {

		LineRenderer line = Instantiate (linePrefab, pos4, Quaternion.identity);
		line.startColor = colors [colorIndex];
		line.endColor = colors [colorIndex];
		line.SetPosition (0, vertex1);
		line.SetPosition (1, vertex2);
		CreateTextMesh ((vertex1+vertex2)/2, colorIndex, dis); 
	}

	void CreateTextMesh(Vector3 position, int colorIndex, float distanceInMeters) {

		float distInInches = distanceInMeters * toInches;
		TextMesh text = Instantiate (textMeshPrefab, position, Quaternion.identity);
		text.color = colors [colorIndex];
		//rotate textMesh to face camera
		text.transform.rotation = Camera.main.transform.rotation;
		string s = System.String.Format ("{0:0.00}", distInInches);
		text.text = s + " Inch ";
	}

	// Update is called once per frame
	void Update () {

		if (Input.touchCount > 0 )
		{
			var touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began) {

				CreateBall (UnityARMatrixOps.GetPosition(currentMatrix) + (Camera.main.transform.forward * penDistance));
			}
	}
}

	public string Distance () {

		float distInInches = distance [ballsCount - 1] * toInches;
		string s = System.String.Format("{0:0.00}",distInInches); 
		return s;
	}

	public string SumOfDistances (float [] dis) {

		float number = 0;
		foreach (var di in dis) {
			number = di + number;
		}
		sumOfDistance =  number * toInches;
		string s = System.String.Format ("{0:0.00}", sumOfDistance);
		return s;
	}

	public void Reset() {

		for (int i = 0; i <distance.Length; i++) {
			distance [i] = 0;
		}
		sumOfDistance = 0;
		ballsCount = 0;
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Prefab");

		for (int i = 0; i < gameObjects.Length; i++)
		{
			Destroy(gameObjects[i]);
		}
		distText.text = "Total Distance:                         " + 0 + " Inches ";
	}
		
	public void BackToMenu() {

		Reset ();
		SceneManager.LoadScene ("LDCStartMenu");
	}
}


