using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Total_Sphere : MonoBehaviour
{
	public int NbTotSphere;

	public int TotalSphere () //Pour déterminer le nombre total de sphères du joueur
	{
		string Sid = "";
		int nbtotcollec = 0;

		for (int id = 0; id < NbTotSphere; id++)
		{
			Sid = id.ToString ();

			if (PlayerPrefs.GetInt (Sid, 0) == 1)
				nbtotcollec++;				
		}

		return nbtotcollec;
	}

	void Update ()
	{
		if (GetComponent<CanvasRenderer> ().GetAlpha () > 0) //Si j'affiche le nombre de sphère collectées je le met à jour
			GetComponent<Text> ().text = TotalSphere () + " / " + NbTotSphere; //Je change le texte du nombre de sphères collectées comme il se doit
	}
}
