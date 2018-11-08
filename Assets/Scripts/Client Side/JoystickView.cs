using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickView : MonoBehaviour {

    [SerializeField]
    Canvas canvas;

    [SerializeField]
    RectTransform stickHandle;

    private float baseRadius;
    public float BaseRadius
    {
        get { return baseRadius; }
    }

    public bool Firing
    {
        get { return firing; }
    }

    // "model" fields
    // relative position of handle from center
    Vector3 handlePosition = Vector3.zero;
    // angle must be kept, since it can't be inferred from 0 position
    float angle = 0f;
    bool firing = false;

    // Use this for initialization
    void Start () {
        baseRadius = GetComponent<RectTransform>().rect.width * canvas.scaleFactor / 2f;
        Debug.Log(baseRadius);
    }
	
    void updateHandle()
    {
        stickHandle.position = transform.position + handlePosition;
    }

    public void ReceiveInput(Vector3 inputPos)
    {
        //Debug.Log(inputPos.x + " " + inputPos.y);

        // calculate clamped handle position
        handlePosition = inputPos - transform.position;
        if (handlePosition.sqrMagnitude > BaseRadius * BaseRadius) {
            handlePosition = handlePosition.normalized * baseRadius;
        }

        angle = Vector3.Angle(handlePosition, Vector3.up);
        if (handlePosition.x > 0) angle *= -1;

        updateHandle();
    }

    // query model
    public float getDistance()
    {
        Debug.Log("scale: " + handlePosition.magnitude);
        return handlePosition.magnitude;
    }
    public float getAngle()
    {
        Debug.Log("Angle: " + angle);
        return angle;
    }

    // should be on button down
    public void FirePressed()
    {
        Debug.Log("Fire pressed");
        firing = true;
    }
    public void FireReleased()
    {
        firing = false;
    }

    public void ResetHandle()
    {
        handlePosition = Vector3.zero;
        updateHandle();
    }

}
