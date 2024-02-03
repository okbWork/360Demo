using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the main class for the Setup Application. Each node will be translated to an ImageNode in the Experience Application.
/// UINodes have a unique ID, a path to their associated image, and a list of id's which represent the unique ID's of the nodes they are linked to
/// </summary>
public class UINode : MonoBehaviour
{
    public int id;
    public string imagePath;
    public RawImage image;

    public List<int> ids;

    public TextMeshProUGUI tmp;
    public string nodeName;
    public void DeleteNode()
    {
        Destroy(gameObject);
    }

    public void ChangeImage(string newPath)
    {
        if (newPath == "")
        {
            newPath = EditorUtility.OpenFilePanel("Select image", "D:\\Profiles\\admin\\AppData\\LocalLow\\DefaultCompany\\360Shrine\\Images", "jpg");
            if (newPath.Length == 0)
                return;
        }
        imagePath = newPath;

        Debug.Log(imagePath);
        // Load the JPG image file from the file path into a Texture2D
        Texture2D texture = new Texture2D(2, 2);
        byte[] imageData = System.IO.File.ReadAllBytes(imagePath);
        texture.LoadImage(imageData);

        // Assign the loaded texture to the RawImage component
        image.texture = texture;
    }
    public string GetInputFieldText()
    {
        return tmp.text;
    }
}
