using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage_Intro : Controle_Personnage
{
	public bool canjump, CanAct;

	protected override bool Jumpable ()	//Fonction pour voir si le perso peut bouger et si oui, comment
	{
		if (kubobstacle.transform.tag != "Cube" && kubobstacle.transform.tag != "Verriere" && canjump)
			return true;
		else
			return false;
	}

	new void Start ()
	{
		//Je cache le curseur
		Cursor.visible = false;
		//Je récupère le gameobject du dieu
		God = GameObject.FindGameObjectWithTag ("God");
		//J'initialise ma prochaine case
		nextcase = transform.position;
		//Je déclare d'où je récupère les infos et méthodes sur les kubs
		Kubinfos = GetComponent<Cube_Rotations> ();
		if (CanAct) //Je dis que le perso peut bouger direct si besoin est
			GetComponent<Can_Act> ().canact = true;
	}
}