using UnityEngine;
using System.Collections;

public class Rotation_UI : MonoBehaviour
{
	private bool fini, rot;
	public GameObject raster;
	private float temps;
	public float timer;
	private Quaternion perspective;

	// Use this for initialization
	void Start ()
	{	
		perspective = Quaternion.Euler (60, 0, -45);
	}
	
	// Update is called once per frame
	void Update ()
	{			
		//je récupère l'information que l'animation de départ est terminée
		fini = GameObject.FindGameObjectWithTag ("Player").GetComponent<Can_Act> ().canact;

		//si l'animation est terminée
		if (fini)
		{
			//j'attends le temps du timer
			temps = temps + Time.deltaTime;
			if (temps > timer)
			{
				rot = !rot;
				temps = 0;
			}
		}

		if (rot)
			transform.rotation = Quaternion.Lerp (transform.rotation, perspective, timer * Time.deltaTime);
		else
			transform.rotation = Quaternion.Lerp (transform.rotation, Quaternion.identity, timer * Time.deltaTime);				
	}
}
