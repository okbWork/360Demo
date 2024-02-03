using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class sphereToRoom : MonoBehaviour
{
    public GameObject spherePlayer;
    public GameObject roomPlayer;
    public GameObject moveToNode;
    public GameObject parentNode;
    public GameObject room;
    // Start is called before the first frame update
    void Start()
    {
        roomPlayer.GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;
        roomPlayer.GetComponent<Camera>().enabled = false;
        foreach (MeshRenderer obj in room.GetComponentsInChildren<MeshRenderer>())
            obj.enabled = false;
    }
    public void OnClick()
    {
        spherePlayer.GetComponent<Camera>().GetComponent<AudioListener>().enabled = false;
        spherePlayer.GetComponent<Camera>().enabled = false;

        foreach (MeshRenderer obj in room.GetComponentsInChildren<MeshRenderer>())
            obj.enabled = true;
        roomPlayer.GetComponent<Camera>().GetComponent<AudioListener>().enabled = true;
        roomPlayer.GetComponent<Camera>().enabled = true;
        roomPlayer.transform.position = moveToNode.transform.position + moveToNode.GetComponent<VRNode>().playerOffset;
        moveToNode.GetComponent<VRNode>().SetActiveNode(true);
        parentNode.GetComponent<VRNode>().SetActiveNode(false);
    }
}
