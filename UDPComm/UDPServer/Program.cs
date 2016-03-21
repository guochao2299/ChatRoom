using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

using UDPComm;
using System.Collections;

namespace UDPServer
{
    class Program
    {
        static UdpClient m_server = null;
        static ArrayList m_userList = null;

        static ClientItem FindItem(IPEndPoint rep)
        {
            ClientItem item = new ClientItem();
            item.EndPoint = rep;

            if (m_userList.Contains(item))
            {
                return (ClientItem)m_userList[m_userList.IndexOf(item)];
            }

            return null;
        }       

        static bool AddMember(IPEndPoint rep)
        {
            bool rt = false;
            ClientItem item = new ClientItem();
            item.EndPoint = rep;

            if (!m_userList.Contains(item))
            {
                rt = true;

                item.SymCrypt = new SymCryptography();
                item.AsymCrypt = new AsymCryptography();

                byte[] data = UDPComm.UDPComm.EncodingASCII(UDPComm.ConstValue.PUBLIC_KEY + item.AsymCrypt.PublicKey);
                m_server.Send(data, data.Length, item.EndPoint);

                IPEndPoint ep = null;
                byte[] buf = m_server.Receive(ref ep);
                string returnData = UDPComm.UDPComm.DecodingASCII(buf);

                if (returnData.StartsWith(UDPComm.ConstValue.SYM_KEY))
                {
                    item.SymCrypt.Key = UDPComm.UDPComm.DecodingASCII(UDPComm.UDPComm.EncodingASCIIPlus(returnData.Substring(UDPComm.ConstValue.SYM_KEY.Length)),
                        item.AsymCrypt);
                    buf=UDPComm.UDPComm.EncodingASCII(UDPComm.ConstValue.OK);
                    m_server.Send(buf, buf.Length,item.EndPoint);
                }
                else
                {
                    rt = false;
                }

                if (rt)
                {
                    m_userList.Add(item);
                }
            }

            if(!rt)
            {
                byte[] data = UDPComm.UDPComm.EncodingASCII(UDPComm.ConstValue.ERR);
                m_server.Send(data, data.Length, item.EndPoint);
            }

            return rt;
        }

        static void DelMember(IPEndPoint rep)
        {
            ClientItem item = FindItem(rep);

            if (item != null)
            {
                byte[] data = UDPComm.UDPComm.EncodingASCII("OK", item.SymCrypt);
                m_server.Send(data, data.Length, item.EndPoint);
                m_userList.Remove(item);
            }
        }

        static void SendToMember(string buf)
        {
            
            foreach (ClientItem mb in m_userList)
            {            
                byte[] data = UDPComm.UDPComm.EncodingASCII(buf,mb.SymCrypt);
                m_server.Send(data, data.Length, mb.EndPoint);
            }
        }

        static void Main(string[] args)
        {
            string m_hostIP = "127.0.0.1";
            int m_port = 6666;
            IPEndPoint m_endPoint;
            ArrayList memberList = new ArrayList();
            bool rt = false;
            byte[] data;
            string m_returnData;

            if (args.Length < 2)
            {
                Console.WriteLine("Usage:UDPServer hostIP port");
            }
            else
            {
                m_hostIP = args[0].ToString();
                m_port = int.Parse(args[1].ToString());
                rt = true;
            }

            if (rt)
            {
                m_userList = new ArrayList();
                IPAddress m_ipA = IPAddress.Parse(m_hostIP);
                m_endPoint = new IPEndPoint(m_ipA, m_port);
                m_server = new UdpClient(m_endPoint);

                Console.WriteLine("Ready for Connect......");

                while (true)
                {
                    data = m_server.Receive(ref m_endPoint);
                    m_returnData = UDPComm.UDPComm.DecodingASCII(data);

                    if ((m_returnData.IndexOf("ADD") > -1) &&
                        (AddMember(m_endPoint)))
                    {                        
                        Console.WriteLine(m_endPoint.ToString() + " has added to group!");
                    }
                    else
                    {
                        m_returnData = UDPComm.UDPComm.DecodingASCII(data, FindItem(m_endPoint).SymCrypt);

                        if (m_returnData.IndexOf("DEL") > -1)
                        {
                            DelMember(m_endPoint);
                            Console.WriteLine(m_endPoint.ToString() + " has deleted from group!");
                        }
                        else
                        {
                            if (FindItem(m_endPoint) != null)
                            {
                                SendToMember(m_returnData + "[" + m_endPoint.ToString() + "]");
                                Console.WriteLine(m_returnData + "[" + m_endPoint.ToString() + "]" +
                                    " has resented to members!");
                            }
                        }
                    }
                }

                m_server.Close();
            }
        }
    }
}
