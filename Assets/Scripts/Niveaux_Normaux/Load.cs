using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load : MonoBehaviour
{
	void Awake ()
	{
		int niveau = PlayerPrefs.GetInt ("Niveau", 1);
		SceneManager.LoadScene (niveau);
	}
}