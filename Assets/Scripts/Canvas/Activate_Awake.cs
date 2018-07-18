using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate_Awake : MonoBehaviour {

	private GameObject[] titres;

	// Use this for initialization
	void Awake () {

		//Je cherche tous les titres de niveaux
		titres = GameObject.FindGameObjectsWithTag("Titre");

		//J'active tous les titres de niveau
		foreach (GameObject titre in titres)
			titre.GetComponent<Canvas>().enabled = true;

	}
}
