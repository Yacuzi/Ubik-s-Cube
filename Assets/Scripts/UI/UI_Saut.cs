using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Saut : MonoBehaviour
{
	public Vector3 PosApparition;

	private Transform Personnage;
	private float maref, alpha;
	private Color color, colorsprite;

	void Start ()
	{	
		alpha = 0;
		Personnage = GameObject.FindGameObjectWithTag ("Player").transform;
		colorsprite = GetComponent<Image> ().color;
	}

	void Update ()
	{	
		//je définis la valeur de l'alpha
		colorsprite.a = alpha;
		GetComponent<Image> ().color = colorsprite;

		if (Personnage.transform.position.x >= PosApparition.x && Personnage.transform.position.y <= PosApparition.y) //Fade in			
		{
			alpha = Mathf.SmoothDamp (GetComponent<Image> ().color.a, 1, ref maref, 0.5F);
		}
		else
		{
			alpha = Mathf.SmoothDamp (GetComponent<Image> ().color.a, 0, ref maref, 0.5F); //Fade out
		}
	}
}