using UnityEngine;
using Unity.Netcode;
using Steamworks;
using UnityEditor.MemoryProfiler;
using System.Runtime.InteropServices;
using System.Text;
using System;
using System.IO;
using System.Net.Sockets;
using System.Net;


public class CustomNetworkManager : NetworkManager
{
    bool connectionAttempted = false;

    private string logFilePath;

    private SNetListenSocket_t listenSocket;

    private HSteamListenSocket listenSocketP2P;

    private HSteamNetConnection connection;

    bool bAccept = false;

    private Callback<SteamNetConnectionStatusChangedCallback_t> steamNetConnectionStatusChangedCallback;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SteamNetworkingUtils.InitRelayNetworkAccess();

        logFilePath = Path.Combine(Application.persistentDataPath, "game_log.txt");

        IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
        SteamIPAddress_t ipAddr = new SteamIPAddress_t(ipAddress);

        listenSocket = SteamNetworking.CreateListenSocket(0, new SteamIPAddress_t(), 0, true);
        listenSocketP2P = SteamNetworkingSockets.CreateListenSocketP2P(0, 0, null);

        steamNetConnectionStatusChangedCallback = Callback<SteamNetConnectionStatusChangedCallback_t>.Create(OnSteamNetConnectionStatusChangedCallback);
    }

    // Update is called once per frame
    void Update()
    {
        if (connectionAttempted)
        {
            // ���� ���¸� Ȯ��
            SteamNetConnectionInfo_t connectionState;
            bool result = SteamNetworkingSockets.GetConnectionInfo(connection, out connectionState);


            if (result)
            {
                if (connectionState.m_eState == ESteamNetworkingConnectionState.k_ESteamNetworkingConnectionState_Connected)
                {
                    System.IO.File.AppendAllText(logFilePath, $"\nConnected : {connection.ToString()}");

                    System.IO.File.AppendAllText(logFilePath, $"\nconnectionAttempted To False");

                    connectionAttempted = false;
                }
                else
                {
                    System.IO.File.AppendAllText(logFilePath, $"\nConnect Fail : {connectionState.m_eState.ToString()}");
                }
            }
        }

        if (SteamNetworking.IsDataAvailable(listenSocket, out uint pcubMsgSize, out SNetSocket_t phSocket))
        {
            System.IO.File.AppendAllText(logFilePath, $"\nIsDataAvailable : {phSocket}");
        }

        //// ���� ���Ͽ� �����͸� ���� ���۸� �غ�
        //IntPtr[] messages = new IntPtr[10]; // �ִ� 10���� �޽���
        //int messageCount = 10;  // �ִ� �޽��� ����
        //// �޽����� �ޱ� ���� ȣ��
        //int receivedCount = SteamNetworkingSockets.ReceiveMessagesOnConnection(listenSocketP2P, messages, messageCount);

        //if (receivedCount > 0)
        //{
        //    for (int i = 0; i < receivedCount; i++)
        //    {
        //        // �޽����� ���� ��� ó��
        //        // �޽��� �����ʹ� IntPtr�̹Ƿ� �̸� ����Ʈ �迭 ������ ��ȯ�ؼ� ����ؾ� ��
        //        byte[] messageData = ConvertIntPtrToByteArray(messages[i]);
        //        string message = System.Text.Encoding.UTF8.GetString(messageData);
        //        Console.WriteLine("Received Message: " + message);
        //    }
        //}
        //else if (receivedCount < 0)
        //{
        //    // ���� ó��
        //    Console.WriteLine("Error receiving messages.");
        //}


        //ReceiveP2PMessagesSteamP2P();
    }

    void OnSteamNetConnectionStatusChangedCallback(SteamNetConnectionStatusChangedCallback_t result)
    {
        SteamNetworkingSockets.AcceptConnection(result.m_hConn);

        System.IO.File.AppendAllText(logFilePath, $"\nm_addrRemote : {result.m_info.m_addrRemote}, m_hListenSocket : {result.m_info.m_hListenSocket.ToString()}");
    }

    public void ConnectToHostSteamP2P(CSteamID hostID)
    {
        SteamNetworkingIdentity identityRemote = new SteamNetworkingIdentity();
        identityRemote.SetSteamID(hostID);

        // SteamNetworkingSockets P2P ���� �õ�
        connection = SteamNetworkingSockets.ConnectP2P(ref identityRemote, 0, 0, null);        

        connectionAttempted = true;
    }

    void SendMessageSteamP2P(string message)
    {
        if (connection != HSteamNetConnection.Invalid)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);

            // �޽����� IntPtr�� ��ȯ
            IntPtr messagePtr = Marshal.AllocHGlobal(messageBytes.Length);
            Marshal.Copy(messageBytes, 0, messagePtr, messageBytes.Length);

            // �޽��� ��ȣ ����� ���� �迭
            long[] result = new long[1];

            // �޽��� ����
            SteamNetworkingSockets.SendMessages(1, new IntPtr[] { messagePtr }, result);

            // ��� ���
            Debug.Log($"Message sent, result: {result[0]}");

            // �޸� ����
            Marshal.FreeHGlobal(messagePtr);
        }
    }

    void ReceiveP2PMessagesSteamP2P()
    {
        if (connection != HSteamNetConnection.Invalid)
        {
            IntPtr[] messages = new IntPtr[1];  // ������ �޽����� �ִ� ����
            int maxMessages = 1; // �ִ� �޽��� ����
            int messagesReceived = SteamNetworkingSockets.ReceiveMessagesOnConnection(connection, messages, maxMessages);

            if (messagesReceived > 0)
            {
                for (int i = 0; i < messagesReceived; i++)
                {
                    byte[] receivedData = new byte[1024]; // ���� ���� ũ��
                    Marshal.Copy(messages[i], receivedData, 0, receivedData.Length);
                    string receivedMessage = Encoding.UTF8.GetString(receivedData);
                    Debug.Log($"Received message: {receivedMessage}");
                }
            }
        }
    }
}
