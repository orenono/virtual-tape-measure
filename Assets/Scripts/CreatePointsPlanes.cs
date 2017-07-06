using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreatePointsPlanes : MonoBehaviour {

	public GameObject ballPrefab;
	public float createHeight;
	public LineRenderer linePrefab;
	public TextMesh textMeshPrefab;
	public Color[] colors;// {Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.blue};
	Vector3 pos4;
	float toInches = 39.3701f;
	int colorIndex;
	public Button resetButton;
	public GameObject [] balls = new GameObject[20];
	int ballsCount = 0;
	float[] distance = new float[20];
	string diString;            
	public Text distText;

	bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
	{
		List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
		if (hitResults.Count > 0) {
			foreach (var hitResult in hitResults) {
				Vector3 position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
				CreateBall (new Vector3 (position.x, position.y + createHeight, position.z));
				return true;
			}
		}
		return false;
	}

	//enable for testing:
	//Vector3 pos1;
	//Vector3 pos2;
	//Vector3 pos3;
	//Vector3 pos5;

	// Use this for initialization
	void Start () {
		
		distText.text = "Distance:                       " + 0 + " Inches ";
		distText.enabled= true;
		pos4 = new Vector3 (0, 0, 0);

		//enable for testing:
		//pos1 = new Vector3(0.0955f,-0.0543f,1);
		//pos2 = new Vector3 (0.01795f, -0.0448f, 1);
		//pos3 = new Vector3 (0.06f, -0.09f, 1);
		//pos5 = new Vector3 (0.04f, -0.01f, 1);
		//CreateBall(pos1);
		//CreateBall (pos2);
		//CreateBall (pos3);
		//CreateBall (pos5);
	}

	void CreateBall(Vector3 atPosition)
	{
		GameObject ballGO = Instantiate (ballPrefab, atPosition, Quaternion.identity);
		balls [ballsCount] = ballGO;
		if(ballsCount > 0 && ballsCount % 2 != 0)
		{
			//save distance between last ball and the previous
			distance[ballsCount - 1] = Vector3.Distance (balls[ballsCount].transform.position, balls[ballsCount - 1].transform.position);
			//create line between the last two objects
			CreateLine(balls[ballsCount].transform.position, balls[ballsCount -1].transform.position, colorIndex, distance[ballsCount-1] );
			//display distance on screen
			diString = System.String.Format ("{0:0.00}", distance [ballsCount - 1] * toInches);
			distText.text = "Distance:                       " + diString + " Inches ";
			//change color next time
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
			if (touch.phase == TouchPhase.Began)
			{
				var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
				ARPoint point = new ARPoint {
					x = screenPosition.x,
					y = screenPosition.y
				};
				ARHitTestResultType[] resultTypes = {
					ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
					// if you want to use infinite planes use this:
					//ARHitTestResultType.ARHitTestResultTypeExistingPlane,
					ARHitTestResultType.ARHitTestResultTypeHorizontalPlane, 
					//Important: enable this when Apple implement this function
					//ARHitTestResultType.ARHitTestResultTypeVerticalPlane,
					ARHitTestResultType.ARHitTestResultTypeFeaturePoint
				}; 
			
				foreach (ARHitTestResultType resultType in resultTypes)
				{
					if (HitTestWithResultType (point, resultType))
					{
						return;
					}
				}
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
		float distInInches =  number * toInches;
		string s = System.String.Format ("{0:0.00}", distInInches);
		return s;
	}

	public void Reset() {

		for (int i = 0; i <distance.Length; i++) {
			distance [i] = 0;
		}
		ballsCount = 0;
		GameObject[] gameObjects = GameObject.FindGameObjectsWithTag ("Prefab");
		for (int i = 0; i < gameObjects.Length; i++)
		{
			Destroy(gameObjects[i]);
		}
		distText.text = "Distance:                         " + 0 + " Inches ";
		colorIndex = 0;
	}

	public void BackToMenu() {
		
		Reset ();
		SceneManager.LoadScene ("LDCStartMenu");
	}
}
