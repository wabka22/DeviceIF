using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SerialPlot
{
    public partial class Form1 : Form
    {

        bool start=false;


        public Form1()
        {
            InitializeComponent();
            StartPosition=FormStartPosition.CenterScreen;
        }

        private int _countSeconds = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;

            chart1.ChartAreas[0].AxisY.Maximum = 10;
            chart1.ChartAreas[0].AxisY.Minimum = -5;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm::ss";
            chart1.Series[0].XValueType = ChartValueType.DateTime;

            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();

            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
            chart1.ChartAreas[0].AxisX.Interval = 5;



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (start)
            {
                DateTime timeNow = DateTime.Now;
                int value = 0;
                label1.Text = value.ToString();
                chart1.Series[0].Points.AddXY(timeNow, value);
                _countSeconds++;

                if (_countSeconds == 440)
                {
                    _countSeconds = 0;
                    chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddMinutes(1).ToOADate();

                    chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    chart1.ChartAreas[0].AxisX.Interval = 5;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                button1.Text = "STOP";
                _countSeconds = 0;
                start = true;
            }
            else {
                button1.Text = "START";
                start = false;

            }
        }



    }
}
