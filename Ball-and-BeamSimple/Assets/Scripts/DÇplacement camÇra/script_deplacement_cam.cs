using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_deplacement_cam : MonoBehaviour {
	
	private float z_pos;
	private double nb_appui;

	// Update is called once per frame
	void FixedUpdate () {
		//Déplacement sur l'axe z
		if(Input.GetKey(KeyCode.UpArrow) && nb_appui<140)
		{
			z_pos=30;
			nb_appui++;
		}
		else if (Input.GetKey(KeyCode.DownArrow) && nb_appui>-130)
		{
			z_pos=-30;
			nb_appui--;
		}
		else 
		{
			z_pos=0;
		}
		
		transform.Translate(0,0,z_pos*Time.fixedDeltaTime);
	}
}
