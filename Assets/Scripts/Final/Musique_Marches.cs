using UnityEngine;
using System.Collections;

public class Musique_Marches : MonoBehaviour {

	public GameObject Perso;
	private float persox, persoy, persoz;
	private AudioSource musique;
	private AudioClip glokenspiel, bits;
	private bool effet = false;
	private float tempseffet;

	// Use this for initialization
	void Start () {
				musique = GetComponent<AudioSource> ();
		}
	
	// Update is called once per frame
	void Update () {
	
				effet = Camera.main.GetComponent<Camera_Final_Acte_I>().effetdramatique;
				tempseffet = Camera.main.GetComponent<Camera_Final_Acte_I>().smoothTimeregard;

				//je récupère la position du perso
				persoy = Perso.transform.position.y;

				//je modifie le volume du son 8 bit
				if (musique.clip.name == "Ubik_Marches_8Bit2")
						musique.volume = (-(persoy + 0.1F) * 0.1F) + 0.65F;

				//je modifie le volume du son Glokenspiel
				if (musique.clip.name == "Ubik_Marches_Glokenspiel2")
						musique.volume = ((persoy + 0.1F) * 0.1F) + 0.05F;

				if (effet)
						musique.pitch = Mathf.Lerp (musique.pitch, 0.5F, tempseffet * 0.1F * Time.deltaTime);
		}
}
