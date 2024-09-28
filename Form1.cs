using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace SerialPlot
{
    public partial class Form1 : Form
    {
        private  bool start=false;
        private bool  isSerialPortSelectChange=false;
        private SerialPort serialPort_;
        private SerialParser parser;
        private string Portname="COM7";
        object lockUpdate = new object();
        private int _countSeconds = 0;
        public Form1()
        {
            InitializeComponent();
            StartPosition=FormStartPosition.CenterScreen;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            serialPort_ = new SerialPort();

            chart1.ChartAreas[0].AxisY.Maximum = 150;
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
                string dataRead = serialPort_.ReadLine();
                int value;
                bool success = int.TryParse(dataRead, out value);

                if (success)
                {
                    chart1.Series[0].Points.AddXY(timeNow, value);
                }
                else
                {
                    chart1.Series[0].Points.AddXY(timeNow, 0);
                }
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
                serialPort_.PortName = Portname;
                serialPort_.BaudRate = 115200;
                serialPort_.Open();
                serialPort_.DataReceived += _serialPort_DataReceived;
                start = true;
            }
            else {
                button1.Text = "START";
                start = false;
            }
        }


        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try{
                string dataRead = serialPort_.ReadLine();
                label1.Text = dataRead.ToString() ;
                lock (lockUpdate)
                {
                    if (start)
                    {
                        Dictionary<string, double> records = parser.ParsingMatch(dataRead);
                        if (records.Count == 0) return;
                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
