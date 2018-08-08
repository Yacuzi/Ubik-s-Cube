using UnityEngine;
using System.Collections;

public class Camera_Intro : Ubik_Camera_Smooth
{
	public float zoomfin = 6F;
	public Titre_UI_FadeOut Launcher;

	void Start ()
	{
		//Pour exécuter le start hérité de la caméra
	}

	void Update ()
	{
		//Si le texte a disparu, je zoom comme un gros porc
		if (Launcher.ended)
		{
			Camera.main.orthographicSize = Mathf.Lerp (Camera.main.orthographicSize, zoomfin, Time.deltaTime); //Je zoom ce qui prend environ 10 secondes	
		}
	}
}
