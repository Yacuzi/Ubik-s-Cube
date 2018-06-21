using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Cube_Rotations_old : MonoBehaviour {

		public Transform God, Perso;
		private GameObject Cube;
		private int rangeex = 0, rangeey=0, rangeez=0;
		private int longueur, hauteur, largeur;
		private Vector3 centrePos, axe, marotation;
		private float centreRot, angletot, anglerot;
		private float milx=0F, mily=0F, milz=0F;
		private float clignoterfloat = 0F, vclignote = 12F;
		private int clignoter = 0;
		private string Name;
		private Renderer[] quads, verres;
		private string temp;
		private Color couleur;
		private int typerangee;
		private bool chute, saut;
		public bool RotationJaune = true, RotationRouge =true, RotationBleu = true ;
		private Vector3 A,B,C,D;
		[HideInInspector]
		public bool Estlarg=false, Esthaut=false, Estlong=false, RotationH=false, RotationAH = false;
		private bool finselectionLargeur=false, finselectionHauteur=false, finselectionLongueur=false;
		private bool rotationpretex=false, rotationpretey=false, rotationpretez=false;
		 
	// Use this for initialization
	void Start () {
				//Récupération de la taille du cube
				hauteur = God.GetComponent<CreationCube> ().Hauteur;
				longueur = God.GetComponent<CreationCube> ().Longueur;
				largeur = God.GetComponent<CreationCube> ().Largeur;

				//Récupération de la taille du cube
				A = GetComponent<Ubik_Camera_Smooth> ().a;
				B = GetComponent<Ubik_Camera_Smooth> ().b;
				C = GetComponent<Ubik_Camera_Smooth> ().c;
				D = GetComponent<Ubik_Camera_Smooth> ().d;

				//Creation du point central pour les centre de rotation
				milx = (float)(((float) longueur / 2) - 0.5F);
				mily = (float)(((float) hauteur / 2) - 0.5F);
				milz = (float)(((float) largeur / 2) - 0.5F);

				//je créé le bloc
				//Bloc = new GameObject ();

		}
	
	// Update is called once per frame
	void Update ()
		{

				//Récupération de la chute
				chute = Perso.GetComponent<Controle_Personnage> ().enchute;
				saut = Perso.GetComponent<Controle_Personnage> ().ensaut;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LARGEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en largeur
				if (((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationJaune) & (transform.position == A))
						|((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationJaune) & (transform.position == C))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationJaune) & (transform.position == B))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationJaune) & (transform.position == D))) {
						Estlarg = true;

						//je fouille tous les cubes de la rangée						
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.z - rangeez < 0.1F) & (Cube.transform.position.z - rangeez > -0.1F)) {
														//		Cube.tag = "Bloc";
														//		Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en jaune
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.yellow;
														}
														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.yellow);
														}
												}
										}
								}
						}
				}

				//si je lache le bouton pour la largeur, je prépare la fin de la selection
				if ((Input.GetButtonUp ("Largeur") & (transform.position == A))|
						(Input.GetButtonUp ("Largeur") & (transform.position == C))|
						(Input.GetButtonUp ("Longueur") & (transform.position == B))|
						(Input.GetButtonUp ("Longueur") & (transform.position == D)))
						finselectionLargeur = true;
				
				if ((finselectionLargeur) & (!RotationH) & (!RotationAH)) {
						Estlarg = false;
						finselectionLargeur = false;

						//je remet les cubes dans leur couleur originelle
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.z-rangeez <0.1F) & (Cube.transform.position.z-rangeez >-0.1F)) {
														//		Cube.tag = "Cube";
														//		Cube.transform.SetParent (null, true);
														//}

														//je remet les cubes à leur couleur originelle
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.white;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.white);
														}
												}
										}
								}
						}
				}

				//Déplacement de la rangée large
				if (Estlarg) {
												
						if ((Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == A))|
								(Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == B))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == C))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == D))) {
								if (rangeez != largeur - 1) {
										rangeez += 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																//je réinitialise le bloc
																if (Cube.transform.position.z-rangeez <0.1F - 1) {
																		//		Cube.tag = "Cube";
																		//		Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}

						} else if ((Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == C))|
								(Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == D))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == A))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == B))) {
								if (rangeez != 0) {
										rangeez -= 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if (Cube.transform.position.z-rangeez <0.1F + 1) {
																		//	Cube.tag = "Cube";
																		//	Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}
						}

						//je créé le bloc et le peint en jaune
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.z-rangeez <0.1F) & (Cube.transform.position.z-rangeez >-0.1F)) {
														//	Cube.tag = "Bloc";
														//	Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en jaune
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.yellow;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.yellow);
														}
												}
										}
								}
						}
				}

				//vérification qu'une rotation est lancée
				if (((Input.GetButtonDown ("RotationH")) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == A))|
						((Input.GetButtonDown ("RotationH")) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == D))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == B))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == C)))
						rotationpretez = true;

				if ((rotationpretez) & ((int)(Perso.position.z + 0.9F) != rangeez) & ((int)(Perso.position.z + 0.1F) != rangeez)) {
						RotationH = true;
						rotationpretez = false;

				} else if (rotationpretez) {
						//pas de rotation de la rangee
						RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.z - rangeez < 0.1F) & (Cube.transform.position.z - rangeez > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.z - rangeez < 0.1F) & (Cube.transform.position.z - rangeez > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.yellow;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.yellow);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretez = false;
						}
				}

				//vérification qu'une rotation est lancée
				if (((Input.GetButtonDown ("RotationAH")) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == A))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == D))|
						((Input.GetButtonDown ("RotationH")) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == B))|
						((Input.GetButtonDown ("RotationH")) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == C)))
						rotationpretez = true;

				if ((rotationpretez) & ((int)(Perso.position.z + 0.9F) != rangeez) & ((int)(Perso.position.z + 0.1F) != rangeez)) {
						RotationAH = true;
						rotationpretez = false;

				} else if (rotationpretez) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.z - rangeez < 0.1F) & (Cube.transform.position.z - rangeez > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.z - rangeez < 0.1F) & (Cube.transform.position.z - rangeez > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.yellow;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.yellow);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretez = false;
						}
				}
				

				//rotation horaire des cubes
				if ((RotationH) & (Estlarg)) {

						//je détermine si les rotations sont de 90° ou 180° en fonction de la taille du "cube"
						if ((largeur != hauteur) | (largeur != longueur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, mily, rangeez);
						axe = new Vector3 (0, 0, 1);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.z-rangeez <0.1F) & (Cube.transform.position.z-rangeez >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}

						//réinitialisation de la rotation
						if ((anglerot + 0.3F >= Mathf.Abs(angletot)) & (Mathf.Abs(angletot) >= anglerot - 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (-anglerot - angletot));
								centreRot = 0;
								RotationH = false;
								angletot = 0;
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Estlarg)) {

						//je détermine si les rotations sont de 90° ou 180° en fonction de la taille du "cube"
						if ((largeur != hauteur) | (largeur != longueur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, mily, rangeez);
						axe = new Vector3 (0, 0, 1);


						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.z-rangeez <0.1F) & (Cube.transform.position.z-rangeez >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}


						//réinitialisation de la rotation
						if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (anglerot - angletot));
								centreRot = 0;
								RotationAH = false;
								angletot = 0;
						}
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++HAUTEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en hauteur
				if ((Input.GetButtonDown ("Hauteur")) & (!Estlarg) & (!Estlong) & (RotationBleu)) {
						Esthaut = true;

						//je créé un bloc rassemblant tous les cubes de la rangée						
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.y-rangeey <0.1F) & (Cube.transform.position.y-rangeey >-0.1F)) {
														// Cube.tag = "Bloc";
														// Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en bleu
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.blue;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.blue);
														}
												}
										}
								}
						}
				}

				//si je lache le bouton pour la largeur, je prépare la fin de la selection
				if (Input.GetButtonUp ("Hauteur"))
						finselectionHauteur = true;
				
				//J'arrête la sélection du bloc en hauteur
				if ((finselectionHauteur) & (!RotationH) & (!RotationAH)) {
						Esthaut = false;
						finselectionHauteur = false;

						//je vide le bloc rassemblant tous les cubes de la rangée
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.y-rangeey <0.1F) & (Cube.transform.position.y-rangeey >-0.1F)) {
														// Cube.tag = "Cube";
														// Cube.transform.SetParent (null, true);
														
														
												}

												//je remet les cubes à leur couleur originelle
												quads = Cube.GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material.color = Color.white;
												}

												//je met la verriere en jaune
												verres = Cube.GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material.SetColor ("_EmissionColor", Color.white);
												}
										}
								}
						}
				}

				//Déplacement de la rangée large
				if (Esthaut) {
												
						if (Input.GetButtonDown ("Haut") & (!RotationH) & (!RotationAH)) {
								if (rangeey != hauteur - 1) {
										rangeey += 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																//je réinitialise le bloc
																if (Cube.transform.position.y-rangeey <0.1F - 1) {
																		// Cube.tag = "Cube";
																		// Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}

						} else if (Input.GetButtonDown ("Bas") & (!RotationH) & (!RotationAH)) {
								if (rangeey != 0) {
										rangeey -= 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if (Cube.transform.position.y-rangeey <0.1F + 1) {
																		// Cube.tag = "Cube";
																		// Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}
						}

						//je créé le bloc et le peint en jaune
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.y-rangeey <0.1F) & (Cube.transform.position.y-rangeey >-0.1F)) {
														// Cube.tag = "Bloc";
														// Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en bleu
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.blue;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.blue);
														}
												}
										}
								}
						}
				}

				//vérification qu'une rotation est lancée
				if ((Input.GetButtonDown ("RotationH")) & (!RotationAH) & (Esthaut) & (!saut) & (!chute))
						rotationpretey = true;

				if ((rotationpretey) & ((int)(Perso.position.y + 0.9F) != rangeey) & ((int)(Perso.position.y + 0.1F) != rangeey)) {
						RotationH = true;
						rotationpretey = false;

				} else if (rotationpretey) {
						//pas de rotation de la rangee
						RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.y - rangeey < 0.1F) & (Cube.transform.position.y - rangeey > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.y - rangeey < 0.1F) & (Cube.transform.position.y - rangeey > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.blue;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.blue);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretey = false;
						}
				}

				//vérification qu'une rotation est lancée
				if ((Input.GetButtonDown ("RotationAH")) & (!RotationH) & (Esthaut) & (!saut) & (!chute))
						rotationpretey = true;

				if ((rotationpretey) & ((int)(Perso.position.y + 0.9F) != rangeey) & ((int)(Perso.position.y + 0.1F) != rangeey)) {
						RotationAH = true;
						rotationpretey = false;

				} else if (rotationpretey) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.y - rangeey < 0.1F) & (Cube.transform.position.y - rangeey > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.y - rangeey < 0.1F) & (Cube.transform.position.y - rangeey > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.blue;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.blue);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretey = false;
						}
				}
				

				//rotation horaire des cubes
				if ((RotationH) & (Esthaut)) {

						if ((hauteur != largeur) | (hauteur != longueur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, rangeey, milz);
						axe = new Vector3 (0, 1, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.y-rangeey <0.1F) & (Cube.transform.position.y-rangeey >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}

						//réinitialisation de la rotation
						if ((anglerot + 0.3F >= Mathf.Abs(angletot)) & (Mathf.Abs(angletot) >= anglerot - 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (-anglerot - angletot));
								centreRot = 0;
								RotationH = false;
								angletot = 0;
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Esthaut)) {

						if ((hauteur != largeur) | (hauteur != longueur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, rangeey, milz);
						axe = new Vector3 (0, 1, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.y-rangeey <0.1F) & (Cube.transform.position.y-rangeey >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}

						//réinitialisation de la rotation
						if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (anglerot - angletot));
								centreRot = 0;
								RotationAH = false;
								angletot = 0;
						}
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LONGUEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en longueur
				if (((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationRouge) & (transform.position == B))
						|((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationRouge) & (transform.position == D))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationRouge) & (transform.position == A))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationRouge) & (transform.position == C))) {
						Estlong = true;

						//je créé un bloc rassemblant tous les cubes de la rangée						
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.x-rangeex <0.1F) & (Cube.transform.position.x-rangeex >-0.1F)) {
														// Cube.tag = "Bloc";
														// Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en rouge
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.red;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.red);
														}
												}
										}
								}
						}
				}

				//si je lache le bouton pour la largeur, je prépare la fin de la selection
				if ((Input.GetButtonUp ("Largeur") & (transform.position == B))|
						(Input.GetButtonUp ("Largeur") & (transform.position == D))|
						(Input.GetButtonUp ("Longueur") & (transform.position == C))|
						(Input.GetButtonUp ("Longueur") & (transform.position == A)))
						finselectionLongueur = true;

				//J'arrête la sélection du bloc en longueur
				if ((finselectionLongueur) & (!RotationH) & (!RotationAH)) {
						Estlong = false;
						finselectionLongueur = false;

						//je vide le bloc rassemblant tous les cubes de la rangée
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												if ((Cube.transform.position.x-rangeex <0.1F) & (Cube.transform.position.x-rangeex >-0.1F)) {
														// Cube.tag = "Cube";
														// Cube.transform.SetParent (null, true);
														
														
												}

												//je remet les cubes à leur couleur originelle
												quads = Cube.GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material.color = Color.white;
												}

												//je met la verriere en jaune
												verres = Cube.GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material.SetColor ("_EmissionColor", Color.white);
												}
										}
								}
						}
				}

				//Déplacement de la rangée large
				if (Estlong) {
												
						if ((Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == B))|
								(Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == C))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == D))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == A))) {
								if (rangeex != longueur - 1) {
										rangeex += 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																//je réinitialise le bloc
																if (Cube.transform.position.x-rangeex <0.1F - 1) {
																		// Cube.tag = "Cube";
																		// Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}

						} else if ((Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == A))|
								(Input.GetButtonDown ("Gauche") & (!RotationH) & (!RotationAH) & (transform.position == D))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == C))|
								(Input.GetButtonDown ("Droite") & (!RotationH) & (!RotationAH) & (transform.position == B))) {
								if (rangeex != 0) {
										rangeex -= 1;

										//je vide le bloc rassemblant tous les cubes de la rangée précédente
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if (Cube.transform.position.x-rangeex <0.1F + 1) {
																		// Cube.tag = "Cube";
																		// Cube.transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}
								}
						}

						//je créé le bloc et le peint en rouge
						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.x-rangeex <0.1F) & (Cube.transform.position.x-rangeex >-0.1F)) {
														// Cube.tag = "Bloc";
														// Cube.transform.SetParent (Bloc.transform, true);

														//je met les cubes en bleu
														quads = Cube.GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material.color = Color.red;
														}

														//je met la verriere en jaune
														verres = Cube.GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material.SetColor ("_EmissionColor", Color.red);
														}
												}
										}
								}
						}
				}

				//vérification qu'une rotation est lancée
				if (((Input.GetButtonDown ("RotationH")) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == A))|
						((Input.GetButtonDown ("RotationH")) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == B))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == C))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == D)))
						rotationpretex = true;

				if ((rotationpretex) & ((int)(Perso.position.x + 0.9F) != rangeex) & ((int)(Perso.position.x + 0.1F) != rangeex)) {
						RotationH = true;
						rotationpretex = false;

				} else if (rotationpretex) {
						//pas de rotation de la rangee
						RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.x - rangeex < 0.1F) & (Cube.transform.position.x - rangeex > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.x - rangeex < 0.1F) & (Cube.transform.position.x - rangeex > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.red;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.red);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretex = false;
						}
				}

				//vérification qu'une rotation est lancée
				if (((Input.GetButtonDown ("RotationAH")) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == A))|
						((Input.GetButtonDown ("RotationAH")) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == B))|
						((Input.GetButtonDown ("RotationH")) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == C))|
						((Input.GetButtonDown ("RotationH")) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == D)))
						rotationpretex = true;

				if ((rotationpretex) & ((int)(Perso.position.x + 0.9F) != rangeex) & ((int)(Perso.position.x + 0.1F) != rangeex)) {
						RotationAH = true;
						rotationpretex = false;

				} else if (rotationpretex) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.x - rangeex < 0.1F) & (Cube.transform.position.x - rangeex > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.white;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.white);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je fouille tous les cubes de la rangée
										for (int i = 0; i < longueur; i++) {
												for (int j = 0; j < hauteur; j++) {
														for (int k = 0; k < largeur; k++) {

																//je parcoure les cubes d'après leur nom
																Name = (i + "_" + j + "_" + k);
																Cube = GameObject.Find (Name);

																if ((Cube.transform.position.x - rangeex < 0.1F) & (Cube.transform.position.x - rangeex > -0.1F)) {
																		//		Cube.tag = "Bloc";
																		//		Cube.transform.SetParent (Bloc.transform, true);

																		//je met les cubes en jaune
																		quads = Cube.GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material.color = Color.red;
																		}

																		//je met la verriere en jaune
																		verres = Cube.GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material.SetColor ("_EmissionColor", Color.red);
																		}
																}
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;
								}
						} else {
								//je réinitialise le clignotement
								clignoter = 0;
								clignoterfloat = 0;
								rotationpretex = false;
						}
				}

				//rotation horaire des cubes
				if ((RotationH) & (Estlong)) {

						if ((longueur != hauteur) | (longueur != largeur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (rangeex, mily, milz);
						axe = new Vector3 (1, 0, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.x-rangeex <0.1F) & (Cube.transform.position.x-rangeex >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}

						//réinitialisation de la rotation
						if ((anglerot + 0.3F >= Mathf.Abs(angletot)) & (Mathf.Abs(angletot) >= anglerot - 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (-anglerot - angletot));
								centreRot = 0;
								RotationH = false;
								angletot = 0;
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Estlong)) {

						if ((longueur != hauteur) | (longueur != largeur))
								anglerot = 180F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (rangeex, mily, milz);
						axe = new Vector3 (1, 0, 0);


						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						for (int i = 0; i < longueur; i++) {
								for (int j = 0; j < hauteur; j++) {
										for (int k = 0; k < largeur; k++) {

												//je parcoure les cubes d'après leur nom
												Name = (i + "_" + j + "_" + k);
												Cube = GameObject.Find (Name);

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((Cube.transform.position.x-rangeex <0.1F) & (Cube.transform.position.x-rangeex >-0.1F)) {
														Cube.transform.RotateAround (centrePos, axe, centreRot);
												}
										}
								}
						}

						//réinitialisation de la rotation
						if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
								Cube.transform.RotateAround (centrePos, axe, (anglerot - angletot));
								centreRot = 0;
								RotationAH = false;
								angletot = 0;
						}
				}
		}
}