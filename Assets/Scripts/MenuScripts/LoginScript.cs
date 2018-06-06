using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class LoginScript : MonoBehaviour
{

    public void Start()
    {
        if (!PlayerPrefs.HasKey("Timer"))
        { 
            PlayerPrefs.SetInt("Timer", 0);
            PlayerPrefs.SetInt("MovingObjects", 0);
            PlayerPrefs.SetInt("DifferentObjects", 0);
            PlayerPrefs.SetInt("LevelAmbient", 0);
            PlayerPrefs.SetInt("DropdownImages", 0);
            PlayerPrefs.SetInt("NumberOfImages", 5);
        }
    }

    public void StartGame()
    {
        if (GameObject.Find("InputField").GetComponent<InputField>().text != "")
        {
            PlayerPrefs.SetString("username", GameObject.Find("InputField").GetComponent<InputField>().text);
            StartCoroutine(DontFuckUp());
        }
    }

    public void GoToOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void SaveAndReturn()
    {
        PlayerPrefs.SetInt("Timer", Convert.ToInt32(GameObject.Find("TimerToggle").GetComponent<Toggle>().isOn));
        PlayerPrefs.SetInt("MovingObjects", Convert.ToInt32(GameObject.Find("MovingObjectsToggle").GetComponent<Toggle>().isOn));
        PlayerPrefs.SetInt("LevelAmbient", GameObject.Find("DropdownLevel").GetComponent<Dropdown>().value);
        PlayerPrefs.SetInt("DropdownImages", GameObject.Find("DropdownImages").GetComponent<Dropdown>().value);
        PlayerPrefs.SetInt("NumberOfImages", int.Parse(GameObject.Find("NumberOfImages").transform.GetChild(2).GetComponent<Text>().text));
        if (PlayerPrefs.GetInt("NumberOfImages") == 0)
            PlayerPrefs.SetInt("NumberOfImages", 1);

        if (PlayerPrefs.GetInt("DropdownImages") == 1)
        {
            PlayerPrefs.SetInt("DifferentObjects", 1);
        }
        else
        {
            PlayerPrefs.SetInt("DifferentObjects", 0);
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("LoginScene");
    }

    private IEnumerator DontFuckUp()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
        SceneManager.LoadScene("Main Menu");

    }
}
