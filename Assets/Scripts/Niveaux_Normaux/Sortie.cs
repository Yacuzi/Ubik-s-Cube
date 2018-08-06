using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sortie : MonoBehaviour
{
	public string niveausuivant;

	private Controle_Personnage Perso;

	bool CharacterFacing () //Si le perso est en face de la sortie comme il faut (pas en chute pas en saut)
	{
		//Je récupère la position du personnage et son regard et son état
		Vector3 posperso = Perso.gameObject.transform.position;
		Vector3 regardperso = Perso.direction * 0.5f;
		bool saut = Perso.ensaut;
		bool chute = Perso.enchute;

		if ((Vector3.Distance (posperso + regardperso, transform.position) <= 0.25f) && (!saut) && (!chute))
			return true;
		else
			return false;	
	}

	void Shine () //Je fais briller la sortie
	{
		Color couleur;

		if (!CharacterFacing ())
		{
			couleur = Color.Lerp (Color.black, Color.white, Mathf.PingPong (Time.time * 0.5f, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		}
		else
		{
			couleur = Color.Lerp (Color.green, Color.white, Mathf.PingPong (Time.time * 0.5f, 1));
			this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
		}
	}

	// Use this for initialization
	void Start ()
	{
		Perso = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controle_Personnage> ();
	}
		
	// Update is called once per frame
	void Update ()
	{	
		Shine ();
				
		//Je charge le niveau suivant
		if (Input.GetButtonDown ("Skip") || (CharacterFacing() && Input.GetButtonDown ("Saut")))
			SceneManager.LoadSceneAsync (niveausuivant);
	}
}
