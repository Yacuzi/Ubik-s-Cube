using UnityEngine;
using System.Collections;

public class Cube_Flottant : MonoBehaviour {

		private float pingpong, posiniy, tempsping;

		// Use this for initialization
		void Start () {
				posiniy = transform.position.y;
		}

		// Update is called once per frame
		void Update () {
				
				//mouvement haut-bas
				tempsping = (transform.position.x * 0.03F) + Time.time;
				pingpong = posiniy + (Mathf.PingPong (tempsping , -5F));
				transform.position = new Vector3 (transform.position.x, pingpong, transform.position.z);

		}
}