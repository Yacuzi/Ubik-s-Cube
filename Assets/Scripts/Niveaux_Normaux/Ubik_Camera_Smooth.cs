using UnityEngine;
using System.Collections;
using UnityEngine.Scripting;

public class Ubik_Camera_Smooth : MonoBehaviour {

		public Transform God, Perso;
		public float smoothTime = 2F;
		public bool rotcam = true;
		private Vector3 velocity = Vector3.zero;
		[HideInInspector]
		public bool rotationH, rotationAH, vuesecondaire, changvue;
		private Vector3 finishpoint, CubPos, possecond;
		private Quaternion CubRot;
		private int largeur= 3 , longueur = 3, hauteur = 3;
		private float camhaut;
		private float milx=0F, mily=0F, milz=0F;
		private Vector3 target;
		private float persox, persoy, persoz;
		private bool Larg, Long, Haut;
		[HideInInspector]
		public Vector3 a, b, c, d;
		[HideInInspector]
		public Vector3 abis, bbis, cbis, dbis;
		[HideInInspector]
		public int finpoin = 0;

		// Use this for initialization
		void Start () {
				
				//Récupérer le milieu et la taille depuis Cube_Creation
				hauteur = God.GetComponent<CreationCube>().Hauteur;
				longueur = God.GetComponent<CreationCube>().Longueur;
				largeur = God.GetComponent<CreationCube>().Largeur;

				//Creation du point central pour la caméra
				milx = (float)(((float) longueur / 2) - 0.5F);
				mily = (float)(((float) hauteur / 2) - 0.5F);
				milz = (float)(((float) largeur / 2) - 0.5F);
				target = new Vector3 (milx,mily,milz);

				//je définis la position de la caméra pour avoir le bon angle (qui fait des jeux de perspectives)
				a = new Vector3 (milx - 4F, mily + 4F, milz - 4F);
				b = new Vector3 (milx - 4F, mily + 4F, milz + 4F);
				c = new Vector3 (milx + 4F, mily + 4F, milz + 4F);
				d = new Vector3 (milx + 4F, mily + 4F, milz - 4F);

				//j'initialise la caméra
				transform.position = a;
				transform.LookAt (target);
				
		}

