using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardController : MonoBehaviour {

    Client client;

    [SerializeField]
    float rotationIncrement = 50;

	// Use this for initialization
	void Start () {

        client = GameObject.Find(NetworkUtility.clientObjectName).GetComponent<Client>();

    }
	
	// Update is called once per frame
	void Update () {

        CalculateAndSendMovement(
            Input.GetKey(KeyCode.UpArrow),
            Input.GetKey(KeyCode.RightArrow),
            Input.GetKey(KeyCode.LeftArrow),
            Input.GetKey(KeyCode.DownArrow)
            );
		
	}

    void CalculateAndSendMovement(bool upPressed, bool rightPressed, bool leftPressed, bool downPressed)
    {
        if (!(upPressed || rightPressed || leftPressed || downPressed)) return;

        ClientUpdateMessage msg = new ClientUpdateMessage();
        msg.speedScale = upPressed ? 1 : 0;
        msg.speedScale -= downPressed ? 1 : 0;

        msg.relativeRotation = true;
        msg.rotateTo = 0;
        if (rightPressed)
            msg.rotateTo -= rotationIncrement;
        if (leftPressed)
            msg.rotateTo += rotationIncrement;

        client.SendClientUpdateMessage(msg);
    }
}
