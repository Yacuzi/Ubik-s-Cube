using UnityEngine;
using System.Collections;

public class Titre_UI_FadeOut : MonoBehaviour
{
	public float timer = 2F, speedalpha = 1f;
	public bool CanActEnabler = true;

	[HideInInspector]
	public bool ended;

	private float temps, alpha = 1f;

	void Update ()
	{
		if (temps <= timer + speedalpha) //j'attends 2 secondes avant de faire disparaître le titre			
			temps += Time.deltaTime;
		else
			ended = true; //J'ai fini de faire disparaitre mon texte

		if (temps > timer && alpha > 0)
		{
			if (CanActEnabler)
				GameObject.FindGameObjectWithTag ("Player").GetComponent<Can_Act> ().canact = true; //Je dis au joueur qu'il peut maintenant agir
			
			alpha -= Time.deltaTime * speedalpha;
			GetComponent<CanvasRenderer> ().SetAlpha (alpha);
		}
	}
}
