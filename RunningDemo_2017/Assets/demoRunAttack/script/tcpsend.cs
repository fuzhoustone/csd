using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;


public class tcpsend : MonoBehaviour {
    private Socket m_socket;
    private static int bufferSize = 1024;
    private byte[] buffer = new byte[bufferSize];
    private List<byte> receiveBuffer = new List<byte>(bufferSize);

   // private Socket m_socket;
    private string m_serverIP;
    private ushort m_serverPort;

    public void Connect(string ip, ushort port)
    {
        m_serverIP = ip;
        m_serverPort = port;

        ConnectToServerv6();
    }

    public void Close()
    {
        if (m_socket == null)
        {
            return;
        }

        m_socket.Close();
        m_socket = null;
    }

    private void ConnectToServerv4()
    {

        try
        {
            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            m_socket.BeginConnect(m_serverIP, (int)m_serverPort, ConnectResultV4, m_socket);
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            if (m_socket != null)
            {
                m_socket.Close();
                m_socket = null;
            }
        }
    }

    private void ConnectToServerv6()
    {

        try
        {
            m_socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);

            m_socket.BeginConnect(m_serverIP, (int)m_serverPort, ConnectResultV6, m_socket);
        }
        catch (Exception e)
        {
            Debug.Log("ConnectToServerv6 Fail. IPV6 not supported.");

            if (m_socket != null)
            {
                m_socket.Close();
                m_socket = null;
            }
            ConnectToServerv4();
        }
    }

    private void ConnectResultV4(IAsyncResult asyncResult)
    {
        var socket = (Socket)asyncResult.AsyncState;

        try
        {
            if (!socket.Connected) throw new Exception("ConnectFailed IPV4");

            //socket.EndConnect(asyncResult);
            //   m_packetReceiver.Start();
            m_socket.EndSend(asyncResult);
            StartReceived();
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            socket.Close();
        }
    }

    private void ConnectResultV6(IAsyncResult asyncResult)
    {
        var socket = (Socket)asyncResult.AsyncState;

        try
        {
            if (!socket.Connected) throw new Exception("ConnectFailed IPV6");

            //   socket.EndConnect(asyncResult);
            //  m_packetReceiver.Start();
            m_socket.EndSend(asyncResult);
            StartReceived();
        }
        catch (Exception e)
        {
            Debug.Log("ConnectResultV6 Fail. IPV6 not supported.");
            socket.Close();

            ConnectToServerv4();
        }
    }

    /*
    //建立连接
    public void SocketConnect(string serverIP, ushort port, Action onConnected)
    {
        //IPAddress ipAddress = Dns.GetHostEntry(server).AddressList[0];
        //IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, port);
       
        m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        m_socket.NoDelay = true;

        Debug.Log("[Socket start connecting]");
        try {
            m_socket.BeginConnect
        (
            m_serverIP,
            (IAsyncResult ar) =>
            {
                Debug.Log("[Socket connected]");

                m_socket.EndSend(ar);
                onConnected();
                StartReceived();
            },
            null // no need
        );
        }

        catch(Exception e)
        {

        }
        
    }
    */

    private void StartReceived()
    {
            m_socket.BeginReceive
            (
                buffer,
                0,
                bufferSize,
                SocketFlags.None,
                Received,
                null
            );
    
    }

	private void Received(IAsyncResult ar)
    {
        int read = m_socket.EndReceive(ar);

        if (read > 0)
        {
            byte[] bytes = new byte[read];
            Buffer.BlockCopy(buffer, 0, bytes, 0, read);
            receiveBuffer.AddRange(bytes);
        }

        if (receiveBuffer.Count > 4)
        {
            byte[] lenBytes = receiveBuffer.GetRange(0, 4).ToArray();
            int len = IPAddress.HostToNetworkOrder(BitConverter.ToInt32(lenBytes, 0));

            // one protocol data received
            if (receiveBuffer.Count - 4 >= len)
            {
                byte[] dataBytes = receiveBuffer.GetRange(4, len).ToArray();
            }
            else
            {
                // protocol data not complete 
            }
        }

        // continue to receive listen
        StartReceived();
    }


}
