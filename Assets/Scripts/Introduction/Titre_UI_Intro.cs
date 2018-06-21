using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Titre_UI_Intro : MonoBehaviour {

		public float timer = 2F, timerfade = 2F;
		private Vector3 posarrivee, maref = Vector3.zero;
		[HideInInspector]
		public bool depok = false, fadok = false;
		private bool passeunefois;
		private float temps;

		// Use this for initialization
		void Start () {

				posarrivee = new Vector3 (transform.position.x - (0.6F * transform.lossyScale.x), transform.position.y - (41.25F * transform.lossyScale.y),0F);
		
		}

		// Update is called once per frame
		void Update () {

				//je déplace le texte vers le centre de l'écran
				transform.position = Vector3.SmoothDamp (transform.position, posarrivee,ref maref, timer);

				//je fais disparaître le texte et j'affiche le raster
				if ((Vector3.Distance (transform.position, posarrivee) <= 1F) & (!passeunefois)) {
						transform.position = posarrivee;
						depok = true;
						passeunefois = true;
				}
				if (Vector3.Distance (transform.position, posarrivee) <= 1F){
						temps = temps + Time.deltaTime;
						GetComponent<CanvasRenderer> ().SetAlpha (timerfade - temps);
				}					

				if ((temps >= timerfade) & (!fadok)) {
						fadok = true;
				}
		}
}