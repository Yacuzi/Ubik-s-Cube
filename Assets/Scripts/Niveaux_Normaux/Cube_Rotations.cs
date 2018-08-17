using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

public class Cube_Rotations : MonoBehaviour
{
	public float speedrot;
	public bool Rotation_Largeur, Rotation_Hauteur, Rotation_Longueur;

	[HideInInspector]
	public bool Larg, Haut, Long;
	[HideInInspector]
	public bool RotationH, RotationAH;

	private Controle_Personnage Perso;
	private int pointfinal;

	private List<GameObject> cubesrot = new List<GameObject> (), verriererot = new List<GameObject> ();
	private GameObject[] allcubes;
	private int rangeex = 0, rangeey = 0, rangeez = 0;
	private int longueur, hauteur, largeur;
	private int clignoter = 0;
	private Vector3 centreRot;
	private float angletot;
	private bool goup, godown, goleft, goright, axisreleased;
	private bool cligne, rotantikub;
	private string rangee;
	private Color lacouleur;

	void Start ()
	{
		SetKubs (); //Je définis le tableau des kubs

		//Je récupère le perso
		Perso = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controle_Personnage> ();

		//Récupération de la taille du cube
		largeur = Mathf.RoundToInt (GetCubeSize ().x);
		hauteur = Mathf.RoundToInt (GetCubeSize ().y);
		longueur = Mathf.RoundToInt (GetCubeSize ().z);

		//Creation du point central pour les centre de rotation
		float milx = (float)(((float)largeur / 2) - 0.5F);
		float mily = (float)(((float)hauteur / 2) - 0.5F);
		float milz = (float)(((float)longueur / 2) - 0.5F);
		centreRot = new Vector3 (milx, mily, milz);
	}

	public Vector3 GetCubeSize () //Méthode pour récupérer la taille du niveau
	{
		GameObject[] Cube;
		int x = 0, y = 0, z = 0;
		Vector3 size;

		Cube = GameObject.FindGameObjectsWithTag ("Cube"); //Je récupère tous les cubes du niveau

		foreach (GameObject uncube in Cube) //Je récupère la plus grande coordonnée que je trouve à chaque fois
		{
			if (Mathf.RoundToInt (uncube.transform.position.x) + 1 > x)
				x = Mathf.RoundToInt (uncube.transform.position.x) + 1;
			if (Mathf.RoundToInt (uncube.transform.position.y) + 1 > y)
				y = Mathf.RoundToInt (uncube.transform.position.y) + 1;
			if (Mathf.RoundToInt (uncube.transform.position.z) + 1 > z)
				z = Mathf.RoundToInt (uncube.transform.position.z) + 1;
		}

		size = new Vector3 (x, y, z);

		return size;
	}

	public void ColorBlock (GameObject lekub, Color lacouleur) //La fonction pour colorier un bloc d'une certaine couleur
	{
		if (lekub.tag == "Verriere") //Si c'est un morceau de verriere je change l'alpha
			lacouleur.a = lekub.GetComponent<Renderer> ().material.color.a;
		else
			lacouleur.a = 0f;

		if (lekub.tag == "Player") //Si c'est le joueur, c'est son émission que je change
			lekub.GetComponent<Renderer> ().material.SetColor("_EmissionColor",lacouleur);
		else if (lekub.GetComponent<Renderer> ())
		{
			Renderer kubcolor = lekub.GetComponent<Renderer> (); //Je colorie le bloc si c'est pas un mur
			kubcolor.material.color = lacouleur;
		}
	}

	public void ResetKubsColor () //Pour colorier les kubs dans un dégradé de gris et le reste du cube en blanc
	{
		Color lacouleur = Color.white;
		float degrade = 0;

		foreach (GameObject lekub in allcubes)
		{
			if (lekub.tag == "Verriere") //Je laisse la verriere transparente
				lacouleur.a = lekub.GetComponent<Renderer> ().material.color.a;
			else
				lacouleur.a = 0f;

			if (lekub.tag != "Verriere") //Si c'est un kub ou un morceau de la paroi
			{
				degrade = (lekub.transform.position.y + 1) / (float)hauteur; //Je détermine sa couleur fonction de sa hauteur
				lacouleur = new Color (degrade, degrade, degrade); //J'assigne sa couleur
			}
			else
				lacouleur = Color.white;
			
			ColorBlock (lekub, lacouleur);
		}
	}

