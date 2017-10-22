using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms.DataVisualization.Charting;


namespace WFClientBAP
{
    public partial class Form2 : Form
    {
        ArrayList xPos = new ArrayList();
        ArrayList zPos = new ArrayList();
        Point current;
        Stopwatch stopwatch = new Stopwatch();
        bool button1State = false;
        SM.SharedMem sm;
        
        

        public Form2()
        {
            InitializeComponent();
            sm = new SM.SharedMem("DatasBAB", true, 4 * sizeof(double));
            timer1.Interval = 50;
            timer1.Enabled = true;           
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            
            sm.Dispose();
            base.OnFormClosed(e);
        }
       
        private unsafe void timer1_Tick(object sender, EventArgs e)
        {
            createChart();
            textBox3.Text = stopwatch.Elapsed.ToString();
            void* root = sm.Root.ToPointer();
            double* pVal = (double*)root;
            textBox1.Text = "X Location: " + pVal[3].ToString("0.00");
            
            textBox2.Text = "Z Location: " + pVal[4].ToString("0.00");
            if (button1State)
            {
                xPos.Add(pVal[3].ToString("0.00"));
                zPos.Add(pVal[4].ToString("0.00"));
            }
            

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            current = new Point(0, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1State == false)
            {
                button1.Text = "Stop";
                button1State = true;
                stopwatch.Start();

            }
            else
            {
                button1.Text = "Start";
                button1State = false;
                stopwatch.Stop();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] xPosArray = (string[])xPos.ToArray(typeof(string));
            string[] zPosArray = (string[])zPos.ToArray(typeof(string));
            System.IO.File.WriteAllLines(@"D:\xPos.txt", xPosArray);
            System.IO.File.WriteAllLines(@"D:\zPos.txt", zPosArray);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stopwatch.Reset();
            button1State = false;
            button1.Text = "Start";
        }

        private void createChart()
        {
            var series = new Series("X Position");
            var series1 = new Series("Z Position");
            //series.Points.DataBindXY(xPos.ToArray(), zPos.ToArray());
            series.Points.DataBindY(xPos.ToArray());
            series1.Points.DataBindY(zPos.ToArray());
            series1.ChartType = SeriesChartType.Line;
            series.ChartType = SeriesChartType.Line;
            chart1.Series.Clear();
            chart1.Series.Add(series);
            chart1.Series.Add(series1);


        }
    }
}
