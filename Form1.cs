using System;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Management;


namespace DeviceIF
{
    public partial class Form1 : Form
    {
        private Device _device = new Device();
        private System.Windows.Forms.Timer _portCheckTimer;
        private bool _isReading = false;
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            string[] currentPorts = SerialPort.GetPortNames();
            UpdatePortList(currentPorts);
            LoadBaudRates();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            _device.OnDataParsed += OnDeviceDataReceived;
            _device.PortsChanged += OnPortsChanged;

            _portCheckTimer = new System.Windows.Forms.Timer();
            _portCheckTimer.Interval = 1000; 
            _portCheckTimer.Tick += (sender, e) => _device.CheckPorts();
            _portCheckTimer.Start();
        }

        private void OnPortsChanged(string[] ports)
        {
            Invoke(new Action(() => UpdatePortList(ports)));
        }

        private void UpdatePortList(string[] ports)
        {
            port_comboBox.Items.Clear();
            port_comboBox.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                port_comboBox.SelectedIndex = 0;
            }
            else
            {
                state_label.Text = "Нет доступных портов";
                start_button.Text = "START";
            }
        }

        private void LoadBaudRates()
        {
            Baud_Rate_comboBox.Items.Clear();

            int[] baudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800 };

            Baud_Rate_comboBox.Items.AddRange(baudRates.Cast<object>().ToArray());
            Baud_Rate_comboBox.SelectedItem = 115200;
        }

        private void OpenSelectedPort()
        {
            if (port_comboBox.SelectedItem != null)
            {
                string selectedPort = port_comboBox.SelectedItem.ToString();
                try
                {
                    _device.Connect(selectedPort, (int)Baud_Rate_comboBox.SelectedItem);
                    state_label.Text = "Подключено";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось открыть порт {selectedPort}: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnDeviceDataReceived(int value)
        {
            if (!_isReading) return;


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
            string text = $"Значение датчика: {value}";

            if (value_label.InvokeRequired)
            {
                value_label.Invoke(new Action(() => value_label.Text = text));
            }
            else
            {
                value_label.Text = text;
            }
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            if (!_isReading)
            {
                OpenSelectedPort(); 
                if (_device.Connected)
                {
                    start_button.Text = "STOP"; 
                    state_label.Text = "Подключено";
                    _isReading = true;
                }
                else
                {
                    state_label.Text = "Порт не найден";
                }
            }
            else if (_isReading && _device.Connected)
            {
                start_button.Text = "START"; 
                state_label.Text = "Остановлено";
                _isReading = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Disable Connection Check")
            {
                _portCheckTimer.Stop();
                button1.Text = "Enable Connection Check"; 
            }
            else
            {
                _portCheckTimer.Start();
                button1.Text = "Disable Connection Check"; 
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _device.Disconnect();
                this.Close();
            }
        }
    }

}