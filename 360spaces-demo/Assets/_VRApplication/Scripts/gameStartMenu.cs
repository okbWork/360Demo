using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{ // participantNum is being initialized to set them to values later
    public Dropdown location;
    public Dropdown narrative;
    public Dropdown poi;
    public InputField sphereXInput;
    public InputField sphereYInput;
    public string locName;
    public string narrativeName;
    public float sphereX;
    public float sphereY;
    public class sphereInfo
    {
        public float x;
        public float y;
        public int ID;
    }
    private void Start()
    {
        updateNarrativeOptions();
        updatePoiOptions();
        location.onValueChanged.AddListener(delegate {
            locationValueChanged(location);
        });
        narrative.onValueChanged.AddListener(delegate {
            narrativeValueChanged(narrative);
        });
        poi.onValueChanged.AddListener(delegate {
            poiValueChanged(poi);
        });

    }/*
    public ArrayList<sphereInfo> getSphereList()
    {
        return new ArrayList<sphereInfo> { };
    }*/
    public int getSpherefromCoordinates(float xCoor, float yCoor/*, sphereList*/)
    {
        return (int)(Math.Floor(xCoor + yCoor) % 3.0);
        /*
        int ID;
        float dist = 2147483647.0
        if (sphereList.Count > 0)
        {
            ID = sphereList[0].ID;
        }
        foreach (sphereInfo sphere in sphereList)
        {
            newDist = Math.Abs(sphere.x - xCoor) + MAth.Abs(sphere.y - yCoor);
            ID = sphere.ID if (newDist < dist) ;
            dist = Math.Min(dist, newDist);
        }
        return ID;
        */
    }

        void locationValueChanged(Dropdown change)
    {
        if(change.value != 0)
        {
            sphereXInput.GetComponent<InputField>().enabled = false;
            sphereYInput.GetComponent<InputField>().enabled = false;
            updateNarrativeOptions();
        }
        else if(narrative.value == 0){
            updatePoiOptions();
            poi.GetComponent<Dropdown>().enabled = true;
            if (poi.value == 0)
            {
                sphereXInput.GetComponent<InputField>().enabled = true;
                sphereYInput.GetComponent<InputField>().enabled = true;
            }
        }
    }
    void narrativeValueChanged(Dropdown change)
    {
        if (change.value != 0)
        {
            poi.GetComponent<Dropdown>().enabled = false;
            sphereXInput.GetComponent<InputField>().enabled = false;
            sphereYInput.GetComponent<InputField>().enabled = false;
        }
        else
        {
            updatePoiOptions();
            poi.GetComponent<Dropdown>().enabled = true;
            if (poi.value == 0){
                sphereXInput.GetComponent<InputField>().enabled = true;
                sphereYInput.GetComponent<InputField>().enabled = true;
            }
        }
    }
    void poiValueChanged(Dropdown change)
    {
        if (change.value != 0)
        {
            sphereXInput.GetComponent<InputField>().enabled = false;
            sphereYInput.GetComponent<InputField>().enabled = false;
        }
        else
        {
            sphereXInput.GetComponent<InputField>().enabled = true;
            sphereYInput.GetComponent<InputField>().enabled = true;
        }
    }
    
    void updateNarrativeOptions()
    {
        List<string> newNarratives = new List<string> {"Free Roam", "Child", "Mendican"};
        //CALL TO DATABASE TO RETRIEVE NARATIVE TITLES
        narrative.ClearOptions();
        narrative.AddOptions(newNarratives);
    }
    void updatePoiOptions()
    {
        List<string> newPois = new List<string> { "None" , "Idolo of Mary", "Street Intersection"};
        //CALL TO DATABASE TO RETRIEVE POI TITLES
        poi.ClearOptions();
        poi.AddOptions(newPois);
    }

    void LoadGameScene(int locationVal, int NarrativeVal, int poiVal = 0)
    {
        ItalySceneStateController.narrative = NarrativeVal;
        ItalySceneStateController.poi = poiVal;
        SceneManager.LoadScene("Italy");
    }

    void Do_FINAL()
    {
        if(narrative.value != 0)
        {
            LoadGameScene(location.value, narrative.value);
        }else
        {
            if (poi.value == 0 && location.value == 0 && sphereXInput.text != "" && sphereYInput.text != "")
            {
                LoadGameScene(location.value, narrative.value, getSpherefromCoordinates(float.Parse(sphereXInput.text), float.Parse(sphereYInput.text) /*getSphereList() METHOD TO RETRIEVE SPHERE LIST FROM DB*/ ));
            }
            else
            {
                LoadGameScene(location.value, narrative.value, poi.value);
            }
        }
    }
    /*
    participantNum has to be initialized on gameObject's script page in the inspector
    from there, you have to go to the button and change the on-click function 
    */
    public static StartGame instance = new StartGame();

    IEnumerator ExposureTime()
    {
        SceneManager.LoadScene("Shrine");
        yield return new WaitForSeconds(900);//15 mintues in real time
        Application.Quit();
    }

    // DO_FINAL encompasses all the functions for the onclick part of the button
    //stores data from list once input file box is filled out by user at the start menu
    /*
    public void Do_FINAL()
    {
        if (participantNum.text != "")
        {
            PlayerPrefs.GetString("Participant", participantNum.text);
            if (SceneManager.GetActiveScene().name == "Input_Menu")
            {
                CSVWriter.instance.writeFilename(participantNum.text, "", "");
                try
                {
                    StartCoroutine(ExposureTime());

                }
                catch (Exception ex)
                {
                    throw new ApplicationException("#### Error in Do_FINAL ####", ex);
                }
            }
            else
            {
                CSVWriter.instance.writeFilename(participantNum.text, "", "");
                try
                {
                    SceneManager.LoadScene("Tutorial");

                }
                catch (Exception ex)
                {
                    throw new ApplicationException("#### Error in Do_FINAL ####", ex);
                }
            }


        }
    }
    */
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
