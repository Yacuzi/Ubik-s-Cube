using UnityEngine;
using System.Collections;

public class Titre_UI_Intro_Zoom : MonoBehaviour
{

	private Vector3 echdebut;
	public float timer = 2F, timerech = 10F;
	private float temps, Rapport;
	private bool arr, arr2, arr3;
	public GameObject texte;
	[HideInInspector]
	public bool finfade;

	// Use this for initialization
	void Start ()
	{

		GetComponent<CanvasRenderer> ().SetAlpha (0F);
		echdebut = transform.localScale;

	}
	
	// Update is called once per frame
	void Update ()
	{
	
		//je vérifie si le texte a bougé ou il faut
		arr = texte.GetComponent<Titre_UI_Intro> ().depok;
		arr2 = texte.GetComponent<Titre_UI_Intro> ().fadok;

		//pour la synchro avec le zoom de la caméra
		Rapport = Camera.main.GetComponent<Camera_Intro> ().rapport;

		//je fais disparaître le texte et j'affiche le raster
		if ((arr) & (temps <= timer)) {
			temps = temps + Time.deltaTime;
			GetComponent<CanvasRenderer> ().SetAlpha (temps / timer);
			if (temps >= timer) {
				temps = 0;
				texte.GetComponent<Titre_UI_Intro> ().depok = false;
			}
		}
					
		//je zoom comme un gros taré jusqu'à atteindre le bon zoom
		if (arr2)
			transform.localScale = new Vector3 (echdebut.x * Rapport, echdebut.y * Rapport, echdebut.z * Rapport);

		if (Mathf.Abs (Camera.main.orthographicSize - Camera.main.GetComponent<Camera_Intro> ().zoomfin) <= 0.05F) {
			//le temps défile
			temps = temps + Time.deltaTime;
			arr2 = false;
			arr3 = true;
			GetComponent<CanvasRenderer> ().SetAlpha (timer - temps);
		}

		//je signale que l'animation est totalement finie
		if ((arr3) & (temps >= timer))
			finfade = true;
	}
}
