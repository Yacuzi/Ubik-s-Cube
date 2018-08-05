using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Scripting;

public class Cube_Rotations : MonoBehaviour
{
	public float speedrot;

	[HideInInspector]
	public bool Larg, Haut, Long;
	[HideInInspector]
	public bool RotationH, RotationAH;

	private Transform God;
	private Controle_Personnage Perso;
	private int pointfinal;

	private List<GameObject> cubesrot = new List<GameObject> ();
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

		//Je récupère le perso et le god
		God = GameObject.FindGameObjectWithTag ("God").transform;
		Perso = GameObject.FindGameObjectWithTag ("Player").GetComponent<Controle_Personnage> ();

		//Récupération de la taille du cube
		hauteur = God.GetComponent<CreationCube> ().Hauteur;
		longueur = God.GetComponent<CreationCube> ().Longueur;
		largeur = God.GetComponent<CreationCube> ().Largeur;

		//Creation du point central pour les centre de rotation
		float milx = (float)(((float)longueur / 2) - 0.5F);
		float mily = (float)(((float)hauteur / 2) - 0.5F);
		float milz = (float)(((float)largeur / 2) - 0.5F);
		centreRot = new Vector3 (milx, mily, milz);
	}

	public void ColorBlock (GameObject lekub, Color lacouleur) //La fonction pour colorier un bloc d'une certaine couleur
	{
		Renderer kubcolor = lekub.GetComponent<Renderer> (); //Je colorie le bloc si c'est pas un mur
		kubcolor.material.color = lacouleur;
	}

	public void ResetKubsColor () //Pour remettre tous les kubs en blanc
	{
		foreach (GameObject lekub in allcubes)
		{
			ColorBlock (lekub, Color.white);
		}
	}

	void SetKubs () //Pour créer le tableau contenant tous les kubs
	{
		GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube") as GameObject[]; //Recherche de tous les cubes
		GameObject[] kubs = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[]; //Recherche de tous les kubs
		GameObject[] antikubs = GameObject.FindGameObjectsWithTag ("Antikub") as GameObject[]; //Recherche de tous les antikubs
		allcubes = new GameObject[cubes.Length + kubs.Length + antikubs.Length];
		cubes.CopyTo (allcubes, 0);
		kubs.CopyTo (allcubes, cubes.Length);
		antikubs.CopyTo (allcubes, cubes.Length + kubs.Length);
	}

	public GameObject[] GetKubs () //Pour récupérer les infos sur les kubs
	{
		return allcubes;
	}

	public bool IsNotRotating () //Pour checker si je prépare une rotation
	{
		if ((Larg) || (Long) || (Haut))
			return false;
		else
			return true;
	}

	bool InputPrepareRotation () //Méthode pour déterminer quelle partie du Cube le joueur sélectionne
	{
		if (Perso.Immobile ()) //Si le perso a fini toutes ses actions je peux sélectionner quelquechose
		{
			if (Input.GetButton ("Largeur"))
			{
				Larg = true;
			}
			if (Input.GetButton ("Hauteur"))
			{
				Haut = true;
			}
			if (Input.GetButton ("Longueur"))
			{
				Long = true;
			}
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
		if (kub.tag == "AntiKub")
		{
			rotantikub = true;
			lacouleur = Color.black;
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
					lacouleur = Color.red;
				}
				if (Long)
				{
					rangee = "z";
					lacouleur = Color.blue;
				}
			}
	}

	void SelectCubes () //Le fonction pour sélectionner les cubes et le colorer
	{
		rotantikub = false; //Je réinitialise le fait qu'il y ait des antikubs ou non
		cubesrot.Clear (); //Je réinitialise le tableau des cubes sélectionnés

		foreach (GameObject kub in allcubes) //Je cherche dans tous les cubes
		{
			if (KubInRow (kub, rangee)) //Si le cube est dans la rangee qui m'intéresse
			{
				cubesrot.Add (kub); //J'ajoute le cube à la liste des cubes que je vais potentiellement faire tourner
				CheckAntiKub (kub); //Je regarde si c'est un antikub
				ColorBlock (kub, lacouleur); //Je colorie le bloc de la bonne couleur
			}
		}
	}

	void InputRangee () //La méthode pour récupérer les inputs de changements de rangee
	{
		if (Input.GetAxisRaw ("VerticalJ") == 0) //Si je relache le stick de manette je peux à nouveau changer de direction			
			axisreleased = true;
		if (Input.GetButtonDown ("Haut") || (Input.GetAxisRaw ("VerticalJ") == -1 && axisreleased))
		{
			axisreleased = false;
			goup = true;
		}
		if (Input.GetButtonDown ("Bas") || (Input.GetAxisRaw ("VerticalJ") == 1 && axisreleased))
		{
			axisreleased = false;
			godown = true;
		}
		if (Input.GetButtonDown ("Gauche") || (Input.GetAxisRaw ("HorizontalJ") == -1 && axisreleased))
		{
			axisreleased = false;
			goleft = true;
		}
		if (Input.GetButtonDown ("Droite") || (Input.GetAxisRaw ("HorizontalJ") == 1 && axisreleased))
		{
			axisreleased = false;
			goright = true;
		}
	}

	int PointBit (int number, bool decalage) //La fonction qui permet d'obtenir 0 deux fois puis 1 deux fois etc... avec possibilité de décalage (0,1,1,0,0,...)
	{
		string pointbit = "";
			
		if (decalage)
			pointbit = (number + 1).ToString ();
		else
			pointbit = number.ToString ();
			
		return (int)pointbit [1];
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

		if (rangee == "x") //Si je sélectionne la largeur, la caméra change des trucs, c'est moins sympa mais ça se fait
		{
			if (goleft)
				rangeex += (2 * PointBit (pointfinal, true)) - 1;
			if (goright)
				rangeex -= (2 * PointBit (pointfinal, true)) - 1;
		}
		if (rangee == "z") //Si je sélectionne la longueur, la caméra change des trucs, c'est moins sympa mais ça se fait
		{
			if (goleft)
				rangeez -= (2 * PointBit (pointfinal, false)) - 1;
			if (goright)
				rangeez += (2 * PointBit (pointfinal, false)) - 1;
		}
	}

	void InputRotation () //Méthode pour récupérer les inputs de rotation des cubes
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

	bool PersoRangee () //Méthode pour déterminer si le perso est dans la rangée que le joueur veut faire pivoter
	{
		if (rangee == "x") //Je tourne les cubes en x
			if (Mathf.RoundToInt (Perso.transform.position.x) == rangeex) //Si le perso est dans la rangee
				return true;
		if (rangee == "y")  //Je tourne les cubes en y
			if (Mathf.RoundToInt (Perso.transform.position.y) == rangeey) //Si le perso est dans la rangee
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
			string clignoterbit = clignoter.ToString ();

			foreach (GameObject kub in cubesrot) //Pour tous les kubs qui devraient être pivotés
			{
				if ((int)clignoterbit [2] == 0) //Toutes les 4 frames je change la couleur
				ColorBlock (kub, Color.white);
				else
					ColorBlock (kub, lacouleur);
			}

			if (clignoter < 24) //Je fais clignoter 6 fois
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

		if (rangee == "x" && hauteur != longueur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
			anglerot = 180f;
		else
			anglerot = 90F;
		if (rangee == "y" && largeur != longueur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
			anglerot = 180f;
		else
			anglerot = 90F;
		if (rangee == "z" && hauteur != largeur) //Je détermine si les rotations sont de 90° ou 180° en fonction de la taille du cube			
			anglerot = 180f;
		else
			anglerot = 90F;

		return anglerot;
	}

	Vector3 AxeRotation ()
	{
		int sensrot = 0;
		Vector3 axerot = Vector3.zero;

		if (rangee == "x") //Axe de rotation pour une rotation en largeur
		{	
			sensrot = (2 * PointBit (pointfinal, true)) - 1; //Pour déterminer le sens de rotation hors hauteur	
			axerot = new Vector3 (0, 0, sensrot);
		}
		if (rangee == "y")  //Axe de rotation pour une rotation en hauteur
			axerot = Vector3.up;

		if (rangee == "z")  //Axe de rotation pour une rotation en longueur
		{	
			sensrot = (2 * PointBit (pointfinal, false)) - 1; //Pour déterminer le sens de rotation hors hauteur	
			axerot = new Vector3 (sensrot, 0, 0);
		}
		return axerot;
	}

	void RotateCubes () //La méthode pour faire tourner les cubes
	{
		if (RotationH || RotationAH) //Si une rotation est lancée
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

	void Update ()
	{
		pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().GetCamNumber (); //Récupération de l'état de la caméra

		ResetKubsColor ();

		InputPrepareRotation (); //Je récupère les inputs du choix de la dimension sélectionnée (x,y,z)
		InputRangee (); //Je récupère les inputs du choix de la rangée
		InputRotation (); //Je récupère les inputs de détermination de la rotation

		SetRangee (); //Je détermine quelle rangée est effectivement sélectionnée par le joueur
		SelectCubes (); //Je surligne les cubes de la bonne couleur et les déclarent comme sélectionnés et prêts à tourner

		ChangeRangee (); //Je change de rangée si le joueur a fait l'input pour

		Clignoter (); //Je clignote si besoin est

		RotateCubes (); //Je fais la rotation des cubes
	}
}