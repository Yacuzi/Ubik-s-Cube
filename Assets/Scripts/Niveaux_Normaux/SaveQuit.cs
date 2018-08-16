using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveQuit : MonoBehaviour
{
	public void SaveSphere () //La méthode pour enregistrer la récupération d'un sphère
	{
		string id = "";

		id = SceneManager.GetActiveScene ().buildIndex.ToString(); //Je récupère l'id de la sphère

		PlayerPrefs.SetInt(id, 1); //Je dis que la sphère en question a été trouvée
	}

	public bool GetSphere () //La méthode pour voir si une sphère a déjà été récupérée ou non
	{
		string id = "";

		id = (string)SceneManager.GetActiveScene ().buildIndex.ToString(); //Je récupère l'id de la sphère

		if (PlayerPrefs.GetInt (id, 0) == 1) //Si la sphère a déjà été trouvée
			return true;
		else
			return false;
	}

	void Start ()
	{
		PlayerPrefs.SetInt("Niveau", SceneManager.GetActiveScene().buildIndex);
	}
	
	void Update ()
	{
		if (Input.GetKey (KeyCode.Escape))
			Application.Quit();

		if (Input.GetKey (KeyCode.E) && Input.GetKey (KeyCode.R) && Input.GetKey (KeyCode.Delete))
		{
			PlayerPrefs.SetInt ("Niveau", 1);

			string Sid = "";

			for (int id = 0; id < Total_Sphere.NbTotSphere; id++)
			{
				Sid = id.ToString ();
				PlayerPrefs.SetInt (Sid, 0);
			}
		}
	}
}
