using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Sortie_Final_Acte_I : MonoBehaviour {

		public GameObject Perso;
		private Vector3 regardperso, posperso;
		private Color couleur;
		public string niveausuivant;
		private bool saut, chute;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {

				//la sortie "brille" en noir
				if (Vector3.Distance (posperso + regardperso, transform.position) >= 0.5) {
						couleur = Color.Lerp (Color.gray, Color.white, Mathf.PingPong (Time.time, 1));
						this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
				} else if ((!saut) & (!chute)){
						couleur = Color.Lerp (Color.green, Color.white, Mathf.PingPong (Time.time, 1));
						this.gameObject.GetComponent<Renderer> ().material.SetColor ("_EmissionColor", couleur);
				}

				//Je récupère la position du personnage et son regard et son état
				posperso = Perso.transform.position;
				regardperso = Perso.GetComponent<Controle_Personnage_Final_Acte_I> ().regard;
				saut = Perso.GetComponent<Controle_Personnage_Final_Acte_I> ().ensaut;
				chute = Perso.GetComponent<Controle_Personnage_Final_Acte_I> ().enchute;

				//Je charge le niveau suivant
				if ((!saut) & (!chute) & (Vector3.Distance (posperso + regardperso, transform.position) <= 0.5) & (Input.GetButtonDown ("Saut")))
						SceneManager.LoadSceneAsync(niveausuivant);

		}
}
