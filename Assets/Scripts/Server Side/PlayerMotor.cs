using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMotor : MonoBehaviour {

    Rigidbody2D rb;

    [SerializeField]
    float manualRotation = 0;

    [SerializeField]
    float maxSpeed = 0; // units per s
    [SerializeField]
    public float lerpFactor;

    float sSinceUpdate = 0;
    Vector2 velocity;
    float nextRotation;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        velocity = Vector3.zero;
        nextRotation = rb.rotation;
	}
	
    void FixedUpdate()
    {
        //RotateTo(manualRotation);
    }

    void Update()
    {
        sSinceUpdate += Time.deltaTime;
        // unclamped, so will keep going forever when no data is received
        rb.MovePosition(rb.position + (velocity * Time.deltaTime));
        rb.rotation = Mathf.LerpAngle(rb.rotation, nextRotation, lerpFactor * sSinceUpdate);//needs to change with time!
    }

    //public void Move(ClientUpdateMessage message)
    //{
    //    if (message.relativeRotation)
    //        RelativeRotateTo(message.rotateTo);
    //    else
    //        RotateTo(message.rotateTo);

    //    AddVelocity(message.speedScale);
    //}


    public void RotateTo(float rotateTo)
    {
        nextRotation = rotateTo;
    }

    void RelativeRotateTo(float rotateTo)
    {
        rb.rotation = Mathf.LerpAngle(rb.rotation, rb.rotation + rotateTo, lerpFactor * Time.deltaTime);
    }

    public void AddVelocity(float speedScale)
    {
        sSinceUpdate = 0;
        velocity = transform.up.normalized * maxSpeed * speedScale;
        // next position to be in after 1 second
        //nextPosition = transform.position + velocity;
        
    }

}
