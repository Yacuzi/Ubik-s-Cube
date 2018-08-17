using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rotations : MonoBehaviour
{

	public Image Largeur, Hauteur, Longueur;

	void Start ()
	{
		Cube_Rotations Rotations = GameObject.FindGameObjectWithTag ("Player").GetComponent<Cube_Rotations> ();

		bool CanLarg = Rotations.Rotation_Largeur;
		bool CanHaut = Rotations.Rotation_Hauteur;
		bool CanLong = Rotations.Rotation_Longueur;

		if (CanLarg)
			Largeur.color = Color.blue;
		if (CanHaut)
			Hauteur.color = Color.yellow;
		if (CanLong)
			Longueur.color = Color.red;

		if (!CanLarg && !CanHaut && !CanLong)
			this.gameObject.SetActive (false);
	}
}
