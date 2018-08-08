using UnityEngine;
using System.Collections;

public class Camera_Follower : Ubik_Camera_Smooth
{
	private Vector3 posdep, posperdep;

	// Use this for initialization
	void Start ()
	{
		Perso = GameObject.FindGameObjectWithTag ("Player");

		posdep = Camera.main.transform.position;
		posperdep = Perso.transform.position;
	}

	// Update is called once per frame
	void Update ()
	{
		//je suis le déplacement du personnage
		Camera.main.transform.position = posdep + Perso.transform.position - posperdep;
	}
}
