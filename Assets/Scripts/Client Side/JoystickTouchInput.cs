using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickTouchInput : MonoBehaviour
{

    [SerializeField]
    JoystickView joystick;
    Transform joystickTransform;

    [SerializeField]
    FireButton fireButton;
    Transform fireButtonTransform;

    bool gettingInput = false;

    int joystickFingerId = -1;
    bool joystickPressed = false;

    int fireFingerId = -1;
    bool firePressed = false;

    void Start()
    {
        joystickTransform = joystick.transform;
        fireButtonTransform = fireButton.transform;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {

            // handle new or lifted touches
            foreach (Touch touch in Input.touches)
            {
                // new touch
                if (touch.phase == TouchPhase.Began)
                {
                    // if not already found the joystick finger
                    if (!joystickPressed)
                    {
                        float distance = Vector3.Distance(joystickTransform.position, touch.position);

                        // if touching joystick, this is the finger to look at
                        if (distance <= joystick.BaseRadius)
                        {
                            joystickFingerId = touch.fingerId;
                            joystickPressed = true;
                            continue; // this finger is reserved - go to next
                        }
                    }

                    // if not already presssing fire
                    if (!firePressed)
                    {
                        float distance = Vector3.Distance(fireButtonTransform.position, touch.position);

                        // if touching joystick, this is the finger to look at
                        if (distance <= fireButton.ButtonRadius)
                        {
                            fireFingerId = touch.fingerId;
                            firePressed = true;
                            continue; // this finger is reserved - go to next
                        }
                    }
                }


                if (touch.phase == TouchPhase.Moved)
                {
                    if (firePressed && touch.fingerId == fireFingerId)
                    {
                        float distance = Vector3.Distance(fireButtonTransform.position, touch.position);
                        // finger moved off of fire button
                        if (distance > fireButton.ButtonRadius)
                        {
                            firePressed = false;
                        }
                    }
                }


                if (touch.phase == TouchPhase.Ended)
                {
                    // if finger was lifted off of joystick
                    if (touch.fingerId == joystickFingerId)
                    {
                        joystickPressed = false;
                        joystick.ResetHandle();
                    }

                    if (touch.fingerId == fireFingerId)
                    {
                        firePressed = false;
                        joystick.FireReleased();
                    }
                }
            }


            // handle movement of joystick finger
            if (joystickPressed)
            {
                Vector2 position = Array.Find(Input.touches, touch => touch.fingerId == joystickFingerId).position;
                joystick.ReceiveInput(position);
            }

            if (firePressed)
            {
                joystick.FirePressed();
            }

        }
    }
}
