using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace BluetoothDataReader
{
    public partial class MainForm : Form
    {
        private string recivedData;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
                cmbPorts.Items.Add(port);
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            recivedData = serialPort.ReadLine();
            this.Invoke(new EventHandler(displayData_Event));
        }

        private void displayData_Event(object sender, EventArgs e)
        {
            txtMessage.Text += recivedData + "\n";
        }

        private void btnConn_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbPorts.Text)) return;
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = cmbPorts.Text;
                    serialPort.BaudRate = 9600;
                    serialPort.Open();
                    btnConn.Text = "Kes";
                    lblState.Text = "Bağlandı";
                    lblState.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblState.Text = "Bağlantı Kesildi";
                    lblState.ForeColor = System.Drawing.Color.Red;
                    btnConn.Text = "Bağlan";
                    serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort.IsOpen) serialPort.Close();
        }
    }
}
