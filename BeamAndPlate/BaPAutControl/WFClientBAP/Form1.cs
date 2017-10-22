using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WFClientBAP
{
    public partial class Form1 : Form
    {
        double xBallPosition;
        double zBallPosition;
        double distance;
        Point current;
        SM.SharedMem sm;
        bool mouseDown = false;
        DateTime Tprec;
        double correcteurT, correcteurK, correcteura;
        double UX, UXprec, UZ, UZprec;
        double cosigne_pos_bille;
        double EXcart, EXcartPrec, EZcart, EZcartPrec;


        public Form1()
        {
            InitializeComponent();
            sm = new SM.SharedMem("DatasBAB", true, 4 * sizeof(double));
            timer1.Interval = 50;
            timer1.Enabled = true;

            correcteurT = 0.05;
            correcteurK = 3;
            correcteura = 30;
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
            xBallPosition = (double)trackBar1.Value;
            //textBox2.Text = "angle " + consigne_pos_bille.ToString();
            label1.Text = "X Value: " + xBallPosition.ToString();
            //pVal[0]= (double)trackBar1.Value;

        }
        private unsafe void trackBar2_Scroll(object sender, EventArgs e)
        {

            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            //TODO
            zBallPosition = (double)trackBar2.Value;
           
            //label1.Text = consigne_pos_bille.ToString();

            label2.Text = "Z value: " + zBallPosition.ToString();
            //pVal[1] = (double)trackBar2.Value;

        }
        private unsafe void timer1_Tick(object sender, EventArgs e)
        {

            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            textBox1.Text = pVal[2].ToString("0.00"); //distance from center
            textBox3.Text = pVal[3].ToString("0.00"); //X Distance
            textBox4.Text = pVal[4].ToString("0.00"); //Z Distance


            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /////////   DEBUT DE LA LOI DE COMMANDE
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            double angleX;
            double angleZ;
            double position_billeZ = pVal[4];
            double position_billeX = -pVal[3];

            double Delta = (DateTime.Now - Tprec).Milliseconds;
            Delta = Delta * 0.001;
            Tprec = DateTime.Now;

            double A = correcteurT / (correcteurT + Delta);
            double B = correcteurK * (Delta + correcteura * correcteurT) / (Delta + correcteurT);
            double C = -(correcteurK * correcteura * correcteurT) / (Delta + correcteurT);

            EZcart = -(xBallPosition + position_billeX);        // ecart 


            EXcart = zBallPosition - position_billeZ; 


            UX = (A * UXprec) + (B * EXcart) + (C * EXcartPrec);    // calcul de la commande
            UZ = (A * UZprec) + (B * EZcart) + (C * EZcartPrec);    // calcul de la commande
            angleX = Math.Max(Math.Min(45, UX), -45); //Saturer la valeur de commande (angle)
            angleZ = Math.Max(Math.Min(45, UZ), -45);
            EXcartPrec = EXcart;
            EZcartPrec = EZcart;
            UXprec = UX;
            UZprec = UZ;


            pVal[0] = angleX;
            pVal[1] = angleZ;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            current = new Point(0, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            Point current = panel1.PointToClient(Cursor.Position);
            current.X = current.X / 6;
            current.Y = current.Y / 6;
            if (current.X <= 0) current.X = 0;
            if (current.Y <= 0) current.Y = 0;
            if (current.X > 40) current.X = 20;
            if (current.Y > 40) current.Y = 20;
            current.X = current.X - 10;
            trackBar1.Value = current.X;
            current.Y = -(current.Y - 10);
            trackBar2.Value = current.Y;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point current = panel1.PointToClient(Cursor.Position);
                current.X = current.X / 6;
                current.Y = current.Y / 6;
                if (current.X <= 0) current.X = 0;
                if (current.Y <= 0) current.Y = 0;
                if (current.X > 20) current.X = 20;
                if (current.Y > 20) current.Y = 20;
                current.X = current.X - 10;
                trackBar1.Value = current.X;
                current.Y = -(current.Y - 10);
                trackBar2.Value = current.Y;
                xBallPosition = current.X;
                label1.Text = "X Value: " + current.X.ToString();
                label2.Text = "Z Value: " + current.Y.ToString();
                zBallPosition = current.Y;
                
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

       
    }
}