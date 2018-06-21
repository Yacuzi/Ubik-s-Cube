using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage_old_2 : MonoBehaviour {

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
		private Vector3 decalagerot = Vector3.zero;
		private float distsaut = 1000F, anglesaut, stoppos, angletot, angletotchute;
		private int kubx, kuby, kubz;
		private int persox, persoy, persoz, pointfinal;
		private int kubsautx, kubsauty, kubsautz;
		private int kubappuix, kubappuiy, kubappuiz;
		private float anglechute;
		private bool persobougex, persobougey, persobougez;
		private float persobougeX, persobougeY, persobougeZ;
		private Vector3 regardgauche, regarddroite;
		//private bool cubegauche, cubedroite;
		[HideInInspector]
		public bool ensaut=false, enchute = false;
		public Vector3 regard;

	// Use this for initialization
	void Start () {
				//initialisation position personnage
				transform.position = new Vector3 (0,-(transform.localScale.y/2),0);
	}
	
	// Update is called once per frame
		void Update () {

				//vérifie si le perso doit bouger à cause d'une rotation
				persobougex = Camera.main.GetComponent<Cube_Rotations> ().persoreplacex;
				persobougey = Camera.main.GetComponent<Cube_Rotations> ().persoreplacey;
				persobougez = Camera.main.GetComponent<Cube_Rotations> ().persoreplacez;

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
																																				
												if ((posfuture.x + (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														
														yacube = true;
														stoppos = kubx;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (stoppos - ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {
												
												//collision avec les bords
												if (posfuture.x + ((transform.localScale.x*0.5F)+0.5F) >= largeur) {
														transform.position = new Vector3 (largeur - ((transform.localScale.x*0.5F)+0.51F), transform.position.y, transform.position.z);
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

												if ((posfuture.x - (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {
												
												//collision avec les bords
												if (posfuture.x - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (-(0.49F - (transform.localScale.x*0.5F)), transform.position.y, transform.position.z);
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

												if ((posfuture.z + (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x*0.5F)+0.5F));
										} else {
														
												//collision avec les bords
												if (posfuture.z + ((transform.localScale.x*0.5F)+0.5F) >= longueur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, longueur - ((transform.localScale.x*0.5F)+0.51F));
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

												if ((posfuture.z - (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x*0.5F)+0.5F));
										} else {
														
												//collision avec les bords
												if (posfuture.z - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x*0.5F)));
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

												if ((posfuture.z - (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														
														yacube = true;
														stoppos = kubz;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x*0.5F)));
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

												if ((posfuture.z + (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z + ((transform.localScale.x*0.5F)+0.5F) >= largeur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x*0.5F)+0.51F));
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

												if ((posfuture.x + (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos - ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + ((transform.localScale.x*0.5F)+0.5F) >= longueur) {
														transform.position = new Vector3 (longueur - ((transform.localScale.x*0.5F)+0.51F), transform.position.y, transform.position.z);
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

												if ((posfuture.x - (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (-(0.49F - (transform.localScale.x*0.5F)), transform.position.y, transform.position.z);
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

												if ((posfuture.x + (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {

														yacube = true;
														stoppos = kubx;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (stoppos - ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + ((transform.localScale.x*0.5F)+0.5F) >= largeur) {
														transform.position = new Vector3 (largeur - ((transform.localScale.x*0.5F)+0.51F), transform.position.y, transform.position.z);
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

												if ((posfuture.x - (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (-(0.49F - (transform.localScale.x*0.5F)), transform.position.y, transform.position.z);
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

												if ((posfuture.z + (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z + ((transform.localScale.x*0.5F)+0.5F) >= longueur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, longueur - ((transform.localScale.x*0.5F)+0.51F));
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

												if ((posfuture.z - (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x*0.5F)));
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

												if ((posfuture.z - (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z - (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {

														yacube = true;
														stoppos = kubz;
												}
										}

										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x*0.5F)));
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

												if ((posfuture.z + (transform.localScale.x*0.5F) <= kubz + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.z + (transform.localScale.x*0.5F) >= kubz - 0.5F)
												    & (posfuture.x - (transform.localScale.x*0.5F) < kubx + 0.499F)
												    & (posfuture.x + (transform.localScale.x*0.5F) > kubx - 0.499F)) {
														yacube = true;
														stoppos = kubz;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x*0.5F)+0.5F));
										} else {

												//collision avec les bords
												if (posfuture.z + ((transform.localScale.x*0.5F)+0.5F) >= largeur) {
														transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x*0.5F)+0.51F));
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

												if ((posfuture.x + (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x + (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos - ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x + ((transform.localScale.x*0.5F)+0.5F) >= longueur) {
														transform.position = new Vector3 (longueur - ((transform.localScale.x*0.5F)+0.51F), transform.position.y, transform.position.z);
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

												if ((posfuture.x - (transform.localScale.x*0.5F) <= kubx + 0.5F)
												    & (persoy == kuby)
												    & (posfuture.x - (transform.localScale.x*0.5F) >= kubx - 0.5F)
												    & (posfuture.z - (transform.localScale.x*0.5F) < kubz + 0.499F)
												    & (posfuture.z + (transform.localScale.x*0.5F) > kubz - 0.499F)) {
														yacube = true;
														stoppos = kubx;
												}
										}
										if (yacube) {
												transform.position = new Vector3 (stoppos + ((transform.localScale.x*0.5F)+0.5F), transform.position.y, transform.position.z);
										} else {

												//collision avec les bords
												if (posfuture.x - (transform.localScale.x*0.5F) <= -0.5F) {
														transform.position = new Vector3 (-(0.49F - (transform.localScale.x*0.5F)), transform.position.y, transform.position.z);
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
								petitsaut = new Vector3 (kubsautx - regard.x - (transform.localScale.x*0.5F), kubsauty + (0.5F - (transform.localScale.y*0.5F)), transform.position.z);
						}

						if (regard.x == -0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x + (transform.localScale.x*0.5F), kubsauty + (0.5F - (transform.localScale.y*0.5F)), transform.position.z);
						}

						if (regard.z == 0.5F) {
								petitsaut = new Vector3 (transform.position.x, kubsauty + (0.5F - (transform.localScale.y*0.5F)), kubsautz - regard.z - (transform.localScale.x*0.5F));
						}

						if (regard.z == -0.5F) {
								petitsaut = new Vector3 (transform.position.x, kubsauty + (0.5F - (transform.localScale.y*0.5F)), kubsautz - regard.z + (transform.localScale.x*0.5F));
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
						transform.position = new Vector3 (petitsaut.x + ((2*transform.localScale.x) * regard.x), petitsaut.y + (transform.localScale.x), petitsaut.z + ((2*transform.localScale.x) * regard.z));
						transform.rotation = Quaternion.identity;
						angletot = 0F;
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "GRAVITE/CHUTE" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				if ((!ensaut) & (transform.position.y <= -(0.5F - (transform.localScale.x * 0.5F)))) {
						transform.position = new Vector3 (transform.position.x, -(0.5F - (transform.localScale.x * 0.5F)), transform.position.z);
						kubappui = true;
						enchute = false;
				} else {
						//je regarde si il y a un cube à gauche
						foreach (GameObject kub in kubs) {

								//je cherche le cube sur lequel repose le perso
								kubx = Mathf.RoundToInt (kub.transform.position.x);
								kuby = Mathf.RoundToInt (kub.transform.position.y);
								kubz = Mathf.RoundToInt (kub.transform.position.z);

								//je définis la gauche
								if (regard.x == 0)
										regardgauche = Vector3.Normalize (new Vector3 (regard.z, 0, 0));
								else
										regardgauche = Vector3.Normalize (new Vector3 (0, 0, regard.x));

								//je trouve l'éventuel cube à gauche
								if ((kubx + regardgauche.x == kubx) & (kubz + regardgauche.z == kubz)
										& (transform.position.y <= kuby + 1F) & (transform.position.y >= kuby + (transform.localScale.x))) {
										//cubegauche = true;
										break;
								}
								else if (!ensaut) {
										//cubegauche = false;
								}
						}

						//je regarde si il y a un cube à droite
						foreach (GameObject kub in kubs) {

								//je cherche le cube sur lequel repose le perso
								kubx = Mathf.RoundToInt (kub.transform.position.x);
								kuby = Mathf.RoundToInt (kub.transform.position.y);
								kubz = Mathf.RoundToInt (kub.transform.position.z);

								//je définis la droite
								if (regard.x == 0)
										regarddroite = Vector3.Normalize (new Vector3 (-regard.z, 0, 0));
								else
										regarddroite = Vector3.Normalize (new Vector3 (0, 0, -regard.x));

								//je trouve l'éventuel cube à droite
								if ((kubx + regarddroite.x == kubx) & (kubz + regarddroite.z == kubz)
										& (transform.position.y <= kuby + 1F) & (transform.position.y >= kuby + (transform.localScale.x))) {
										//cubedroite = true;
										break;
								}
								else if (!ensaut) {
										//cubegauche = false;
								}
						}

						//je cherche si le perso repose encore sur un cube
						foreach (GameObject kub in kubs) {

								//je cherche le cube sur lequel repose le perso
								kubx = Mathf.RoundToInt (kub.transform.position.x);
								kuby = Mathf.RoundToInt (kub.transform.position.y);
								kubz = Mathf.RoundToInt (kub.transform.position.z);
								
								//je cherche s'il y a un cube sous le perso
								if ((!ensaut) & (persox == kubx) & (persoz == kubz)
								    & (transform.position.y <= kuby + 1F) & (transform.position.y >= kuby + (transform.localScale.x))) {

										kubappui = true;
										enchute = false;
										transform.position = new Vector3 (transform.position.x, kuby + ((transform.localScale.x * 0.5F) + 0.5F), transform.position.z);

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
				}

				/*
				//si je veux me compliquer la vie
				if ((cubegauche) & (transform.position.x * regardgauche.x <= (kubappuix  + (0.1F * regardgauche.x)) * regardgauche.x)
						& (transform.position.z  * regardgauche.z <= (kubappuiz  + (0.1F * regardgauche.z)) * regardgauche.z))
				*/

				if ((!kubappui) & (!enchute) & (angletotchute == 0F)) {
						
						//je repositionne le perso pour qu'il soit juste au bord et je défini le point de rotation
						if (regard.x != 0) {
								transform.position = new Vector3 (kubappuix + regard.x, kubappuiy + ((transform.localScale.x * 0.5F) + 0.5F), transform.position.z);
								pchute = new Vector3 (kubappuix + regard.x, kubappuiy + 0.5F, transform.position.z);
						}
						if (regard.z != 0) {
								transform.position = new Vector3 (transform.position.x, kubappuiy + ((transform.localScale.x * 0.5F) + 0.5F), kubappuiz + regard.z);
								pchute = new Vector3 (transform.position.x, kubappuiy + 0.5F, kubappuiz + regard.z);
						}
				}

				if ((!kubappui) & (!enchute)) {

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

//++++++++++++++++++++++++++++++++++++++++++++++++ "Recentrer le perso si un peu dans la rangée" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//je définis les positions pour se recentrer
				persobougeX = Mathf.RoundToInt(transform.position.x);
				persobougeY = Mathf.RoundToInt(transform.position.y);
				persobougeZ = Mathf.RoundToInt(transform.position.z);

				//recentrer si dans la largeur (X)
				if (persobougex) {
						decalagerot = new Vector3 (persobougeX, transform.position.y ,transform.position.z);
						transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
				}
				//recentrer si dans la largeur (Y)
				if (persobougey) {
						decalagerot = new Vector3 (transform.position.x, persobougeY ,transform.position.z);
						transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
				}
				//recentrer si dans la largeur (Z)
				if (persobougez) {
						decalagerot = new Vector3 (transform.position.x, transform.position.y , persobougeZ);
						transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
				}
						

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "RESET" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				retry = SceneManager.GetActiveScene ().name;

				if (Input.GetButtonDown ("Reset")) {
						SceneManager.LoadScene (retry);
				}
		}
}