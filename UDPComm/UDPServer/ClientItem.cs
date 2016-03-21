using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;

namespace UDPServer
{
    public class ClientItem
    {
        private IPEndPoint m_ipEndPoint=null;
        private UDPComm.SymCryptography m_symCryPt=null;
        private UDPComm.AsymCryptography m_asymCryPt = null;

        public ClientItem()
        { 
        }

        public IPEndPoint EndPoint
        {
            get
            {
                return m_ipEndPoint;
            }
            set
            {
                m_ipEndPoint = value;
            }
        }

        public UDPComm.SymCryptography SymCrypt
        {
            get
            {
                return m_symCryPt;
            }
            set
            {
                m_symCryPt = value;
            }
        }

        public UDPComm.AsymCryptography AsymCrypt
        {
            get
            {
                return m_asymCryPt;
            }
            set
            {
                m_asymCryPt = value;
            }
        }

        public override bool Equals(object obj)
        {
            return (m_ipEndPoint.ToString() == (obj as ClientItem).EndPoint.ToString());
        }

        public override int GetHashCode()
        {
            return m_ipEndPoint.GetHashCode();
        }
    }
}
