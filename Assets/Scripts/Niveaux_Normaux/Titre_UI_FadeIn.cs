using UnityEngine;
using System.Collections;

public class Titre_UI_FadeIn : MonoBehaviour
{
	public float timer = 2F, speedalpha = 1f;

	private float temps, alpha;

	void Start ()
	{
		GetComponent<CanvasRenderer> ().SetAlpha (0); //Au début c'est invisible
	}

	void Update ()
	{
		if (temps <= timer) //j'attends 2 secondes avant de faire disparaître le titre			
			temps += Time.deltaTime;

		if (temps > timer && GetComponent<CanvasRenderer> ().GetAlpha () < 1)
		{
			alpha += Time.deltaTime * speedalpha;
			GetComponent<CanvasRenderer> ().SetAlpha (alpha);
		}
	}
}