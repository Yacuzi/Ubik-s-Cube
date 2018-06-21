using UnityEngine;
using System.Collections;

public class Camera_Intro : MonoBehaviour {

		public float timer = 10F, zoomfin = 6F;
		private float zoomini;
		private bool arr;
		public GameObject texte;
		[HideInInspector]
		public float rapport;

	// Use this for initialization
	void Start () {
	
				zoomini = Camera.main.orthographicSize;

	}
	
	// Update is called once per frame
	void Update () {
	
				//je vérifie si le texte est arrivé
				arr = texte.GetComponent<Titre_UI_Intro> ().fadok;

				//je zoom comme un gros porc
				if ((arr) & (timer >= 0)) {
						Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, zoomfin, Time.deltaTime);	
						timer = timer - Time.deltaTime;
				}

				//Pour synchroniser la caméra et le titre
				rapport = zoomini / Camera.main.orthographicSize;

	}
}
