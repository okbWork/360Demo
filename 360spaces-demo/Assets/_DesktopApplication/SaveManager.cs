using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

/// <summary>
/// Saves a layout of UINodes (also called a map) to be loaded into the User Experience Application
/// </summary>
public class SaveManager : MonoBehaviour
{
    public Transform NodeParent;
    public Transform LineParent;
    public string latestMapPath; // stores the path so the LoadNavTree can read it

    private void Start()
    {
        LoadData();
    }
    public void SaveData()
    {
        // Open file manager for user to select file to save
        string path = EditorUtility.SaveFilePanel("Name CSV file", "D:\\Profiles\\admin\\AppData\\LocalLow\\DefaultCompany\\360Shrine","NewMap", "csv");
        if (path.Length == 0)
            return;

        string filePath = path;
        savePath(filePath);

        StreamWriter writer = new StreamWriter(filePath);

        writer.WriteLine("NodeID,pos,scale,image,connection_count, connection pairs, name");
        foreach (Transform child in NodeParent)
        {
            UINode node = child.GetComponent<UINode>();
            //NodeID,pos,rotation,scale,image,#connections, connection pairs
            string pos = child.transform.localPosition.ToString().Replace(",", "").Replace("(", "").Replace(")", "");
            string rot = child.transform.rotation.ToString().Replace(",", "").Replace("(", "").Replace(")", ""); ;
            string scale = child.transform.localScale.ToString().Replace(",", "").Replace("(", "").Replace(")", ""); ;
            string ids = "";
            string imagePath = child.transform.GetComponent<UINode>().imagePath;
            string nodeName = node.GetInputFieldText();
            //add line(name+id, pos, scale, image, connection_count, connections
            foreach (int id in node.ids)
            { ids += id.ToString() + " "; }
            ids = ids.Trim(); //remove space on end of last int

            writer.WriteLine(node.id + "," + pos + "," + scale + "," + imagePath + "," + node.ids.Count + "," + ids + "," + nodeName);
        }

        writer.Close();
        UnityEngine.Debug.Log("saved to path = " + filePath);
    }
    public void LoadData()
    {
        ClearMap();
        // Open file manager for user to select file to load
        string path = EditorUtility.OpenFilePanel("Select CSV file", "D:\\Profiles\\admin\\AppData\\LocalLow\\DefaultCompany\\360Shrine", "csv");
        if (path.Length == 0)            
            return;

        string filePath = path;
        savePath(filePath);

        // TODO will throw error if there are no connection ids, would be more stable if it checked for empty string first

        string savedLine;
        int lnCount = 0;
        Dictionary<int, GameObject> idDict = new Dictionary<int, GameObject>();
        System.IO.StreamReader file = new System.IO.StreamReader(filePath);
        while ((savedLine = file.ReadLine()) != null)
        {
            lnCount++;
            if (lnCount == 1) { continue; } //lnCount 1 is only header info, not important
            //Debug.Log(savedLine); //print out saved line
            List<string> currentLine = savedLine.Split(',').ToList<string>();

            // create a new node
            GameObject newNode = GetComponent<UiInput>().ReturnNode();

            UINode nodeData = newNode.GetComponent<UINode>();

            // add new node ID to dictionary
            idDict.Add(int.Parse(currentLine[0]), newNode);

            // give it  id
            nodeData.id = lnCount -1;
            // set position
            float[] newPosCoordinates = currentLine[1].Split(new string[] { " " }, StringSplitOptions.None).Select(x => float.Parse(x)).ToArray();
            Vector3 savedPos = new Vector3(newPosCoordinates[0], newPosCoordinates[1], newPosCoordinates[2]);

            newNode.transform.localPosition = savedPos;

            // set image
            if (currentLine[3].Length>0)
                nodeData.ChangeImage(currentLine[3]);
            // set name
            nodeData.tmp.text = currentLine[6];
        }
        // setting connections require seperate loop, since the first node could try to connect to the last, which hasn't been created yet
        // this way all nodes are created before we try connecting
        lnCount = 0;
        System.IO.StreamReader fileR = new System.IO.StreamReader(path);
        while ((savedLine = fileR.ReadLine()) != null)
        {
            lnCount++;
            UnityEngine.Debug.Log("loading line " + lnCount);
            if (lnCount == 1) { continue; } //lnCount 1 is only header info, so we skip that line

            List<string> currentLine = savedLine.Split(',').ToList<string>();

            //if node has no connections then skip it
            if (currentLine[5].Length == 0) {continue;}
            List<int> connections = currentLine[5].Split().Select(int.Parse).ToList();

            // spawn lines between points
            foreach(int connection in connections)
            {
                GetComponent<UiInput>().AddBranch(idDict[int.Parse(currentLine[0])].transform);
                GetComponent<UiInput>().EndBranch(idDict[connection].transform);
            }
        }

        UnityEngine.Debug.Log("loading complete");
    }

    private void ClearMap()
    {
        foreach (Transform child in NodeParent)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in LineParent)
        {
            Destroy(child.gameObject);
        }
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    private void savePath(string path)
    {
        PlayerPrefs.SetString("mapPath", path);
    }

}
