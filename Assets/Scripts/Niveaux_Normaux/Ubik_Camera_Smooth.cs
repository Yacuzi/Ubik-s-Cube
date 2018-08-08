using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Ubik_Camera_Smooth : MonoBehaviour
{
	public float smoothTime = 2F;
	public float delayrotcam = 1f;
	public bool CanRotate = true, CanZoom = true;

	[HideInInspector]
	public bool vuesecondaire;
	[HideInInspector]
	public Vector3[] poscam = new Vector3[4];
	[HideInInspector]
	public Vector3[] poscamsec = new Vector3[4];
	[HideInInspector]
	public int finpoin = 0;

	protected GameObject God, Perso;
	protected Vector3 velocity = Vector3.zero;
	protected bool rotationH, rotationAH;
	protected Vector3 finishpoint;
	protected Vector3 target;
	protected Vector3 cameraposprec;
	protected float timer;

	// Use this for initialization
	void Start ()
	{
		//Je dit que je peut tourner la caméra
		timer = delayrotcam;

		//Je récupère le perso et le god
		Perso = GameObject.FindGameObjectWithTag ("Player");
		God = GameObject.FindGameObjectWithTag ("God");

		//Récupérer le milieu et la taille depuis Cube_Creation
		int hauteur = God.GetComponent<CreationCube> ().Hauteur;
		int longueur = God.GetComponent<CreationCube> ().Longueur;
		int largeur = God.GetComponent<CreationCube> ().Largeur;

		//Creation du point central pour la caméra
		float milx = (float)(((float)longueur / 2) - 0.5F);
		float mily = (float)(((float)hauteur / 2) - 0.5F);
		float milz = (float)(((float)largeur / 2) - 0.5F);
		target = new Vector3 (milx, mily, milz);

		//je définis la position de la caméra pour avoir le bon angle (qui fait des jeux de perspectives)
		poscam [0] = new Vector3 (milx - 4F, mily + 4F, milz - 4F);
		poscam [1] = new Vector3 (milx - 4F, mily + 4F, milz + 4F);
		poscam [2] = new Vector3 (milx + 4F, mily + 4F, milz + 4F);
		poscam [3] = new Vector3 (milx + 4F, mily + 4F, milz - 4F);

		//j'initialise la caméra
		cameraposprec = poscam [GetCamNumber ()];
		transform.position = poscam [GetCamNumber ()];
		transform.LookAt (target);				
	}

	// Update is called once per frame
	void Update ()
	{
		//J'incrémente le timer
		MyTimer ();

		//J'adapte la caméra à la situation actuelle
		OrthoSize ();
		CameraLookAt ();
		CameraPos ();

		//Je récupère les inputs du joueur
		ChangeVue ();
		GetInputRotation ();

		//Je sauvegarde l'ancienne position de la caméra
		SaveCameraPosPrec ();
	}

	void GetInputRotation ()
	{
		if (CanRotate) //Si je permet à ma caméra de tourner
		{
			//jJe détermine à quel point est la caméra selon la rotation
			if ((timer >= delayrotcam) && (Input.GetButtonDown ("CameraH")) && !Perso.GetComponent<Cube_Rotations> ().RotationReady ())
			{
				timer = 0;
				rotationH = true;
				ChangeCameraPos (1);
			}

			if ((timer >= delayrotcam) && (Input.GetButtonDown ("CameraAH")) && !Perso.GetComponent<Cube_Rotations> ().RotationReady ())
			{
				timer = 0;
				rotationAH = true;
				ChangeCameraPos (-1);
			}
		}
	}

	void MyTimer ()
	{
		//J'incrémente mon timer à chaque seconde
		timer += Time.deltaTime;
	}

	//La fonction qui dit où est-ce que l caméra regarde
	void CameraLookAt ()
	{
		//Je change le regard que si je fais un rotation
		if (rotationAH || rotationH)
		{
			//je regarde toujours dans la bonne direction
			if (!vuesecondaire)
				transform.LookAt (target);
			else
				transform.LookAt (Perso.transform);
		}
	}

	//Pour déterminer le point de position de la caméra
	void ChangeCameraPos (int sens)
	{
		if (finpoin + sens >= 0)
			finpoin += sens;
		else
			finpoin = 3;
	}

	public int GetCamNumber ()
	{
		return finpoin % 4;
	}

	//Pour déterminer l'orthographic size
	void OrthoSize ()
	{
		//Ma variable de taille orthographique
		float orthosize = 1.5f;
		float vel = 0f;

		//Récupérer le milieu et la taille depuis Cube_Creation
		int hauteur = God.GetComponent<CreationCube> ().Hauteur;
		int longueur = God.GetComponent<CreationCube> ().Longueur;
		int largeur = God.GetComponent<CreationCube> ().Largeur;

		//choix du "zoom" de la caméra
		if (!vuesecondaire)
		{
			if ((hauteur > longueur) || (hauteur > largeur))
				orthosize = (hauteur + longueur + largeur) * 0.35F;
			else
				orthosize = (hauteur + longueur + largeur) * 0.3F;
		}

		if (vuesecondaire)
			orthosize = 1.5F;

		Camera.main.orthographicSize = Mathf.SmoothDamp (Camera.main.orthographicSize, orthosize, ref vel, smoothTime / 4);
	}

	//Pour faire la transition entre les vues
	void ChangeVue ()
	{
		if (CanZoom)
		{
			//transition de la vue secondaire à la vue principale
			if (Input.GetButtonDown ("Vue") && CameraStable ())
			{
				timer = 0;
				vuesecondaire = !vuesecondaire;
			}
		}
	}

	//Pour checker si la caméra ne bouge pas
	bool CameraStable ()
	{
		if (Camera.main.transform.position - cameraposprec == Vector3.zero)
			return true;
		else
			return false;
	}

	//Pour sauvegarder la précédente posisiont de la caméra
	void SaveCameraPosPrec ()
	{
		cameraposprec = Camera.main.transform.position;
	}

	//Pour déterminer où placer la caméra dans le contexte actuel
	void CameraPos ()
	{
		if (!vuesecondaire)
		{

			//Je détermine vers où doit aller la caméra
			finishpoint = poscam [GetCamNumber ()];

		}
		else
		{
			
			//Je récupère la position du perso
			float persox = Perso.transform.position.x;
			float persoy = Perso.transform.position.y;
			float persoz = Perso.transform.position.z;

			//Positions successives de la caméra secondaire
			poscamsec [0] = new Vector3 (persox - 0.8F, persoy + 0.8F, persoz - 0.8F);
			poscamsec [1] = new Vector3 (persox - 0.8F, persoy + 0.8F, persoz + 0.8F);
			poscamsec [2] = new Vector3 (persox + 0.8F, persoy + 0.8F, persoz + 0.8F);
			poscamsec [3] = new Vector3 (persox + 0.8F, persoy + 0.8F, persoz - 0.8F);

			//Je définit la position de la caméra principale
			finishpoint = poscamsec [GetCamNumber ()];
		}		

		//Je déplace la caméra où je le souhaite
		transform.position = Vector3.SmoothDamp (transform.position, finishpoint, ref velocity, smoothTime);

		//Je réinitialise la caméra
		if (CameraStable ())
		{
			rotationH = false;
			rotationAH = false;
		}
	}
}