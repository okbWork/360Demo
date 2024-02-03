using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.XR.LegacyInputHelpers;
using TMPro;

public class ObjectClick: MonoBehaviour{
    public GameObject objectMold;
    public GameObject itemDescription;
    public bool touchable;
    private void Start()
    {
        HideMesh();
        HideInfo();
        if (touchable)
        {
            objectMold.GetComponent<MeshRenderer>().enabled = true;
        }
    }
    public void InfoPanel()
    {
        if (itemDescription.GetComponent<MeshRenderer>() != null)
        {
            itemDescription.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            itemDescription.GetComponent<Image>().enabled = true;
        }
    }
    public void HideInfo()
    {
        if (itemDescription.GetComponent<MeshRenderer>() != null)
        {
            itemDescription.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            itemDescription.GetComponent<Image>().enabled = false;
        }
    }
    public void HideMesh()
    {
        objectMold.GetComponent<MeshRenderer>().enabled = false;
    }
    public void OnClick()
    {
        if (itemDescription.GetComponent<MeshRenderer>().enabled == true)
        {
            Debug.Log("Hiding Text");
            HideInfo();
        }else{
            Debug.Log("Revealing Text");
            InfoPanel();
        }
    }
}