using UnityEngine;
using System.Collections;

public class Shaky_Cam : MonoBehaviour
{
	public GameObject barriere;
	public float strength = 0.3f;

	private bool ouvert, graine;
	private Vector3 shaky, initial;

	// Use this for initialization
	void Start ()
	{	
		initial = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void Update ()
	{
		ouvert = barriere.GetComponent<Ouverture_Porte> ().opened;
		graine = barriere.GetComponent<Ouverture_Porte> ().sesame;

		if ((graine) & (!ouvert))
		{
			shaky = new Vector3 (initial.x + Random.Range (-strength, strength), initial.y + Random.Range (-strength, strength), initial.z + Random.Range (-strength, strength));
			Camera.main.transform.position = shaky;
		}	
	}
}
