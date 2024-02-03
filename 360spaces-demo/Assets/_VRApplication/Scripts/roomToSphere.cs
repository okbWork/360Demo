using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomToSphere : MonoBehaviour
{
    public GameObject spherePlayer;
    public GameObject roomPlayer;
    public GameObject moveToNode;
    public GameObject parentNode;
    public GameObject room;
    public void OnClick()
    {
        spherePlayer.GetComponent<Camera>().enabled = true;
        roomPlayer.GetComponent<Camera>().enabled = false;
        foreach (MeshRenderer obj in room.GetComponentsInChildren<MeshRenderer>())
            obj.enabled = false;
        spherePlayer.transform.position = moveToNode.transform.position + moveToNode.GetComponent<VRNode>().playerOffset;
        moveToNode.GetComponent<VRNode>().SetActiveNode(true);
        parentNode.GetComponent<VRNode>().SetActiveNode(false);
    }
}
