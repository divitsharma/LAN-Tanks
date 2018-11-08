using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Server))]
public class ServerManager : MonoBehaviour {

    Dictionary<int, Player> players = new Dictionary<int, Player>();

    Server server;

    [SerializeField]
    GameObject playerPrefab;


	// Use this for initialization
	void Start () {
        server = GetComponent<Server>();

        server.OnNetworkConnectEvent += HandleNetworkConnectEvent;
        server.OnNetworkDisconnectEvent += HandleNetworkDisconnectEvent;

        NetworkUtility.messageDelegates[EMessageType.ClientUpdate] += HandleClientUpdateMessage;
        //NetworkUtility.messageDelegates[EMessageType.Fire] += HandleFireMessage;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void HandleNetworkConnectEvent(int connectionId)
    {
        // spawn new player
        GameObject newPlayer = Instantiate(playerPrefab);
        players.Add(connectionId, newPlayer.GetComponent<Player>());

    }

    void HandleNetworkDisconnectEvent(int connectionId)
    {
        Destroy(players[connectionId].gameObject);
        players.Remove(connectionId);
    }

    void HandleClientUpdateMessage(MessageBase msgBase)
    {
        Debug.Log("Receiving client update");
        ClientUpdateMessage message = (ClientUpdateMessage)msgBase;
        players[message.connectionId].UpdateFromClient(message);
    }

    //void HandleFireMessage(MessageBase msgBase)
    //{
    //    Debug.Log("Receiving fire message");
    //    FireMessage message = (FireMessage)msgBase;
    //    players[message.connectionId].Fire();
    //}
}
