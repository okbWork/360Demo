using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

public class ItalySceneLoader : MonoBehaviour
{
	public GameObject player;
    public GameObject narr0_1;
    public GameObject narr0_2;
    public GameObject narr1_1;
    public GameObject narr2_1;
    public GameObject text1;
    public GameObject text2;
    public GameObject text3;
    public GameObject image1;
    public GameObject image2;
    public GameObject poi0;
	public GameObject poi1;
	public GameObject poi2;
	public void Start()
    {
        ItalySceneStateController.narrative = 0;
        ItalySceneStateController.poi = 0;
        string m_Path = Application.dataPath;

        //Output the Game data path to the console
        UnityEngine.Debug.Log("dataPath : " + m_Path);
        UnityEngine.Debug.Log("Starting");
        Dictionary<int, GameObject> objectMap =
                       new Dictionary<int, GameObject>();
        objectMap.Add(1, image1);
        objectMap.Add(2, text1);
        objectMap.Add(3, text2);
        objectMap.Add(4, text3);
        objectMap.Add(5, image2);

        Dictionary<int, Dictionary<int, GameObject>> narrMap = new Dictionary<int, Dictionary<int, GameObject>>();
        narrMap.Add(0, new Dictionary<int, GameObject>());
        narrMap.Add(1, new Dictionary<int, GameObject>());
        narrMap.Add(2, new Dictionary<int, GameObject>());
        narrMap[0].Add(1,narr0_1);
        narrMap[0].Add(2,narr0_2);
        narrMap[1].Add(1,narr1_1);
        narrMap[2].Add(1,narr2_1);

        Dictionary<int, GameObject> poiMap = new Dictionary<int, GameObject>();
        poiMap.Add(0, poi0);
        poiMap.Add(1, poi1);
        poiMap.Add(2, poi2);

        List<interactableObjectFile> records = new List<interactableObjectFile>();
        string filePath =
      @"\Resources\Italy\ObjectList.csv";
        filePath = m_Path + filePath;
        StreamReader reader = null;
        if (File.Exists(filePath))
        {
            reader = new StreamReader(File.OpenRead(filePath));
            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                UnityEngine.Debug.Log(line);
                var values = line.Split(',');
				var record = new interactableObjectFile();
                record.Id = int.Parse(values[0]);
                record.Area = values[1];
                record.Filetype = values[2];
                record.Filepath = values[3];
				if(record.Area == "i")
				{
					records.Add(record);
				}
            }
        }
        else
        {
            UnityEngine.Debug.Log("File doesn't exist");
        }
        foreach (interactableObjectFile record in records)
		{
			if(objectMap.ContainsKey(record.Id) && record.Filetype == "t")
			{
                string filePath1 = record.Filepath;
                StreamReader reader1 = null;
                string objectText = "";
                if (File.Exists(filePath1))
                {
                    reader1 = new StreamReader(File.OpenRead(filePath1));
                    while (!reader1.EndOfStream)
                    {
                        var line = reader1.ReadLine();
                        objectText = objectText+ line + "\n";
                    }
                    objectMap[record.Id].GetComponent<TextMeshPro>().text = objectText;
                }
                else
                {
                    UnityEngine.Debug.Log("File doesn't exist");
                }
            }else if (objectMap.ContainsKey(record.Id) && record.Filetype == "i")
            {
                string filePath1 = record.Filepath;
                if (File.Exists(filePath1))
                {
                    byte[] bytes = File.ReadAllBytes(filePath1);
                    Texture2D Tex2D = new Texture2D(2, 2);
                    Tex2D.LoadImage(bytes);
                    Sprite NewSprite = Sprite.Create(Tex2D, new Rect(0, 0, Tex2D.width, Tex2D.height), new Vector2(0, 0), 100.0f);

                    UnityEngine.Debug.Log("Here");
                    objectMap[record.Id].GetComponent<Image>().sprite = NewSprite;
                }
                else
                {
                    UnityEngine.Debug.Log("File doesn't exist");
                }
            }
		}
        if (ItalySceneStateController.narrative == 0)
        {
            GameObject poi = poiMap[ItalySceneStateController.poi];
            player.transform.position = poi.transform.position + poi.GetComponent<VRNode>().playerOffset;
            poi.GetComponent<VRNode>().SetActiveNode(true);
        }else{
            foreach (var pair in narrMap)
            {
                if (pair.Key != ItalySceneStateController.narrative)
                {
                    foreach(var narrObj in pair.Value)
                    {
                        narrObj.Value.GetComponent<Collider>().enabled = false;
                    }
                }
            }
            GameObject poi = poiMap[ItalySceneStateController.poi];
            player.transform.position = poi.transform.position + poi.GetComponent<VRNode>().playerOffset;
            poi.GetComponent<VRNode>().SetActiveNode(true);
        }
	}
}