using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// This class loads a previously saved tree or layout of UINodes from the Setup Application and creates the layout in 3D space for the UserExperience Application
/// </summary>
public class LoadNavTree : MonoBehaviour
{
    public Transform nodesParent;
    public GameObject pfNode, pfMoveButton;
    public GameObject player;

    private void Start()
    {
        LoadFile();
    }
    public void LoadFile()
    {
        string savedLine;
        int lnCount = 0;

        string loadedMapPath = (PlayerPrefs.GetString("mapPath"));
        if(loadedMapPath == "") { Debug.LogError("LoadedMapPath not found in playerprefs"); }

        Dictionary<int, GameObject> idDict = new Dictionary<int, GameObject>();
        System.IO.StreamReader file = new System.IO.StreamReader(loadedMapPath);
        while ((savedLine = file.ReadLine()) != null)
        {
            lnCount++;
            if (lnCount == 1) { continue; } //lnCount 1 is only header info, not important
            //Debug.Log(savedLine); //print out saved line
            List<string> currentLine = savedLine.Split(',').ToList<string>();

            // create a new node
            GameObject newNode = Instantiate(pfNode, nodesParent.position, Quaternion.identity);

            // sets the first node as the starting node
            if (lnCount == 2)
            {
                GetComponent<NavigationManager>().startingNode.GetComponent<VRBranch>().moveToNode = newNode;
            }

            // give it  id
            newNode.GetComponent<VRNode>().id = lnCount - 1;

            if (lnCount == 2)
                newNode.GetComponent<VRNode>().startingNode = true;

            // add new node to dictionary
            idDict.Add(int.Parse(currentLine[0]), newNode);

            //set position from csv
            float[] newPosCoordinates = currentLine[1].Split(new string[] { " " }, StringSplitOptions.None).Select(x => float.Parse(x)).ToArray();
            Vector3 savedPos = new Vector3(newPosCoordinates[0], 0, newPosCoordinates[1]) / 5;
            newNode.transform.localPosition = savedPos;

            //set picture
            Material newMaterial = Resources.Load("mat " + ((lnCount))%4) as Material;
            newNode.GetComponent<Renderer>().material = newMaterial;
            // it seems like raycasts go through skybox rendered materials, so need to spawn a fake sphere at the same position for raycast to hit.
            // or change the rendering through c# after raycast hit to be skybox rendered.
            // raycast actually does work if the skybox rendered is there beforehand, but when loaded below the sphere may not be created in time for the raycast to detect it
        }

        lnCount = 0;
        System.IO.StreamReader fileR = new System.IO.StreamReader(loadedMapPath);
        while ((savedLine = fileR.ReadLine()) != null)
        {
            lnCount++;
            if (lnCount == 1) { continue; } //lnCount 1 is only header info, so we skip that line

            List<string> currentLine = savedLine.Split(',').ToList<string>();

            if (currentLine[5].Length == 0) { continue; }
            List<int> connections = currentLine[5].Split().Select(int.Parse).ToList();
            
            // spawn lines between points
            foreach (int connection in connections)
            {
                Transform startingPos = idDict[int.Parse(currentLine[0])].transform;
                Transform endingPos = idDict[connection].transform;

                bool startingBranch = false;
                if (lnCount == 2) startingBranch = true;
                //need to wait for some amount of time before raycasting, otherwise it doesn't work, using coroutine to wait first before casting
                StartCoroutine(hitAllSpheres(startingPos, endingPos, endingPos.GetComponent<VRNode>().id));
                StartCoroutine(hitAllSpheres(endingPos, startingPos, startingPos.GetComponent<VRNode>().id, startingBranch));
            }
        }
        //finally move player to starting position once all positions have been loaded
        GetComponent<NavigationManager>().PositionUser();
    }
    private IEnumerator hitAllSpheres(Transform startingPos, Transform endingPos, int connectedID, bool startingBranch = false)
    {
        yield return new WaitForSeconds(.1f);
        Debug.DrawLine(startingPos.position, endingPos.position, Color.green, 200);
        RaycastHit[] hits = Physics.RaycastAll(startingPos.position, endingPos.position - startingPos.position, 200);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.TryGetComponent(out VRNode imageNode))
            {
                if (imageNode.id != connectedID)
                    continue;

                GameObject button = Instantiate(pfMoveButton, hit.point, Quaternion.identity);
                // set movement button as child of node
                button.transform.parent = endingPos.transform;
                // set player object on ImageBranchNode
                button.GetComponent<VRBranch>().player = player;
                // set moveToNode object on ImageBranchNode
                button.GetComponent<VRBranch>().moveToNode = startingPos.gameObject;
                
                if(startingBranch)
                    button.GetComponent<MeshRenderer>().enabled = true;
                //update the nodes mesh renderers with the new button
                endingPos.GetComponent<VRNode>().AddRenderer(button.GetComponent<MeshRenderer>());


                // if the branch is in setup mode and has dragable component, we send it the xrray interactor
                if (button.TryGetComponent(out DragBranch branch))
                    branch.rayInteractor = player.GetComponentInChildren<XRRayInteractor>();
            }
        }
    }
}
