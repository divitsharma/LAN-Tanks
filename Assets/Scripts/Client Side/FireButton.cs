using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireButton : MonoBehaviour {


    [SerializeField]
    ControllerManager manager;
    [SerializeField]
    Canvas canvas;


    float buttonRadius;
    public float ButtonRadius
    {
        get { return buttonRadius; }
    }

    // Use this for initialization
    void Start () {
		
        buttonRadius = GetComponent<RectTransform>().rect.width * canvas.scaleFactor / 2f;

    }

}
