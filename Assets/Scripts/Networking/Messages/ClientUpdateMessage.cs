using UnityEngine;

public class ClientUpdateMessage : UnityEngine.Networking.MessageBase {

    public int connectionId;

    public float rotateTo;
    public bool relativeRotation = false;

    public float speedScale = 0;

    public bool firing = false;
}
