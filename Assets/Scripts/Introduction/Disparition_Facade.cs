using UnityEngine;
using System.Collections;

public class Disparition_Facade : MonoBehaviour
{
	public SpriteRenderer SpriteSaut;

	private Transform Personnage;
	private float maref, alpha;
	private Color color, colorsprite;

	void Start ()
	{	
		alpha = GetComponent<Renderer> ().material.color.a;
		Personnage = GameObject.FindGameObjectWithTag ("Player").transform;
		colorsprite = SpriteSaut.material.color;
	}
	
	void Update ()
	{	
		//je définis la valeur de l'alpha
		color.a = alpha;
		colorsprite.a = 1 - alpha;
		GetComponent<Renderer> ().material.color = color;
		SpriteSaut.material.color = colorsprite;

		//fade out
		if ((Personnage.transform.position.x > transform.position.x + 4F) & (GetComponent<Renderer> ().material.color.a >= 0))
		{
			alpha = Mathf.SmoothDamp (GetComponent<Renderer> ().material.color.a, 0, ref maref, 0.5F);
		}

		//fade in
		if ((Personnage.transform.position.x <= transform.position.x + 4F) & (GetComponent<Renderer> ().material.color.a <= 1))
		{
			alpha = Mathf.SmoothDamp (GetComponent<Renderer> ().material.color.a, 1, ref maref, 0.5F);
		}				
	}
}
