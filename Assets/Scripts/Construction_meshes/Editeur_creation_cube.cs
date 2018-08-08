#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreationCube))]
public class Editeur_creation_cube : Editor {
	

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector ();

		CreationCube myScript = (CreationCube)target;

		if (GUILayout.Button ("Créer cube vide")) {
			myScript.CreateEmptyCube ();
		}

		if (GUILayout.Button ("Créer cube plein")) {
			myScript.CreateEmptyCube ();
			myScript.CreateFullCube ();
		}
	}
}
#endif