using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonInput : MonoBehaviour
{
    public float sensitivity = 100;
    public Camera mainCam;

    void Update()
    {
        FreeLook();
    }

    private void FreeLook()
    {
        float mouseX = UnityEngine.Input.GetAxis("Mouse X");
        float mouseY = UnityEngine.Input.GetAxis("Mouse Y");

        mainCam.transform.Rotate(Vector3.up, mouseX * sensitivity * Time.deltaTime, Space.World);
        mainCam.transform.Rotate(Vector3.right, -mouseY * sensitivity * Time.deltaTime, Space.Self);
    }
}
