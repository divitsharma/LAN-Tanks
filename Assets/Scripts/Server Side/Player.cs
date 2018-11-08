using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(BasicGun))] // rn doing this, make guns prefabs later
public class Player : MonoBehaviour {

    PlayerMotor motor;
    Gun gun;

	// Use this for initialization
	void Start () {
        motor = GetComponent<PlayerMotor>();
        gun = GetComponent<BasicGun>();
    }

    public void UpdateFromClient(ClientUpdateMessage msg)
    {
        motor.RotateTo(msg.rotateTo);
        motor.AddVelocity(msg.speedScale);
        if (msg.firing) gun.Fire();
    }

}
