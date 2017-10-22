using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace WFClientRotary
{
    public partial class Form1 : Form
    {
        SM.SharedMem sm;

        public Form1()
        {
            InitializeComponent();
            sm = new SM.SharedMem("DatasRotary", true, 4 * sizeof(double));
            timer1.Interval = 50;
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

            label1.Text = "Desired Rotation Speed: " + trackBar1.Value.ToString();
            pVal[0]= (double)trackBar1.Value;
            
        }
        
        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            textBox1.Text =  pVal[2].ToString("0.00");
            textBox3.Text =  pVal[3].ToString("0.00");
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        
    }
}
