using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

	public Transform [] backgrounds; 				// List of backgrounds and foregrounds to be parallaxed
	public float [] parallaxScales;				// The amount of movement for parallax
	public float smoothing;

	public new Camera camera;
	private Transform cam;
	private Vector3 previousCamPos;

	// Use this for initialization
	void Start () {
		cam = camera.transform;
		previousCamPos = cam.position;
		parallaxScales = new float[backgrounds.Length];

		for (int i=0; i < backgrounds.Length; i++) {
			parallaxScales [i] = backgrounds[i].position.z * 2;
		}
	}
	
	// Update is called once per frame
	void LateUpdate () {
		for (int i=0; i < backgrounds.Length; i++) {

			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales [i];
			float backgroundTargetPosX = backgrounds [i].position.x + parallax;
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		//	Debug.Log (backgrounds [i].position.x);
		}
		previousCamPos = cam.position;
	}
}
