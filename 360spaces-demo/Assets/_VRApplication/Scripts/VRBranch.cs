using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// The image branch node is how users move between nodes, 
/// this class defines the selection sphere that is interacted by raycast interactable on the user XR rig
/// </summary>
public class VRBranch : MonoBehaviour
{
    public GameObject parentNode;
    public GameObject moveToNode;
    public GameObject player;

    public Canvas infoCanvas;

    private void Start()
    {
        if(infoCanvas!=null)
            infoCanvas.worldCamera = Camera.main;
        parentNode = transform.parent.gameObject;
        HideInfo();
    }
    private void Update()
    {
        transform.LookAt(player.transform.position);
    }
    public void OnClick()
    {
        Debug.Log("moving player");
        player.transform.position = moveToNode.transform.position + moveToNode.GetComponent<VRNode>().playerOffset;

        moveToNode.GetComponent<VRNode>().SetActiveNode(true);
        parentNode.GetComponent<VRNode>().SetActiveNode(false);
    }
    public void InfoPanel()
    {
        if (infoCanvas != null)
        {
            infoCanvas.enabled = true;
            infoCanvas.GetComponent<Canvas>().enabled = true;
        }
    }
    public void HideInfo()
    {
        if (infoCanvas != null)
            infoCanvas.GetComponent<Canvas>().enabled = false;
    }

    private void OnDrawGizmos()
    {
        if (moveToNode == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, moveToNode.transform.position);
    }
}
