using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace USBListener
{
    public partial class Form1 : Form
    {
        public delegate string ReadFromUSB();
        static SerialPort sp;
        static string message;
        static int myCounter;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                cBPortName.Items.Add(s);
            }
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                cBParity.Items.Add(s);
            }
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                cBStopBits.Items.Add(s);
            }
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                cBFlowControl.Items.Add(s);
            }
        }

        private async void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string portName = cBPortName.SelectedItem.ToString();
                string baudRate = cBBaudrate.SelectedItem.ToString();
                string parity = cBParity.SelectedItem.ToString();
                Parity parityCasted = (Parity)Enum.Parse(typeof(Parity), parity, true);
                string dataBits = cBDataBits.SelectedItem.ToString();
                string stopBits = cBStopBits.SelectedItem.ToString();
                StopBits stopBitsCasted = (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
                string handshake = cBFlowControl.SelectedItem.ToString();
                Handshake handShakeCasted = (Handshake)Enum.Parse(typeof(Handshake), handshake, true);

                sp = new SerialPort();
                sp.PortName = portName;
                sp.BaudRate = int.Parse(baudRate);
                sp.Parity = parityCasted;
                sp.DataBits = int.Parse(dataBits);
                sp.StopBits = stopBitsCasted;
                sp.Handshake = handShakeCasted;
                sp.ReadTimeout = 500;
                sp.WriteTimeout = 500;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            var progress = new Progress<string>(s => listBox1.Items.Add(s));
            var secondProgress = new Progress<string>(t => chartData.Series.Add(t));
            await Task.Factory.StartNew(() => SecondThreadConcern.LongWork(progress, secondProgress, sp), TaskCreationOptions.LongRunning);

            DataTable dt = DataFormatter.ReadFile("file.csv");
        }
    }
}
