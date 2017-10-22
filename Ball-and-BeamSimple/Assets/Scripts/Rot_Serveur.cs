//----------------------------------------------------------------------
//  Ball and Beam 2014
// 
//  Serveur d'écoute du Ball and Beam
//----------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Threading;

//import pour la socket serveur, les streams et le timer
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Timers;

//import du script pour l'utilisation de la bibliothèque du BaB
using Fonction_BaB;




public unsafe class Rot_Serveur : MonoBehaviour
{

	private BaB_lib Ball_and_Beam = new BaB_lib();
	
    //éléments du serveur
	private static SM.SharedMem sharedMem;
	private static int nb_client = 0; 			//Nonmbre de client connecté
	private static System.Timers.Timer timer;	//Timer servant la lecture et l'écriture synchrone
	const int LIMIT = 1;						//Nombre de client maximum autorisés à se connecter en meme temps, ici un seul

   
    //Eléments du ball and beam
	private static float Angle_recu_client;		//Angle envoyé par le client
	private static string message = "";			//Conteneur de message (informations, erreurs...)
	private static float posBille = 0;			//Position de la bille sur la règle
    private float Angle_règle = 0;				//Angle de la règle
	private float Couple = 0;					//Couple exercé sur la règle
    public GameObject regle;					//Regle dans unity
    public GameObject sphere;					//Bille dans unity
	private JointMotor join;					//Liaison servant à motoriser une jointure
    public HingeJoint jointure;					//Jointure entre la regle et le cube lui servant de support

	private static bool debutTick = false;		//Booléen servant de controle du demarrage (start) du timer
	private static bool arretProgramme = false;	//Booléen servant de controle d'une demande d'arret de l'application

    private static float tempNumTest; //for testing multiple variable passing through shared memory

	public Rot_Serveur()
	{
		sharedMem = new SM.SharedMem("DatasBAB",false,4*sizeof(double));
	}

    
    // Methode pour l'initialisation
    void Start()
    {

		//Initialisation des booléens et de la force du moteur
		arretProgramme = false;
		debutTick = false;
		join.force = 100;

	
		//On affiche le message dans la box prévu à cet effet
        message = "Running ";


		//Initialisation des paramètres du Timer
		timer = new System.Timers.Timer();
		timer.Interval = 5;
		timer.Elapsed += Tick_Timer;
		timer.Start();

    }

    
    //Fonction d'un thread, elle permet une nouvelle écoute d'une connexion client entrante
	static void Service()
    {

    }

        
    // Fonction appelée à chaque Interruption du timers
	static unsafe void Tick_Timer(object source, EventArgs e)
    {

		void * root = sharedMem.Root.ToPointer(); 
		
		double * pVal = (double *) root;

        //On met à jour l'angle
        Angle_recu_client = (float)pVal[0];
        
    }

    //Fonction permettant l'affichage et l'apparence de la fenetre
	void OnGUI()
    {
        //Affichage des infos du coté serveur
		GUI.Box(new Rect(5, 5, 150, 60),"");
        GUI.Label(new Rect(10, 10, 100, 25),"" );
        GUI.Label(new Rect(10, 10, 200, 25), "Angle recu : " + Angle_recu_client.ToString("0.00"));
        GUI.Label(new Rect(10, 25, 200, 25), "Angle réel :" + Angle_règle.ToString("0.00"));
        GUI.Label(new Rect(10, 40, 400, 25), "DistanceFromCenter :" + posBille.ToString("0.00"));
        GUI.Label(new Rect(10, 60, 600, 25), "Temp :" + tempNumTest.ToString("000.00"));

        //Remise en position initial de la bille et de la regle
        if (GUI.Button(new Rect(Screen.width-100, 10, 75, 25), "Reset"))
        {
            Angle_recu_client = 0;
            regle.transform.localRotation = new Quaternion(0, 0, 0, 0);
            sphere.transform.localPosition = new Vector3(50, 51.13F, 50);
            sphere.transform.localRotation = new Quaternion(0, 0, 0, 0);
            sphere.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

      

        
    }

    //Fonction appelée a chaque raffraichissement de l'image
    void Update()
    {
        //Réglage de l'angle, l'angle de la règle va de (-180° à 180°)
        if (regle.transform.eulerAngles.z > 180)
        {
            Angle_règle = 360 - regle.transform.eulerAngles.z;
        }
        else
        {
            Angle_règle = -regle.transform.eulerAngles.z;
        }

        //Réglage du couple de la règle
        Couple = (Angle_règle - Angle_recu_client) * 10;

        //Application du couple au système virtuel
        join.targetVelocity = Couple;
        jointure.motor = join;

        //Lecture de la position de la bille
        posBille = Ball_and_Beam.Lect_dis_bille(sphere);

		void * root = sharedMem.Root.ToPointer(); 
		
		double * pVal = (double *) root;
		pVal[1]=(double) posBille;
        
    }

    //Fonction appelée avant la fermeture de l'application
	void OnDestroy()
    {
		//On arrete le thread
		arretProgramme = true;

		sharedMem.Dispose();

	       //On stop le timer
		if(timer.Enabled)
		{
			try{
				timer.Stop();
				timer.Dispose();
			}catch(Exception ex){
				message = "Erreur lors de l'arret du timer : " + ex.ToString();
			}
		}
	}
}
