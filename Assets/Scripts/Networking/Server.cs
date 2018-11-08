using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;




public class Server : MonoBehaviour {

    const int MAX_CONNECTIONS = 100;

    public static int serverPort = 5701;
    int localHostId;

    int reliableChannel;
    int unreliableChannel;

    bool serverSetUp = false;
    byte error;

    public static int broadcastKey = 66778283;
    public static int broadcastVersion = 1;
    public static int broadcastSubversion = 0;
    public static int broadcastFrequency = 1000; //Broadcast every 1000ms


    // events for connection statuses (not connection messages)
    public event System.Action<int> OnNetworkConnectEvent;
    public event System.Action<int> OnNetworkDisconnectEvent;



    void Start()
    {
        OnNetworkConnectEvent += HandleNetworkConnectEvent;

        // init and config transport layer
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        // number of connections and types of channels for this particular host/socket
        HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);
        localHostId = NetworkTransport.AddHost(topo, serverPort, null); // null means accept from everyone

        // prepare to receive broadcasts from clients
        NetworkTransport.SetBroadcastCredentials(localHostId, broadcastKey, broadcastVersion, broadcastSubversion, out error);
        if ((NetworkError)error != NetworkError.Ok)
            Debug.LogError("Error setting network broadcast credentials: " + (NetworkError)error);
        else
            Debug.Log("Successfully set broadcast receive credentials");

        serverSetUp = true;
    }

    // listening
    void Update()
    {
        if (!serverSetUp) return;

        // information about message received
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.ReceiveFromHost(localHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.ConnectEvent:       // when a new connection is made
                Debug.Log("Player " + connectionId + " has connected.");
                OnNetworkConnectEvent(connectionId);
                break;

            case NetworkEventType.DataEvent:
                NetworkUtility.HandleNetworkDataEvent(recBuffer, dataSize);
                break;

            case NetworkEventType.DisconnectEvent:
                // need to remove from things on disconnect
                Debug.Log("Player " + connectionId + " has disconnected.");
                OnNetworkDisconnectEvent(connectionId);
                break;

            case NetworkEventType.BroadcastEvent:
                Debug.Log("Received broadcast event.");
                HandleNetworkBroadcastEvent();
                break;
        }
    }

    void HandleNetworkConnectEvent(int connectionId)
    {
        // add new client to a list (in manager)

        Debug.Log("Sending connect message back to client");
        // tell client that they are connected and give them the connection id
        ConnectMessage cm = new ConnectMessage();
        cm.connectionId = connectionId;
        NetworkUtility.Send(EMessageType.Connect, cm, localHostId, reliableChannel, connectionId);
        // also notify observers that a new connection has been made
    }


    void HandleNetworkBroadcastEvent()
    {
        string broadcasterIP = "";
        int broadcasterPort = -1;
        byte broadcastConnectionInfoError;

        NetworkTransport.GetBroadcastConnectionInfo(localHostId, out broadcasterIP, out broadcasterPort, out broadcastConnectionInfoError);
        int connectionId = NetworkTransport.Connect(localHostId, broadcasterIP, broadcasterPort, 0, out error);

        if ((NetworkError)error != NetworkError.Ok)
            Debug.LogError("ConnectTo error: " + (NetworkError)error);
        else
            Debug.Log("Successfully connected to broadcaster at " + broadcasterIP);
    }


}
