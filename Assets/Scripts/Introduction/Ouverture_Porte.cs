using UnityEngine;
using System.Collections;

public class Ouverture_Porte : MonoBehaviour
{
	public GameObject sphere;
	public float timer;

	[HideInInspector]
	public bool sesame, opened;

	private Vector3 ouvert;

	void Start ()
	{
		ouvert = new Vector3 (transform.position.x, transform.position.y - (1.01F * transform.lossyScale.y) - 0.5F, transform.position.z);	
	}
	
	// Update is called once per frame
	void Update ()
	{	
		//je vérifie si la sphère a été prise
		sesame = sphere.GetComponent<Sphere_collectible> ().recupere;

		if (sesame)
			transform.position = Vector3.Lerp (transform.position, ouvert, timer * Time.deltaTime);

		if (Mathf.Abs (transform.position.y - ouvert.y) <= 0.45F)
		{
			GetComponent<BoxCollider>().enabled = false; //Je désactive le collider du cube
			opened = true;
		}
	}
}
