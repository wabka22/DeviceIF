using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace SerialPlot
{
    public partial class Form1 : Form
    {
        private  bool start=false;
        private bool first_connection = true;
        private bool  isSerialPortSelectChange=false;
        private SerialPort serialPort_;
        private SerialParser parser;
        private object lockUpdate = new object();
        private int _countSeconds = 0;

        public Form1()
        {
            InitializeComponent();
            StartPosition=FormStartPosition.CenterScreen;
            LoadAvailablePorts();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown); 
        }

        private void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();

            comboBox1.Items.Clear();

            comboBox1.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                comboBox1.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет доступных COM-портов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OpenSelectedPort()
        {
            if (comboBox1.SelectedItem != null)
            {
                string selectedPort = comboBox1.SelectedItem.ToString();
                try
                {
                    serialPort_ = new SerialPort(selectedPort);
                    serialPort_.BaudRate = 115200;
                    serialPort_.Open();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть порт {selectedPort}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

   
        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            serialPort_ = new SerialPort();

            chart1.ChartAreas[0].AxisY.Maximum = 150;
            chart1.ChartAreas[0].AxisY.Minimum = -20;

            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "H:mm::ss";
            chart1.Series[0].XValueType = ChartValueType.DateTime;

            chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
            chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(30).ToOADate();

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

                if (int.TryParse(dataRead, out value))
                {
                    chart1.Series[0].Points.AddXY(timeNow, value);
                }
                else
                {
                    chart1.Series[0].Points.AddXY(timeNow, 0);
                }

                serialPort_.DataReceived += _serialPort_DataReceived;
                _countSeconds++;
                UpdateSensorData(value);

                if (_countSeconds == 200)
                {
                    _countSeconds = 0;
                    chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.ToOADate();
                    chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.AddSeconds(30).ToOADate();

                    chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                    chart1.ChartAreas[0].AxisX.Interval = 5;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                if (first_connection)
                {
                    OpenSelectedPort();
                    button1.Text = "STOP";
                    _countSeconds = 0;
                    first_connection = false;   
                }
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
                lock (lockUpdate)
                {
                    if (start)
                    {
                        label.Text = dataRead.ToString();
                        Dictionary<string, double> records = parser.ParsingMatch(dataRead);
                        if (records.Count == 0) start=false;

                    }
                }
            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
        }

        private void UpdateSensorData(double value)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = $"Значение датчика: {value}"));
            }
            else
            {
                label.Text = $"Значение датчика: {value}";
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                start=false;
                this.Close();  
            }
        }

    }
}
