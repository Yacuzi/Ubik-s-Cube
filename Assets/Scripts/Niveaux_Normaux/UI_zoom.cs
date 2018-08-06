using UnityEngine;
using System.Collections;

public class UI_zoom : MonoBehaviour
{
	private bool zoom = false;
	
	// Update is called once per frame
	void Update ()
	{
		//je vérifie s'il y a un zoom
		zoom = Camera.main.GetComponent<Ubik_Camera_Smooth> ().vuesecondaire;

		//si je zoom, je masque l'UI
		if (zoom)
			GetComponent<Canvas> ().enabled = false;
		else
			GetComponent<Canvas> ().enabled = true;	
	}
}
