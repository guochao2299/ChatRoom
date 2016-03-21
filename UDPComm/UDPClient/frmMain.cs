using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Text;
using System.Windows.Forms;

using UDPComm;

namespace UDPClient
{
    public partial class frmMain : Form
    {
        private string m_hostIP = "127.0.0.1";
        private int m_port = 6666;
        private UdpClient m_client;
        private volatile bool m_done = false;
        private byte[] m_data = null;
        private string m_sendData=string.Empty, m_returnData=string.Empty;
        private IPEndPoint m_endPoint;
        private Thread m_receiveThread;
        private SymCryptography m_symCrpt;
        private AsymCryptography m_asymCrpt;
        private const int DELAY_TIME = 2000;

        public frmMain()
        {
            InitializeComponent();

            m_client = null;
            txtIP.Text = m_hostIP;
            txtPort.Text = m_port.ToString();
            m_symCrpt = new SymCryptography();
        }

        private void Listener()
        {
            m_done = false;

            try
            {
                while (!m_done)
                {
                    IPEndPoint ep = null;
                    byte[] buf = m_client.Receive(ref ep);
                    m_returnData = UDPComm.UDPComm.DecodingASCII(buf, m_symCrpt);
                    this.Invoke(new MethodInvoker(DisplayReceiveMessage));
                }
            }
            catch
            {
                return;
            }
            finally
            {
                if(m_client!=null)
                {
                    m_client.Close();
                    m_client=null;
                }
            }            
        }

        private void DisplayReceiveMessage()
        {
            string time = DateTime.Now.ToString("t");
            txtContent.Text = time + "  " + m_returnData + "\r\n" + txtContent.Text;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                m_sendData = txtMsg.Text;
                m_data = UDPComm.UDPComm.EncodingASCII(m_sendData, m_symCrpt);
                m_client.Send(m_data, m_data.Length);
            }
        }

        private bool ReceiveData(string title,ref string content)
        {
            DateTime de, ds = DateTime.Now;
            de = ds;
            TimeSpan ts;

            while ((ts = (de - ds)).TotalMilliseconds < DELAY_TIME)//超时就是这样实现的？
            {
                IPEndPoint ep = null;
                byte[] buf = m_client.Receive(ref ep);
                m_returnData = UDPComm.UDPComm.DecodingASCII(buf);

                if (m_returnData.StartsWith(title))
                {                    
                    content = m_returnData.Substring(title.Length, m_returnData.Length - title.Length);
                                     
                    break;
                }

                de = DateTime.Now;
            }

            if ((ts = (de - ds)).TotalMilliseconds >= DELAY_TIME)
            {
                m_client.Close();
                m_client = null;

                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                m_hostIP = txtIP.Text;
                IPAddress ipA = IPAddress.Parse(m_hostIP);
                m_port = int.Parse(txtPort.Text.Trim());
                m_endPoint = new IPEndPoint(ipA, m_port);

                m_client = new UdpClient();
                m_client.Connect(m_endPoint);

                m_sendData = "ADD";
                m_data = UDPComm.UDPComm.EncodingASCII(m_sendData);
                m_client.Send(m_data, m_data.Length);

                string content=string.Empty;

                if (ReceiveData(UDPComm.ConstValue.PUBLIC_KEY, ref content))
                {
                    m_asymCrpt = new AsymCryptography();
                    m_asymCrpt.PublicKey = content;
                    content = m_asymCrpt.Encrypt(m_symCrpt.Key);
                    content = string.Format("{0}{1}", UDPComm.ConstValue.SYM_KEY,content);

                    m_data = UDPComm.UDPComm.EncodingASCII(content);
                    m_client.Send(m_data, m_data.Length);

                    if (ReceiveData(UDPComm.ConstValue.OK, ref content))
                    {
                        string time = DateTime.Now.ToString("t");
                        txtContent.Text = time + "  " + "Add sucessfully\r\n" + txtContent.Text;
                    }
                }

                if (m_client != null)
                {
                    m_receiveThread = new Thread(new ThreadStart(Listener));
                    m_receiveThread.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLeave_Click(object sender, EventArgs e)
        {
            if (m_client != null)
            {
                m_sendData = "DEL";
                m_data = UDPComm.UDPComm.EncodingASCII(m_sendData, m_symCrpt);
                m_client.Send(m_data, m_data.Length);

                m_done = true;
            }
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            btnLeave_Click(sender, e);
            Thread.Sleep(1);

            if (m_receiveThread != null)
            {
                m_receiveThread.Join(1);
            }

            this.Close();
        }

    }
}
