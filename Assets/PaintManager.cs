using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;


public class PaintManager : MonoBehaviour {
	public ParticleSystem particleSystemTemplate;

	private bool newPaintVertices;
	private bool paintingOn;
	private Color paintcolor;
	private Vector3 previousPosition;

	private List<Vector3> currentVertices; // stores current camera positions to paint
	private ParticleSystem ps;

	void OnEnable() {
		UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
	}

	void OnDestroy() {
		UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
	}

	// Use this for initialization
	void Start () {
		paintingOn = false;
		newPaintVertices = false;
		ps = Instantiate (particleSystemTemplate);
		currentVertices = new List<Vector3> ();
		paintcolor = Color.black;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (paintingOn && newPaintVertices) {
			if (currentVertices.Count > 0) {
				ParticleSystem.Particle[] particles = new ParticleSystem.Particle[currentVertices.Count];
				int index = 0;
				foreach(Vector3 vtx in currentVertices) {
					particles[index].position = vtx;
					particles[index].color = paintcolor;
					particles[index].size = 0.1f;
					index++;
				}
				ps.SetParticles(particles, currentVertices.Count);
				newPaintVertices = false;
			}
		}
		
	}

	public void Paint() {
		paintingOn = !paintingOn;
	}

	public void CalDistance() {
		//calculate the distance between the first particle point to the last

		//show result on the screen
	}

	public void Reset() {
		Destroy (ps);
		paintingOn = false;
		ps = Instantiate (particleSystemTemplate);
		currentVertices = new List<Vector3> ();
	}

	private void ARFrameUpdated(UnityARCamera arCamera) {
		Vector3 paintPosition = GetCameraPosition(arCamera) + (Camera.main.transform.forward * 0.2f);
		if (Vector3.Distance(paintPosition, previousPosition) > 0.025f) {
			if (paintingOn)
				currentVertices.Add (paintPosition);
			
			previousPosition = paintPosition;
			newPaintVertices = true;
		}
	}

	private Vector3 GetCameraPosition(UnityARCamera cam) {
		Matrix4x4 matrix = new Matrix4x4 ();
		matrix.SetColumn (3, cam.worldTransform.column3);
		return UnityARMatrixOps.GetPosition (matrix);
	}
}
	
