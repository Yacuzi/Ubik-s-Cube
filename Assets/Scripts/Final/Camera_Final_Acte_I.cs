using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Camera_Final_Acte_I : MonoBehaviour {

		public Transform God, Perso, Planete;
		public float smoothTimeregard, smoothTimeeffet;
		private float persox, persoy, persoz;
		private bool saut = false;
		[HideInInspector]
		public bool effetdramatique = false, fineffetdramatique = false, abouge = false;
		private float fieldofview;
		private float camerapanx, camerapany;
		private float velocity;
		private Vector3 positionposteffet;

		// Use this for initialization
		void Start () {
				
		}

		// Update is called once per frame
		void Update () {
				
				//je récupère la position du perso
				persox = Perso.position.x;
				persoy = Perso.position.y;
				persoz = Perso.position.z;

				//je récupère l'état du perso
				saut = Perso.GetComponent<Controle_Personnage_Final_Acte_I> ().ensaut;

				//je regarde si le joueur bouge après le plan compensé
				if ((Input.anyKeyDown) & (fineffetdramatique))
						abouge = true;

				//caractéristiques caméra fonction marche sur laquelle est le personnage avant l'effet dramamtique
				if ((persox <= 5.9F) & (!effetdramatique) & (!fineffetdramatique)) {
						
						Quaternion targetRotation = Quaternion.LookRotation (Planete.transform.position - transform.position);

						// Smoothly rotate towards the target point.
						transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, smoothTimeregard * Time.deltaTime);
						transform.position = new Vector3 (persox - 2F - (2.7F * 0.1666666F * (persoy + 0.1F)), persoy + 0.9F + (0.1F * (persoy + 0.1F)), persoz);
				}

				//caractéristiques caméra fonction marche sur laquelle est le personnage après l'effet dramamtique
				if ((persox > 6.9F) & (!effetdramatique) & (fineffetdramatique) & (abouge)) {
						
						Quaternion targetRotation = Quaternion.LookRotation(Perso.transform.position - transform.position);

						// Smoothly rotate towards the target point.
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTimeregard * Time.deltaTime);

						positionposteffet = new Vector3 (persox - 21.1F + (2.8225F * (persox - 5.9F)), persoy + 12.25F + (-0.2666666F * (persoy - 5.9F)), persoz);
						transform.position = Vector3.Lerp (transform.position, positionposteffet, smoothTimeregard*Time.deltaTime);
				}

				//caractéristiques caméra fonction marche sur laquelle est le personnage après l'effet dramamtique
				if ((persox >= 5.9) & (persox < 6.9F) & (!effetdramatique) & (fineffetdramatique) & (abouge)) {

						Quaternion targetRotation = Quaternion.LookRotation(Planete.transform.position - transform.position);

						// Smoothly rotate towards the target point.
						transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, smoothTimeregard * Time.deltaTime);

						positionposteffet = new Vector3 (persox - 21.1F + (2.8225F * (persox - 5.9F)), persoy + 0.9F + (0.1F * (persoy + 0.1F)), persoz);
						transform.position = Vector3.Lerp (transform.position, positionposteffet, smoothTimeregard*Time.deltaTime);
				}

				//caractéristiques caméra fonction marche sur laquelle est le personnage avant l'effet dramamtique
				if ((persox < 5.9F) & (!effetdramatique) & (fineffetdramatique) & (abouge)) {

						Quaternion targetRotation = Quaternion.LookRotation (Perso.transform.position - transform.position);

						// Smoothly rotate towards the target point.
						transform.rotation = Quaternion.Slerp (transform.rotation, targetRotation, smoothTimeregard * Time.deltaTime);
						positionposteffet = new Vector3 (persox - 2F - (4.7F * (persoy + 0.1F)), persoy + 0.9F + (2F * (persoy + 0.1F)), persoz);
						transform.position = Vector3.Lerp (transform.position, positionposteffet, smoothTimeregard*Time.deltaTime);
				}

				//je fais le plan compensé au sommet de l'escalier
				if ((Mathf.RoundToInt (persoy) == 6) & (!saut) & (!fineffetdramatique)) {
						effetdramatique = true;
						Camera.main.fieldOfView = Camera.main.fieldOfView - smoothTimeeffet * Time.deltaTime;
						camerapanx = (35.35F * ((Camera.main.fieldOfView - 12.06F)/(Camera.main.fieldOfView + 1.072F))) - 28.52F;
						camerapany = (-0.0133F * Camera.main.fieldOfView) + 8.4145F;
						transform.position = new Vector3 (camerapanx, camerapany, transform.position.z);
				}

				if (Camera.main.fieldOfView - 20F <= 0) {
						Camera.main.fieldOfView = 20F;
						fineffetdramatique = true;
						effetdramatique = false;
				}

		}
}