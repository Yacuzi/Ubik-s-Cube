using UnityEngine;
using System.Collections ;

public class Renommer_Cubes : MonoBehaviour {

		private GameObject Cube;
		private string Name;
		private Vector3 pos;
		private int x,y,z;

		// Use this for initialization
	void Start () {
				//renommer les cubes de base
				GameObject[] cubes = GameObject.FindGameObjectsWithTag ("Cube") as GameObject[];
				foreach (GameObject cube in cubes) {
						x = (int) cube.transform.position.x;
						y = (int) cube.transform.position.y;
						z = (int) cube.transform.position.z;
						cube.name = ( x + "_" + y + "_" + z);
				}

				//renommer les kubs
				GameObject[] kubs = GameObject.FindGameObjectsWithTag ("Kubs") as GameObject[];
				foreach (GameObject kub in kubs) {
						x = (int) kub.transform.position.x;
						y = (int) kub.transform.position.y;
						z = (int) kub.transform.position.z;
						kub.name = ( x + "_" + y + "_" + z);
				}

				//renommer les antikubs
				GameObject[] antikubs = GameObject.FindGameObjectsWithTag ("Antikub") as GameObject[];
				foreach (GameObject antikub in antikubs) {
						x = (int)antikub.transform.position.x;
						y = (int)antikub.transform.position.y;
						z = (int)antikub.transform.position.z;
						antikub.name = (x + "_" + y + "_" + z);
				}
		}
	
	// Update is called once per frame
	void Update () {						
				}
}
