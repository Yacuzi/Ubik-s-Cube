using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage_old : MonoBehaviour {

		public Transform God;
		public float vitesse = 5F, vpetitsaut = 1F, gravite = 1F;
		private Renderer kubcolor;
		private string retry="rien";
		private bool Rotx, Roty, Rotz, yacube, sautpret, bloque;
		private bool devant=true, derriere=true, gauche=true, droite=true, kubappui = true;
		private int largeur, hauteur, longueur;
		private Vector3 a,b,c,d, abis, bbis, cbis, dbis;
		private Vector3 posfuture;
		private Vector3 possaut, rotsaut, axesaut, petitsaut, velocity = Vector3.zero;
		private Vector3 pchute, axechute;
		private float distsaut = 1000F, anglesaut, stoppos, angletot, angletotchute;
		private int kubx, kuby, kubz;
		private int persox, persoy, persoz, pointfinal;
		private int kubsautx, kubsauty, kubsautz;
		private int kubappuix, kubappuiy, kubappuiz;
		private float anglechute;
		[HideInInspector]
		public bool ensaut=false, enchute = false;
		public Vector3 regard;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
		void Update () {

				//recherche de tous les kubs
				GameObject[] kubs = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[];

				//récupération de l'état de la rotation
				Rotx = Camera.main.GetComponent<Cube_Rotations> ().Estlarg;
				Roty = Camera.main.GetComponent<Cube_Rotations> ().Esthaut;
				Rotz = Camera.main.GetComponent<Cube_Rotations> ().Estlong;

				//récupération de l'état de la caméra
				pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().finpoin;

				//récupération de la longueur largeur et hauteur
				largeur = God.GetComponent<CreationCube> ().Largeur;
				hauteur = God.GetComponent<CreationCube> ().Hauteur;
				longueur = God.GetComponent<CreationCube> ().Longueur; 

				//position arrondie à l'entier du perso
				persox = Mathf.RoundToInt (transform.position.x);
				persoy = Mathf.RoundToInt (transform.position.y);
				persoz = Mathf.RoundToInt (transform.position.z);

				//récupération des touches
				if (Input.GetButton ("Haut")) {
						devant = true;
				} else
						devant = false;

				if (Input.GetButton ("Bas")) {
						derriere = true;
				} else
						derriere = false;

				if (Input.GetButton ("Gauche")) {
						gauche = true;
				} else
						gauche = false;

				if (Input.GetButton ("Droite")) {
						droite = true;
				} else
						droite = false;
				
				if ((!Rotx) ^ (!Roty) ^ (!Rotz)) {
	
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point a++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

						if (pointfinal == 0) {

								//Les quatre directions

								if ((devant) & (!gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {
												
												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);
																																				
												if ((posfuture.x + 0.4F >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + 0.4F <= kubx + 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														
														yacube = true;
														stoppos = kubx;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (stoppos - 0.9F, transform.position.y, transform.position.z);
										} else {
												
												//collision avec les bords
												if (posfuture.x + 0.9F >= largeur) {
														transform.position = new Vector3 (largeur - 0.91F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;
								}
								
								if ((!devant) & (!gauche) & (!droite) & (derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (-0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x - 0.4F <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - 0.4F >= kubx - 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + 0.9F, transform.position.y, transform.position.z);
										} else {
												
												//collision avec les bords
												if (posfuture.x - 0.4F <= -0.5F) {
														transform.position = new Vector3 (-0.09F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}

										yacube = false;	
								}

								if ((!devant) & (gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, 0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z + 0.4F >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + 0.4F <= kubz + 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - 0.9F);
										} else {
														
												//collision avec les bords
												if (posfuture.z + 0.9F >= longueur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, longueur - 0.91F);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;	
								}
								
								if ((!devant) & (!gauche) & (droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, -0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z - 0.4F <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - 0.4F >= kubz - 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + 0.9F);
										} else {
														
												//collision avec les bords
												if (posfuture.z - 0.4F <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -0.09F);
												} else {
														transform.position = posfuture;
												}

										}
								}
								yacube = false;	
						}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point b++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

						if (pointfinal == 1) {

								//Les quatre directions

								if ((devant) & (!gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, -0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z - 0.4F >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - 0.4F <= kubz + 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														
														yacube = true;
														stoppos = kubz;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z - 0.4F <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -0.09F);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;
								}

								if ((!devant) & (!gauche) & (!droite) & (derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, 0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z + 0.4F <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + 0.4F >= kubz - 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z + 0.9F >= largeur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - 0.91F);
												} else {
														transform.position = posfuture;
												}
										}

										yacube = false;	
								}

								if ((!devant) & (gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x + 0.4F >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + 0.4F <= kubx + 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos - 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + 0.9F >= longueur) {
														transform.position = new Vector3 (longueur - 0.91F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;	
								}

								if ((!devant) & (!gauche) & (droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (-0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x - 0.4F <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - 0.4F >= kubx - 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - 0.4F <= -0.5F) {
														transform.position = new Vector3 (-0.09F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}

										}
								}
								yacube = false;	
						}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point c++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

						if (pointfinal == 2) {

								//Les quatre directions

								if ((!devant) & (!gauche) & (!droite) & (derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x + 0.4F >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + 0.4F <= kubx + 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {

														yacube = true;
														stoppos = kubx;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (stoppos - 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + 0.9F >= largeur) {
														transform.position = new Vector3 (largeur - 0.91F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;
								}

								if ((devant) & (!gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (-0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x - 0.4F <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - 0.4F >= kubx - 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - 0.4F <= -0.5F) {
														transform.position = new Vector3 (-0.09F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}

										yacube = false;	
								}

								if ((!devant) & (!gauche) & (droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, 0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z + 0.4F >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + 0.4F <= kubz + 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z + 0.9F >= longueur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, longueur - 0.91F);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;	
								}

								if ((!devant) & (gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, -0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z - 0.4F <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - 0.4F >= kubz - 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z - 0.4F <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -0.09F);
												} else {
														transform.position = posfuture;
												}

										}
								}
								yacube = false;	
						}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point d++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

						if (pointfinal == 3) {

								//Les quatre directions

								if ((!devant) & (!gauche) & (!droite) & (derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, -0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z - 0.4F >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - 0.4F <= kubz + 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {

														yacube = true;
														stoppos = kubz;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z - 0.4F <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -0.09F);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;
								}

								if ((devant) & (!gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
										regard = new Vector3 (0, 0, 0.5F);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.z + 0.4F <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + 0.4F >= kubz - 0.5F)
												    & (posfuture.x - 0.4F < kubx + 0.499F)
												    & (posfuture.x + 0.4F > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - 0.9F);
										} else {

												//collision avec les bords
												if (posfuture.z + 0.9F >= largeur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - 0.91F);
												} else {
														transform.position = posfuture;
												}
										}

										yacube = false;	
								}

								if ((!devant) & (!gauche) & (droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x + 0.4F >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + 0.4F <= kubx + 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos - 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + 0.9F >= longueur) {
														transform.position = new Vector3 (longueur - 0.91F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}
										}
										yacube = false;	
								}

								if ((!devant) & (gauche) & (!droite) & (!derriere) & (!ensaut) & (kubappui)) {
										posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
										regard = new Vector3 (-0.5F, 0, 0);

										//collision avec les kubs
										foreach (GameObject kub in kubs) {

												kubx = Mathf.RoundToInt (kub.transform.position.x);
												kuby = Mathf.RoundToInt (kub.transform.position.y);
												kubz = Mathf.RoundToInt (kub.transform.position.z);

												if ((posfuture.x - 0.4F <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - 0.4F >= kubx - 0.5F)
												    & (posfuture.z - 0.4F < kubz + 0.499F)
												    & (posfuture.z + 0.4F > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + 0.9F, transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - 0.4F <= -0.5F) {
														transform.position = new Vector3 (-0.09F, transform.position.y, transform.position.z);
												} else {
														transform.position = posfuture;
												}

										}
								}
								yacube = false;	
						}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "SAUT" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

						//préparation du saut
						possaut = transform.position + regard;
						distsaut = 1000F;
						sautpret = false;

						//Détermination du kub sur lequel le joueur peut sauter
						foreach (GameObject kub in kubs) {

								//distance entre le perso en prenant en compte son sens de rotation et le kub
								distsaut = Vector3.Distance (possaut, kub.transform.position);

								//je cherche la distance minimum
								if ((distsaut <= 0.5F) & (Mathf.RoundToInt (kub.transform.position.y) == persoy)) {
										
										//coordonnées du kub le plus proche
										kubsautx = Mathf.RoundToInt (kub.transform.position.x);
										kubsauty = Mathf.RoundToInt (kub.transform.position.y);
										kubsautz = Mathf.RoundToInt (kub.transform.position.z);

										//je réinitialise l'état de blocage du saut
										bloque = false;

										foreach (GameObject kub2 in kubs) {
												//il y a un cube au dessus du cube de saut ou au dessus du joueur
												if (((Mathf.RoundToInt (kub2.transform.position.y) - 1F == kubsauty)
												    & (Mathf.RoundToInt (kub2.transform.position.x) == kubsautx)
												    & (Mathf.RoundToInt (kub2.transform.position.z) == kubsautz))
												    ^ ((Mathf.RoundToInt (kub2.transform.position.y) - 1F == persoy)
												    & (Mathf.RoundToInt (kub2.transform.position.x) == persox)
												    & (Mathf.RoundToInt (kub2.transform.position.z) == persoz))
												    ^ (persoy + 1F == hauteur))
														//le saut est bloqué par un cube
														bloque = true;
										}

										//je colorie le bloc sur lequel on peut sauter en vert
										if (!bloque) {
												sautpret = true;
												kubcolor = kub.GetComponent<Renderer> ();
												kubcolor.material.color = Color.green;
										} else {
												//saut pas pret et cube colorié en blanc
												sautpret = false;
												kubcolor = kub.GetComponent<Renderer> ();
												kubcolor.material.color = Color.white;
										}
								} else {
										//saut pas pret et cube colorié en blanc
										kubcolor = kub.GetComponent<Renderer> ();
										kubcolor.material.color = Color.white;								
								}
						}																		
				}

				if (((!Rotx) ^ (!Roty) ^ (!Rotz))
				    & (Input.GetButtonDown ("Saut"))
				    & (!ensaut)
				    & (sautpret)) {

						//je déclare le perso comme en saut
						ensaut = true;

						//je trouve le point de rotation
						rotsaut = new Vector3 (kubsautx - regard.x, kubsauty + 0.5F, kubsautz - regard.z);

						//je trouve l'axe
						if (regard.x == 0.5F) {
								axesaut = new Vector3 (0, 0, -1);
						}
						if (regard.x == -0.5F) {
								axesaut = new Vector3 (0, 0, 1);
						}
						if (regard.z == 0.5F) {
								axesaut = new Vector3 (1, 0, 0);
						}
						if (regard.z == -0.5F) {
								axesaut = new Vector3 (-1, 0, 0);
						}

						//je choisis la destination pour que le kub fasse un petit saut pour atteindre le bord du bloc
						if (regard.x == 0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x - 0.4F, kubsauty + 0.1F, kubsautz);
						}

						if (regard.x == -0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x + 0.4F, kubsauty + 0.1F, kubsautz);
						}

						if (regard.z == 0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x, kubsauty + 0.1F, kubsautz - regard.z - 0.4F);
						}

						if (regard.z == -0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x, kubsauty + 0.1F, kubsautz - regard.z + 0.4F);
						}

						//je fais le petit saut de préparation
						transform.position = Vector3.SmoothDamp (transform.position, petitsaut, ref velocity, vpetitsaut * Time.deltaTime);

				}

				if (ensaut) {
						
						//angle progressif de rotation
						anglesaut = Mathf.LerpAngle (0F, 180F, 0.05F);
						angletot = angletot + anglesaut;

						//je fais la rotation
						transform.RotateAround (rotsaut, axesaut, anglesaut);
				}

				//réinitialisation des paramètres
				if ((ensaut) & (angletot <= 181F) & (angletot >= 179F)) {
						ensaut = false;
						transform.position = new Vector3 (petitsaut.x + (1.6F * regard.x), petitsaut.y + 0.8F, petitsaut.z + (1.6F * regard.z));
						transform.rotation = Quaternion.identity;
						angletot = 0F;
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "GRAVITE/CHUTE" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				if ((!ensaut) & (transform.position.y <= -0.1F)) {
						transform.position = new Vector3 (transform.position.x, -0.1F, transform.position.z);
						kubappui = true;
						enchute = false;
				} else
						foreach (GameObject kub in kubs) {

								//je cherche le cube sur lequel repose le perso
								kubx = Mathf.RoundToInt (kub.transform.position.x);
								kuby = Mathf.RoundToInt (kub.transform.position.y);
								kubz = Mathf.RoundToInt (kub.transform.position.z);

								//je cherche s'il y a un cube sous le perso
								if ((!ensaut) & (persox == kubx) & (persoz == kubz)
								    & (transform.position.y <= kuby + 1F) & (transform.position.y >= kuby + 0.8F)) {

										kubappui = true;
										enchute = false;
										transform.position = new Vector3 (transform.position.x, kuby + 0.9F, transform.position.z);

										//je trouve les coordonnées du cube sur lequel repose le perso
										kubappuix = Mathf.RoundToInt (kub.transform.position.x);
										kubappuiy = Mathf.RoundToInt (kub.transform.position.y);
										kubappuiz = Mathf.RoundToInt (kub.transform.position.z);

										//je sort de la boucle
										break;

								} else if (!ensaut) {
										kubappui = false;
								}
						}

				if ((!kubappui) & (!enchute) & (angletotchute == 0F)) {
						//je repositionne le perso pour qu'il soit juste au bord
						transform.position = new Vector3 (kubappuix + regard.x, kubappuiy + 0.9F, kubappuiz + regard.z);
				}

				if ((!kubappui) & (!enchute)) {
						
						//définition du point de rotation pour la chute
						pchute = new Vector3 (kubappuix + regard.x, kubappuiy + 0.5F, kubappuiz + regard.z);

						//définition de l'axe de rotation pour la chute
						if (regard.x == 0.5F) {
								axechute = new Vector3 (0, 0, -1);
						}
						if (regard.x == -0.5F) {
								axechute = new Vector3 (0, 0, 1);
						}
						if (regard.z == 0.5F) {
								axechute = new Vector3 (1, 0, 0);
						}
						if (regard.z == -0.5F) {
								axechute = new Vector3 (-1, 0, 0);
						}

						//définition de la rotation de la chute
						anglechute = Mathf.LerpAngle (0F, 90F, 0.2F);
						angletotchute = angletotchute + anglechute;

						//je fait la rotation
						transform.RotateAround (pchute, axechute, anglechute);

						//le perso a fini de pivoter pour chuter
						if ((angletotchute <= 91F) & (angletotchute >= 89F)) {
								transform.rotation = Quaternion.identity;		
								enchute = true;
								angletotchute = 0F;
						}
				}

				//le perso chute
				if (enchute)
						transform.Translate (Vector3.down * gravite * Time.deltaTime);

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "RESET" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				retry = SceneManager.GetActiveScene ().name;

				if (Input.GetButtonDown ("Reset")) {
						Debug.Log (retry);
						SceneManager.LoadScene (retry);
				}
		}
}