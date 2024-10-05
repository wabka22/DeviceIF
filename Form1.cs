using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace DeviceIF
{
    public partial class Form1 : Form
    {
        private bool start = false;
        private bool first_connection = true;
        private Device _device = new Device();
        private object lockUpdate = new object();
        private int _countSeconds = 0;

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            LoadAvailablePorts();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            port_comboBox.SelectedIndexChanged += port_comboBox_SelectedIndexChanged;
            _device.DataReceived += OnDeviceDataReceived;
        }

        private void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();

            port_comboBox.Items.Clear();
            Baud_Rate_comboBox.Items.Clear();

            int[] baudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800 };

            Baud_Rate_comboBox.Items.AddRange(baudRates.Cast<object>().ToArray());
            Baud_Rate_comboBox.SelectedItem = 115200;

            port_comboBox.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                port_comboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет доступных COM-портов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void port_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (first_connection)
            {
                OpenSelectedPort();
                first_connection = false;
                start = true;
            }
        }

        private void OpenSelectedPort()
        {
            if (port_comboBox.SelectedItem != null)
            {
                string selectedPort = port_comboBox.SelectedItem.ToString();
                try
                {
                    _device.Open(selectedPort, (int)Baud_Rate_comboBox.SelectedItem);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть порт {selectedPort}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnDeviceDataReceived(int value)
        {
            DateTime timeNow = DateTime.Now; 
            Invoke(new Action(() =>
            {
                chart1.Series[0].Points.AddXY(timeNow, value);

                chart1.ChartAreas[0].AxisX.Minimum = DateTime.Now.AddSeconds(-30).ToOADate(); 
                chart1.ChartAreas[0].AxisX.Maximum = DateTime.Now.ToOADate(); 

                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Seconds;
                chart1.ChartAreas[0].AxisX.Interval = 5;

                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

                UpdateSensorData(value); 
            }));
        }

        private void UpdateSensorData(double value)
        {
            if (value_label.InvokeRequired)
            {
                value_label.Invoke(new Action(() => value_label.Text = $"Значение датчика: {value}"));
            }
            else
            {
                value_label.Text = $"Значение датчика: {value}";
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                start = false;
                this.Close();
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                if (first_connection)
                {
                    OpenSelectedPort();
                    start_button.Text = "STOP";
                    _countSeconds = 0;
                    first_connection = false;
                }
                start = true;
            }
            else
            {
                start_button.Text = "START";
                start = false;
            }
        }
    }
}