#if (UNITY_EDITOR)
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class CreationCube : MonoBehaviour {

	public int Largeur, Longueur, Hauteur;
	public GameObject Coin, Angle, Tuile, Vide, Plein, Cubesnoirs;
	private Vector3 CubPos;
	private Quaternion CubRot;
	private GameObject Cube, Cubesparents;
	private int RotX=0, RotY=0, RotZ=0;
	private int Bout1=0, Bout2=0, Bout3=0, ToutBout=0;

	//Fonction de création de cubes pleins
	public void CreateFullCube () {

		//Je créé le parent dans lequel je vais mettre tous les morceaux de cubes
		Cubesparents = new GameObject();
		Cubesparents.name = "Ubik's Cube";

		//Traverser tout le cube :
		for (int i = 0; i < Largeur; i++) {
			for (int j = 0; j < Hauteur; j++) {
				for (int k = 0; k < Longueur; k++) {

					//La position du cube
					CubPos = new Vector3 (k, j, i);

					//Créer le cube
					if ((k == 0) | (j == 0) | (i == 0) |
					   (k == Longueur - 1) | (j == Hauteur - 1) | (i == Largeur - 1)) {

						Cube = PrefabUtility.InstantiatePrefab (Plein) as GameObject;
						Cube.transform.SetParent (Cubesparents.transform);
						Cube.transform.position = CubPos;
						Cube.transform.rotation = CubRot;
						Cube.tag = "Kubs";
						Cube.name = (k + "_" + j + "_" + i);

					} else {
						
						Cube = PrefabUtility.InstantiatePrefab (Cubesnoirs) as GameObject;
						Cube.transform.SetParent (Cubesparents.transform);
						Cube.transform.position = CubPos;
						Cube.transform.rotation = CubRot;
						Cube.tag = "Kubs";
						Cube.name = (k + "_" + j + "_" + i);
					}
				}
			}
		}	
	}

	// Fonction de création de cubes vides
	public void CreateEmptyCube () {

		//Je créé le parent dans lequel je vais mettre tous les morceaux de cubes
		Cubesparents = new GameObject();
		Cubesparents.name = "Ubik's Cube";

		//Traverser tout le cube :
		for (int i = 0; i < Largeur; i++) {
			for (int j = 0; j < Hauteur; j++) {
				for (int k = 0; k < Longueur; k++) {

					//Est-il au bout du cube?
					Bout1 = 0;
					Bout2 = 0;
					Bout3 = 0;

					if ((k == 0) ^ (k == Longueur - 1))
						Bout1 = 1;
					if ((j == 0) ^ (j == Hauteur - 1))
						Bout2 = 1;
					if ((i == 0) ^ (i == Largeur - 1))
						Bout3 = 1;

					ToutBout = Bout1 + Bout2 + Bout3;

					//Quel type de cube
					if (ToutBout == 0)
						Cube = Vide;
					else if (ToutBout == 1)
						Cube = Tuile;
					else if (ToutBout == 2)
						Cube = Angle;
					else if (ToutBout == 3)
						Cube = Coin;
										
					//La position du cube
					CubPos = new Vector3 (k, j, i);

					//La rotation des coins
					if (Cube == Coin) {
						if ((i == 0) & (j == 0) & (k == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = 0;
						}	
						if ((i == 0) & (j == 0) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = -90;
							RotZ = 0;
						}	
						if ((i == 0) & (j == Hauteur - 1) & (k == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = -90;
						}	
						if ((i == 0) & (j == Hauteur - 1) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = 0;
							RotZ = 180;
						}	
						if ((i == Largeur - 1) & (j == 0) & (k == 0)) {
							RotX = 0;
							RotY = 90;
							RotZ = 0;
						}	
						if ((i == Largeur - 1) & (j == 0) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = 180;
							RotZ = 0;
						}	
						if ((i == Largeur - 1) & (j == Hauteur - 1) & (k == 0)) {
							RotX = 0;
							RotY = 180;
							RotZ = 180;
						}	
						if ((i == Largeur - 1) & (j == Hauteur - 1) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = 270;
							RotZ = 180;
						}
					}

					//La rotation des tuiles
					else if (Cube == Tuile) {
						if ((k != 0) & (j != 0) & (i == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = 0;
						}
						if ((k != 0) & (j == 0) & (i != 0)) {
							RotX = -90;
							RotY = 0;
							RotZ = 0;
						}
						if ((k == 0) & (j != 0) & (i != 0)) {
							RotX = 0;
							RotY = 90;
							RotZ = 0;
						}
						if ((i == Largeur - 1) & (j != 0) & (k != 0)) {
							RotX = -180;
							RotY = 0;
							RotZ = 0;
						}
						if ((k != 0) & (j == Hauteur - 1) & (i != 0)) {
							RotX = 90;
							RotY = 0;
							RotZ = 0;
						}
						if ((i != 0) & (j != 0) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = -90;
							RotZ = 0;
						}
					}

					//La rotation des angles
					else if (Cube == Angle) {
						if ((Bout1 == 0) & (j == 0) & (i == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = 0;													
						}
						if ((Bout1 == 0) & (j == Hauteur - 1) & (i == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = 180;													
						}
						if ((Bout1 == 0) & (j == 0) & (i == Largeur - 1)) {
							RotX = 0;
							RotY = 180;
							RotZ = 0;													
						}
						if ((Bout1 == 0) & (j == Hauteur - 1) & (i == Largeur - 1)) {
							RotX = 0;
							RotY = 180;
							RotZ = 180;													
						}
						if ((Bout2 == 0) & (k == 0) & (i == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = -90;													
						}
						if ((Bout2 == 0) & (k == Longueur - 1) & (i == 0)) {
							RotX = 0;
							RotY = 0;
							RotZ = 90;													
						}
						if ((Bout2 == 0) & (k == 0) & (i == Largeur - 1)) {
							RotX = 0;
							RotY = 90;
							RotZ = -90;													
						}
						if ((Bout2 == 0) & (k == Longueur - 1) & (i == Largeur - 1)) {
							RotX = 0;
							RotY = 180;
							RotZ = -90;													
						}
						if ((Bout3 == 0) & (j == 0) & (k == 0)) {
							RotX = 0;
							RotY = 90;
							RotZ = 0;												
						}
						if ((Bout3 == 0) & (j == 0) & (k == Longueur - 1)) {
							RotX = 0;
							RotY = -90;
							RotZ = 0;													
						}
						if ((Bout3 == 0) & (j == Hauteur - 1) & (k == 0)) {
							RotX = 90;
							RotY = 90;
							RotZ = 0;													
						}
						if ((Bout3 == 0) & (j == Hauteur - 1) & (k == Longueur - 1)) {
							RotX = 90;
							RotY = -90;
							RotZ = 0;													
						}																									
					}

					//Rotation dans le vide pour le style
					else if (Cube == Vide) {
						RotX = 0;
						RotY = 0;
						RotZ = 0;
					}

					//Rotation finale
					CubRot = Quaternion.Euler (RotX, RotY, RotZ);
												
					//Créer le cube
					Cube = PrefabUtility.InstantiatePrefab (Cube) as GameObject;
					Cube.transform.SetParent (Cubesparents.transform);
					Cube.transform.position = CubPos;
					Cube.transform.rotation = CubRot;
					Cube.tag = "Cube";
					Cube.name = (k + "_" + j + "_" + i);

				}
			}
		}
	}
}
#endif