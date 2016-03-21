using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Windows.Forms;

using UDPComm;

namespace UDPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            //string m_hostIP = "10.6.100.128";
            //int m_port = 6666;
            //UdpClient m_client;
            //byte[] m_data;
            //string m_sendData, m_returnData;
            //IPEndPoint m_endPoint;

            //IPAddress m_ipA = IPAddress.Parse(m_hostIP);
            //m_endPoint = new IPEndPoint(m_ipA, m_port);
            //m_client = new UdpClient();
            //m_client.Connect(m_endPoint);

            //while (true)
            //{
            //    Console.WriteLine("Input [ADD|DEL|REF|QUIT|Message]:");
            //    m_sendData = Console.ReadLine();

            //    if (m_sendData.IndexOf("QUIT") > -1)
            //    {
            //        m_sendData = "DEL";
            //    }

            //    if (m_sendData.IndexOf("REF") <= -1)
            //    {
            //        m_data = UDPComm.UDPComm.EncodingASCII(m_sendData);
            //        m_client.Send(m_data, m_data.Length);
            //    }

            //    if (m_sendData.IndexOf("QUIT") > -1)
            //    {
            //        break;
            //    }

            //    m_data = m_client.Receive(ref m_endPoint);
            //    m_returnData = UDPComm.UDPComm.DecodingASCII(m_data);
            //    Console.WriteLine(m_returnData);
            //}

            //Console.WriteLine("Byte!");
            //m_client.Close();
        }
    }
}
