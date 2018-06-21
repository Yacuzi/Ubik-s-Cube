using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Sauvegarde_Meshes : MonoBehaviour {

		public GameObject Meshasauvegarder;
		public string nommesh = "nommanquant";
		private string chemin;
		private Mesh lemesh;


	// Use this for initialization
	void Start () {

				//je récupère les informations concernant le mesh à sauvegarder
				lemesh = Meshasauvegarder.GetComponent<MeshFilter>().sharedMesh;

				//je définit le nom du chemin
				chemin = "Assets/Meshes/" + nommesh + ".asset";

				//pour enlever les alertes à la con
				if (lemesh.name == null)
						Debug.Log ("Prout");
				if (chemin == null)
						Debug.Log ("Caca!");

				//je sauvegarde le mesh
				#if UNITY_EDITOR
				AssetDatabase.CreateAsset(lemesh, chemin);
				AssetDatabase.SaveAssets();
				#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}