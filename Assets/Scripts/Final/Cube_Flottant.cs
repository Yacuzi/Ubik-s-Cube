using UnityEngine;
using System.Collections;

public class Cube_Flottant : MonoBehaviour
{
	private float pingpong, posiniy, tempsping;

	void Start ()
	{
		posiniy = transform.position.y;
	}

	void Update ()
	{		
		//mouvement haut-bas
		tempsping = (transform.position.x * 0.03F) + Time.time;
		pingpong = posiniy - (Mathf.PingPong (tempsping, 5F));
		transform.position = new Vector3 (transform.position.x, pingpong, transform.position.z);
	}
}