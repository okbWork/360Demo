using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The image node is the sphere in which the user looks around
/// Spheres are linked together by branches, which the user can interact with
/// </summary>
public class VRNode : MonoBehaviour
{
    List<MeshRenderer> meshRenderers;
    public int id;
    public Vector3 playerOffset;

    public string imgPath;

    public bool startingNode = false;

    private void Start()
    {
        UpdateRenderers();
        SetActiveNode(startingNode);
    }
    public void SetActiveNode(bool isActive)
    {
        if (meshRenderers == null)
            return;
        foreach(MeshRenderer renderer in meshRenderers)
            renderer.enabled = isActive;
    }
    public void UpdateRenderers()
    {
        meshRenderers = new List<MeshRenderer>
        {
            GetComponent<MeshRenderer>()
        };
        foreach (Transform child in transform)
            meshRenderers.Add(child.GetComponent<MeshRenderer>());

    }
    public void AddRenderer(MeshRenderer renderer)
    {
        meshRenderers.Add(renderer);
    }

}
