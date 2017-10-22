using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class script_rotation : MonoBehaviour {

	private float vitesse_rotation;

	// Update is called once per frame
	void FixedUpdate () 
    {
		// Rotation sur l'axe Y
		if(Input.GetKey(KeyCode.LeftArrow))
		{
			vitesse_rotation=50;
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			vitesse_rotation=-50;
		}
		else 
		{
			vitesse_rotation=0;
		}

		// Rotation sur l'axe Y
		transform.Rotate(0,vitesse_rotation*Time.fixedDeltaTime,0);
    }
}




