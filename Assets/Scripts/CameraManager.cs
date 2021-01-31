using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    public GameObject CarToFollow;

    public float fBackDistance = 10.0f;
    public float fHeightDistance = 4.0f;
    public float fAngleX = 15.0f;

    void Update()
    {
        if (CarToFollow)
        {
            Vector3 vCarLocation = CarToFollow.transform.position;
            vCarLocation -= CarToFollow.transform.up * -fBackDistance; // Using up*-1 because Car has a base rotation of -90
            vCarLocation.y += fHeightDistance;
            gameObject.transform.position = vCarLocation;

            gameObject.transform.rotation = CarToFollow.transform.rotation;
            gameObject.transform.rotation *= Quaternion.Euler(90.0f + fAngleX, 0.0f, 0.0f);
        }
    }
}
