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
        

        public Form1()
        {
            InitializeComponent();
            sm = new SM.SharedMem("DatasBAB", true, 4 * sizeof(double));
            timer1.Interval = 300;
            timer1.Enabled = true;           
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

            pVal[0]= (double)trackBar1.Value;
            
        }
        private unsafe void trackBar2_Scroll(object sender, EventArgs e)
        {
            
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            //TODO
            zBallPosition = (double)trackBar2.Value;
           
            //label1.Text = consigne_pos_bille.ToString();
            
            label2.Text = "Z value: " + zBallPosition.ToString();

            pVal[1] = (double)trackBar2.Value;
            
        }
        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            textBox1.Text =  pVal[2].ToString("0.00");
            textBox3.Text =  pVal[3].ToString("0.00");
            textBox4.Text =  pVal[4].ToString("0.00");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            current = new Point(0, 0);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            updatePoint();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                updatePoint();
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;
        }

        private unsafe void updatePoint()
        {
            current = panel1.PointToClient(Cursor.Position);
            current.X = current.X / 3;
            current.Y = current.Y / 3;
            if (current.X <= 0) current.X = 0;
            if (current.Y<= 0) current.Y = 0;
            if (current.X > 40) current.X = 40;
            if (current.Y > 40) current.Y = 40;
            current.X = current.X - 20;
            trackBar1.Value = current.X;
            current.Y = -(current.Y - 20);
            trackBar2.Value = current.Y;
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            pVal[0] = current.X;
            label1.Text = "X Value: " + current.X.ToString();

            pVal[1] = current.Y;
            label2.Text = "Z Value: " + current.Y.ToString();
            distance = pVal[2];
            
            
        }

       
    }
}
