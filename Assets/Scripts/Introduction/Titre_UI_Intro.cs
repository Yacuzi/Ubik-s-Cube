using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Titre_UI_Intro : MonoBehaviour
{
	public float timerbegin = 2F, timerfade = 2F;
	public Transform posarrivee;

	private Vector3 maref = Vector3.zero;
	private float tempsbegin, temps;

	void Update ()
	{
		if (tempsbegin < timerbegin) //J'attends au début avant de faire bouger le texte
		{
			tempsbegin += Time.deltaTime;
		}
		else
		{
			transform.position = Vector3.SmoothDamp (transform.position, posarrivee.position, ref maref, timerbegin * 0.6f); //je déplace le texte vers le centre de l'écran

			if (Vector3.Distance (transform.position, posarrivee.position) <= 1F && temps < timerfade) //Je recentre bien le texte et dis qu'il peut commencer à disparaître			
			{
				temps += Time.deltaTime;
				GetComponent<CanvasRenderer> ().SetAlpha (timerfade - temps); //Je fais dispraître le texte progressivement
			}
		}
	}
}