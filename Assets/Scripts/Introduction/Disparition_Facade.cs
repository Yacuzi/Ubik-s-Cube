using UnityEngine;
using System.Collections;

public class Disparition_Facade : MonoBehaviour {

		public Transform Personnage;
		private float maref, alpha;
		private Color color;

	// Use this for initialization
	void Start () {
	
				alpha = GetComponent<Renderer> ().material.color.a;

	}
	
	// Update is called once per frame
	void Update () {
	
				//je définis la valeur de l'alpha
				color.a = alpha;
				GetComponent<Renderer> ().material.color = color;

				//fade out
				if ((Personnage.transform.position.x > transform.position.x + 4F) & (GetComponent<Renderer> ().material.color.a >= 0)) {
						alpha = Mathf.SmoothDamp (GetComponent<Renderer> ().material.color.a, 0, ref maref, 0.5F);
				}

				//fade in
				if ((Personnage.transform.position.x <= transform.position.x + 4F) & (GetComponent<Renderer> ().material.color.a <= 1)) {
						alpha = Mathf.SmoothDamp (GetComponent<Renderer> ().material.color.a, 1, ref maref, 0.5F);
				}
				
	}
}
