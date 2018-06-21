using UnityEngine;
using System.Collections;

public class Ubik_Camera : MonoBehaviour {

		public Transform target;
		private int turndirect = 0;
		private Vector3 a, b, c, d;
		public float smoothTime = 10F;
		private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start () {
				a= new Vector3(-2,4,-2);
				b= new Vector3(-2,4,4);
				c= new Vector3(4,4,4);
				d= new Vector3(4,4,-2);
	}
	
	// Update is called once per frame
	void Update () {
				if (Input.GetKeyDown (KeyCode.A)) {
						if (transform.position == a) {
								while (transform.position != b) {
										transform.position = Vector3.SmoothDamp (transform.position, b, ref velocity, smoothTime * Time.deltaTime);
										transform.LookAt (target);
								}
						} else if (transform.position == b) {
								while (transform.position != c) {
										transform.position = Vector3.SmoothDamp (transform.position, c, ref velocity, smoothTime * Time.deltaTime);
										transform.LookAt (target);
								}
						} else if (transform.position == c) {
								while (transform.position != d) {
										transform.position = Vector3.SmoothDamp (transform.position, d, ref velocity, smoothTime * Time.deltaTime);
										transform.LookAt (target);
								}
						} else if (transform.position == d) {
								while (transform.position != a) {
										transform.position = Vector3.SmoothDamp (transform.position, a, ref velocity, smoothTime * Time.deltaTime);
										transform.LookAt (target);
								}
						}
				}
				if (Input.GetKeyDown(KeyCode.E))
				if (turndirect == 0) {
						transform.Translate (Vector3.right * 6, Space.World);
						turndirect = 3;
						transform.LookAt (target);
				}
				else if (turndirect == 1){
						transform.Translate (Vector3.back * 6, Space.World);
						turndirect--;
						transform.LookAt (target);
				}
				else if (turndirect == 2){
						transform.Translate (Vector3.left * 6, Space.World);
						turndirect--;
						transform.LookAt (target);
				}
				else if (turndirect == 3){
						transform.Translate (Vector3.forward * 6, Space.World);
						turndirect--;
						transform.LookAt (target);
				}
	}
}
