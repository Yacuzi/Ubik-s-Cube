using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sortie_Intro : MonoBehaviour
{

	public GameObject Perso;
	private Vector3 regardperso, posperso;
	private Color couleur;
	public string niveausuivant;
	private bool saut, chute;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

		//Je récupère la position du personnage et son regard et son état
		posperso = Perso.transform.position;
		regardperso = Perso.GetComponent<Controle_Personnage_Intro> ().regard;
		saut = Perso.GetComponent<Controle_Personnage_Intro> ().ensaut;
		chute = Perso.GetComponent<Controle_Personnage_Intro> ().enchute;

		//la sortie "brille" en noir
		if (Vector3.Distance (posperso + regardperso, transform.position) >= 0.5) {
			couleur = Color.Lerp (Color.gray, Color.white, Mathf.PingPong (Time.time, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		} else if ((!saut) & (!chute)) {
			couleur = Color.Lerp (Color.green, Color.white, Mathf.PingPong (Time.time, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		}			

		//Je charge le niveau suivant
		if ((Input.GetButtonDown ("Skip")) | ((!saut) & (!chute) & (Vector3.Distance (posperso + regardperso, transform.position) <= 0.5) & (Input.GetButtonDown ("Saut"))))
			SceneManager.LoadSceneAsync (niveausuivant);

	}
}

