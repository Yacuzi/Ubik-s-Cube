using UnityEngine;
using System.Collections;

public class Titre_UI_FadeIn : MonoBehaviour
{
	public float timer = 3F, speedalpha = 1f;

	[HideInInspector]
	public bool ended;

	private float temps, alpha;

	void Start ()
	{
		GetComponent<CanvasRenderer> ().SetAlpha (0); //Au début c'est invisible
	}

	void Update ()
	{
		if (temps <= timer + speedalpha) //j'attends X secondes avant de faire apparaître le titre			
			temps += Time.deltaTime;
		else
			ended = true;

		if (temps > timer && alpha < 1)
		{
			alpha += Time.deltaTime / speedalpha;
			GetComponent<CanvasRenderer> ().SetAlpha (alpha);
		}
	}
}