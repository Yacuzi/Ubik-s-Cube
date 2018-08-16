using UnityEngine;
using System.Collections;

public class Sphere_collectible : MonoBehaviour
{
	private float posiniy;
	private AudioSource spherebeep;
	private AudioClip cloche;
	[HideInInspector]
	public bool recupere;
	private SaveQuit saveme;

	void TurnAround () //animation de la sphère
	{
		float pingpong;

		//rotation
		transform.RotateAround (transform.position, Vector3.up, Time.deltaTime * 30F);
		//mouvement haut-bas
		pingpong = posiniy + (Mathf.PingPong (Time.time * 0.2F, 0.4F));
		transform.position = new Vector3 (transform.position.x, pingpong, transform.position.z);
	}

	void Start ()
	{
		saveme = Camera.main.GetComponent<SaveQuit> (); //Je récupère le composant de sauvegarde

		if (saveme.GetSphere ())
			this.gameObject.SetActive (false);

		//J'initialise la sphère
		posiniy = transform.position.y;
		spherebeep = GetComponent<AudioSource> ();
		cloche = spherebeep.clip;
		recupere = false;
	}

	void Update ()
	{
		TurnAround ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.tag == "Player")
			//si égal à la position de la sphère, alors elle disparaît
			if (!recupere)
			{
				recupere = true;
				spherebeep.PlayOneShot (cloche);
				saveme.SaveSphere ();
				GetComponent<Renderer> ().enabled = false;
			}
	}
}