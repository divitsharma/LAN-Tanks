using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {


    Client client;

    [SerializeField]
    JoystickView joystick;

    float timer = 0;
    float sendRate = 50; //ms

	void Start () {
        GameObject cObj = GameObject.Find(NetworkUtility.clientObjectName);
        if (cObj)
        {
            client = cObj.GetComponent<Client>();
            client.OnNetworkDisconnectEvent += HandleNetworkDisconnectEvent;
        }
    }

    void Update()
    {
        timer += Time.deltaTime * 1000;
        if (timer >= sendRate)
        {
            timer = 0;
            onNetworkTimerElapsed();
        }
    }


    void HandleNetworkDisconnectEvent()
    {
        SceneManager.LoadScene("Client");
    }

    // generate clientupdatemessage and send it to server
    void onNetworkTimerElapsed()
    {
        //Debug.Log("timer fired");
        if (!client || !joystick) return;

        ClientUpdateMessage message = new ClientUpdateMessage();
        message.rotateTo = joystick.getAngle();
        message.speedScale = Mathf.Clamp01(joystick.getDistance() / joystick.BaseRadius);
        message.firing = joystick.Firing;

        // send a message every 100ms
        client.SendClientUpdateMessage(message);
    }
    

}
