using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This allows users to drag the branch node in order to place manually while in setup mode
/// </summary>
public class DragBranch : MonoBehaviour
{
    public XRRayInteractor rayInteractor;
    private bool dragable;
    float distance = 10f;
    private void Update()
    {
        if (dragable)
        {
            // you can modify the distance here if you want to place the object on the image better
            // a better option would be to just modify the scale, to give the illusion of distance

            Vector3 attachTransformForward = rayInteractor.attachTransform.forward;
            Vector3 rotatedForward = rayInteractor.transform.TransformDirection(attachTransformForward);
            Ray ray = new Ray(rayInteractor.transform.position, rotatedForward);

            // add the preset distance
            Vector3 targetPosition = ray.GetPoint(distance);

            // Move the gameobject to the target position
            transform.position = targetPosition;

        }
    }

    public void StartDrag()
    {
        dragable = true;
    }
    public void StopDrag()
    {
        dragable = false;
    }
}
