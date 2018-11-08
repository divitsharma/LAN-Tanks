using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Client : MonoBehaviour {

    const int MAX_CONNECTIONS = 100;


    int reliableChannel;
    int unreliableChannel;

    int localHostId;
    int myConnectionId;
    bool hasStarted = false;
    float connectionTime;

    byte error;

    Text debugText;


    // when server disconnects
    public event System.Action OnNetworkDisconnectEvent;



    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        // THE CONNECT BUTTON ONCLICK ISNT REGISTERED DUH

        debugText = GameObject.Find("DebugLabel").GetComponent<Text>();

        OnNetworkDisconnectEvent += HandleNetworkDisconnectEvent;
        NetworkUtility.messageDelegates[EMessageType.Connect] += HandleConnectMessage;

        // init and config transport layer
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);
        unreliableChannel = cc.AddChannel(QosType.Unreliable);

        // add this client host
        HostTopology topo = new HostTopology(cc, MAX_CONNECTIONS);
        localHostId = NetworkTransport.AddHost(topo, 0); // 0 means assign it a random port
    }


    // called on connect button click
    public void Connect()
    {
        // broadcast my info for server to recieve and connect to
        if (NetworkTransport.IsBroadcastDiscoveryRunning())
        {
            if (debugText) debugText.text = "Will not StartBroadcast(): Broadcast discovery is already running";
            return;
        }

        hasStarted = NetworkTransport.StartBroadcastDiscovery(
                        localHostId, Server.serverPort, Server.broadcastKey, Server.broadcastVersion,
                        Server.broadcastSubversion, null, 0, Server.broadcastFrequency, out error);

        if (!hasStarted)
            if (debugText) debugText.text = "Failed to start Network Broadcast Discovery";
        else
            if (debugText) debugText.text = "Network Broadcast Discovery started targeting port: " + Server.serverPort;
        
    }


    void Update()
    {
        if (!hasStarted) return;

        // Listen for network messages
        int recHostId;
        int connectionId;
        int channelId;
        byte[] recBuffer = new byte[1024];
        int bufferSize = 1024;
        int dataSize;
        byte error;
        NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
        switch (recData)
        {
            case NetworkEventType.DisconnectEvent:
                OnNetworkDisconnectEvent();
                break;

            case NetworkEventType.DataEvent:
                NetworkUtility.HandleNetworkDataEvent(recBuffer, dataSize);                
                break;
        }
    }

    public void SendClientUpdateMessage(ClientUpdateMessage message)
    {
        message.connectionId = myConnectionId;
        NetworkUtility.Send(EMessageType.ClientUpdate, message, localHostId, unreliableChannel, myConnectionId);
    }

    public void SendFireMessage(FireMessage message)
    {
        message.connectionId = myConnectionId; // make this deprecated
        NetworkUtility.Send(EMessageType.Fire, message, localHostId, unreliableChannel, myConnectionId);
    }



    void HandleConnectMessage(MessageBase msg)
    {
        ConnectMessage cMsg = (ConnectMessage)msg;

        NetworkTransport.StopBroadcastDiscovery();
        myConnectionId = cMsg.connectionId;
        if (debugText) debugText.text = "We have connection id " + myConnectionId;
    }

    void HandleNetworkDisconnectEvent()
    {
        if (debugText)
            debugText.text = "Disconnected";
        hasStarted = false;
        // the other variables are now invalid, shouldn't be used without calling connect first
    }


}
