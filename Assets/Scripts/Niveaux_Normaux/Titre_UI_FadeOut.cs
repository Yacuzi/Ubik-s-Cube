using UnityEngine;
using System.Collections;

public class Titre_UI_FadeOut : MonoBehaviour
{
	public float timer = 2F, speedalpha = 1f;

	private float temps, alpha = 1f;
	
	void Update ()
	{
		if (temps <= timer) //j'attends 2 secondes avant de faire disparaître le titre			
			temps += Time.deltaTime;

		if (temps > timer && GetComponent<CanvasRenderer> ().GetAlpha () > 0)
		{
			GameObject.FindGameObjectWithTag("Player").GetComponent<Can_Act>().canact = true; //Je dis au joueur qu'il peut maintenant agir
			alpha -= Time.deltaTime * speedalpha;
			GetComponent<CanvasRenderer> ().SetAlpha (alpha);
		}
	}
}
