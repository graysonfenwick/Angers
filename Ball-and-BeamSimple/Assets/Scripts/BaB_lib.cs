using UnityEngine;
using System;

namespace Fonction_BaB 
{
	public class BaB_lib {
		
		public float Intervalle_angle = 60; 
		public float longueur_regle=77; //longueur de la règle
		public float position_origine_sphere_x = 50;
		public float position_origine_sphere_y = 51.12F;


		//lecture de la position de la bille
		public float Lect_dis_bille (GameObject sphere)
		{
			//(50;51.13) centre de la sphere lorsqu'elle est en position initiale
			float x = sphere.transform.localPosition.x - position_origine_sphere_x;
			float y = sphere.transform.localPosition.y - position_origine_sphere_y;
			double distance=Math.Sqrt(x*x+y*y);
			
			if (sphere.transform.localPosition.x<50)
				distance=-distance;
			
			return ((float)distance);
		}

		//Le U12 a une plage de lecture de 0 à5V
		//Paramètre pour la carte U12 et U3
		private float Volt_carte_U12=5;	// Entrée intervalle de lecture 10V (La commande est à 5V)
		private float Volt_carte_S_U12=5;	//Sortie Maximum de la carte Labjack 5 V
		//Convertir des volts en degrés (Lecture)
		public float Conversion_volt_degre_regle_U12 (float volt)
		{
			return((volt)*Intervalle_angle/Volt_carte_U12-(Intervalle_angle/2)); 
		}
		
		//Convertir les degrés en volts (Ecriture)
		public float Conversion_degre_volt_regle_U12 (float degre)
		{
			return((degre+Intervalle_angle/2)*Volt_carte_S_U12/Intervalle_angle);
		}	
		
		// Conversion de la distance en volt (Ecriture)
		public float Conversion_distance_volt_bille_U12 (float distance)
		{
			return((distance+longueur_regle/2)*Volt_carte_S_U12/longueur_regle);
		}

		// Conversion des volt en distance (Lecture)
		public float Conversion_volt_distance_bille_U12 (float volt)
		{
			return(volt/Volt_carte_S_U12*longueur_regle-longueur_regle/2);
		}

		//Le U12 a une plage de lecture de 0 à5V
		//Paramètre pour la carte U12 et U3
		private float Volt_carte_U3=2.5f;	// Entrée intervalle de lecture 10V (La commande est à 5V)
		private float Volt_carte_S_U3=5;	//Sortie Maximum de la carte Labjack 5 V
		//Convertir des volts en degrés (Lecture)
		public float Conversion_volt_degre_regle_U3 (float volt)
		{
			return((volt)*Intervalle_angle/Volt_carte_U3-(Intervalle_angle/2)); 
		}
		
		//Convertir les degrés en volts (Ecriture)
		public float Conversion_degre_volt_regle_U3 (float degre)
		{
			return((degre+Intervalle_angle/2)*Volt_carte_S_U3/Intervalle_angle);
		}	
		
		// Conversion de la distance en volt (Ecriture)
		public float Conversion_distance_volt_bille_U3 (float distance)
		{
			return((distance+longueur_regle/2)*Volt_carte_S_U3/longueur_regle);
		}
		
		// Conversion des volt en distance (Lecture)
		public float Conversion_volt_distance_bille_U3 (float volt)
		{
			return(volt/Volt_carte_S_U3*longueur_regle-longueur_regle/2);
		}

	}
}
