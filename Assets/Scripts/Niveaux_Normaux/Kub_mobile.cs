using UnityEngine;
using System.Collections;

public class Kub_mobile : MonoBehaviour
{		
	public GameObject Perso;
	public Transform God;
	private Vector3 possaut, direction, posfuturekub, posfutureperso, velocity = Vector3.zero, centrer, regard;
	private bool kubappui, enrotation, pointaffecte;
	private int largeur, longueur;
	private int kubx, kuby, kubz;
	private GameObject[] allcubes;
	[HideInInspector]
	public bool pretbouger = false, bouger = false;

	void Start ()
	{
		//je récupère le tableau des kubs
		allcubes = Perso.GetComponent<Cube_Rotations> ().GetKubs ();	
	}
	
	void Update ()
	{
		//je récupère les valeurs liées au personnage
		possaut = Perso.GetComponent<Controle_Personnage> ().possaut;
		direction = Perso.GetComponent<Controle_Personnage> ().direction;
		regard = Perso.GetComponent<Controle_Personnage> ().direction;

		//je récupère les données liées au niveau
		largeur = God.GetComponent<CreationCube> ().Largeur;
		longueur = God.GetComponent<CreationCube> ().Longueur;

		//je vérifie s'il y a une rotation
		if ((Camera.main.GetComponent<Cube_Rotations> ().Haut)
		    | (Camera.main.GetComponent<Cube_Rotations> ().Larg)
		    | (Camera.main.GetComponent<Cube_Rotations> ().Long))
			enrotation = true;
		else
			enrotation = false;

		//je recentre proprement le cube quand il ne bouge pas
		if ((!bouger) & (!enrotation))
			transform.position = new Vector3 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y), Mathf.RoundToInt (transform.position.z));
							
		//je m'assure que le perso bouge pas quand il déplace le cube
		if (pretbouger)
		{
			if (!pointaffecte)
			{
				Perso.GetComponent<Controle_Personnage> ().pretbouger += 1;
				pointaffecte = true;
			}
		}
		else if (pointaffecte)
			{
				Perso.GetComponent<Controle_Personnage> ().pretbouger -= 1;
				pointaffecte = false;
			}
				
		//je vérifie si le joueur saute ou reste en place et prépare l'éventuel déplacement
		if ((Vector3.Distance (possaut, transform.position) <= 0.5F)
		    & (Mathf.RoundToInt (transform.position.y) == Mathf.RoundToInt (Perso.transform.position.y)))
		{

			//je colorie le bloc en vert
			GetComponent<Renderer> ().material.color = Color.green;

			//si j'ai bougé le kub, alors je sauterais pas dessus, sinon oui
			if (bouger)
				Perso.GetComponent<Controle_Personnage> ().pousse = true;						
			if (Input.GetButtonDown ("Saut"))
				Perso.GetComponent<Controle_Personnage> ().pousse = false;
						
			if (Input.GetButton ("Saut"))
				pretbouger = true;
			else if (!bouger)
					pretbouger = false;					
		}

		Debug.Log (pretbouger);

		if ((pretbouger) & (!bouger) & (direction != (Vector3.zero)))
		{
			posfuturekub = transform.position + direction;
			posfutureperso = Perso.transform.position + direction;

			//je vérifie que ça sorte pas du cube
			if (((posfuturekub.x >= 0) & (posfuturekub.x <= longueur - 1)
			    & (posfuturekub.z >= 0) & (posfuturekub.z <= largeur - 1))
			    & ((posfutureperso.x >= 0) & (posfutureperso.x <= longueur - 1)
			    & (posfutureperso.z >= 0) & (posfutureperso.z <= largeur - 1)))
				bouger = true;
						
			//je vérifie qu'il y a pas de kub qui bloque le mouvement
			foreach (GameObject kub in allcubes)
			{

				kubx = Mathf.RoundToInt (kub.transform.position.x);
				kuby = Mathf.RoundToInt (kub.transform.position.y);
				kubz = Mathf.RoundToInt (kub.transform.position.z);

				if (((posfuturekub.x == kubx) & (posfuturekub.y == kuby) & (posfuturekub.z == kubz))
				    | (((Mathf.RoundToInt (posfutureperso.x) == kubx)
				    & (Mathf.RoundToInt (posfutureperso.y) == kuby)
				    & (Mathf.RoundToInt (posfutureperso.z) == kubz))
				    & (transform.position != kub.transform.position)))
				{
					bouger = false;
					break;
				}
			}

			//je vérifie qu'il y aura un kub sous le joueur
			foreach (GameObject kub in allcubes)
			{

				kubx = Mathf.RoundToInt (kub.transform.position.x);
				kuby = Mathf.RoundToInt (kub.transform.position.y);
				kubz = Mathf.RoundToInt (kub.transform.position.z);

				if (Mathf.RoundToInt (posfutureperso.y) == 0)
				{
					kubappui = true;
					break;
				}
										
				if ((Mathf.RoundToInt (posfutureperso.x) == kubx)
				    & (Mathf.RoundToInt (posfutureperso.y) == (kuby + 1))
				    & (Mathf.RoundToInt (posfutureperso.z) == kubz))
				{
					kubappui = true;
					break;
				}
				else
				{
					kubappui = false;
				}
			}
			if ((bouger) & (kubappui))
				bouger = true;
			else
				bouger = false;
		}

		//je recentre le perso s'il veut bouger le kub
		if ((regard.x == -0.5F) | (regard.x == 0.5F))
		{
			centrer = new Vector3 (Perso.transform.position.x, Perso.transform.position.y, Mathf.RoundToInt (Perso.transform.position.z));
		}						

		if ((regard.z == 0.5F) | (regard.z == -0.5F))
		{
			centrer = new Vector3 (Mathf.RoundToInt (Perso.transform.position.x), Perso.transform.position.y, Perso.transform.position.z);
		}

		//je fais le petit déplacement de préparation
		if ((pretbouger) & (Perso.transform.position != centrer))
			Perso.transform.position = Vector3.SmoothDamp (Perso.transform.position, centrer, ref velocity, 1F * Time.deltaTime);
		if ((pretbouger) & ((Perso.transform.position.x - centrer.x) <= 0.001F)
		    & ((Perso.transform.position.y - centrer.y) <= 0.001F)
		    & ((Perso.transform.position.z - centrer.z) <= 0.001F))
			Perso.transform.position = centrer;

		//je déplace le kub et le perso en même temps
		if ((bouger) & (transform.position != posfuturekub))
		{
			transform.position = Vector3.SmoothDamp (transform.position, posfuturekub, ref velocity, 1F * Time.deltaTime);
			Perso.transform.position = Vector3.SmoothDamp (Perso.transform.position, posfutureperso, ref velocity, 1F * Time.deltaTime);
		}
		else
		{
			if (bouger)
				transform.position = new Vector3 (Mathf.RoundToInt (transform.position.x), Mathf.RoundToInt (transform.position.y), Mathf.RoundToInt (transform.position.z));
			bouger = false;
		}
	}
}