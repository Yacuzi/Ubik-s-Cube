using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sortie : MonoBehaviour
{
	private Controle_Personnage Perso;

	bool CharacterFacing () //Si le perso est en face de la sortie comme il faut (pas en chute pas en saut)
	{
		//Je récupère la position du personnage et son regard et son état
		Vector3 posperso = Perso.gameObject.transform.position;
		bool saut = Perso.ensaut;
		bool chute = Perso.enchute;

		if ((Vector3.Distance (posperso, transform.position) <= 0.55f) && (!saut) && (!chute) && Perso.directiontemp == Vector3.zero)
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

	void ColorDoor () //Je colore en vert la sortie si le perso est devant
	{
		if (CharacterFacing())
			this.gameObject.GetComponent<Renderer> ().material.color = Color.green;
		else
			this.gameObject.GetComponent<Renderer> ().material.color = Color.white;
	}

	// Use this for initialization
	void Start ()
	{
		Perso = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controle_Personnage> ();
	}

	bool CheatCodePlus () //Pour aller un niveau après
	{
		int CurrentScene = SceneManager.GetActiveScene().buildIndex;
		int TotalScene = SceneManager.sceneCountInBuildSettings;

		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.RightArrow) && CurrentScene + 1 < TotalScene)
			return true;
		else
			return false;
	}

	bool CheatCodeMinus () //Pour aller un niveau avant
	{
		int CurrentScene = SceneManager.GetActiveScene().buildIndex;
		int TotalScene = SceneManager.sceneCountInBuildSettings;

		if (Input.GetKey (KeyCode.LeftControl) && Input.GetKey (KeyCode.LeftArrow) && CurrentScene - 1 > 0)
			return true;
		else
			return false;
	}
		
	// Update is called once per frame
	void Update ()
	{	
		//Shine (); //Je fais briller la sortie
			
		ColorDoor (); //Je colore en vert la sortie si le perso est devant

		if (CheatCodePlus () || (CharacterFacing () && Input.GetButtonDown ("Saut"))) //Je charge le niveau suivant
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
		if (CheatCodeMinus ())//Je charge le niveau précédent
		    SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex - 1);
	}
}
