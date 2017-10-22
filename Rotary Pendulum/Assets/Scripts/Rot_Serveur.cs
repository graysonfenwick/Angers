//----------------------------------------------------------------------
// Ball and Plate 2017
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

//import du script pour l'utilisation de la bibliothèque du RP
using Function_RP;




public unsafe class Rot_Serveur : MonoBehaviour
{

    private RotPend_Lib RPLib= new RotPend_Lib();

    //éléments du serveur
    private static SM.SharedMem sharedMem;
    private static int nb_client = 0;           //Nonmbre de client connecté
    private static System.Timers.Timer timer;   //Timer servant la lecture et l'écriture synchrone
    const int LIMIT = 1;
    private float Couple = 0;
    //Nombre de client maximum autorisés à se connecter en meme temps, ici un seul


    //Eléments du ball and beam
    
    private static string message = "";         //Conteneur de message (informations, erreurs...)
    private static float anglePlate = 0;              //Angles at current time
    private static float angleArmY = 0;
    private static float memPlateAngle = 0;
    private static float currAngle = 0;
    public HingeJoint jointure;
    private JointMotor join;
    private static Vector3 velBase = new Vector3(0, 0, 0);
    public GameObject armY;					//Regle dans unity
    public GameObject basePlate;                   //Ball dans unity
    public Rigidbody sphere;
    private static bool debutTick = false;      //Booléen servant de controle du demarrage (start) du timer
    private static bool arretProgramme = false;	//Booléen servant de controle d'une demande d'arret de l'application
    public Camera mainCam;
    public Camera secCam;
    private static float hSliderValue = 1;

    private static float tempNumTest; //for testing multiple variable passing through shared memory

    public Rot_Serveur()
    {
        sharedMem = new SM.SharedMem("DatasRotary", false, 4 * sizeof(double));
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
        basePlate.GetComponent<Rigidbody>().WakeUp();

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

        void* root = sharedMem.Root.ToPointer();

        double* pVal = (double*)root;

        //On met à jour l'angle
        memPlateAngle = (float)pVal[0];
        angleArmY = (float)pVal[3];
    }

    //Fonction permettant l'affichage et l'apparence de la fenetre
    void OnGUI()
    {
        //Affichage des infos du coté serveur
        GUI.Box(new Rect(5, 5, 220, 85), "");
        GUI.Label(new Rect(10, 10, 100, 25), "");
        GUI.Label(new Rect(10, 10, 200, 25), "Requested Degree : " + memPlateAngle.ToString("0.00"));
        GUI.Label(new Rect(10, 25, 200, 25), "Current Angle : " + currAngle.ToString("0.00"));
        GUI.Label(new Rect(10, 40, 300, 25), "Rotation Speed : " + velBase.ToString("0.00"));
        GUI.Label(new Rect(10, 55, 400, 25), "Arm Angle : " + angleArmY.ToString("0.00"));
        GUI.Label(new Rect(10, 70, 800, 25), "Mass of Sphere: " + hSliderValue.ToString());
        hSliderValue = GUI.HorizontalSlider(new Rect(120, 75, 100, 30), hSliderValue, 0.01F, 10.0F);

        if (GUI.Button(new Rect(Screen.width - 100, 37, 75, 25), "Camera"))
        {
            mainCam.enabled = !mainCam.enabled;
            secCam.enabled = !secCam.enabled;
        }

        //Remise en position initial de la bille et de la regle
        if (GUI.Button(new Rect(Screen.width - 100, 10, 75, 25), "Reset"))
        {
            memPlateAngle = 0;
            basePlate.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            basePlate.transform.localRotation = new Quaternion(0, 0, 0, 0);
            armY.transform.localPosition = new Vector3(0, 0, (float)12.75);
            armY.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            armY.transform.localRotation = new Quaternion(0, 0, 0, 0);
            armY.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            //Might need to add in order to reset rotation
        }

        sphere = GetComponent<Rigidbody>();
        sphere.mass = hSliderValue;


    }

    //Fonction appelée a chaque raffraichissement de l'image
    void FixedUpdate()
    {
        void* root = sharedMem.Root.ToPointer();
        double* pVal = (double*)root;
        Transform rotation = basePlate.transform;
        
        if (rotation.eulerAngles.y > 360)
        {
            currAngle = 360 - rotation.eulerAngles.y;
        }
        
        else
        {
            currAngle = rotation.eulerAngles.y;
        }
        
        
        Couple = (currAngle - memPlateAngle) / 10;
        join.targetVelocity = Couple;
        jointure.motor = join;
        pVal[2] = (float)basePlate.transform.rotation.eulerAngles.y; //plate rotation position
        pVal[3] = (float)RPLib.angleYArm(armY);
    }

    //Fonction appelée avant la fermeture de l'application
    void OnDestroy()
    {
        //On arrete le thread
        arretProgramme = true;

        sharedMem.Dispose();

        //On stop le timer
        if (timer.Enabled)
        {
            try
            {
                timer.Stop();
                timer.Dispose();
            }
            catch (Exception ex)
            {
                message = "Erreur lors de l'arret du timer : " + ex.ToString();
            }
        }
    }
}
