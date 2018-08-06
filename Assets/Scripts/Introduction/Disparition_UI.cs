using UnityEngine;
using System.Collections;

public class Disparition_UI : MonoBehaviour
{
	private bool fini;
	public GameObject raster;
	private float temps;
	public float timer;

	void Start ()
	{	
		GetComponent<CanvasRenderer> ().SetAlpha (0);
	}
	
	void Update ()
	{	
		fini = raster.GetComponent<Titre_UI_Intro_Zoom> ().finfade;

		if (fini)
		{
			temps = temps + Time.deltaTime;
			GetComponent<CanvasRenderer> ().SetAlpha (temps / timer);
		}
	}
}
