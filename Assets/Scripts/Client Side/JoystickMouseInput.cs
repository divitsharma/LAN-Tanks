using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// provides mouse input to joystick
public class JoystickMouseInput : MonoBehaviour {


    JoystickView joystick;

    [SerializeField]
    FireButton fireButton;
    Transform fireButtonTransform;

    bool gettingInput = false;
    bool firing = false;

    // Use this for initialization
    void Start () {
        joystick = GetComponent<JoystickView>();
        fireButtonTransform = fireButton.transform;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetMouseButtonDown(0)) {
            float distance = Vector3.Distance(transform.position, Input.mousePosition);
            if (distance <= joystick.BaseRadius)
                gettingInput = true;
            else if (Vector3.Distance(fireButtonTransform.position, Input.mousePosition) <= fireButton.ButtonRadius)
                firing = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            gettingInput = false;
            firing = false;
            joystick.ResetHandle();
            joystick.FireReleased();
        }

        if (gettingInput)
        {
            joystick.ReceiveInput(Input.mousePosition);
        }

        if (firing)
        {
            joystick.FirePressed();
        }
    }
}
