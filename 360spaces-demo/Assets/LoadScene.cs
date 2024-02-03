using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadCampus()
    {
        SceneManager.LoadScene("Scenes/Campus");
    }
    public void LoadItaly()
    {
        SceneManager.LoadScene("Scenes/Italy");
    }
}
