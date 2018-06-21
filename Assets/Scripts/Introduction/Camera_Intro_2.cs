using UnityEngine;
using System.Collections;

public class Camera_Intro_2 : MonoBehaviour {

		public Transform Personnage;
		private Vector3 posdep, posperdep;

		// Use this for initialization
		void Start () {

				posdep = Camera.main.transform.position;
				posperdep = Personnage.transform.position;

		}

		// Update is called once per frame
		void Update () {

				//je suis le déplacement du personnage
				Camera.main.transform.position = posdep + Personnage.transform.position - posperdep;

		}
}
