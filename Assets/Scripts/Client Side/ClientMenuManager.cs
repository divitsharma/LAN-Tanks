using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientMenuManager : MonoBehaviour {

    [SerializeField]
    GameObject clientObject;

    Client client;

	// Use this for initialization
	void Start () {

        GameObject foundClientObject = GameObject.Find(NetworkUtility.clientObjectName);
        if (foundClientObject)
            client = foundClientObject.GetComponent<Client>();
        else
        {
            client = Instantiate(clientObject).GetComponent<Client>();
            client.gameObject.name = NetworkUtility.clientObjectName;
        }
        NetworkUtility.messageDelegates[EMessageType.Connect] += HandleConnectMessage;

    }

    public void OnConnectButton()
    {
        client.Connect();
    }

    void HandleConnectMessage(UnityEngine.Networking.MessageBase msg)
    {
        SceneManager.LoadScene("ClientController");
    }
}
