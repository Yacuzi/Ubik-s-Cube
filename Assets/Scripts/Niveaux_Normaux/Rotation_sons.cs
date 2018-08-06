using UnityEngine;
using System.Collections;

public class Rotation_sons : MonoBehaviour
{
	private Cube_Rotations Perso;
	private int alea;
	private AudioSource[] sonrotation;
	private AudioClip leson;
	private bool sonencours;

	// Use this for initialization
	void Start ()
	{
		//Je dis où est ma source audio
		sonrotation = GetComponents<AudioSource> ();
		//Je dis où est le perso
		Perso = GameObject.FindGameObjectWithTag ("Player").GetComponent<Cube_Rotations> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Perso.RotationOn() && !sonencours) //Je regarde s'il y a une rotation et si le son est en cours et je joue un son au hasard
			
		{
			sonencours = true;
			alea = (int)Random.Range (0, 6);
			leson = sonrotation [alea].clip;
			sonrotation [alea].PlayOneShot (leson);
		}

		if (!Perso.RotationOn()) //Je réinitialise les sons de rotation
			sonencours = false;
	}
}
