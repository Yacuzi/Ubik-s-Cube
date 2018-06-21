using UnityEngine;
using System.Collections;

public class Creation_Cube_Plein : MonoBehaviour {

		public int Largeurplein, Longueurplein, Hauteurplein;
		public Transform Cubes, Cubesnoirs;
		private Vector3 CubPos;
		private Quaternion CubRot = Quaternion.identity;
		private string Name;

	// Use this for initialization
	void Start () {

				//Traverser tout le cube :
				for (int i = 0; i < Largeurplein; i++) {
						for (int j = 0; j < Hauteurplein; j++) {
								for (int k = 0; k < Longueurplein; k++) {

										//La position du cube
										CubPos = new Vector3 (k, j, i);

										//Créer le cube
										if ((k == 0) | (j == 0) | (i == 0) |
												(k == Longueurplein - 1) | (j == Hauteurplein - 1) | (i == Largeurplein - 1)) {
												Cubes.tag = "Kubs";
												Instantiate (Cubes, CubPos, CubRot);
												Name = (k + "_" + j + "_" + i); 
												Cubes.name = Name;
										} else {
												Cubes.tag = "Kubs";
												Instantiate (Cubesnoirs, CubPos, CubRot);
												Name = (k + "_" + j + "_" + i); 
												Cubes.name = Name;
										}

								}
						}
				}
	
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
