using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Controle_Personnage : MonoBehaviour
{
	public float vitesse = 5f, vitpetit = 1f, gravite = 1f, speedrot = 90f, speedbalance = 45f;
	public LayerMask themask;
	[HideInInspector]
	public bool ensaut, enchute, equilibre, balance, pousse;
	[HideInInspector]
	public Vector3 directiontemp, direction, possaut;
	[HideInInspector]
	public int pretbouger;

	private GameObject God;
	private Cube_Rotations Kubinfos;
	private Vector3 direquil, nextcase;
	private float angletot, angletotbalance;
	private bool wait, moving, rotjump;
	private float waittime;
	private Transform kubobstacle, kubsaut, kubsupport;

	//Je récupère le déplacement voulu du joueur
	void MoveInput ()
	{
		//Je réinitialise le fait qu'une seule direction soit utilisée
		bool onedirection = false;

		//récupération des touches
		if ((Input.GetButton ("Haut") && !wait) || (Input.GetAxisRaw ("VerticalJ") == -1))
		{			
			onedirection = true;
			directiontemp = Vector3.right;
		}
		else if ((Input.GetButton ("Bas") && !wait) || (Input.GetAxisRaw ("VerticalJ") == 1))
			{			
				if (onedirection)
				{
					directiontemp = Vector3.zero;
					return;
				}
				onedirection = true;
				directiontemp = Vector3.left;
			}
			else if ((Input.GetButton ("Gauche") && !wait) || (Input.GetAxisRaw ("HorizontalJ") == -1))
				{			
					if (onedirection)
					{
						directiontemp = Vector3.zero;
						return;
					}
					onedirection = true;
					directiontemp = Vector3.forward;
				}
				else if ((Input.GetButton ("Droite") && !wait) || (Input.GetAxisRaw ("HorizontalJ") == 1))
					{			
						if (onedirection)
						{
							directiontemp = Vector3.zero;
							return;
						}
						onedirection = true;
						directiontemp = Vector3.back;
					}
					else
					{
						directiontemp = Vector3.zero;
					}
	}

	//Fonction pour voir si c'est un kub surlequel on peut sauter
	bool Jumpable ()
	{
		if ((kubobstacle.transform.tag != "Cube") && (kubobstacle.transform.tag != "Verriere"))
			return true;
		else
			return false;
	}

	//Fonction pour voir si le saut du joueur sera empêché
	bool CanJump ()
	{
		//Je dis que le joueur peut peut-être sauter sur ce kub
		kubsaut = kubobstacle;

		//Coordonnées du kub sur lequel le joueur veut sauter
		int kubsautx = Mathf.RoundToInt (kubsaut.transform.position.x);
		int kubsauty = Mathf.RoundToInt (kubsaut.transform.position.y);
		int kubsautz = Mathf.RoundToInt (kubsaut.transform.position.z);

		//Position arrondie à l'entier du perso
		int persox = Mathf.RoundToInt (transform.position.x);
		int persoy = Mathf.RoundToInt (transform.position.y);
		int persoz = Mathf.RoundToInt (transform.position.z);

		//Je récupère tous les kubs
		GameObject[] allcubes = Kubinfos.GetKubs ();

		//Pour tous les kubs
		foreach (GameObject kubtest in allcubes)
		{
			//Si ce n'est pas un cube de la verriere
			if ((kubtest.tag != "Cube") && (kubtest.tag != "Verriere"))
			{
				//Coordonnées du kub pour lequel on teste s'il peut gêner
				int kubtestx = Mathf.RoundToInt (kubtest.transform.position.x);
				int kubtesty = Mathf.RoundToInt (kubtest.transform.position.y);
				int kubtestz = Mathf.RoundToInt (kubtest.transform.position.z);

				//S'il y a un kub au-dessus du kub sur lequel le joueur veut sauter
				if ((kubtesty - 1F == kubsauty) && (kubtestx == kubsautx) && (kubtestz == kubsautz))
					return false;
				else if ((kubtesty - 1F == persoy) && (kubtestx == persox) && (kubtestz == persoz))
				//S'il y a un kub au-dessus du joueur
					return false;
					else if (persoy + 1F == God.GetComponent<CreationCube> ().Hauteur)
				//Si le joueur est au plus haut du niveau
						return false;
			}
		}
		//S'il a rien trouvé, c'est OK
		return true;
	}

	void InputJump ()
	{		
		if ((Input.GetButtonUp ("Saut")) && (!pousse))
		{
			//Je déclare le perso comme en saut
			ensaut = true;
		}
	}

	void Jump ()
	{
		if (ensaut)
		{
			int lekubsautx = Mathf.RoundToInt (kubsaut.position.x);
			int lekubsauty = Mathf.RoundToInt (kubsaut.position.y);
			int lekubsautz = Mathf.RoundToInt (kubsaut.position.z);

			if (!rotjump) //Je fais le petit saut d'abord
			{				
				Vector3 petitsaut = Vector3.zero;

				//Je choisis la destination pour que le kub fasse un petit saut pour atteindre le bord du bloc
				float petitsautx = lekubsautx - (direction.x * 0.5f * (kubsaut.lossyScale.x + transform.localScale.x));
				float petitsauty = lekubsauty + (0.5f * (kubsaut.lossyScale.y - transform.localScale.y));
				float petitsautz = lekubsautz - (direction.z * 0.5f * (kubsaut.lossyScale.z + transform.localScale.z));
				petitsaut = new Vector3 (petitsautx, petitsauty, petitsautz);

				//Je fais le petit saut de préparation
				if (MoveTowards (petitsaut, vitpetit))
				{
					rotjump = true;
				}
			}

			//Quand je l'ai fini je passe à la rotation
			if (rotjump)
			{
				//Je trouve le point de rotation
				Vector3 rotsaut = new Vector3 (lekubsautx - (direction.x * 0.5F * kubsaut.lossyScale.x), kubsaut.position.y + (0.5F * kubsaut.lossyScale.y), lekubsautz - (direction.z * 0.5F * kubsaut.lossyScale.y));
				//Je trouve l'axe
				Vector3 axesaut = Vector3.Cross (direction, Vector3.down);

				//Tant qu'il a pas fini sa rotation
				if (angletot + speedrot * Time.deltaTime <= 180f)
				{
					//Je fais la rotation
					transform.RotateAround (rotsaut, axesaut, speedrot * Time.deltaTime);
					//Je calcule l'angle total qu'a fait la rotation
					angletot += speedrot * Time.deltaTime;
				}
				else
				{
					//Je remet le perso droit et j'arrête de le faire tourner
					transform.rotation = Quaternion.identity;
					//Je recentre le personnage
					if (CenterCharacter ())
					{
						angletot = 0f;
						rotjump = false;
						ensaut = false;
						wait = true;
					}
				}
			}
		}
	}

	void Wait ()
	{
		if (wait)
		{
			waittime += Time.deltaTime;
			if (waittime >= 0.1f)
			{
				waittime = 0F;
				wait = false;
			}
		}
	}

	//Je met à jour le sens dans lequel le joueur veut aller
	void Regard ()
	{
		if (!moving && !enchute && !ensaut)
		{
			//Le vecteur de déplacement temporaire
			Vector3 deplacetemp = directiontemp;
			//Récupération de l'état de la caméra
			int pointfinal = Camera.main.GetComponent<Ubik_Camera_Smooth> ().GetCamNumber ();

			//Je défini le bon sens de déplacement du perso en fonction de la caméra
			for (int i = 0; i < pointfinal; i++)
			{
				deplacetemp = Vector3.Cross (deplacetemp, Vector3.down);
			}

			//Je met à jour ma direction
			direction = deplacetemp;
			//Je lui dis quelle est la position où le joueur voudrait aller
			nextcase = transform.position + direction;
		}
	}

	//La fonction pour bouger le personnage
	void Move ()
	{
		if (moving)
		{
			//Je fais bouger le personnage d'une case dans le sens désiré
			if (MoveTowards (nextcase, vitesse))
			{
				//Je dis que le perso n'est plus en train de bouger
				moving = false;
			}
		}
	}

	//La fonction pour faire l'effet de déséquilibre du perso quand il est au bord d'un kub
	void Equilibrium ()
	{
		//L'axe autour duquel va balancer le perso
		Vector3 axechute;
		//Si le joueur va vers le vide ou non
		bool going = false;

		if (equilibre)
		{			
			//Je check si le joueur est toujours en train d'aller vers le vide
			if (direction == direquil)
				going = true;
			//S'il continue à avancer mais qu'il ne se balance pas
			if (going && !balance)
			{
				//Je définis où est la position du perso pour qu'il soit bien au bord du kub
				Vector3 bordkub = new Vector3 (kubsupport.transform.position.x + (direquil.x * 0.5f * (kubsupport.transform.lossyScale.x - transform.lossyScale.x)), 
				                               kubsupport.transform.position.y + (0.5f * (kubsupport.transform.lossyScale.x + transform.lossyScale.x)), 
				                               kubsupport.transform.position.z + (direquil.z * 0.5f * (kubsupport.transform.lossyScale.z - transform.lossyScale.z)));
				
				//Je bouge le personnage vers cette position
				if (MoveTowards (bordkub, vitpetit))
				{
					//Une fois qu'il y est je lui dit qu'il se balance
					balance = true;
				}
			}
			//S'il n'avance plus mais qu'il ne se balance pas, je le recentre
			else if (!balance)
				{
					if (CenterCharacter ())
						equilibre = false;
				}

			//Si le perso se balance
			if (balance)
			{
				//Je définis le point autour duquel le perso va tourner
				float pchutex = kubsupport.position.x + (kubsupport.lossyScale.x * 0.5f * direquil.x);
				float pchutey = kubsupport.position.y + (kubsupport.lossyScale.y * 0.5f);
				float pchutez = kubsupport.position.z + (kubsupport.lossyScale.z * 0.5f * direquil.z);
				Vector3 pchute = new Vector3 (pchutex, pchutey, pchutez);

				//Définition de l'axe de rotation pour la chute
				axechute = Vector3.Cross (direquil, Vector3.down);

				//Si le perso n'a pas dépassé un angle de 45°
				if (angletotbalance <= 45F)
				{
					//Si le joueur avance toujours
					if (going)
					{					
						//J'incrémente la rotation totale
						angletotbalance += speedbalance * Time.deltaTime;
						//Je fait la rotation
						transform.RotateAround (pchute, axechute, speedbalance * Time.deltaTime);
					}
					//S'il n'avance plus dans la direction du vide
					else
					{
						//Je m'assure que l'angle total de rotation fait pas n'importe quoi
						if (angletotbalance - (speedbalance * Time.deltaTime * 2f) >= 0F)
						{
							//Je diminue l'angle de rotation pour que le perso revienne en place
							angletotbalance -= speedbalance * Time.deltaTime * 2f;
							//Je fait la rotation
							transform.RotateAround (pchute, axechute, -speedbalance * Time.deltaTime * 2f);
						}
						else
						{
							//Je remet le perso bien et je dis qu'il se balance plus
							transform.RotateAround (pchute, axechute, -angletotbalance);
							angletotbalance = 0f;
							balance = false;
						}
					}
				}

				//Si l'angle dépasse 45°, le perso chute
				if (angletotbalance > 45f)
				{
					if (angletotbalance + (speedbalance * Time.deltaTime * 2f) <= 90f)
					{
						//La rotation totale augmente
						angletotbalance += speedbalance * Time.deltaTime * 2f;
						//Je fais la rotation
						transform.RotateAround (pchute, axechute, speedbalance * Time.deltaTime * 2f);
					}
					else
					{
						//Je fais la rotation
						transform.RotateAround (pchute, axechute, 90 - angletotbalance);
						//Je réinitialise l'angle de chute
						angletotbalance = 0f;
						transform.rotation = Quaternion.identity;
						balance = false;
						equilibre = false;
						enchute = true;
					}
				}
			}
		}
	}

	void Falling ()
	{
		//Le perso est censer chuter
		if (enchute)
		{
			//Si le personnage est toujours en chute
			if (!Physics.Raycast (transform.position, Vector3.down, transform.lossyScale.y * 0.51f, themask))
			{
				transform.Translate (Vector3.down * gravite * Time.deltaTime);
			}
			else
			{
				//S'il a atteint un sol dur, alors je le place bien, je dis qu'il chute plus et il fait une pause
				if (CenterCharacter ())
				{
					wait = true;
					enchute = false;
				}
			}
		}
	}

	//La fonction pour centrer le personnage sur son cube actuel
	bool CenterCharacter ()
	{
		//Je récupère les coordonnées arrondies de la position du personnage
		int centerx = Mathf.RoundToInt (transform.position.x);
		float centery = Mathf.RoundToInt (transform.position.y) - (0.5f * (1f - transform.lossyScale.y));
		int centerz = Mathf.RoundToInt (transform.position.z);
		Vector3 center = new Vector3 (centerx, centery, centerz);

		//Je le déplace vers cette position
		if (MoveTowards (center, vitpetit))
			return true;
		else
			return false;
	}

	//La fonction pour que ça bouge enfin comme je veux
	bool MoveTowards (Vector3 objective, float speed)
	{
		if (Vector3.Distance (transform.position, objective) >= speed * Time.deltaTime)
		{
			transform.position += Vector3.Normalize (objective - transform.position) * speed * Time.deltaTime;
			return false;
		}
		else
		{
			transform.position = objective;
			return true;
		}
	}

	//Fonction pour récupérer le cube sur lequel est le joueur
	bool GetKubAppui ()
	{
		//Le kub vers lequel le joueur veut aller
		Vector3 kubfutur = transform.position + direction;

		//Je fait un raycast qui check s'il y a du sol là où veut aller le joueur
		if (Physics.Raycast (kubfutur, Vector3.down, 1f, themask))
		{
			return true;
		}
		else
			return false;
	}

	//Fonction pour récupérer le cube sur lequel est le joueur
	bool GetKubSupport ()
	{
		//Pour récupérer les informations sur un éventuel kub vers lequel va le joueur
		RaycastHit hitkub;

		//Je fait un raycast qui check s'il y a du sol là où veut aller le joueur
		if (Physics.Raycast (transform.position, Vector3.down, out hitkub, 1f, themask))
		{
			kubsupport = hitkub.transform;
			return true;
		}
		else
			return false;
	}

	//Fonction pour récupérer le cube sur lequel est le joueur
	bool GetKubObstacle ()
	{
		//Pour récupérer les informations sur un éventuel kub vers lequel va le joueur
		RaycastHit hitkub;

		//Je fait un raycast qui check s'il y a du sol là où veut aller le joueur
		if (Physics.Raycast (transform.position, direction, out hitkub, 0.55f, themask))
		{
			//J'attribue ce kub comme obstacle
			kubobstacle = hitkub.transform;
			return true;
		}
		else
			return false;
	}

	//Fonction pour voir si le perso peut bouger et si oui, comment
	void MoveChecker ()
	{
		//Si le joueur n'est pas en saut ou en chute ou en équilibre ou en mouvement
		if (pretbouger == 0 && !ensaut && !enchute && !equilibre && !moving)
		{
			//Je fais un raycast pour voir s'il y a un cube en face du joueur
			if (GetKubObstacle ())
			{
				//Si ce n'est pas un kub de la verriere
				if (Jumpable ())
				{
					//Je vérifie si le joueur est bloqué dans son saut
					if (CanJump ())
					{	
						//Je colorie le bloc sur lequel on peut sauter en vert et je dis que le joueur peut sauter
						Kubinfos.ColorBlock (kubsaut.gameObject, Color.green);
						InputJump ();
					}
				}	
			}
			else if (GetKubAppui ()) //Je fait un raycast qui check s'il y a du sol là où veut aller le joueur				
				{
					moving = true; //Je dis que je peux bouger le joueur
				}
				else
				{
					//Je récupère le kub qui est sous le joueur
					GetKubSupport ();
					//Je sauvegarde le sens dans lequel il est quand il commence à se mettre en équilibre
					direquil = direction;
					//Je le met en équilibre au bord du kub sur lequel il est
					equilibre = true;
				}
		}
	}

	void Reset ()
	{
		if (Input.GetButtonDown ("Reset"))
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
	}

	public bool Immobile () //Fonction pour savoir si le personnage a fini toutes ses actions
	{
		if (!moving && !ensaut && !equilibre && !enchute && !pousse)
			return true;
		else
			return false;
	}

	// Use this for initialization
	void Start ()
	{
		//Je cache le curseur
		Cursor.visible = false;
		//Je récupère le gameobject du dieu
		God = GameObject.FindGameObjectWithTag ("God");
		//J'initialise ma prochaine case
		nextcase = transform.position;
		//Je déclare d'où je récupère les infos et méthodes sur les kubs
		Kubinfos = GetComponent<Cube_Rotations> ();
	}

	void LateUpdate () //Je fais l'update en dernière à cause du nettoyage de la couleur des cubes et que je sais pas programmer proprement
	{
		if (this.GetComponent<Can_Act> ().canact) //J'attends que l'UI de fade soit passé pour permettre au joueur de faire des trucs
		{
			//Si je suis pas en train de préparer une rotation
			if (!Kubinfos.RotationReady ())
			{
				Reset ();

				if (!Kubinfos.RotationOn ()) //Je ne reçois les inputs que si aucune rotation n'est en cours
				{
					MoveInput ();
				}

				Regard ();
				MoveChecker ();
				Move ();

				Jump ();
				Equilibrium ();
				Falling ();

				Wait ();

				//Debug.Log ("Falling " + enchute + " Jumping " + ensaut + " Equilibrium " + equilibre);
			}
		}
	}
}