using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform startLocation;

    public void UpdateStartLoction(Transform newStartLocation) {
        startLocation = newStartLocation;
        transform.position = new Vector3 (startLocation.position.x, startLocation.position.y, transform.position.z);
    }
}
