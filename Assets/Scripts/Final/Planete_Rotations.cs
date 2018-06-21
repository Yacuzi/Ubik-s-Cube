using UnityEngine;
using System.Collections;

public class Planete_Rotations : MonoBehaviour {

		private Vector3 centre, rot1, rot2;
		public GameObject Planete;
		public bool rota1= true, rota2 = false, rot1positif = false;
		private float anglerot, anglerottot, echelle;

		// Use this for initialization
		void Start () {
				if (rot1positif)
						rot1 = new Vector3 (0, 1, 1);
				else
						rot1 = new Vector3 (0, 1, -1);
				
				rot2 = new Vector3 (0, 1, 0);
				anglerottot = 90F;
				echelle = Mathf.Pow (transform.localScale.x, -0.2F);
		}

		// Update is called once per frame
		void Update () {
				//animation de la planete

				//rotation sur elle-même
				transform.RotateAround (transform.position, Vector3.up, echelle * Time.deltaTime * 15F);

				//rotation autour de sa planète mère (si elle en a)
				centre = Planete.transform.position;

				if (name != "La_Planete") {
						if (rota1) {
								anglerot = echelle * Time.deltaTime * 30F;
								anglerottot = anglerottot + anglerot;
								transform.RotateAround (centre, rot1, anglerot);
								if (anglerottot >= 180) {
										rota1 = false;
										rota2 = true;
										rot1 = new Vector3 (rot1.x, rot1.y, -rot1.z);
										anglerottot = 0;
								}
						
						}
						if (rota2) {
								anglerot = echelle * Time.deltaTime * 30F;
								anglerottot = anglerottot + anglerot;
								transform.RotateAround (centre, rot2, anglerot);
								if (anglerottot >= 180) {
										rota1 = true;
										rota2 = false;
										anglerottot = 0;
								}
						}
				}
		}
}

