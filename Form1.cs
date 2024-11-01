using System;
using System.IO.Ports;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot.WindowsForms;

namespace DeviceIF
{
    public partial class Form1 : Form
    {
        private Device _device = new Device();
        private System.Windows.Forms.Timer _portCheckTimer;
        private Filter filter;
        private bool _isReading = false;
        private string _lastSelectedPort = null;
        private PlotView _plotView;
        private LineSeries _lineSeries;

        public Form1()
        {
            InitializeComponent();
            InitializeChart();
            StartPosition = FormStartPosition.CenterScreen;
            string[] currentPorts = SerialPort.GetPortNames();
            UpdatePortList(currentPorts);
            LoadBaudRates();

            filter = new Filter(1000.0);

            _device.OnDataParsed += OnDeviceDataReceived;
            _device.PortsChanged += OnPortsChanged;

            _portCheckTimer = new System.Windows.Forms.Timer();
            _portCheckTimer.Interval = 1000;
            _portCheckTimer.Tick += (sender, e) => _device.CheckPorts();
            _portCheckTimer.Tick += (sender, e) => TryReconnect();
            _portCheckTimer.Start();

            port_comboBox.SelectedIndexChanged += port_comboBox_SelectedIndexChanged;
        }

        private void port_comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_device.Connected && _portCheckTimer.Enabled)
            {
                OpenSelectedPort();
            }
        }

        private void OnPortsChanged(string[] ports)
        {
            Invoke(new Action(() => UpdatePortList(ports)));
        }

        private void TryReconnect()
        {
            if (_device.Connected && !string.IsNullOrEmpty(_lastSelectedPort))
            {
                _device.Connect(_lastSelectedPort, (int)Baud_Rate_comboBox.SelectedItem);
                state_label.Text = "Автоматически переподключено";
                _isReading = true;
                start_button.Text = "STOP";
            }
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
                _lastSelectedPort = selectedPort;
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

        private void InitializeChart()
        {
            _plotView = new PlotView
            {
                Location = new Point(10, 110),
                Size = new Size(900, 500)
            };

            var plotModel = new PlotModel();

            _lineSeries = new LineSeries
            {
                Color = OxyColors.Black,
                StrokeThickness = 1.5,
            };

            plotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                Title = "Время",
                IntervalType = DateTimeIntervalType.Seconds,
                IntervalLength = 50,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
            });

            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Значение",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
            });

            plotModel.Series.Add(_lineSeries);

            _plotView.Model = plotModel;
            this.Controls.Add(_plotView);
        }

        private void OnDeviceDataReceived(int value)
        {
            if (!_isReading) return;

            double filteredValue = filter.ApplyFilter(value);
            DateTime timeNow = DateTime.Now;

            value_label.BeginInvoke(new Action(() =>
            {
                Console.WriteLine(filteredValue);
                value_label.Text = $"Значение датчика: {value}";
                value_label.Update();
            }));

            _plotView.BeginInvoke(new Action(() =>
            {
                _lineSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(timeNow), value));

                var xAxis = _plotView.Model.Axes.FirstOrDefault(a => a is DateTimeAxis);
                if (xAxis != null && _isReading)
                {
                    xAxis.Minimum = DateTimeAxis.ToDouble(DateTime.Now.AddSeconds(-3));
                    xAxis.Maximum = DateTimeAxis.ToDouble(DateTime.Now);
                }

                _plotView.Model.InvalidatePlot(true);
            }));
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
            if (_portCheckTimer.Enabled)
            {
                _portCheckTimer.Stop();
                connection.Text = "Enable Connection Check";
            }
            else
            {
                _portCheckTimer.Start();
                connection.Text = "Disable Connection Check";
            }
        }


    }

}