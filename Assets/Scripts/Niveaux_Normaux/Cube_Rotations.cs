using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Cube_Rotations : MonoBehaviour {

		public Transform God, Perso;
		private GameObject[] Cubes, Antikub, Kubs, TousCubes;
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
		public bool RotationBleue = true, RotationRouge =true, RotationJaune = true ;
		private Vector3 A,B,C,D;
		private GameObject [] antikub;
		private int [] Antilongueur, Antihauteur, Antilargeur;
		private bool antikubx=false, antikuby=false, antikubz=false;
		private bool enhaut = false, enbas = false, agauche = false, adroite = false, unefois = true;
		[HideInInspector]
		public bool Estlarg=false, Esthaut=false, Estlong=false;
		[HideInInspector]
		public bool RotationH=false, RotationAH = false;
		[HideInInspector]
		public bool persoreplacez, persoreplacey, persoreplacex;
		private bool finselectionLargeur=false, finselectionHauteur=false, finselectionLongueur=false;
		private bool rotationpretex=false, rotationpretey=false, rotationpretez=false;
		 
	// Use this for initialization
	void Start () {
		
				//Récupération de la taille du cube
				hauteur = God.GetComponent<CreationCube> ().Hauteur;
				longueur = God.GetComponent<CreationCube> ().Longueur;
				largeur = God.GetComponent<CreationCube> ().Largeur;

				//Creation du point central pour les centre de rotation
				milx = (float)(((float) longueur / 2) - 0.5F);
				mily = (float)(((float) hauteur / 2) - 0.5F);
				milz = (float)(((float) largeur / 2) - 0.5F);

				//Récupération de la taille du cube
				hauteur = God.GetComponent<CreationCube> ().Hauteur;
				longueur = God.GetComponent<CreationCube> ().Longueur;
				largeur = God.GetComponent<CreationCube> ().Largeur;

				//Creation du point central pour les centre de rotation
				milx = (float)(((float)longueur / 2) - 0.5F);
				mily = (float)(((float)hauteur / 2) - 0.5F);
				milz = (float)(((float)largeur / 2) - 0.5F);

				//je crée un tableau avec tous les cubes du niveau
				Cubes = GameObject.FindGameObjectsWithTag ("Cube");
				Antikub = GameObject.FindGameObjectsWithTag ("Antikub");
				Kubs = GameObject.FindGameObjectsWithTag ("Kubs");
				TousCubes = new GameObject [Cubes.Length + Antikub.Length + Kubs.Length];
				Cubes.CopyTo (TousCubes, 0);
				Antikub.CopyTo (TousCubes, Cubes.Length);
				Kubs.CopyTo (TousCubes, Cubes.Length + Antikub.Length);

		}
	
	// Update is called once per frame
	void Update ()
		{

				//Récupération de la chute
				chute = Perso.GetComponent<Controle_Personnage> ().enchute;
				saut = Perso.GetComponent<Controle_Personnage> ().ensaut;

				//Récupération de la direction
				enhaut = Perso.GetComponent<Controle_Personnage> ().devant;
				enbas = Perso.GetComponent<Controle_Personnage> ().derriere;
				agauche = Perso.GetComponent<Controle_Personnage> ().gauche;
				adroite = Perso.GetComponent<Controle_Personnage> ().droite;

				//je fais en sorte que l'action ne se passe qu'une fois
				if ((!enhaut) & (!enbas) & (!adroite) & (!agauche) & (!unefois))
						unefois = true;

				//Récupération de la position de la caméra
				A = GetComponent<Ubik_Camera_Smooth> ().a;
				B = GetComponent<Ubik_Camera_Smooth> ().b;
				C = GetComponent<Ubik_Camera_Smooth> ().c;
				D = GetComponent<Ubik_Camera_Smooth> ().d;

				//je créé un tableau avec toutes les coordonnées des antikub
				antikub = GameObject.FindGameObjectsWithTag("Antikub");
				int[] Antilongueur = new int[antikub.Length];
				int[] Antilargeur = new int[antikub.Length];
				int[] Antihauteur = new int[antikub.Length];

				//je définis les colonnes, lignes et hauteurs où la rotation est pas possible
				for (int i = 0; i < antikub.Length; i++) {
						Antilongueur [i] = Mathf.RoundToInt(antikub[i].transform.position.x);
						Antihauteur [i] = Mathf.RoundToInt(antikub[i].transform.position.y);
						Antilargeur [i] = Mathf.RoundToInt(antikub[i].transform.position.z);
				}

				//je cherche si la rotation est impossible pour les différents sens de rotation
				foreach (int interdit in Antilongueur) {
						if (rangeex == interdit) {
								antikubx = true;
								break;
						} else
								antikubx = false;
				}
				foreach (int interdit in Antihauteur) {
						if (rangeey == interdit) {
								antikuby = true;
								break;
						} else
								antikuby = false;
				}
				foreach (int interdit in Antilargeur) {
						if (rangeez == interdit) {
								antikubz = true;
								break;
						} else
								antikubz = false;
				}
						
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LARGEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en largeur
				if (((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationBleue) & (transform.position == A))
				    | ((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationBleue) & (transform.position == C))
				    | ((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationBleue) & (transform.position == B))
				    | ((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationBleue) & (transform.position == D))) {
						Estlarg = true;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {
								
								if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {

										if (antikubz) {
												//je met les cubes en noir
												quads = TousCubes[i].GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material.color = Color.black;
												}
												//je met la verriere en noir
												verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material.SetColor ("_EmissionColor", Color.black);
												}
										} else {
												//je met les cubes en bleu
												quads = TousCubes[i].GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material.color = Color.blue;
												}
												//je met la verriere en bleu
												verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material.SetColor ("_EmissionColor", Color.blue);
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

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {
										//		TousCubes[i].tag = "Cube";
										//		TousCubes[i].transform.SetParent (null, true);
										//}

										//je remet les cubes à leur couleur originelle
										quads = TousCubes[i].GetComponents<Renderer> ();
										foreach (Renderer lequad in quads) {
												lequad.material = lequad.GetComponent<Couleur> ().materielini;
										}

										//je met la verriere en couleur originelle
										verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
										foreach (Renderer leverre in verres) {
												leverre.material = leverre.GetComponent<Couleur> ().materielini;
										}
								}
						}
				}
			
				//Déplacement de la rangée large
				if (Estlarg) {
												
						if (((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == A)) |
						    ((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == B)) |
						    ((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == C)) |
						    ((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == D))) {
								if (rangeez != largeur - 1) {										
										unefois = false;
										rangeez += 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

												//je réinitialise le bloc
												if (TousCubes[i].transform.position.z - rangeez < 0.1F - 1) {
														//		TousCubes[i].tag = "Cube";
														//		TousCubes[i].transform.SetParent (null, true);
																		
																		

														//je remet les cubes à leur couleur originelle
														quads = TousCubes[i].GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material = lequad.GetComponent<Couleur> ().materielini;
														}

														//je met la verriere en couleur originelle
														verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material = leverre.GetComponent<Couleur> ().materielini;
														}
												}
										}
								}

						} else if (((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == C)) |
						           ((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == D)) |
						           ((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == A)) |
						           ((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == B))) {
								if (rangeez != 0) {
										unefois = false;
										rangeez -= 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

												if (TousCubes[i].transform.position.z - rangeez < 0.1F + 1) {
														//	TousCubes[i].tag = "Cube";
														//	TousCubes[i].transform.SetParent (null, true);
																		
																		

														//je remet les cubes à leur couleur originelle
														quads = TousCubes[i].GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material = lequad.GetComponent<Couleur> ().materielini;
														}

														//je met la verriere en couleur originelle
														verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material = leverre.GetComponent<Couleur> ().materielini;
														}
												}
										}
								}
						}

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((TousCubes[i].transform.position.z-rangeez <0.1F) & (TousCubes[i].transform.position.z-rangeez >-0.1F)) {

														if (antikubz) {
																//je met les cubes en noir
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.black;
																}
																//je met la verriere en noir
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.black);
																}
														} else {
																//je met les cubes en bleu
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.blue;
																}

																//je met la verriere en bleu
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.blue);
																}
														}
												}
										}
								}

				//vérification qu'une rotation est lancée
				if ((((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == A))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == D))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == B))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationAH) & (Estlarg) & (!saut) & (!chute) & (transform.position == C)))
						rotationpretez = true;

				//le personnage est pas dans dans la rangée et pas d'antikub donc rotation
				if ((rotationpretez) & ((Perso.position.z + 0.9F <= rangeez) | (Perso.position.z - 0.9F >= rangeez)) & (!antikubz)) {
						RotationH = true;
						rotationpretez = false;
						persoreplacez = false;
				}
						//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretez) & ((Perso.position.z - 0.5F > rangeez) | (Perso.position.z + 0.5F < rangeez)) & (!antikubz)) {
						persoreplacez = true;
				}

				else if (rotationpretez) {
				//pas de rotation de la rangee
				RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

												if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {
																		
														//je met les cubes en blanc
														quads = TousCubes[i].GetComponents<Renderer> ();
														foreach (Renderer lequad in quads) {
																lequad.material = lequad.GetComponent<Couleur> ().materielini;
														}

														//je met la verriere en blanc
														verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
														foreach (Renderer leverre in verres) {
																leverre.material = leverre.GetComponent<Couleur> ().materielini;
														}
												}
										}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {

																		if (antikubz) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				
																				//je met les cubes en bleu
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.blue;
																				}

																				//je met la verriere en bleu
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.blue);
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
				if ((((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == A))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == D))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == B))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationH) & (Estlarg) & (!saut) & (!chute) & (transform.position == C)))
						rotationpretez = true;

				//le personnage est pas dans dans la rangée donc rotation
				if ((rotationpretez) & ((Perso.position.z + 0.9F <= rangeez) | (Perso.position.z - 0.9F >= rangeez)) & (!antikubz)) {
						RotationAH = true;
						rotationpretez = false;
						persoreplacez = false;
				}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretez) & ((Perso.position.z - 0.5F > rangeez) | (Perso.position.z + 0.5F < rangeez)) & (!antikubz)) {
						persoreplacez = true;
				}

				else if (rotationpretez) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {
																		//		TousCubes[i].tag = "Bloc";
																		//		TousCubes[i].transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en blanc
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.z - rangeez < 0.1F) & (TousCubes[i].transform.position.z - rangeez > -0.1F)) {

																		if (antikubz) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				//je met les cubes en bleu
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.blue;
																				}

																				//je met la verriere en bleu
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.blue);
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
						if (hauteur != longueur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, mily, rangeez);
						axe = new Vector3 (0, 0, 1);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes [i].transform.position.z - rangeez < 0.1F) & (TousCubes [i].transform.position.z - rangeez > -0.1F)) {
										TousCubes [i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//réinitialisation de la rotation

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {
								
								if ((anglerot + 0.3F >= Mathf.Abs (angletot)) & (Mathf.Abs (angletot) >= anglerot - 0.3F)) {
										TousCubes [i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
										centreRot = 0;
										RotationH = false;
										angletot = 0;
								}
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Estlarg)) {

						//je détermine si les rotations sont de 90° ou 180° en fonction de la taille du "cube"
						if (hauteur != longueur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, mily, rangeez);
						axe = new Vector3 (0, 0, 1);


						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes [i].transform.position.z - rangeez < 0.1F) & (TousCubes [i].transform.position.z - rangeez > -0.1F)) {
										TousCubes [i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {
								
								//réinitialisation de la rotation
								if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
										TousCubes [i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
										centreRot = 0;
										RotationAH = false;
										angletot = 0;
								}
						}
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++HAUTEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en hauteur
				if ((Input.GetButtonDown ("Hauteur")) & (!Estlarg) & (!Estlong) & (RotationJaune)) {
						Esthaut = true;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												if ((TousCubes[i].transform.position.y-rangeey <0.1F) & (TousCubes[i].transform.position.y-rangeey >-0.1F)) {

														if (antikuby) {
																//je met les cubes en noir
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.black;
																}
																//je met la verriere en noir
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.black);
																}
														} else {
																//je met les cubes en jaune
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.yellow;
																}

																//je met la verriere en jaune
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.yellow);
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

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												if ((TousCubes[i].transform.position.y-rangeey <0.1F) & (TousCubes[i].transform.position.y-rangeey >-0.1F)) {
														// TousCubes[i].tag = "Cube";
														// TousCubes[i].transform.SetParent (null, true);
														
														
												}

												//je remet les cubes à leur couleur originelle
												quads = TousCubes[i].GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material = lequad.GetComponent<Couleur>().materielini;
												}

												//je met la verriere en couleur originelle
												verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material = leverre.GetComponent<Couleur>().materielini;
												}
										}
								}

				//Déplacement de la rangée large
				if (Esthaut) {
												
						if ((enhaut) & (unefois) & (!RotationH) & (!RotationAH)) {
								if (rangeey != hauteur - 1) {
										unefois = false;
										rangeey += 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																//je réinitialise le bloc
																if (TousCubes[i].transform.position.y-rangeey <0.1F - 1) {
																		// TousCubes[i].tag = "Cube";
																		// TousCubes[i].transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en couleur originelle
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}
												}

						} else if ((enbas) & (unefois) & (!RotationH) & (!RotationAH)) {
								if (rangeey != 0) {
										unefois = false;
										rangeey -= 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if (TousCubes[i].transform.position.y-rangeey <0.1F + 1) {
																		// TousCubes[i].tag = "Cube";
																		// TousCubes[i].transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en couleur originelle
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}
												}
										}

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((TousCubes[i].transform.position.y-rangeey <0.1F) & (TousCubes[i].transform.position.y-rangeey >-0.1F)) {

														if (antikuby) {
																//je met les cubes en noir
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.black;
																}
																//je met la verriere en noir
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.black);
																}
														} else {
																//je met les cubes en jaune
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.yellow;
																}

																//je met la verriere en jaune
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.yellow);
																}
														}
												}
										}
								}

				//vérification qu'une rotation est lancée
				if (((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationAH) & (Esthaut) & (!saut) & (!chute))
						rotationpretey = true;
				
				if ((rotationpretey) & ((int)(Perso.position.y - 0.8F) != rangeey) & ((int)(Perso.position.y + 0.2F) != rangeey) & (!antikuby)) {
						RotationH = true;
						rotationpretey = false;

				} else if (rotationpretey) {
						//pas de rotation de la rangee
						RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										
										///je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {
																		
																		//je met les cubes en blanc
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en blanc
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {

																		if (antikuby) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				//je met les cubes en jaune
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.yellow;
																				}

																				//je met la verriere en jaune
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.yellow);
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
				if (((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationH) & (Esthaut) & (!saut) & (!chute))
						rotationpretey = true;

				if ((rotationpretey) & ((int)(Perso.position.y - 0.8F) != rangeey) & ((int)(Perso.position.y + 0.2F) != rangeey) & (!antikuby)) {
						RotationAH = true;
						rotationpretey = false;

				} else if (rotationpretey) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {
																		//		TousCubes[i].tag = "Bloc";
																		//		TousCubes[i].transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en blanc
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {

																		if (antikuby) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				//je met les cubes en jaune
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.yellow;
																				}

																				//je met la verriere en jaune
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.yellow);
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

						if (largeur != longueur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, rangeey, milz);
						axe = new Vector3 (0, 1, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//réinitialisation de la rotation
								if ((anglerot + 0.3F >= Mathf.Abs (angletot)) & (Mathf.Abs (angletot) >= anglerot - 0.3F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
										centreRot = 0;
										RotationH = false;
										angletot = 0;
								}
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Esthaut)) {

						if (largeur != longueur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (milx, rangeey, milz);
						axe = new Vector3 (0, 1, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes[i].transform.position.y - rangeey < 0.1F) & (TousCubes[i].transform.position.y - rangeey > -0.1F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//réinitialisation de la rotation
								if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
										centreRot = 0;
										RotationAH = false;
										angletot = 0;
								}
						}
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LONGUEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//Rotation préparée pour les rangées en longueur
				if (((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationRouge) & (transform.position == B))
						|((Input.GetButtonDown ("Largeur")) & (!Esthaut) & (!Estlong) & (RotationRouge) & (transform.position == D))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationRouge) & (transform.position == A))
						|((Input.GetButtonDown ("Longueur")) & (!Esthaut) & (!Estlarg) & (RotationRouge) & (transform.position == C))) {
						Estlong = true;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												if ((TousCubes[i].transform.position.x-rangeex <0.1F) & (TousCubes[i].transform.position.x-rangeex >-0.1F)) {

														if (antikubx) {
																//je met les cubes en noir
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.black;
																}
																//je met la verriere en noir
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.black);
																}
														} else {
																//je met les cubes en rouge
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.red;
																}

																//je met la verriere en rouge
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.red);
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

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												if ((TousCubes[i].transform.position.x-rangeex <0.1F) & (TousCubes[i].transform.position.x-rangeex >-0.1F)) {
														// TousCubes[i].tag = "Cube";
														// TousCubes[i].transform.SetParent (null, true);
														
														
												}

												//je remet les cubes à leur couleur originelle
												quads = TousCubes[i].GetComponents<Renderer> ();
												foreach (Renderer lequad in quads) {
														lequad.material = lequad.GetComponent<Couleur>().materielini;
												}

												//je met la verriere en couleur originelle
												verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
												foreach (Renderer leverre in verres) {
														leverre.material = leverre.GetComponent<Couleur>().materielini;
												}
										}
								}

				//Déplacement de la rangée large
				if (Estlong) {
												
						if (((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == B))|
								((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == C))|
								((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == D))|
								((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == A))) {
								if (rangeex != longueur - 1) {
										unefois = false;
										rangeex += 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																//je réinitialise le bloc
																if (TousCubes[i].transform.position.x-rangeex <0.1F - 1) {
																		// TousCubes[i].tag = "Cube";
																		// TousCubes[i].transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en couleur originelle
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}
												}

						} else if (((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == A))|
								((agauche) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == D))|
								((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == C))|
								((adroite) & (unefois) & (!RotationH) & (!RotationAH) & (transform.position == B))) {
								if (rangeex != 0) {
										unefois = false;
										rangeex -= 1;

										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if (TousCubes[i].transform.position.x-rangeex <0.1F + 1) {
																		// TousCubes[i].tag = "Cube";
																		// TousCubes[i].transform.SetParent (null, true);
																		
																		

																		//je remet les cubes à leur couleur originelle
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en couleur originelle
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}
												}
										}

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

												//si le cube est dans la rangee
												//je créé un bloc avec les nouveaux cubes sélectionnés
												if ((TousCubes[i].transform.position.x-rangeex <0.1F) & (TousCubes[i].transform.position.x-rangeex >-0.1F)) {

														if (antikubx) {
																//je met les cubes en noir
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.black;
																}
																//je met la verriere en noir
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.black);
																}
														} else {
																//je met les cubes en rouge
																quads = TousCubes[i].GetComponents<Renderer> ();
																foreach (Renderer lequad in quads) {
																		lequad.material.color = Color.red;
																}

																//je met la verriere en rouge
																verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																foreach (Renderer leverre in verres) {
																		leverre.material.SetColor ("_EmissionColor", Color.red);
																}
														}
												}
										}
								}

				//vérification qu'une rotation est lancée
				if ((((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == A))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == B))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == C))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationAH) & (Estlong) & (!saut) & (!chute) & (transform.position == D)))
						rotationpretex = true;

				//le personnage est pas dans dans la rangée donc rotation
				if ((rotationpretex) & ((Perso.position.x + 0.9F <= rangeex) | (Perso.position.x - 0.9F >= rangeex)) & (!antikubx)) {
						RotationH = true;
						rotationpretex = false;
						persoreplacex = false;
				}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretex) & ((Perso.position.x - 0.5F > rangeex) | (Perso.position.x + 0.5F < rangeex)) & (!antikubx)) {
						persoreplacex = true;
				}

				else if (rotationpretex) {
						//pas de rotation de la rangee
						RotationH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {
																		//		TousCubes[i].tag = "Bloc";
																		//		TousCubes[i].transform.SetParent (Bloc.transform, true);

																		//je met les cubes en blanc
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en blanc
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {

																		if (antikubx) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				//je met les cubes en rouge
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.red;
																				}

																				//je met la verriere en rouge
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.red);
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
				if ((((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == A))|
						(((Input.GetButtonDown ("RotationAH")) | (Input.GetAxis("RotationJH") == 1)) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == B))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == C))|
						(((Input.GetButtonDown ("RotationH")) | (Input.GetAxis("RotationJAH") == 1)) & (!RotationH) & (Estlong) & (!saut) & (!chute) & (transform.position == D)))
						rotationpretex = true;

				//le personnage est pas dans dans la rangée donc rotation
				if ((rotationpretex) & ((Perso.position.x + 0.9F <= rangeex) | (Perso.position.x - 0.9F >= rangeex)) & (!antikubx)) {
						RotationAH = true;
						rotationpretex = false;
						persoreplacex = false;
				}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretex) & ((Perso.position.x - 0.5F > rangeex) | (Perso.position.x + 0.5F < rangeex)) & (!antikubx)) {
						persoreplacex = true;
				}

				else if (rotationpretex) {
						//pas de rotation de la rangee
						RotationAH = false;

						//je clignote en blanc tous les sixièmes de seconde
						if (clignoter < 6) {
								if (clignoter % 2 == 0) {
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {
																		
																		//je met les cubes en blanc
																		quads = TousCubes[i].GetComponents<Renderer> ();
																		foreach (Renderer lequad in quads) {
																				lequad.material = lequad.GetComponent<Couleur>().materielini;
																		}

																		//je met la verriere en blanc
																		verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																		foreach (Renderer leverre in verres) {
																				leverre.material = leverre.GetComponent<Couleur>().materielini;
																		}
																}
														}

										//j'attends avant de changer de clignotement
										clignoterfloat += vclignote * Time.deltaTime;
										clignoter = (int)clignoterfloat;

										// je clignote en jaune tous les sixièmes de seconde
								} else {
										//je cherche dans le tableau les cubes qui m'intéressent
										for (int i = 0; i < TousCubes.Length; i++) {

																if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {

																		if (antikubx) {
																				//je met les cubes en noir
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.black;
																				}
																				//je met la verriere en noir
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.black);
																				}
																		} else {
																				//je met les cubes en rouge
																				quads = TousCubes[i].GetComponents<Renderer> ();
																				foreach (Renderer lequad in quads) {
																						lequad.material.color = Color.red;
																				}

																				//je met la verriere en rouge
																				verres = TousCubes[i].GetComponentsInChildren<Renderer> ();
																				foreach (Renderer leverre in verres) {
																						leverre.material.SetColor ("_EmissionColor", Color.red);
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

						if (largeur != hauteur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (rangeex, mily, milz);
						axe = new Vector3 (1, 0, 0);

						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, -anglerot, 0.2F);
						angletot = angletot + centreRot;

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//réinitialisation de la rotation
								if ((anglerot + 0.3F >= Mathf.Abs (angletot)) & (Mathf.Abs (angletot) >= anglerot - 0.3F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
										centreRot = 0;
										RotationH = false;
										angletot = 0;
								}
						}
				}

				//rotation anti-horaire des cubes
				if ((RotationAH) & (Estlong)) {

						if (largeur != hauteur)
								anglerot = 179.9999F;
						else
								anglerot = 90F;
						
						//Position du centre et de l'axe de rotation pour les cubes en largeur
						centrePos = new Vector3 (rangeex, mily, milz);
						axe = new Vector3 (1, 0, 0);


						//rotation du bloc
						centreRot = Mathf.LerpAngle (0, anglerot, 0.2F);
						angletot = angletot + centreRot;

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//si le cube est dans la rangee
								//je créé un bloc avec les nouveaux cubes sélectionnés
								if ((TousCubes[i].transform.position.x - rangeex < 0.1F) & (TousCubes[i].transform.position.x - rangeex > -0.1F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, centreRot);
								}
						}

						//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
						//je cherche dans le tableau les cubes qui m'intéressent
						for (int i = 0; i < TousCubes.Length; i++) {

								//réinitialisation de la rotation
								if ((anglerot - 0.3F <= angletot) & (angletot <= anglerot + 0.3F)) {
										TousCubes[i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
										centreRot = 0;
										RotationAH = false;
										angletot = 0;
								}
						}
				}
		}
}