		// Update is called once per frame
		void Update () {

				//je regarde toujours dans la bonne direction
				if (!vuesecondaire)
						transform.LookAt (target);
				
				//pour vérification si la largeur/hauteur/longueur est sélectionnée
				Larg = GetComponent<Cube_Rotations>().Estlarg;
				Long = GetComponent<Cube_Rotations>().Estlong;
				Haut = GetComponent<Cube_Rotations>().Esthaut;
				
				//choix du "zoom" de la caméra
				if (!vuesecondaire) {
						if ((hauteur > longueur) | (hauteur > largeur))
								Camera.main.orthographicSize = (hauteur + longueur + largeur) * 0.35F;
						else
								Camera.main.orthographicSize = (hauteur + longueur + largeur) * 0.3F;
				}

				if (vuesecondaire)
						Camera.main.orthographicSize = 1.5F;
				
				//transition de la vue secondaire à la vue principale
				if (Input.GetButtonDown ("Vue")) {
						vuesecondaire = !vuesecondaire;
						changvue = true;
				}

				if ((!vuesecondaire) & (changvue)){
						transform.position = finishpoint;
				}

				if ((vuesecondaire) & (changvue)) {
						transform.position = possecond;
						transform.LookAt (Perso);
				}

				//je réinitialise la caméra pour le changement de point de vue
				if ((transform.position == a) ^ (transform.position == b) ^ (transform.position == c) ^ (transform.position == d)
						^ (transform.position == abis) ^ (transform.position == bbis) ^ (transform.position == cbis) ^ (transform.position == dbis))
						changvue = false;
				
				//je détermine à quel point est la caméra selon la rotation
				if (((transform.position == a) ^ (transform.position == b) ^ (transform.position == c) ^ (transform.position == d))
						& (Input.GetButtonDown ("CameraH")) & (!rotationAH) & (!vuesecondaire) & (!Larg) & (!Long) & (!Haut) & (rotcam)) {
						rotationH = true;
						if (finpoin != 3)
								finpoin++;
						else
								finpoin = 0;
				}
				if (((transform.position == a) ^ (transform.position == b) ^ (transform.position == c) ^ (transform.position == d))
						& (Input.GetButtonDown ("CameraAH")) & (!rotationH) & (!vuesecondaire) & (!Larg) & (!Long) & (!Haut) & (rotcam)) {
						rotationAH = true;
						if (finpoin != 0)
								finpoin--;
						else
								finpoin = 3;
				}

				//je définit la position de la caméra principale
				if ((finpoin == 0) & (!vuesecondaire))
						finishpoint = a;
				else if ((finpoin == 1) & (!vuesecondaire))
						finishpoint = b;
				else if ((finpoin == 2) & (!vuesecondaire))
						finishpoint = c;
				else if ((finpoin == 3) & (!vuesecondaire))
						finishpoint = d;

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++VUE PRINCIPALE+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

				//je fais les rotations horaires
				if ((rotationH) & (!vuesecondaire)) {
						transform.position = Vector3.SmoothDamp (transform.position, finishpoint, ref velocity, smoothTime * Time.deltaTime);
						transform.LookAt (target);
				}

				//je fais les rotations anti-horaires
				if ((rotationAH) & (!vuesecondaire)) {
						transform.position = Vector3.SmoothDamp (transform.position, finishpoint, ref velocity, smoothTime * Time.deltaTime);
						transform.LookAt (target);
				}

				//je réinitialise la caméra
				if ((transform.position == a) ^ (transform.position == b)
				    ^ (transform.position == c) ^ (transform.position == d)
				    & (!vuesecondaire)) {
						rotationH = false;
						rotationAH = false;
				}

//++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++VUE SECONDAIRE+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		
				//je récupère la position du perso
				persox = Perso.position.x;
				persoy = Perso.position.y;
				persoz = Perso.position.z;

				//positions successives de la caméra secondaire
				abis = new Vector3 (persox - 0.8F, persoy + 0.8F, persoz - 0.8F);
				bbis = new Vector3 (persox - 0.8F, persoy + 0.8F, persoz + 0.8F);
				cbis = new Vector3 (persox + 0.8F, persoy + 0.8F, persoz + 0.8F);
				dbis = new Vector3 (persox + 0.8F, persoy + 0.8F, persoz - 0.8F);

				//je détermine à quel point est la caméra selon la rotation
				if (((transform.position == abis) ^ (transform.position == bbis) ^ (transform.position == cbis) ^ (transform.position == dbis))
						& (Input.GetButtonDown ("CameraH")) & (!rotationAH) & (vuesecondaire)) {
						rotationH = true;
						if (finpoin != 3)
								finpoin++;
						else
								finpoin = 0;
				}
				if (((transform.position == abis) ^ (transform.position == bbis) ^ (transform.position == cbis) ^ (transform.position == dbis))
						& (Input.GetButtonDown ("CameraAH")) & (!rotationH) & (vuesecondaire)) {
						rotationAH = true;
						if (finpoin != 0)
								finpoin--;
						else
								finpoin = 3;
				}

				//je définit la position de la caméra principale
				if ((finpoin == 0) & (vuesecondaire))
						possecond = abis;
				else if ((finpoin == 1) & (vuesecondaire))
						possecond = bbis;
				else if ((finpoin == 2) & (vuesecondaire))
						possecond = cbis;
				else if ((finpoin == 3) & (vuesecondaire))
						possecond = dbis;

				//je déplace la caméra avec le personnage
				if ((vuesecondaire) & (!rotationH) & (!rotationAH)) {
						transform.position = possecond;
				}

				//je fais les rotations horaires
				if ((rotationH) & (vuesecondaire)) {
						transform.position = Vector3.SmoothDamp (transform.position, possecond, ref velocity, smoothTime * Time.deltaTime);
						transform.LookAt (Perso);
				}

				//je fais les rotations horaires
				if ((rotationAH) & (vuesecondaire)) {
						transform.position = Vector3.SmoothDamp (transform.position, possecond, ref velocity, smoothTime * Time.deltaTime);
						transform.LookAt (Perso);
				}

				//je réinitialise la caméra
				if ((transform.position == abis) ^ (transform.position == bbis)
						^ (transform.position == cbis) ^ (transform.position == dbis)
						& (vuesecondaire)) {
						rotationH = false;
						rotationAH = false;
				}
		}
}