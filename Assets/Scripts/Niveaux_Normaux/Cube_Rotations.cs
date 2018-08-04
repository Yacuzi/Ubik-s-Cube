using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Cube_Rotations : MonoBehaviour
{

	private Transform God, Perso;
	private int rangeex = 0, rangeey = 0, rangeez = 0;
	private int longueur, hauteur, largeur;
	private Vector3 centrePos, axe, marotation;
	private float centreRot, angletot, anglerot;
	private float milx = 0F, mily = 0F, milz = 0F;
	private float clignoterfloat = 0F, vclignote = 12F;
	private int clignoter = 0;
	private Renderer[] quads, verres;
	private bool chute, saut;
	private GameObject[] antikub;
	private bool antikubx = false, antikuby = false, antikubz = false;
	private bool enhaut = false, enbas = false, agauche = false, adroite = false, unefois = true;
	[HideInInspector]
	public bool Larg = false, Haut = false, Long = false;
	[HideInInspector]
	public bool RotationH = false, RotationAH = false;
	private bool finselectionLargeur = false, finselectionHauteur = false, finselectionLongueur = false;
	private bool rotationpretex = false, rotationpretey = false, rotationpretez = false;

	private int pointfinal;
	private GameObject[] allcubes;

	public void ColorBlock (GameObject lekub, Color lacouleur)
	{
		//Je colorie le bloc si c'est pas un mur
		Renderer kubcolor = lekub.GetComponent<Renderer> ();
		kubcolor.material.color = lacouleur;
	}

	//Pour remettre tous les kubs en blanc
	public void ResetKubsColor ()
	{
		foreach (GameObject lekub in allcubes)
		{
			ColorBlock (lekub, Color.white);
		}
	}

	//Pour créer le tableau contenant tous les kubs
	void SetKubs ()
	{
		//Recherche de tous les kubs
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube") as GameObject[];
		GameObject[] kubs = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[];
		GameObject[] antikubs = GameObject.FindGameObjectsWithTag ("Antikub") as GameObject[];
		allcubes = new GameObject[cubes.Length + kubs.Length + antikubs.Length];
		cubes.CopyTo (allcubes, 0);
		kubs.CopyTo (allcubes, cubes.Length);
		antikubs.CopyTo (allcubes, cubes.Length + kubs.Length);
	}

	//Pour récupérer les infos sur les kubs
	public GameObject[] GetKubs ()
	{
		return allcubes;
	}

	//Pour checker si je prépare une rotation
	public bool IsNotRotating ()
	{
		if ((Larg) || (Long) || (Haut))
			return false;
		else
			return true;
	}

	// Use this for initialization
	void Start ()
	{
		//Je définis le tableau des kubs
		SetKubs ();

		//Je récupère le perso et le god
		God = GameObject.FindGameObjectWithTag("God").transform;
		Perso = GameObject.FindGameObjectWithTag("Player").transform;

		//Récupération de la taille du cube
		hauteur = God.GetComponent<CreationCube> ().Hauteur;
		longueur = God.GetComponent<CreationCube> ().Longueur;
		largeur = God.GetComponent<CreationCube> ().Largeur;

		//Creation du point central pour les centre de rotation
		milx = (float)(((float)longueur / 2) - 0.5F);
		mily = (float)(((float)hauteur / 2) - 0.5F);
		milz = (float)(((float)largeur / 2) - 0.5F);

		//Récupération de l'état de la caméra
		pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().GetCamNumber ();

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
		if ((!enhaut) && (!enbas) && (!adroite) && (!agauche) && (!unefois))
			unefois = true;
				
		//je créé un tableau avec toutes les coordonnées des antikub
		antikub = GameObject.FindGameObjectsWithTag ("Antikub");
		int[] Antilongueur = new int[antikub.Length];
		int[] Antilargeur = new int[antikub.Length];
		int[] Antihauteur = new int[antikub.Length];

		//je définis les colonnes, lignes et hauteurs où la rotation est pas possible
		for (int i = 0; i < antikub.Length; i++)
		{
			Antilongueur [i] = Mathf.RoundToInt (antikub [i].transform.position.x);
			Antihauteur [i] = Mathf.RoundToInt (antikub [i].transform.position.y);
			Antilargeur [i] = Mathf.RoundToInt (antikub [i].transform.position.z);
		}

		//je cherche si la rotation est impossible pour les différents sens de rotation
		foreach (int interdit in Antilongueur)
		{
			if (rangeex == interdit)
			{
				antikubx = true;
				break;
			}
			else
				antikubx = false;
		}
		foreach (int interdit in Antihauteur)
		{
			if (rangeey == interdit)
			{
				antikuby = true;
				break;
			}
			else
				antikuby = false;
		}
		foreach (int interdit in Antilargeur)
		{
			if (rangeez == interdit)
			{
				antikubz = true;
				break;
			}
			else
				antikubz = false;
		}
						
//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LARGEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//Rotation préparée pour les rangées en largeur
		if (((Input.GetButtonDown ("Largeur")) && (!Haut) && (!Long) && (pointfinal == 0))
		    || ((Input.GetButtonDown ("Largeur")) && (!Haut) && (!Long) && (pointfinal == 2))
		    || ((Input.GetButtonDown ("Longueur")) && (!Haut) && (!Larg) && (pointfinal == 1))
		    || ((Input.GetButtonDown ("Longueur")) && (!Haut) && (!Larg) && (pointfinal == 3)))
		{
			Larg = true;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{
								
				if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
				{

					if (antikubz)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en bleu
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.blue;
						}
						//je met la verriere en bleu
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.blue);
						}
					}
				}
			}
		}

		//si je lache le bouton pour la largeur, je prépare la fin de la selection
		if ((Input.GetButtonUp ("Largeur") && (pointfinal == 0)) ||
		    (Input.GetButtonUp ("Largeur") && (pointfinal == 2)) ||
		    (Input.GetButtonUp ("Longueur") && (pointfinal == 1)) ||
		    (Input.GetButtonUp ("Longueur") && (pointfinal == 3)))
			finselectionLargeur = true;
				
		if ((finselectionLargeur) && (!RotationH) && (!RotationAH))
		{
			Larg = false;
			finselectionLargeur = false;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
				{
					//		allcubes[i].tag = "Cube";
					//		allcubes[i].transform.SetParent (null, true);
					//}

					//je remet les cubes à leur couleur originelle
					quads = allcubes [i].GetComponents<Renderer> ();
					foreach (Renderer lequad in quads)
					{
						lequad.material = lequad.GetComponent<Couleur> ().materielini;
					}

					//je met la verriere en couleur originelle
					verres = allcubes [i].GetComponentsInChildren<Renderer> ();
					foreach (Renderer leverre in verres)
					{
						leverre.material = leverre.GetComponent<Couleur> ().materielini;
					}
				}
			}
		}
			
		//Déplacement de la rangée large
		if (Larg)
		{
												
			if (((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 0)) ||
			    ((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 1)) ||
			    ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 2)) ||
			    ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 3)))
			{
				if (rangeez != largeur - 1)
				{										
					unefois = false;
					rangeez += 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						//je réinitialise le bloc
						if (allcubes [i].transform.position.z - rangeez < 0.1F - 1)
						{
							//		allcubes[i].tag = "Cube";
							//		allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}

			}
			else if (((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 2)) ||
			         ((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 3)) ||
			         ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 0)) ||
			         ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 1)))
			{
				if (rangeez != 0)
				{
					unefois = false;
					rangeez -= 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if (allcubes [i].transform.position.z - rangeez < 0.1F + 1)
						{
							//	allcubes[i].tag = "Cube";
							//	allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}
			}

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
				{

					if (antikubz)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en bleu
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.blue;
						}

						//je met la verriere en bleu
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.blue);
						}
					}
				}
			}
		}

		//vérification qu'une rotation est lancée
		if ((((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationAH) && (Larg) && (!saut) && (!chute) && (pointfinal == 0)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationAH) && (Larg) && (!saut) && (!chute) && (pointfinal == 3)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationAH) && (Larg) && (!saut) && (!chute) && (pointfinal == 1)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationAH) && (Larg) && (!saut) && (!chute) && (pointfinal == 2)))
			rotationpretez = true;

		//le personnage est pas dans dans la rangée et pas d'antikub donc rotation
		if ((rotationpretez) && ((Perso.position.z + 0.9F <= rangeez) || (Perso.position.z - 0.9F >= rangeez)) && (!antikubz))
		{
			RotationH = true;
			rotationpretez = false;
		}
						//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretez) && ((Perso.position.z - 0.5F > rangeez) || (Perso.position.z + 0.5F < rangeez)) && (!antikubz))
		{
		}
		else if (rotationpretez)
		{
			//pas de rotation de la rangee
			RotationH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
						{
																		
							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
						{

							if (antikubz)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
																				
								//je met les cubes en bleu
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.blue;
								}

								//je met la verriere en bleu
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.blue);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretez = false;
			}
		}

		//vérification qu'une rotation est lancée
		if ((((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationH) && (Larg) && (!saut) && (!chute) && (pointfinal == 0)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationH) && (Larg) && (!saut) && (!chute) && (pointfinal == 3)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationH) && (Larg) && (!saut) && (!chute) && (pointfinal == 1)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationH) && (Larg) && (!saut) && (!chute) && (pointfinal == 2)))
			rotationpretez = true;

		//le personnage est pas dans dans la rangée donc rotation
		if ((rotationpretez) && ((Perso.position.z + 0.9F <= rangeez) || (Perso.position.z - 0.9F >= rangeez)) && (!antikubz))
		{
			RotationAH = true;
			rotationpretez = false;
		}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretez) && ((Perso.position.z - 0.5F > rangeez) || (Perso.position.z + 0.5F < rangeez)) && (!antikubz))
		{
		}
		else if (rotationpretez)
		{
			//pas de rotation de la rangee
			RotationAH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
						{
							//		allcubes[i].tag = "Bloc";
							//		allcubes[i].transform.SetParent (Bloc.transform, true);

							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
						{

							if (antikubz)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
								//je met les cubes en bleu
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.blue;
								}

								//je met la verriere en bleu
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.blue);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretez = false;
			}
		}
				

		//rotation horaire des cubes
		if ((RotationH) && (Larg))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//réinitialisation de la rotation

			//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{
								
				if ((anglerot + 0.3F >= Mathf.Abs (angletot)) && (Mathf.Abs (angletot) >= anglerot - 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
					centreRot = 0;
					RotationH = false;
					angletot = 0;
				}
			}
		}

		//rotation anti-horaire des cubes
		if ((RotationAH) && (Larg))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.z - rangeez < 0.1F) && (allcubes [i].transform.position.z - rangeez > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{
								
				//réinitialisation de la rotation
				if ((anglerot - 0.3F <= angletot) && (angletot <= anglerot + 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
					centreRot = 0;
					RotationAH = false;
					angletot = 0;
				}
			}
		}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++HAUTEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//Rotation préparée pour les rangées en hauteur
		if ((Input.GetButtonDown ("Hauteur")) && (!Larg) && (!Long))
		{
			Haut = true;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
				{

					if (antikuby)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en jaune
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.yellow;
						}

						//je met la verriere en jaune
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
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
		if ((finselectionHauteur) && (!RotationH) && (!RotationAH))
		{
			Haut = false;
			finselectionHauteur = false;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
				{
					// allcubes[i].tag = "Cube";
					// allcubes[i].transform.SetParent (null, true);
														
														
				}

				//je remet les cubes à leur couleur originelle
				quads = allcubes [i].GetComponents<Renderer> ();
				foreach (Renderer lequad in quads)
				{
					lequad.material = lequad.GetComponent<Couleur> ().materielini;
				}

				//je met la verriere en couleur originelle
				verres = allcubes [i].GetComponentsInChildren<Renderer> ();
				foreach (Renderer leverre in verres)
				{
					leverre.material = leverre.GetComponent<Couleur> ().materielini;
				}
			}
		}

		//Déplacement de la rangée large
		if (Haut)
		{
												
			if ((enhaut) && (unefois) && (!RotationH) && (!RotationAH))
			{
				if (rangeey != hauteur - 1)
				{
					unefois = false;
					rangeey += 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						//je réinitialise le bloc
						if (allcubes [i].transform.position.y - rangeey < 0.1F - 1)
						{
							// allcubes[i].tag = "Cube";
							// allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}

			}
			else if ((enbas) && (unefois) && (!RotationH) && (!RotationAH))
			{
				if (rangeey != 0)
				{
					unefois = false;
					rangeey -= 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if (allcubes [i].transform.position.y - rangeey < 0.1F + 1)
						{
							// allcubes[i].tag = "Cube";
							// allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}
			}

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
				{

					if (antikuby)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en jaune
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.yellow;
						}

						//je met la verriere en jaune
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.yellow);
						}
					}
				}
			}
		}

		//vérification qu'une rotation est lancée
		if (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationAH) && (Haut) && (!saut) && (!chute))
			rotationpretey = true;
				
		if ((rotationpretey) && ((int)(Perso.position.y - 0.8F) != rangeey) && ((int)(Perso.position.y + 0.2F) != rangeey) && (!antikuby))
		{
			RotationH = true;
			rotationpretey = false;

		}
		else if (rotationpretey)
		{
			//pas de rotation de la rangee
			RotationH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
										
					///je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
						{
																		
							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
						{

							if (antikuby)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
								//je met les cubes en jaune
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.yellow;
								}

								//je met la verriere en jaune
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.yellow);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretey = false;
			}
		}

		//vérification qu'une rotation est lancée
		if (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationH) && (Haut) && (!saut) && (!chute))
			rotationpretey = true;

		if ((rotationpretey) && ((int)(Perso.position.y - 0.8F) != rangeey) && ((int)(Perso.position.y + 0.2F) != rangeey) && (!antikuby))
		{
			RotationAH = true;
			rotationpretey = false;

		}
		else if (rotationpretey)
		{
			//pas de rotation de la rangee
			RotationAH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
						{
							//		allcubes[i].tag = "Bloc";
							//		allcubes[i].transform.SetParent (Bloc.transform, true);

							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
										
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
						{

							if (antikuby)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
								//je met les cubes en jaune
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.yellow;
								}

								//je met la verriere en jaune
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.yellow);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretey = false;
			}
		}
				

		//rotation horaire des cubes
		if ((RotationH) && (Haut))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//réinitialisation de la rotation
				if ((anglerot + 0.3F >= Mathf.Abs (angletot)) && (Mathf.Abs (angletot) >= anglerot - 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
					centreRot = 0;
					RotationH = false;
					angletot = 0;
				}
			}
		}

		//rotation anti-horaire des cubes
		if ((RotationAH) && (Haut))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.y - rangeey < 0.1F) && (allcubes [i].transform.position.y - rangeey > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//réinitialisation de la rotation
				if ((anglerot - 0.3F <= angletot) && (angletot <= anglerot + 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
					centreRot = 0;
					RotationAH = false;
					angletot = 0;
				}
			}
		}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++LONGUEUR++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

		//Rotation préparée pour les rangées en longueur
		if (((Input.GetButtonDown ("Largeur")) && (!Haut) && (!Long) && (pointfinal == 1))
		    || ((Input.GetButtonDown ("Largeur")) && (!Haut) && (!Long) && (pointfinal == 3))
		    || ((Input.GetButtonDown ("Longueur")) && (!Haut) && (!Larg) && (pointfinal == 0))
		    || ((Input.GetButtonDown ("Longueur")) && (!Haut) && (!Larg) && (pointfinal == 2)))
		{
			Long = true;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
				{

					if (antikubx)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en rouge
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.red;
						}

						//je met la verriere en rouge
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.red);
						}
					}
				}
			}
		}

		//si je lache le bouton pour la largeur, je prépare la fin de la selection
		if ((Input.GetButtonUp ("Largeur") && (pointfinal == 1)) ||
		    (Input.GetButtonUp ("Largeur") && (pointfinal == 3)) ||
		    (Input.GetButtonUp ("Longueur") && (pointfinal == 2)) ||
		    (Input.GetButtonUp ("Longueur") && (pointfinal == 0)))
			finselectionLongueur = true;

		//J'arrête la sélection du bloc en longueur
		if ((finselectionLongueur) && (!RotationH) && (!RotationAH))
		{
			Long = false;
			finselectionLongueur = false;

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
				{
					// allcubes[i].tag = "Cube";
					// allcubes[i].transform.SetParent (null, true);
														
														
				}

				//je remet les cubes à leur couleur originelle
				quads = allcubes [i].GetComponents<Renderer> ();
				foreach (Renderer lequad in quads)
				{
					lequad.material = lequad.GetComponent<Couleur> ().materielini;
				}

				//je met la verriere en couleur originelle
				verres = allcubes [i].GetComponentsInChildren<Renderer> ();
				foreach (Renderer leverre in verres)
				{
					leverre.material = leverre.GetComponent<Couleur> ().materielini;
				}
			}
		}

		//Déplacement de la rangée large
		if (Long)
		{
												
			if (((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 1)) ||
			    ((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 2)) ||
			    ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 3)) ||
			    ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 0)))
			{
				if (rangeex != longueur - 1)
				{
					unefois = false;
					rangeex += 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						//je réinitialise le bloc
						if (allcubes [i].transform.position.x - rangeex < 0.1F - 1)
						{
							// allcubes[i].tag = "Cube";
							// allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}

			}
			else if (((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 0)) ||
			         ((agauche) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 3)) ||
			         ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 2)) ||
			         ((adroite) && (unefois) && (!RotationH) && (!RotationAH) && (pointfinal == 1)))
			{
				if (rangeex != 0)
				{
					unefois = false;
					rangeex -= 1;

					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if (allcubes [i].transform.position.x - rangeex < 0.1F + 1)
						{
							// allcubes[i].tag = "Cube";
							// allcubes[i].transform.SetParent (null, true);
																		
																		

							//je remet les cubes à leur couleur originelle
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en couleur originelle
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}
				}
			}

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
				{

					if (antikubx)
					{
						//je met les cubes en noir
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.black;
						}
						//je met la verriere en noir
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.black);
						}
					}
					else
					{
						//je met les cubes en rouge
						quads = allcubes [i].GetComponents<Renderer> ();
						foreach (Renderer lequad in quads)
						{
							lequad.material.color = Color.red;
						}

						//je met la verriere en rouge
						verres = allcubes [i].GetComponentsInChildren<Renderer> ();
						foreach (Renderer leverre in verres)
						{
							leverre.material.SetColor ("_EmissionColor", Color.red);
						}
					}
				}
			}
		}

		//vérification qu'une rotation est lancée
		if ((((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationAH) && (Long) && (!saut) && (!chute) && (pointfinal == 0)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationAH) && (Long) && (!saut) && (!chute) && (pointfinal == 1)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationAH) && (Long) && (!saut) && (!chute) && (pointfinal == 2)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationAH) && (Long) && (!saut) && (!chute) && (pointfinal == 3)))
			rotationpretex = true;

		//le personnage est pas dans dans la rangée donc rotation
		if ((rotationpretex) && ((Perso.position.x + 0.9F <= rangeex) || (Perso.position.x - 0.9F >= rangeex)) && (!antikubx))
		{
			RotationH = true;
			rotationpretex = false;
		}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretex) && ((Perso.position.x - 0.5F > rangeex) || (Perso.position.x + 0.5F < rangeex)) && (!antikubx))
		{
		}
		else if (rotationpretex)
		{
			//pas de rotation de la rangee
			RotationH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
						{
							//		allcubes[i].tag = "Bloc";
							//		allcubes[i].transform.SetParent (Bloc.transform, true);

							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
						{

							if (antikubx)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
								//je met les cubes en rouge
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.red;
								}

								//je met la verriere en rouge
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.red);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretex = false;
			}
		}

		//vérification qu'une rotation est lancée
		if ((((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationH) && (Long) && (!saut) && (!chute) && (pointfinal == 0)) ||
		    (((Input.GetButtonDown ("RotationAH")) || (Input.GetAxis ("RotationJH") == 1)) && (!RotationH) && (Long) && (!saut) && (!chute) && (pointfinal == 1)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationH) && (Long) && (!saut) && (!chute) && (pointfinal == 2)) ||
		    (((Input.GetButtonDown ("RotationH")) || (Input.GetAxis ("RotationJAH") == 1)) && (!RotationH) && (Long) && (!saut) && (!chute) && (pointfinal == 3)))
			rotationpretex = true;

		//le personnage est pas dans dans la rangée donc rotation
		if ((rotationpretex) && ((Perso.position.x + 0.9F <= rangeex) || (Perso.position.x - 0.9F >= rangeex)) && (!antikubx))
		{
			RotationAH = true;
			rotationpretex = false;
		}
				//le personnage est un peu dans la rangée, bouge le perso, puis rotation
				else if ((rotationpretex) && ((Perso.position.x - 0.5F > rangeex) || (Perso.position.x + 0.5F < rangeex)) && (!antikubx))
		{
		}
		else if (rotationpretex)
		{
			//pas de rotation de la rangee
			RotationAH = false;

			//je clignote en blanc tous les sixièmes de seconde
			if (clignoter < 6)
			{
				if (clignoter % 2 == 0)
				{
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
						{
																		
							//je met les cubes en blanc
							quads = allcubes [i].GetComponents<Renderer> ();
							foreach (Renderer lequad in quads)
							{
								lequad.material = lequad.GetComponent<Couleur> ().materielini;
							}

							//je met la verriere en blanc
							verres = allcubes [i].GetComponentsInChildren<Renderer> ();
							foreach (Renderer leverre in verres)
							{
								leverre.material = leverre.GetComponent<Couleur> ().materielini;
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;

					// je clignote en jaune tous les sixièmes de seconde
				}
				else
				{
					//je cherche dans le tableau les cubes qui m'intéressent
					for (int i = 0; i < allcubes.Length; i++)
					{

						if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
						{

							if (antikubx)
							{
								//je met les cubes en noir
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.black;
								}
								//je met la verriere en noir
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.black);
								}
							}
							else
							{
								//je met les cubes en rouge
								quads = allcubes [i].GetComponents<Renderer> ();
								foreach (Renderer lequad in quads)
								{
									lequad.material.color = Color.red;
								}

								//je met la verriere en rouge
								verres = allcubes [i].GetComponentsInChildren<Renderer> ();
								foreach (Renderer leverre in verres)
								{
									leverre.material.SetColor ("_EmissionColor", Color.red);
								}
							}
						}
					}

					//j'attends avant de changer de clignotement
					clignoterfloat += vclignote * Time.deltaTime;
					clignoter = (int)clignoterfloat;
				}
			}
			else
			{
				//je réinitialise le clignotement
				clignoter = 0;
				clignoterfloat = 0;
				rotationpretex = false;
			}
		}

		//rotation horaire des cubes
		if ((RotationH) && (Long))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//réinitialisation de la rotation
				if ((anglerot + 0.3F >= Mathf.Abs (angletot)) && (Mathf.Abs (angletot) >= anglerot - 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (-anglerot - angletot));
					centreRot = 0;
					RotationH = false;
					angletot = 0;
				}
			}
		}

		//rotation anti-horaire des cubes
		if ((RotationAH) && (Long))
		{

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
			for (int i = 0; i < allcubes.Length; i++)
			{

				//si le cube est dans la rangee
				//je créé un bloc avec les nouveaux cubes sélectionnés
				if ((allcubes [i].transform.position.x - rangeex < 0.1F) && (allcubes [i].transform.position.x - rangeex > -0.1F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, centreRot);
				}
			}

			//ATTENTION POTENTIEL BUG, A VERIFIER !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			//je cherche dans le tableau les cubes qui m'intéressent
			for (int i = 0; i < allcubes.Length; i++)
			{

				//réinitialisation de la rotation
				if ((anglerot - 0.3F <= angletot) && (angletot <= anglerot + 0.3F))
				{
					allcubes [i].transform.RotateAround (centrePos, axe, (anglerot - angletot));
					centreRot = 0;
					RotationAH = false;
					angletot = 0;
				}
			}
		}
	}
}