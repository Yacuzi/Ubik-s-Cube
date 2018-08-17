using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Select_Rangee : MonoBehaviour
{
	public GameObject Selection, Mouvements;
	public bool TutoLarg, TutoHaut, TutoLong;

	private bool Larg, Haut, Long;
	private CanvasRenderer[] lesslections, lesmouvements;

	void HideNShow (CanvasRenderer[] hide, CanvasRenderer[] show)
	{
		foreach (CanvasRenderer lecanvas in hide)
			lecanvas.SetAlpha (0);
		foreach (CanvasRenderer lecanvas in show)
			lecanvas.SetAlpha (1);
	}

	void Start ()
	{
		lesslections = Selection.GetComponentsInChildren<CanvasRenderer> ();
		lesmouvements = Mouvements.GetComponentsInChildren<CanvasRenderer> ();
	}

	void Update ()
	{
		Larg = GameObject.FindGameObjectWithTag ("Player").GetComponent<Cube_Rotations> ().Larg;
		Haut = GameObject.FindGameObjectWithTag ("Player").GetComponent<Cube_Rotations> ().Haut;
		Long = GameObject.FindGameObjectWithTag ("Player").GetComponent<Cube_Rotations> ().Long;

		if (lesslections [0].GetComponent<Titre_UI_FadeIn> ().ended)
		{
			if (TutoLarg)
				if (Larg)
				{
					HideNShow (lesslections, lesmouvements);
				}
				else
				{
					HideNShow (lesmouvements, lesslections);
				}

			if (TutoHaut)
				if (Haut)
				{
					HideNShow (lesslections, lesmouvements);
				}
				else
				{
					HideNShow (lesmouvements, lesslections);
				}

			if (TutoLong)
				if (Long)
				{
					HideNShow (lesslections, lesmouvements);
				}
				else
				{
					HideNShow (lesmouvements, lesslections);
				}
		}
	}
}
