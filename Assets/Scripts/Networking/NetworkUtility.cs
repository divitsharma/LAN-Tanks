using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public enum EMessageType
{
    Connect = 0,
    ClientUpdate,
    Fire
}


// In charge of operations common to both server and client
public class NetworkUtility : MonoBehaviour {

    public const string clientObjectName = "Client";

    static Dictionary<EMessageType, MessageBase> typeToInstance = new Dictionary<EMessageType, MessageBase>()
    {
        { EMessageType.Connect, new ConnectMessage() },
        { EMessageType.ClientUpdate, new ClientUpdateMessage() },
        { EMessageType.Fire, new FireMessage() }
    };

    // events to let other classes handle network messages
    static event Action<MessageBase> OnConnectMessage; // is an instance of the delegate
    static event Action<MessageBase> OnClientUpdateMessage;
    static event Action<MessageBase> OnFireMessage;

    public static Dictionary<EMessageType, Action<MessageBase>> messageDelegates = new Dictionary<EMessageType, Action<MessageBase>>()
    {
        { EMessageType.Connect, OnConnectMessage },
        { EMessageType.ClientUpdate, OnClientUpdateMessage },
        { EMessageType.Fire, OnFireMessage }
    };



    public static void Send(EMessageType type, MessageBase message, int hostId, int channelId, int connId)
    {
        // serialize the message, prepend the message type
        NetworkWriter writer = new NetworkWriter();
        writer.Write((short)type);
        message.Serialize(writer);

        byte error;
        byte[] msgBytes = writer.AsArray();
        NetworkTransport.Send(hostId, connId, channelId, msgBytes, msgBytes.Length * sizeof(char), out error);

    }

    public static void HandleNetworkDataEvent(byte[] dataBuffer, int dataSize)
    {
        NetworkReader reader = new NetworkReader(dataBuffer);
        EMessageType type = (EMessageType)reader.ReadInt16();
        Debug.Log("Received message type " + (int)type);

        // things that change on adding a new message:
        // add to types, create new event, create new case to launch that event

        // without switching for type:
        // deserialize the messagebase = [type, inst]

        MessageBase msg = typeToInstance[type];
        msg.Deserialize(reader);
        messageDelegates[type](msg);

        // deserialize and raise events based on message type
        //switch (type)
        //{
        //    case EMessageType.Connect:
        //        ConnectMessage cMsg = new ConnectMessage();
        //        cMsg.Deserialize(reader);

        //        // raise the event
        //        //OnConnectMessage(cMsg);
        //        dels[0](cMsg);
        //        break;
        //}
    }

}
