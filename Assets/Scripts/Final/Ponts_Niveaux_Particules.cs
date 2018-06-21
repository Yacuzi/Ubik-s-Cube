using UnityEngine;
using System.Collections;

public class Ponts_Niveaux_Particules : MonoBehaviour {

		public Transform Porte;
		private float duree;
		private ParticleSystem particules;

	// Use this for initialization
	void Start () {
				particules = this.GetComponent<ParticleSystem>();
				duree = Vector3.Distance (Porte.position, transform.position);
				particules.startLifetime = duree;
	}
	
	// Update is called once per frame
	void Update () {
				transform.LookAt (Porte);
				duree = Vector3.Distance (Porte.position, transform.position);
				particules.startLifetime = duree;
	}
}
