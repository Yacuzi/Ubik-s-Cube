using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Select_Controller : MonoBehaviour
{
	public Sprite XBoxSprite, KeyBoardSprite;

	// Update is called once per frame
	void Update ()
	{
		if (Controle_Personnage.XBox360) //Si c'est une UI pour la manette de Xbox et qu'une manette est branchée, je l'affiche
			this.GetComponent<Image>().sprite = XBoxSprite;
		
		if (!Controle_Personnage.XBox360)  //Si c'est une UI pour le clavier et qu'aucune manette n'est branchée, je l'affiche
			this.GetComponent<Image>().sprite = KeyBoardSprite;
	}
}
