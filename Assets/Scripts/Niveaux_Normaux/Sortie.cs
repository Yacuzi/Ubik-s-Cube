using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sortie : MonoBehaviour
{

	private GameObject Perso;
	private Vector3 regardperso, posperso;
	private Color couleur;
	public string niveausuivant;
	private bool saut, chute;

	// Use this for initialization
	void Start ()
	{
		Perso = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update ()
	{

		//Je récupère la position du personnage et son regard et son état
		posperso = Perso.transform.position;
		regardperso = Perso.GetComponent<Controle_Personnage> ().regard;
		saut = Perso.GetComponent<Controle_Personnage> ().ensaut;
		chute = Perso.GetComponent<Controle_Personnage> ().enchute;
	
		//la sortie "brille" en noir
		if (Vector3.Distance (posperso + regardperso, transform.position) >= 0.5) {
			couleur = Color.Lerp (Color.black, Color.white, Mathf.PingPong (Time.time*0.5f, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		} else if ((!saut) & (!chute)) {
			couleur = Color.Lerp (Color.green, Color.white, Mathf.PingPong (Time.time*0.5f, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		}
				
		//Je charge le niveau suivant
		if ((Input.GetButtonDown ("Skip")) | ((!saut) & (!chute) & (Vector3.Distance (posperso + regardperso, transform.position) <= 0.5) & (Input.GetButtonDown ("Saut"))))
			SceneManager.LoadSceneAsync (niveausuivant);

	}
}
