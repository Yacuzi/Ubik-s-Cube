using UnityEngine;
using System.Collections;

public class Rotation_sons : MonoBehaviour
{
	private GameObject Perso;
	private bool rotation1, rotation2;
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
		Perso = GameObject.FindGameObjectWithTag("Player");

	}
	
	// Update is called once per frame
	void Update ()
	{
	
		//je définis les variables de rotation
		rotation1 = Perso.GetComponent<Cube_Rotations> ().RotationH;
		rotation2 = Perso.GetComponent<Cube_Rotations> ().RotationAH;

		//je regarde s'il y a une rotation et si le son est en cours et je joue un son au hasard
		if (((rotation1) | (rotation2)) & (!sonencours))
		{
			sonencours = true;
			alea = (int)Random.Range (0, 6);
			leson = sonrotation [alea].clip;
			sonrotation [alea].PlayOneShot (leson);
		}

		if ((!rotation1) & (!rotation2))
			sonencours = false;
	}
}
