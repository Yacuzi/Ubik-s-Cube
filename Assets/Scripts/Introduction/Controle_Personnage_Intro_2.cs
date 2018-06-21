using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage_Intro_2 : MonoBehaviour {

		public Transform God;
		public float vitesse = 5F, vpetitsaut = 1F, gravite = 1F;
		private Renderer kubcolor;
		private bool yacube, bloque;
		[HideInInspector]
		public bool devant=false, derriere=false, gauche=false, droite=false, kubappui = true, kubappuitemp = true;
		private Vector3 a,b,c,d, abis, bbis, cbis, dbis;
		private Vector3 posfuture;
		private Vector3 rotsaut, axesaut, petitsaut, velocity = Vector3.zero;
		private Vector3 pchute, axechute;
		private float anglesaut, stoppos, angletot, angletotchute;
		private int persox, persoy, persoz, pointfinal;
		private int kubsautx, kubsauty, kubsautz;
		private int kubappuix, kubappuiy, kubappuiz;
		private int kubxchute, kubychute, kubzchute;
		private float anglechute;
		private bool yacubstop = false, wait = false, bougepas = false, kubdesequilibre = false; 
		private float waittime;
		[HideInInspector]
		public bool sautpret=false, ensaut=false, enchute = false, pousse = false;
		[HideInInspector]
		public Vector3 regard, direction, possaut;
		[HideInInspector]
		public GameObject[] kubs;
		[HideInInspector]
		public int pretbouger = 0;
		private float kubechx, kubechz;

		// Use this for initialization
		void Start () {
				//initialisation position personnage
				//transform.position = new Vector3 (0,-(transform.localScale.y/2),0);

				//je cache le curseur
				Cursor.visible = false;
		}

		// Update is called once per frame
		void Update () {

				//recherche de tous les kubs
				GameObject[] kubs1 = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[];
				GameObject[] decor = GameObject.FindGameObjectsWithTag ("Decor") as GameObject[];
				kubs = new GameObject[kubs1.Length + decor.Length];
				kubs1.CopyTo (kubs, 0);
				decor.CopyTo (kubs, (kubs1.Length));

				//position arrondie à l'entier du perso
				persox = Mathf.RoundToInt (transform.position.x);
				persoy = Mathf.RoundToInt (transform.position.y);
				persoz = Mathf.RoundToInt (transform.position.z);

				//récupération des touches
				if ((Input.GetButton ("Haut") & (!bougepas)) | ((Input.GetAxisRaw ("VerticalJ") == -1) & (!bougepas))) {
						devant = true;
				} else
						devant = false;

				if ((Input.GetButton ("Bas") & (!bougepas)) | ((Input.GetAxisRaw ("VerticalJ") == 1) & (!bougepas))) {
						derriere = true;
				} else
						derriere = false;

				if ((Input.GetButton ("Gauche") & (!bougepas)) | ((Input.GetAxisRaw ("HorizontalJ") == -1) & (!bougepas))) {
						gauche = true;
				} else
						gauche = false;

				if ((Input.GetButton ("Droite") & (!bougepas)) | ((Input.GetAxisRaw ("HorizontalJ") == 1) & (!bougepas))) {
						droite = true;
				} else
						droite = false;
				if ((!devant) & (!derriere) & (!gauche) & (!droite)) {
						posfuture = transform.position;
						direction = Vector3.zero;
				}

				//Les quatre directions
				if ((devant) & (!gauche) & (!droite) & (!derriere))
						direction = new Vector3 (1, 0, 0);
				if ((devant) & (!gauche) & (!droite) & (!derriere)
				    & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
						posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
						regard = new Vector3 (0.5F, 0, 0);

						//collision avec les kubs
						foreach (GameObject kub in kubs) {

								if ((posfuture.x + (transform.localScale.x * 0.5F) >= kub.transform.position.x - (kub.transform.lossyScale.x * 0.5F))
								    & (transform.position.x < kub.transform.position.x)
								    & (posfuture.y + (transform.localScale.y * 0.49F) > kub.transform.position.y - (kub.transform.lossyScale.y * 0.5F))
								    & (posfuture.y - (transform.localScale.y * 0.49F) < kub.transform.position.y + (kub.transform.lossyScale.y * 0.5F))
								    & (posfuture.z - (transform.localScale.x * 0.5F) <= kub.transform.position.z + (kub.transform.lossyScale.z * 0.5F))
								    & (posfuture.z + (transform.localScale.x * 0.5F) >= kub.transform.position.z - (kub.transform.lossyScale.z * 0.5F))) {
										yacube = true;
										stoppos = kub.transform.position.x - (kub.transform.lossyScale.x * 0.5F);
								}
						}

						if (yacube) {
								yacubstop = true;
								transform.position = new Vector3 (stoppos - (transform.localScale.x * 0.51F), transform.position.y, transform.position.z);
						} /* else {
								yacubstop = false;
								//collision avec les bords
								if (posfuture.x + ((transform.localScale.x * 0.5F) + 0.5F) >= longueur) {
										transform.position = new Vector3 (longueur - ((transform.localScale.x * 0.5F) + 0.51F), transform.position.y, transform.position.z);
										yamurstop = true;
								} */ else {
								transform.position = posfuture;
								yacubstop = false;

						}
						//}
						
						yacube = false;
				}

				if ((!devant) & (!gauche) & (!droite) & (derriere))
						direction = new Vector3 (-1, 0, 0);
				if ((!devant) & (!gauche) & (!droite) & (derriere)
						& (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
						posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
						regard = new Vector3 (-0.5F, 0, 0);

						//collision avec les kubs
						foreach (GameObject kub in kubs) {

								if ((posfuture.x - (transform.localScale.x * 0.5F) <= kub.transform.position.x + (kub.transform.lossyScale.x * 0.5F))
										& (transform.position.x > kub.transform.position.x)
										& (posfuture.y + (transform.localScale.y * 0.49F) > kub.transform.position.y - (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.y - (transform.localScale.y * 0.49F) < kub.transform.position.y + (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.z - (transform.localScale.x * 0.5F) <= kub.transform.position.z + (kub.transform.lossyScale.z * 0.5F))
										& (posfuture.z + (transform.localScale.x * 0.5F) >= kub.transform.position.z - (kub.transform.lossyScale.z * 0.5F))) {
										yacube = true;
										stoppos = kub.transform.position.x + (kub.transform.lossyScale.x * 0.5F);
								}
						}
						if (yacube) {
								yacubstop = true;
								transform.position = new Vector3 (stoppos + (transform.localScale.x * 0.51F), transform.position.y, transform.position.z);
						} /* else {
								yacubstop = false;
								//collision avec les bords
								if (posfuture.x - (transform.localScale.x * 0.5F) <= -0.5F) {
										transform.position = new Vector3 (-(0.49F - (transform.localScale.x * 0.5F)), transform.position.y, transform.position.z);
										yamurstop = true;
								} */ else {
										transform.position = posfuture;
								yacubstop = false;
								}
						//}

						yacube = false;	
				}

				if ((!devant) & (gauche) & (!droite) & (!derriere))
						direction = new Vector3 (0, 0, 1);
				if ((!devant) & (gauche) & (!droite) & (!derriere)
						& (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
						posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
						regard = new Vector3 (0, 0, 0.5F);

						//collision avec les kubs
						foreach (GameObject kub in kubs) {

								if ((posfuture.z + (transform.localScale.x * 0.5F) >= kub.transform.position.z - (kub.transform.lossyScale.z * 0.5F))
										& (transform.position.z < kub.transform.position.z)
										& (posfuture.y + (transform.localScale.y * 0.49F) > kub.transform.position.y - (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.y - (transform.localScale.y * 0.49F) < kub.transform.position.y + (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.x - (transform.localScale.x * 0.5F) < kub.transform.position.x + (kub.transform.lossyScale.x * 0.5F))
										& (posfuture.x + (transform.localScale.x * 0.5F) > kub.transform.position.x - (kub.transform.lossyScale.x * 0.5F))) {
										yacube = true;
										stoppos = kub.transform.position.z - (kub.transform.lossyScale.z * 0.5F);
								}
						}
						if (yacube) {
								yacubstop = true;
								transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - (transform.localScale.x * 0.51F));
						} /* else {

								yacubstop = false;
								//collision avec les bords
								if (posfuture.z + ((transform.localScale.x * 0.5F) + 0.5F) >= largeur) {
										transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x * 0.5F) + 0.51F));
										yamurstop = true;
								} */ else {
										transform.position = posfuture;
								yacubstop = false;
								}
						//}
								
						yacube = false;	
				}

				if ((!devant) & (!gauche) & (droite) & (!derriere))
						direction = new Vector3 (0, 0, -1);
				if ((!devant) & (!gauche) & (droite) & (!derriere)
						& (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
						posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
						regard = new Vector3 (0, 0, -0.5F);

						//collision avec les kubs
						foreach (GameObject kub in kubs) {

								if ((posfuture.z - (transform.localScale.x * 0.5F) <= kub.transform.position.z + (kub.transform.lossyScale.z * 0.5F))
										& (transform.position.z > kub.transform.position.z)
										& (posfuture.y + (transform.localScale.y * 0.49F) > kub.transform.position.y - (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.y - (transform.localScale.y * 0.49F) < kub.transform.position.y + (kub.transform.lossyScale.y * 0.5F))
										& (posfuture.x - (transform.localScale.x * 0.5F) < kub.transform.position.x + (kub.transform.lossyScale.x * 0.5F))
										& (posfuture.x + (transform.localScale.x * 0.5F) > kub.transform.position.x - (kub.transform.lossyScale.x * 0.5F))) {
										yacube = true;
										stoppos = kub.transform.position.z + (kub.transform.lossyScale.z * 0.5F);
								}
						}
						if (yacube) {
								yacubstop = true;
								transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + (transform.localScale.x * 0.51F));
						} /*else {

								yacubstop = false;
								//collision avec les bords
								if (posfuture.z - (transform.localScale.x * 0.5F) <= -0.5F) {
										transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x * 0.5F)));
										yamurstop = true;
								} */ else {
										transform.position = posfuture;
								yacubstop = false;
								}
						//}
				}

				yacube = false;

				//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "SAUT" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//préparation du saut
				sautpret = false;

				Debug.Log (kubsautx + "kubsautx");
				Debug.Log (kubsauty + "kubsauty");

				Debug.Log (kubsautz + "kubsautz");


				//Détermination du kub sur lequel le joueur peut sauter
				foreach (GameObject kub in kubs) {

						//je cherche la distance minimum
						if ((!ensaut) & (yacubstop) & (Mathf.RoundToInt (kub.transform.position.y) == persoy)
								& (((regard.x != 0) 
										& (Mathf.Abs((transform.position.x + (regard.x * (kub.transform.lossyScale.x + transform.lossyScale.x))) - (kub.transform.position.x)) <= 0.01F)
										& (transform.position.z < kub.transform.position.z + (0.5F * kub.transform.lossyScale.z) - (0.5F * transform.lossyScale.z))
										& (transform.position.z > kub.transform.position.z - (0.5F * kub.transform.lossyScale.z) + (0.5F * transform.lossyScale.z)))
										| ((regard.z != 0) 
												& (Mathf.Abs((transform.position.z + (regard.z * (kub.transform.lossyScale.z + transform.lossyScale.z))) - (kub.transform.position.z)) <= 0.01F)
												& (transform.position.x < kub.transform.position.x + (0.5F * kub.transform.lossyScale.x) - (0.5F * transform.lossyScale.x))
												& (transform.position.x > kub.transform.position.x - (0.5F * kub.transform.lossyScale.x) + (0.5F * transform.lossyScale.x))))
								& (kub.tag == "Kubs")) {

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
										    & (Mathf.RoundToInt (kub2.transform.position.z) == persoz)))
												//le saut est bloqué par un cube
												bloque = true;
								}

								if (bloque) {
										sautpret = false;
										//saut pas pret et cube colorié en blanc
										kubcolor = kub.GetComponent<Renderer> ();
										kubcolor.material.color = Color.white;
								} else {
										sautpret = true;
										//saut pret et cube colorié en blanc
										kubcolor = kub.GetComponent<Renderer> ();
										kubcolor.material.color = Color.green;
								}								
						} else if (kub.tag == "Kubs") {
								//saut pret et cube colorié en blanc
								kubcolor = kub.GetComponent<Renderer> ();
								kubcolor.material.color = Color.white;
						}
				}

				if ((Input.GetButtonDown ("Saut")) & (!ensaut) & (sautpret)) {

						//je déclare le perso comme en saut
						ensaut = true;

						//je trouve le point de rotation
						rotsaut = new Vector3 (kubsautx - regard.x, kubsauty + 0.5F, transform.position.z);

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
								petitsaut = new Vector3 (kubsautx - regard.x - (transform.localScale.x * 0.5F), kubsauty + (0.5F - (transform.localScale.y * 0.5F)), transform.position.z);
						}

						if (regard.x == -0.5F) {
								petitsaut = new Vector3 (kubsautx - regard.x + (transform.localScale.x * 0.5F), kubsauty + (0.5F - (transform.localScale.y * 0.5F)), transform.position.z);
						}

						if (regard.z == 0.5F) {
								petitsaut = new Vector3 (transform.position.x, kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz - regard.z - (transform.localScale.x * 0.5F));
						}

						if (regard.z == -0.5F) {
								petitsaut = new Vector3 (transform.position.x, kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz - regard.z + (transform.localScale.x * 0.5F));
						}

						//je fais le petit saut de préparation
						transform.position = Vector3.SmoothDamp (transform.position, petitsaut, ref velocity, vpetitsaut * Time.deltaTime);

				}

				if (ensaut) {

						//angle progressif de rotation
						anglesaut = Mathf.LerpAngle (0F, 180F, 0.03F);
						angletot = angletot + anglesaut;

						//je fais la rotation
						transform.RotateAround (rotsaut, axesaut, anglesaut);
				}

				//réinitialisation des paramètres
				if ((ensaut) & (angletot >= 180F)) {
						transform.position = new Vector3 (petitsaut.x + ((2.01F * transform.localScale.x) * regard.x), petitsaut.y + (transform.localScale.y), petitsaut.z + ((2.01F * transform.localScale.z) * regard.z));
						transform.rotation = Quaternion.identity;
						angletot = 0F;
						ensaut = false;
						wait = true;
				}

				if (wait) {
						bougepas = true;
						waittime += Time.deltaTime;
						if (waittime >= 0.2F) {
								waittime = 0F;
								wait = false;
								bougepas = false;
						}
				}

				//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "GRAVITE/CHUTE" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				if ((!ensaut) & (transform.position.y <= -(0.5F - (transform.localScale.x * 0.5F)))) {
						transform.position = new Vector3 (transform.position.x, -(0.5F - (transform.localScale.x * 0.5F)), transform.position.z);
						kubappui = true;
						if (enchute) {
								wait = true;
								enchute = false;
						}

						if (wait) {
								bougepas = true;
								waittime += Time.deltaTime;
								if (waittime >= 0.2F) {
										waittime = 0F;
										wait = false;
										bougepas = false;
								}
						}

				} else {
						foreach (GameObject kub in kubs) {

								//je cherche le cube sur lequel repose le perso
								kubxchute = Mathf.RoundToInt (kub.transform.position.x);
								kubychute = Mathf.RoundToInt (kub.transform.position.y);
								kubzchute = Mathf.RoundToInt (kub.transform.position.z);

								//je récupère l'échelle du cube
								kubechx = kub.transform.lossyScale.x;
								kubechz = kub.transform.lossyScale.z;

								//je cherche si le perso est pile-poil sur un cube
								if ((!ensaut) 
										& (((transform.position.z <= (kubzchute + (kubechz * 0.5F) - (transform.lossyScale.z * 0.5F)))
												& (transform.position.z >= (kubzchute - (kubechz * 0.5F) + (transform.lossyScale.z * 0.5F))))
												& ((transform.position.x < (kubxchute + (kubechx * 0.5F) - (transform.lossyScale.x * 0.5F)))
														& (transform.position.x > (kubxchute - (kubechx * 0.5F) + (transform.lossyScale.x * 0.5F)))))
										& (transform.position.y <= kubychute + 1F) 
										& (transform.position.y >= kubychute + (transform.lossyScale.z))) {

										if (enchute) {
												wait = true;
												enchute = false;
										}

										if (wait) {
												bougepas = true;
												waittime += Time.deltaTime;
												if (waittime >= 0.2F) {
														waittime = 0F;
														wait = false;
														bougepas = false;
												}
										}

										kubappui = true;
										kubappuitemp = true;
										kubdesequilibre = false;
										transform.rotation = Quaternion.identity;
										transform.position = new Vector3 (transform.position.x, kubychute + ((transform.localScale.y * 0.5F) + 0.5F), transform.position.z);

										//je trouve les coordonnées du cube sur lequel repose le perso
										kubappuix = Mathf.RoundToInt (kub.transform.position.x);
										kubappuiy = Mathf.RoundToInt (kub.transform.position.y);
										kubappuiz = Mathf.RoundToInt (kub.transform.position.z);

										//je récupère l'échelle du cube
										kubechx = kub.transform.lossyScale.x;
										kubechz = kub.transform.lossyScale.z;

										//je sort de la boucle
										break;
								} else if (!ensaut) {
										//kubappuitemp = false; inverser avec kubappui en dessous si besoin
										kubappui = false;
										kubdesequilibre = false;
								}
						}

						/*if ((!kubappuitemp) & (!ensaut) & (!yacubstop) & (!yamurstop)) {
								if ((regard.x != 0) 
										& (transform.position.z <= kubappuiz + (kubechz * 0.5F) - (transform.lossyScale.z * 0.5F))
										& (transform.position.z >= kubappuiz - (kubechz * 0.5F) + (transform.lossyScale.z * 0.5F))) {
										kubappui = false;
										kubdesequilibre = false;
								} else if ((regard.z != 0) 
										& (transform.position.x <= kubappuix + (kubechx * 0.5F) - (transform.lossyScale.x * 0.5F))
										& (transform.position.x >= kubappuix - (kubechx * 0.5F) + (transform.lossyScale.x * 0.5F))) {
										kubappui = false;
										kubdesequilibre = false;
								} else
										kubdesequilibre = true;
						}
						*/
				}
						
				if (((!kubappui) | (kubdesequilibre)) & (!enchute) & (transform.rotation == Quaternion.identity)) {
						//je repositionne le perso pour qu'il soit juste au bord et je défini le point de rotation
						if (regard.x != 0) {
								transform.position = new Vector3 (kubappuix + (0.199F * regard.x), kubappuiy + ((transform.localScale.x * 0.5F) + 0.5F), transform.position.z);
								pchute = new Vector3 (kubappuix + regard.x, kubappuiy + 0.5F, transform.position.z);
						}
						if (regard.z != 0) {
								transform.position = new Vector3 (transform.position.x, kubappuiy + ((transform.localScale.x * 0.5F) + 0.5F), kubappuiz + (0.199F * regard.z));
								pchute = new Vector3 (transform.position.x, kubappuiy + 0.5F, kubappuiz + regard.z);
						}
				}

				if (((!kubappui) | (kubdesequilibre)) & (!enchute)) {

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
				}

				if ((angletotchute <= 45F) & ((!kubappui) | (kubdesequilibre)) & (!enchute)
						& ((direction.x * regard.x > 0) | (direction.z * regard.z > 0))) {

						//définition de la rotation de la chute
						anglechute = (100F * Time.deltaTime);
						angletotchute = angletotchute + anglechute;

						//je fait la rotation
						transform.RotateAround (pchute, axechute, anglechute);

				} else if ((angletotchute <= 45F) & ((!kubappui) | (kubdesequilibre)) & (!enchute)) {

						//définition de la rotation de la chute
						anglechute = -(150F * Time.deltaTime);
						angletotchute = angletotchute + anglechute;

						//je m'assure que l'angle totale de rotation fait pas n'importe quoi
						if (angletotchute < 0F) {								
								anglechute = -(150F * Time.deltaTime) - angletotchute;
								angletotchute = 0F;
								kubappui = true;
								kubdesequilibre = false;
						}

						//je fait la rotation
						transform.RotateAround (pchute, axechute, anglechute);
				}

				/*
				//déséqulibre si un semblant d'appui
				if ((angletotchute > 45F) & (kubdesequilibre) & (!enchute)) {
						//définition de la rotation de la chute
						anglechute = -(150F * Time.deltaTime);
						angletotchute = angletotchute + anglechute;

						//je fait la rotation
						transform.RotateAround (pchute, axechute, anglechute);
				}
				*/

				//chute si le perso n'a plus d'appui
				if ((angletotchute > 45F) & (!kubappui) & (!enchute)) {

						//définition de la rotation de la chute
						anglechute = anglechute + (4F * Time.deltaTime);
						angletotchute = angletotchute + anglechute;

						//je m'assure que l'angle totale de rotation fait pas n'importe quoi

						if (angletotchute > 90F) {
								anglechute = (4F * Time.deltaTime) - (90F - angletotchute);
								angletotchute = 90F;
						}

						//je fait la rotation
						transform.RotateAround (pchute, axechute, anglechute);

						//le perso a fini de pivoter pour chuter
						if ((angletotchute <= 95F) & (angletotchute >= 90F)) {
								if (regard.x != 0)
										transform.position = new Vector3 (pchute.x + (0.81F * regard.x), transform.position.y, transform.position.z);
								if (regard.z != 0)
										transform.position = new Vector3 (transform.position.x, transform.position.y, pchute.z + (0.81F * regard.z));
								transform.rotation = Quaternion.identity;		
								enchute = true;
								angletotchute = 0F;
						}
				}

				//le perso chute
				if (enchute)
						transform.Translate (Vector3.down * gravite * Time.deltaTime);

		}
}