	void SetKubs () //Pour créer le tableau contenant tous les kubs
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube") as GameObject[]; //Recherche de tous les cubes
		GameObject[] kubs = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[]; //Recherche de tous les kubs
		GameObject[] antikubs = GameObject.FindGameObjectsWithTag ("Antikub") as GameObject[]; //Recherche de tous les antikubs
		GameObject[] verriere = GameObject.FindGameObjectsWithTag ("Verriere") as GameObject[]; //Recherche de tous les kubs de la verriere
		allcubes = new GameObject[cubes.Length + kubs.Length + antikubs.Length + verriere.Length];
		cubes.CopyTo (allcubes, 0);
		kubs.CopyTo (allcubes, cubes.Length);
		antikubs.CopyTo (allcubes, cubes.Length + kubs.Length);
		verriere.CopyTo (allcubes, cubes.Length + kubs.Length + antikubs.Length);
	}

	public GameObject[] GetKubs () //Pour récupérer les infos sur les kubs
	{
		return allcubes;
	}

	public bool RotationReady () //Pour checker si je prépare une rotation
	{
		if ((Larg) || (Long) || (Haut))
			return true;
		else
			return false;
	}

	bool InputPrepareRotation () //Méthode pour déterminer quelle partie du Cube le joueur sélectionne
	{
		if (Perso.Immobile ()) //Si le perso a fini toutes ses actions je peux sélectionner quelquechose
		{
			if (Rotation_Largeur) //Je récupère les inputs en précisant bien que je le fais seulement si j'ai le droit dans ce niveau
				Larg = Input.GetButton ("Largeur");
			if (Rotation_Hauteur)
				Haut = Input.GetButton ("Hauteur");
			if (Rotation_Longueur)
				Long = Input.GetButton ("Longueur");

			if (!(Larg ^ Haut ^ Long) || (Larg && Haut && Long)) //Si j'appuie sur deux boutons en même temps
			{
				return false;
			}
			else
				return true;
		}
		else
			return false;
	}

	void CheckAntiKub (GameObject kub)
	{
		if (kub.tag == "Antikub") //Si ya un antikub
		{
			rotantikub = true; //Je déclare qu'il y a un antikub
			lacouleur = Color.black; //On coloriera tout en noir parce que c'est trop dark
		}
	}

	bool KubInRow (GameObject kub, string rangee) //Fonction pour checker si un kub est dans la rangée qui intéresse le joueur
	{
		int kubint = 0;
		int row = 0;

		if (rangee == "x")
		{
			kubint = Mathf.RoundToInt (kub.transform.position.x); //J'arrondi sa position au nombre entier le plus proche
			row = rangeex;
		}
		else if (rangee == "y")
			{
				kubint = Mathf.RoundToInt (kub.transform.position.y); //J'arrondi sa position au nombre entier le plus proche
				row = rangeey;
			}
			else if (rangee == "z")
				{
					kubint = Mathf.RoundToInt (kub.transform.position.z); //J'arrondi sa position au nombre entier le plus proche
					row = rangeez;
				}
				else
					Debug.Log ("Erreur de code, mauvaise coordonnées données");

		if (kubint == row) //S'il est dans la colonne c'est bon
			return true;
		else
			return false;
	}

	void SetRangee () //La méthode pour déterminer quelle est la rangée effectivement sélectionnée par le joueur en fonction de la caméra
	{
		if (Haut) //La hauteur n'est pas fonction de la caméra
		{
			rangee = "y";
			lacouleur = Color.yellow;
		}
		else if (pointfinal == 0 || pointfinal == 2) //Si je suis à une position pair
			{
				if (Larg)
				{
					rangee = "z";
					lacouleur = Color.blue;
				}
				if (Long)
				{
					rangee = "x";
					lacouleur = Color.red;
				}
			}
			else//Si je suis à une position impaire
			{
				if (Larg)
				{
					rangee = "x";
					lacouleur = Color.blue;
				}
				if (Long)
				{
					rangee = "z";
					lacouleur = Color.red;
				}
			}
	}

	void SelectCubes () //Le fonction pour sélectionner les cubes et le colorer
	{
		if (RotationReady ()) //Si je suis en train de faire une rotation
		{
			rotantikub = false; //Je réinitialise le fait qu'il y ait des antikubs ou non
			cubesrot.Clear (); //Je réinitialise le tableau des cubes sélectionnés
			verriererot.Clear (); //Je réinitialise le tableau des verrieres sélectionnées

			foreach (GameObject kub in allcubes) //Je cherche dans tous les cubes les cubes que je veux tourner
			{
				if (KubInRow (kub, rangee)) //Si le cube est dans la rangee qui m'intéresse
				{
					if (kub.tag != "Verriere") //Je n'ajoute pas les verrieres à la liste, car ce sont des enfants des autres cubes, ça créé des bugs à la con
					{
						cubesrot.Add (kub); //J'ajoute le cube à la liste des cubes que je vais potentiellement faire tourner
					}
					else
						verriererot.Add (kub); //Si c'est un morceau de verrière je l'ajoute à mon tableau de verriere pour le problème de couleur
				}
			}

			foreach (GameObject kub in cubesrot) //Je regarde si il y a un antikub dans la rangee, si c'est le cas, je change la couleur et je déclare qu'il y a un antikub				
				CheckAntiKub (kub);
			
			foreach (GameObject kub in cubesrot) //Je colorie tous les blocs de la rangee de la bonne couleur		
				ColorBlock (kub, lacouleur); //Je colorie le bloc

			foreach (GameObject kub in verriererot) //Je colorie les bloc de la verrière dans la rangee de la bonne couleur
				ColorBlock (kub, lacouleur);
		}
	}

	void InputRangee () //La méthode pour récupérer les inputs de changements de rangee
	{
		if (RotationReady ()) //Je ne prend les inputs que si une rotation est prête
		{
			//Je réinitialise les valeurs avant
			goup = false;
			godown = false;
			goleft = false;
			goright = false;

			if (Mathf.Abs (Input.GetAxisRaw ("VerticalJ")) <= 0.5f && Mathf.Abs (Input.GetAxisRaw ("HorizontalJ")) <= 0.5f) //Si je relache le stick de manette je peux à nouveau changer de direction			
			axisreleased = true;
		
			if (Input.GetButtonDown ("Haut") || (Input.GetAxisRaw ("VerticalJ") >= 0.6f && axisreleased))
			{
				axisreleased = false;
				goup = true;
			}
			if (Input.GetButtonDown ("Bas") || (Input.GetAxisRaw ("VerticalJ") <= -0.6f && axisreleased))
			{
				axisreleased = false;
				godown = true;
			}
			if (Input.GetButtonDown ("Gauche") || (Input.GetAxisRaw ("HorizontalJ") <= -0.6f && axisreleased))
			{
				axisreleased = false;
				goleft = true;
			}
			if (Input.GetButtonDown ("Droite") || (Input.GetAxisRaw ("HorizontalJ") >= 0.6f && axisreleased))
			{
				axisreleased = false;
				goright = true;
			}
		}
	}

	int SimplePointBit (int number, int frequence, int decalage, int result1, int result2) //La méthode moins conne et plus souple pour obtenir une alternance entre deux chiffres toutes les x fois
	{
		if (((number + 1) + decalage) % (frequence * 2) >= frequence)
			return result1;
		else
			return result2;
	}

	int PointBit (int number, bool decalage, int digit) //La fonction qui permet d'obtenir 0 deux fois puis 1 deux fois etc... avec possibilité de décalage (0,1,1,0,0,...)
	{	
		int num = number;
		int bit = 0;

		if (decalage) //Je crée le décalage
		{
			num = number + 1;
		}

		for (int i = 0; i < digit; i++) //Je récupère de manière foireuse le bon chiffre binaire que je veux
		{
			bit = num % 2;
			num /= 2;
		}
		return bit;
	}

	void ChangeRangee () //Pour déterminer dans quel sens va le changement de rangee en fonction de la caméra et de la rangee selectionnée
	{
		if (rangee == "y") //Si je sélectionne la hauteur, la caméra ne change rien, c'est trop facile
		{
			if (goup && rangeey < hauteur - 1) //Je je veux aller vers le haut et que je suis pas déjà tout en haut
				rangeey++;
			if (godown && rangeey > 0) //Je je veux aller vers le bas et que je suis pas déjà tout en bas
				rangeey--;
		}

		if (rangee == "x") //Si je sélectionne la largeur (rouge), la caméra change des trucs, c'est moins sympa mais ça se fait
		{
			if (goleft)
				rangeex += (2 * PointBit (pointfinal, true, 2)) - 1;
			if (goright)
				rangeex -= (2 * PointBit (pointfinal, true, 2)) - 1;
			if (goup)
				rangeex -= (2 * PointBit (pointfinal, false, 2)) - 1;
			if (godown)
				rangeex += (2 * PointBit (pointfinal, false, 2)) - 1;
			
			if (rangeex < 0) //Je corrige si besoin est
				rangeex = 0;
			if (rangeex > largeur - 1)
				rangeex = largeur - 1;
		}
		if (rangee == "z") //Si je sélectionne la longueur (bleue), la caméra change des trucs, c'est moins sympa mais ça se fait
		{
			
			if (goleft)
				rangeez -= (2 * PointBit (pointfinal, false, 2)) - 1;
			if (goright)
				rangeez += (2 * PointBit (pointfinal, false, 2)) - 1;
			if (goup)
				rangeez -= (2 * PointBit (pointfinal, true, 2)) - 1;
			if (godown)
				rangeez += (2 * PointBit (pointfinal, true, 2)) - 1;
			
			if (rangeez < 0) //Je corrige si besoin est
				rangeez = 0;
			if (rangeez > longueur - 1)
				rangeez = longueur - 1;
		}
	}

	void InputRotation () //Méthode pour récupérer les inputs de rotation des cubes
	{
		if (RotationReady ())
		{
			if (Input.GetButtonDown ("RotationH") || Input.GetAxisRaw ("RotationJH") == 1 && !RotationAH && !RotationH) // Si je veux trouner dans le sens horaire et qu'une rotation est pas en cours
			if (PersoRangee () || rotantikub) //Si le perso est dans la rangée ou qu'il y a un antikub dans la rangée, je clignote et ne tourne pas
				cligne = true;
				else
					RotationH = true;
			if (Input.GetButtonDown ("RotationAH") || Input.GetAxisRaw ("RotationJAH") == 1 && !RotationAH && !RotationH) // Si je veux trouner dans le sens anti-horaire et qu'une rotation est pas en cours
			if (PersoRangee () || rotantikub) //Si le perso est dans la rangée ou qu'il y a un antikub dans la rangée, je clignote et ne tourne pas
				cligne = true;
				else
					RotationAH = true;
		}
	}

	bool PersoRangee () //Méthode pour déterminer si le perso est dans la rangée que le joueur veut faire pivoter
	{
		if (rangee == "x") //Je tourne les cubes en x
			if (Mathf.RoundToInt (Perso.transform.position.x) == rangeex) //Si le perso est dans la rangee
				return true;
		if (rangee == "y")  //Je tourne les cubes en y
			if ((Mathf.RoundToInt (Perso.transform.position.y) == rangeey) || (Mathf.RoundToInt (Perso.transform.position.y) - 1 == rangeey)) //Si le perso est dans la rangee ou juste au dessus d'elle
				return true;
		if (rangee == "z")  //Je tourne les cubes en z
			if (Mathf.RoundToInt (Perso.transform.position.z) == rangeez) //Si le perso est dans la rangee
				return true;
		return false; //Si j'ai tout parcouru mais rien trouvé, c'est qu'il est pas dans la rangée
	}

	void Clignoter () //Fonction pour faire clignoter les cubes si ça va pas
	{
		if (cligne)
		{
			foreach (GameObject kub in cubesrot) //Pour tous les kubs qui devraient être pivotés
			{
				if (PointBit (clignoter, false, 4) == 0) //Toutes les 8 frames je change la couleur
					ColorBlock (kub, Color.white);
				else
					ColorBlock (kub, lacouleur);
			}

			foreach (GameObject kub in verriererot) //Pour tous les kubs de la verrière qui devraient être pivotés
			{
				if (PointBit (clignoter, false, 4) == 0) //Toutes les 8 frames je change la couleur
					ColorBlock (kub, Color.white);
				else
					ColorBlock (kub, lacouleur);
			}

			if (PointBit (clignoter, false, 4) == 0) //Toutes les 8 frames je change la couleur du perso
				ColorBlock (Perso.gameObject, Color.white);
			else
				ColorBlock (Perso.gameObject, lacouleur);
			
			if (clignoter < 39) //Je fais clignoter 6 fois
				clignoter++;
			else //Au bout de 6 fois j'arrête de clignoter
			{
				clignoter = 0;
				cligne = false;
			}
		}
	}

	float AngleRotation ()
	{
		float anglerot = 0f;

		if (rangee == "x")
			if (hauteur != longueur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
				anglerot = 180f;
			else
				anglerot = 90f;
		
		if (rangee == "y")
			if (largeur != longueur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
				anglerot = 180f;
			else
				anglerot = 90f;
		
		if (rangee == "z")
			if (hauteur != largeur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
				anglerot = 180f;
			else
				anglerot = 90f;

		return anglerot;
	}

	Vector3 AxeRotation ()
	{
		int sensrot = 0;
		Vector3 axerot = Vector3.zero;

		if (rangee == "x") //Axe de rotation pour une rotation en largeur
		{	
			sensrot = (2 * PointBit (pointfinal, false, 2)) - 1; //Pour déterminer le sens de rotation hors hauteur
			axerot = new Vector3 (sensrot, 0, 0);
		}
		if (rangee == "y")  //Axe de rotation pour une rotation en hauteur
			axerot = Vector3.up;

		if (rangee == "z")  //Axe de rotation pour une rotation en longueur
		{	
			sensrot = (2 * PointBit (pointfinal, true, 2)) - 1; //Pour déterminer le sens de rotation hors hauteur	
			axerot = new Vector3 (0, 0, sensrot);
		}

		return axerot;
	}

	void RotateCubes () //La méthode pour faire tourner les cubes
	{
		if (RotationOn ()) //Si une rotation est lancée
		{
			int sensrot = 0; //Pour calculer le sens de la rotation

			if (RotationH) //Je détermine le sens de la rotation
				sensrot = 1;
			if (RotationAH)
				sensrot = -1;
			
			if (angletot + speedrot * Time.deltaTime <= AngleRotation ()) //Si je vais pas dépasser l'angle de rotation total souhaité je tourne les cubes
			{
				foreach (GameObject kub in cubesrot) //Je fais tourner les cubes
				{
					kub.transform.RotateAround (centreRot, AxeRotation (), sensrot * speedrot * Time.deltaTime);
				}
				angletot += speedrot * Time.deltaTime;
			}
			else //Sinon c'est que je suis au bout de la rotation, je la fini proprement
			{
				foreach (GameObject kub in cubesrot) //Je fais tourner les cubes
				{
					kub.transform.RotateAround (centreRot, AxeRotation (), sensrot * (AngleRotation () - angletot));
				}
				//Je réinitialise l'état de la rotation
				RotationH = false;
				RotationAH = false;
				angletot = 0f;
			}
		}
	}

	public bool RotationOn () //Méthode pour tester si une rotation est en cours
	{
		if (RotationH || RotationAH)
			return true;
		else
			return false;
	}

	void Update ()
	{
		ResetKubsColor (); //Je remet les cubes dans leur couleur d'origine fonction de leur hauteur

		if (this.GetComponent<Can_Act> ().canact) //J'attends que l'UI de fade soit passé pour permettre au joueur de faire des trucs
		{
			pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().GetCamNumber (); //Récupération de l'état de la caméra

			if (!RotationH && !RotationAH && !cligne) //J'attends toujours qu'une rotation soit finie et que le clignotement soit terminé pour recevoir de nouveaux inputs
			{
				InputPrepareRotation (); //Je récupère les inputs du choix de la dimension sélectionnée (x,y,z)
				InputRangee (); //Je récupère les inputs du choix de la rangée
				InputRotation (); //Je récupère les inputs de détermination de la rotation

				SetRangee (); //Je détermine quelle rangée est effectivement sélectionnée par le joueur
				SelectCubes (); //Je surligne les cubes de la bonne couleur et les déclarent comme sélectionnés et prêts à tourner

				ChangeRangee (); //Je change de rangée si le joueur a fait l'input pour
			}

			Clignoter (); //Je clignote si besoin est
			RotateCubes (); //Je fais la rotation des cubes
		}
	}
}