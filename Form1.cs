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
        private bool _isReading = false;
        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;

            LoadAvailablePorts();
            LoadBaudRates();

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);

            port_comboBox.SelectedIndexChanged += port_comboBox_SelectedIndexChanged;
            _device.OnDataParsed += OnDeviceDataReceived;
            _device.PortsNeedRefresh += LoadAvailablePorts;

        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DEVICECHANGE = 0x0219;
            const int DBT_DEVICEARRIVAL = 0x8000;
            const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

            if (m.Msg == WM_DEVICECHANGE && (m.WParam.ToInt32() == DBT_DEVICEARRIVAL || m.WParam.ToInt32() == DBT_DEVICEREMOVECOMPLETE))
            {
                LoadAvailablePorts();
            }

            base.WndProc(ref m);
        }

        public void LoadAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();

            port_comboBox.Items.Clear();

            port_comboBox.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                port_comboBox.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Нет доступных COM-портов", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                state_label.Text = "Отключено";
                start_button.Text = "START";
                LoadAvailablePorts();
                port_comboBox.Items.Clear();
            }
        }

        private void LoadBaudRates()
        {
            Baud_Rate_comboBox.Items.Clear();

            int[] baudRates = { 110, 300, 600, 1200, 2400, 4800, 9600, 19200, 38400, 57600, 115200, 230400, 460800 };

            Baud_Rate_comboBox.Items.AddRange(baudRates.Cast<object>().ToArray());
            Baud_Rate_comboBox.SelectedItem = 115200;
        }

        private void port_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_device.Connected)
            {
                OpenSelectedPort();
            }
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                _device.Disconnect(); 
                this.Close();
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
            else if (start_button.Text == "STOP" && _device.Connected)
            {
                start_button.Text = "START"; 
                state_label.Text = "Остановлено";
                _isReading = false;
            }
        }

    }
}