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
        double xBallPosition;
        double zBallPosition;
        double distance;
        SM.SharedMem sm;
       

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
            distance = pVal[2];
            textBox1.Text = "angle " + pVal[1].ToString();
            textBox2.Text = "distance " + distance.ToString();
        }

        
    }
}
