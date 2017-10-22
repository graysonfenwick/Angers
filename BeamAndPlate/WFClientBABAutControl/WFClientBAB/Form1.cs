using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WFClientBAB
{
    public partial class Form1 : Form
    {
        double Ecart, EcartPrec;
        double U, Uprec;
        DateTime Tprec;
        double correcteurT, correcteurK, correcteura;
        double consigne_pos_bille;

        SM.SharedMem sm;
       

        public Form1()
        {
            InitializeComponent();
            sm = new SM.SharedMem("DatasBAB", true, 4 * sizeof(double));
            timer1.Interval = 20;
            timer1.Enabled = true;

            correcteurT = 0.07;
            correcteurK = 1;
            correcteura = 5;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            sm.Dispose();
            base.OnFormClosed(e);
        }

        private unsafe void trackBar1_Scroll(object sender, EventArgs e)
        {

            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;            
            consigne_pos_bille = (double)trackBar1.Value;
            textBox2.Text = "consigne " + consigne_pos_bille.ToString();
        }


        

        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            textBox1.Text = pVal[2].ToString("F2");
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////   DEBUT DE LA LOI DE COMMANDE
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            double angle;
            double position_bille=pVal[2];

            double Delta = (DateTime.Now - Tprec).Milliseconds;
            Delta = Delta * 0.001;
            Tprec = DateTime.Now;
            
            double A = correcteurT/(correcteurT+Delta);
            double B = correcteurK * (Delta + correcteura * correcteurT) / (Delta + correcteurT);
            double C = -(correcteurK * correcteura * correcteurT) / (Delta + correcteurT);
                       
            Ecart = consigne_pos_bille - position_bille;        // ecart 
            U = (A * Uprec) + (B * Ecart) + (C * EcartPrec);    // calcul de la commande
            
            angle = Math.Max(Math.Min(45, U), -45); //Saturer la valeur de commande (angle)

            EcartPrec = Ecart;
            Uprec = U;


            pVal[0] = angle;
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
