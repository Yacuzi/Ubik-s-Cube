using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage : MonoBehaviour
{

	public Transform God;
	public float vitesse = 5F, vpetitsaut = 1F, gravite = 1F;
	private Renderer kubcolor;
	private string retry = "rien";
	private bool Rotx, Roty, Rotz, yacube, bloque;
	[HideInInspector]
	public bool devant = false, derriere = false, gauche = false, droite = false, kubappui = true, kubappuitemp = true;
	private int longueur, hauteur, largeur;
	private Vector3 a, b, c, d, abis, bbis, cbis, dbis;
	private Vector3 posfuture;
	private Vector3 rotsaut, axesaut, petitsaut, velocity = Vector3.zero;
	private Vector3 pchute, axechute;
	private Vector3 decalagerot = Vector3.zero;
	private float distsaut = 1000F, anglesaut, stoppos, angletot, angletotchute;
	private int kubx, kuby, kubz;
	private int persox, persoy, persoz, pointfinal;
	private int kubsautx, kubsauty, kubsautz;
	private int kubappuix, kubappuiy, kubappuiz;
	private int kubxchute, kubychute, kubzchute;
	private float anglechute;
	private bool persobougex, persobougey, persobougez;
	private float persobougeX, persobougeY, persobougeZ;
	private int kubfuturx, kubfuturz;
	private bool yacubstop = false, yamurstop = false, wait = false, bougepas = false, kubdesequilibre = false;
	private float waittime;
	[HideInInspector]
	public bool sautpret = false, ensaut = false, enchute = false, pousse = false;
	[HideInInspector]
	public Vector3 regard, direction, possaut;
	[HideInInspector]
	public GameObject[] kubs;
	[HideInInspector]
	public int pretbouger;

	// Use this for initialization
	void Start ()
	{
		//initialisation position personnage
		//transform.position = new Vector3 (0,-(transform.localScale.y/2),0);

		//je cache le curseur
		Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update ()
	{

		//vérifie si le perso doit bouger à cause d'une rotation
		persobougex = Camera.main.GetComponent<Cube_Rotations> ().persoreplacex;
		persobougey = Camera.main.GetComponent<Cube_Rotations> ().persoreplacey;
		persobougez = Camera.main.GetComponent<Cube_Rotations> ().persoreplacez;

		//recherche de tous les kubs
		GameObject[] kubs1 = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[];
		GameObject[] antikubs = GameObject.FindGameObjectsWithTag ("Antikub") as GameObject[];
		kubs = new GameObject[kubs1.Length + antikubs.Length];
		kubs1.CopyTo (kubs, 0);
		antikubs.CopyTo (kubs, (kubs1.Length));

		//récupération de l'état de la rotation
		Rotx = Camera.main.GetComponent<Cube_Rotations> ().Estlarg;
		Roty = Camera.main.GetComponent<Cube_Rotations> ().Esthaut;
		Rotz = Camera.main.GetComponent<Cube_Rotations> ().Estlong;

		//récupération de l'état de la caméra
		pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().GetCamNumber ();

		//récupération de la largeur longueur et hauteur
		largeur = God.GetComponent<CreationCube> ().Largeur;
		hauteur = God.GetComponent<CreationCube> ().Hauteur;
		longueur = God.GetComponent<CreationCube> ().Longueur;

		//position arrondie à l'entier du perso
		persox = Mathf.RoundToInt (transform.position.x);
		persoy = Mathf.RoundToInt (transform.position.y);
		persoz = Mathf.RoundToInt (transform.position.z);

		//récupération des touches
		if ((Input.GetButton ("Haut") & (!bougepas)) | (Input.GetAxisRaw ("VerticalJ") == -1)) {
			devant = true;
		} else
			devant = false;

		if ((Input.GetButton ("Bas") & (!bougepas)) | (Input.GetAxisRaw ("VerticalJ") == 1)) {
			derriere = true;
		} else
			derriere = false;

		if ((Input.GetButton ("Gauche") & (!bougepas)) | (Input.GetAxisRaw ("HorizontalJ") == -1)) {
			gauche = true;
		} else
			gauche = false;

		if ((Input.GetButton ("Droite") & (!bougepas)) | (Input.GetAxisRaw ("HorizontalJ") == 1)) {
			droite = true;
		} else
			droite = false;
		if ((!devant) & (!derriere) & (!gauche) & (!droite)) {
			posfuture = transform.position;
			direction = Vector3.zero;
		}
						
		if ((!Rotx) ^ (!Roty) ^ (!Rotz)) {
	
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point a++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (pointfinal == 0) {

				//Les quatre directions
				if ((devant) & (!gauche) & (!droite) & (!derriere))
					direction = new Vector3 (1, 0, 0);
				if ((devant) & (!gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {
												
						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);
																																				
						if ((posfuture.x + (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x + (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
														
							yacube = true;
							stoppos = kubx;
						}
					}

					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos - ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {
						yacubstop = false;
						//collision avec les bords
						if (posfuture.x + ((transform.localScale.x * 0.5F) + 0.5F) >= longueur) {
							transform.position = new Vector3 (longueur - ((transform.localScale.x * 0.5F) + 0.51F), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
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

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x - (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x - (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos + ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {
						yacubstop = false;
						//collision avec les bords
						if (posfuture.x - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (-(0.49F - (transform.localScale.x * 0.5F)), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}

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

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z + (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x * 0.51F) + 0.5F));
					} else {
														
						yacubstop = false;
						//collision avec les bords
						if (posfuture.z + ((transform.localScale.x * 0.5F) + 0.5F) >= largeur) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x * 0.5F) + 0.51F));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
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

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z - (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x * 0.51F) + 0.5F));
					} else {
														
						yacubstop = false;
						//collision avec les bords
						if (posfuture.z - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x * 0.5F)));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}

					}
				}
				yacube = false;	
			}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point b++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (pointfinal == 1) {

				//Les quatre directions
				if ((devant) & (!gauche) & (!droite) & (!derriere))
					direction = new Vector3 (0, 0, -1);
				if ((devant) & (!gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, -0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z - (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
														
							yacube = true;
							stoppos = kubz;
						}
					}

					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x * 0.5F)));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;
				}

				if ((!devant) & (!gauche) & (!droite) & (derriere))
					direction = new Vector3 (0, 0, 1);
				if ((!devant) & (!gauche) & (!droite) & (derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, 0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z + (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z + ((transform.localScale.x * 0.5F) + 0.5F) >= largeur) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x * 0.5F) + 0.51F));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}

					yacube = false;	
				}

				if ((!devant) & (gauche) & (!droite) & (!derriere))
					direction = new Vector3 (1, 0, 0);
				if ((!devant) & (gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x + (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x + (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos - ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x + ((transform.localScale.x * 0.5F) + 0.5F) >= longueur) {
							transform.position = new Vector3 (longueur - ((transform.localScale.x * 0.5F) + 0.51F), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;	
				}

				if ((!devant) & (!gauche) & (droite) & (!derriere))
					direction = new Vector3 (-1, 0, 0);
				if ((!devant) & (!gauche) & (droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (-0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x - (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x - (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos + ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (-(0.49F - (transform.localScale.x * 0.5F)), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}

					}
				}
				yacube = false;	
			}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point c++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (pointfinal == 2) {

				//Les quatre directions
				if ((!devant) & (!gauche) & (!droite) & (derriere))
					direction = new Vector3 (1, 0, 0);
				if ((!devant) & (!gauche) & (!droite) & (derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x + (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x + (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {

							yacube = true;
							stoppos = kubx;
						}
					}

					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos - ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x + ((transform.localScale.x * 0.5F) + 0.5F) >= longueur) {
							transform.position = new Vector3 (longueur - ((transform.localScale.x * 0.5F) + 0.51F), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;
				}

				if ((devant) & (!gauche) & (!droite) & (!derriere))
					direction = new Vector3 (-1, 0, 0);
				if ((devant) & (!gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (-0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x - (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x - (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos + ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (-(0.49F - (transform.localScale.x * 0.5F)), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}

					yacube = false;	
				}

				if ((!devant) & (!gauche) & (droite) & (!derriere))
					direction = new Vector3 (0, 0, 1);
				if ((!devant) & (!gauche) & (droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, 0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z + (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z + ((transform.localScale.x * 0.5F) + 0.5F) >= largeur) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x * 0.5F) + 0.51F));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;	
				}

				if ((!devant) & (gauche) & (!droite) & (!derriere))
					direction = new Vector3 (0, 0, -1);
				if ((!devant) & (gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, -0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z - (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x * 0.5F)));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}

					}
				}
				yacube = false;	
			}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++Caméra au point d++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

			if (pointfinal == 3) {

				//Les quatre directions
				if ((!devant) & (!gauche) & (!droite) & (derriere))
					direction = new Vector3 (0, 0, -1);
				if ((!devant) & (!gauche) & (!droite) & (derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, -0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z - (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {

							yacube = true;
							stoppos = kubz;
						}
					}

					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos + ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, -(0.49F - (transform.localScale.x * 0.5F)));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;
				}

				if ((devant) & (!gauche) & (!droite) & (!derriere))
					direction = new Vector3 (0, 0, 1);
				if ((devant) & (!gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.forward * vitesse * Time.deltaTime));
					regard = new Vector3 (0, 0, 0.5F);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.z + (transform.localScale.x * 0.5F) <= kubz + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.5F)
						          & (posfuture.x - (transform.localScale.x * 0.5F) < kubx + 0.499F)
						          & (posfuture.x + (transform.localScale.x * 0.5F) > kubx - 0.499F)) {
							yacube = true;
							stoppos = kubz;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (transform.position.x, transform.position.y, stoppos - ((transform.localScale.x * 0.51F) + 0.5F));
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.z + ((transform.localScale.x * 0.5F) + 0.5F) >= largeur) {
							transform.position = new Vector3 (transform.position.x, transform.position.y, largeur - ((transform.localScale.x * 0.5F) + 0.51F));
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}

					yacube = false;	
				}

				if ((!devant) & (!gauche) & (droite) & (!derriere))
					direction = new Vector3 (1, 0, 0);
				if ((!devant) & (!gauche) & (droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position + (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x + (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x + (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos - ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x + ((transform.localScale.x * 0.5F) + 0.5F) >= longueur) {
							transform.position = new Vector3 (longueur - ((transform.localScale.x * 0.5F) + 0.51F), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
						}
					}
					yacube = false;	
				}

				if ((!devant) & (gauche) & (!droite) & (!derriere))
					direction = new Vector3 (-1, 0, 0);
				if ((!devant) & (gauche) & (!droite) & (!derriere)
				        & (pretbouger == 0) & (!ensaut) & (kubappui) & (!kubdesequilibre)) {
					posfuture = (transform.position - (transform.right * vitesse * Time.deltaTime));
					regard = new Vector3 (-0.5F, 0, 0);

					//collision avec les kubs
					foreach (GameObject kub in kubs) {

						kubx = Mathf.RoundToInt (kub.transform.position.x);
						kuby = Mathf.RoundToInt (kub.transform.position.y);
						kubz = Mathf.RoundToInt (kub.transform.position.z);

						if ((posfuture.x - (transform.localScale.x * 0.5F) <= kubx + 0.5F)
						          & (persoy == kuby)
						          & (posfuture.x - (transform.localScale.x * 0.5F) >= kubx - 0.5F)
						          & (posfuture.z - (transform.localScale.x * 0.5F) <= kubz + 0.499F)
						          & (posfuture.z + (transform.localScale.x * 0.5F) >= kubz - 0.499F)) {
							yacube = true;
							stoppos = kubx;
						}
					}
					if (yacube) {
						yacubstop = true;
						yamurstop = false;
						transform.position = new Vector3 (stoppos + ((transform.localScale.x * 0.51F) + 0.5F), transform.position.y, transform.position.z);
					} else {

						yacubstop = false;
						//collision avec les bords
						if (posfuture.x - (transform.localScale.x * 0.5F) <= -0.5F) {
							transform.position = new Vector3 (-(0.49F - (transform.localScale.x * 0.5F)), transform.position.y, transform.position.z);
							yamurstop = true;
						} else {
							transform.position = posfuture;
							yamurstop = false;
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
		      & (Input.GetButtonUp ("Saut"))
		      & (!ensaut)
		      & (sautpret)
		      & (!pousse)) {

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
				petitsaut = new Vector3 (kubsautx - regard.x - (transform.localScale.x * 0.5F), kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz);
			}

			if (regard.x == -0.5F) {
				petitsaut = new Vector3 (kubsautx - regard.x + (transform.localScale.x * 0.5F), kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz);
			}

			if (regard.z == 0.5F) {
				petitsaut = new Vector3 (kubsautx, kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz - regard.z - (transform.localScale.x * 0.5F));
			}

			if (regard.z == -0.5F) {
				petitsaut = new Vector3 (kubsautx, kubsauty + (0.5F - (transform.localScale.y * 0.5F)), kubsautz - regard.z + (transform.localScale.x * 0.5F));
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
			transform.position = new Vector3 (petitsaut.x + ((2 * transform.localScale.x) * regard.x), petitsaut.y + (transform.localScale.x), petitsaut.z + ((2 * transform.localScale.x) * regard.z));
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

		kubfuturx = Mathf.RoundToInt (posfuture.x + (0.8F * regard.x));
		kubfuturz = Mathf.RoundToInt (posfuture.z + (0.8F * regard.z));

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

				//je cherche si le perso est pile-poil sur un cube
				if ((!ensaut) & (kubfuturx == kubxchute) & (kubfuturz == kubzchute)
				        & (((regard.x != 0) & (transform.position.z <= (persoz + 0.11F)) & (transform.position.z >= (persoz - 0.11F)))
				        | ((regard.z != 0) & (transform.position.x < (persox + 0.11F)) & (transform.position.x > (persox - 0.11F))))
				        & (transform.position.y <= kubychute + 1F) & (transform.position.y >= kubychute + (transform.localScale.x))) {

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
					transform.position = new Vector3 (transform.position.x, kubychute + ((transform.localScale.x * 0.5F) + 0.5F), transform.position.z);

					//je trouve les coordonnées du cube sur lequel repose le perso
					kubappuix = Mathf.RoundToInt (kub.transform.position.x);
					kubappuiy = Mathf.RoundToInt (kub.transform.position.y);
					kubappuiz = Mathf.RoundToInt (kub.transform.position.z);

					//je sort de la boucle
					break;
				} else {
					kubappuitemp = false;
					kubdesequilibre = false;
				}
			}

			//je vérifie que le personnage est pile sur un cube, pas à moitié
			if ((!kubappuitemp) & (!ensaut) & (!yacubstop) & (!yamurstop)) {
				if ((regard.x != 0) & (transform.position.z <= (persoz + 0.11F))
				        & (transform.position.z >= (persoz - 0.11F))) {
					kubappui = false;
					kubdesequilibre = false;
				} else if ((regard.z != 0) & (transform.position.x <= (persox + 0.11F))
				               & (transform.position.x >= (persox - 0.11F))) {
					kubappui = false;
					kubdesequilibre = false;
				} else
					kubdesequilibre = true;
			}
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

		//déséqulibre si un semblant d'appui
		if ((angletotchute > 45F) & (kubdesequilibre) & (!enchute)) {
			//définition de la rotation de la chute
			anglechute = -(150F * Time.deltaTime);
			angletotchute = angletotchute + anglechute;

			//je fait la rotation
			transform.RotateAround (pchute, axechute, anglechute);
		}
				
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
					transform.position = new Vector3 (pchute.x + (0.8F * regard.x), transform.position.y, transform.position.z);
				if (regard.z != 0)
					transform.position = new Vector3 (transform.position.x, transform.position.y, pchute.z + (0.8F * regard.z));
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
		persobougeX = Mathf.RoundToInt (transform.position.x);
		persobougeY = Mathf.RoundToInt (transform.position.y);
		persobougeZ = Mathf.RoundToInt (transform.position.z);

		//recentrer si dans la largeur (X)
		if (persobougex) {
			decalagerot = new Vector3 (persobougeX, transform.position.y, transform.position.z);
			transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
		}
		//recentrer si dans la largeur (Y)
		if (persobougey) {
			decalagerot = new Vector3 (transform.position.x, persobougeY, transform.position.z);
			transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
		}
		//recentrer si dans la largeur (Z)
		if (persobougez) {
			decalagerot = new Vector3 (transform.position.x, transform.position.y, persobougeZ);
			transform.position = Vector3.SmoothDamp (transform.position, decalagerot, ref velocity, Time.deltaTime * 0.5F);
		}
						

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++ "RESET" ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		retry = SceneManager.GetActiveScene ().name;

		if (Input.GetButtonDown ("Reset")) {
			SceneManager.LoadScene (retry);
		}
	}
}