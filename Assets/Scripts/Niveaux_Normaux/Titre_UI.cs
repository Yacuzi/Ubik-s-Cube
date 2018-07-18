using UnityEngine;
using System.Collections;

public class Titre_UI : MonoBehaviour
{

	private float temps;
	public float timer = 2F;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		//j'attends 2 secondes avant de faire disparaître le titre
		temps = temps + Time.deltaTime;
		GetComponent<CanvasRenderer> ().SetAlpha (timer - temps);				
	}
}
