using UnityEngine;
using System.Collections;

public class Ouverture_Porte : MonoBehaviour {

		public GameObject sphere;
		[HideInInspector]
		public bool sesame, opened;
		private Vector3 ouvert;
		public float timer;

	// Use this for initialization
	void Start () {

				ouvert = new Vector3 (transform.position.x, transform.position.y - (1.01F * transform.lossyScale.y) - 0.5F, transform.position.z);
	
	}
	
	// Update is called once per frame
	void Update () {
	
				//je vérifie si la sphère a été prise
				sesame = sphere.GetComponent<Sphere_collectible> ().recupere;

				if (sesame)
						transform.position = Vector3.Lerp (transform.position, ouvert, timer * Time.deltaTime);

				if (Mathf.Abs (transform.position.y - ouvert.y) <= 0.45F)
						opened = true;

	}
}
