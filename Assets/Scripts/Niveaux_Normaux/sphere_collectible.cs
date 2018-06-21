using UnityEngine;
using System.Collections;

public class sphere_collectible : MonoBehaviour {

		private int persox, persoy, persoz;
		private int spherex, spherey, spherez;
		private float pingpong, posiniy;
		private AudioSource spherebeep;
		private AudioClip cloche;
		[HideInInspector]
		public bool recupere;
		public GameObject perso;

	// Use this for initialization
	void Start () {
				posiniy = transform.position.y;
				spherebeep = GetComponent<AudioSource>();
				cloche = spherebeep.clip;
				recupere = false;
	}
	
	// Update is called once per frame
	void Update () {
				
				//je récupère la position arrondie à l'entier du personnage
				persox = Mathf.RoundToInt (perso.transform.position.x);
				persoy = Mathf.RoundToInt (perso.transform.position.y);
				persoz = Mathf.RoundToInt (perso.transform.position.z);

				//position de la sphère arrondier à l'entier
				spherex = Mathf.RoundToInt (transform.position.x);
				spherey = Mathf.RoundToInt (transform.position.y);
				spherez = Mathf.RoundToInt (transform.position.z);

				//animation de la sphère
				//rotation
				transform.RotateAround(transform.position, Vector3.up, Time.deltaTime * 30F);
				//mouvement haut-bas
				pingpong = posiniy + (Mathf.PingPong (Time.time * 0.2F , 0.4F));
				transform.position = new Vector3 (transform.position.x, pingpong, transform.position.z);

				//si égal à la position de la sphère, alors elle disparaît
				if ((spherex == persox) & (spherey == persoy) & (spherez == persoz) & (!recupere)) {
						recupere = true;
						GetComponent<Renderer> ().enabled = false;
						spherebeep.PlayOneShot (cloche);
				}
	}
}
