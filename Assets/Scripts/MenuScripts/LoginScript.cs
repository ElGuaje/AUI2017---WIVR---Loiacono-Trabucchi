using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class LoginScript : MonoBehaviour
{

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
        PlayerPrefs.SetInt("DifferentObjects", Convert.ToInt32(GameObject.Find("DifferentObjectsToggle").GetComponent<Toggle>().isOn));
        PlayerPrefs.SetInt("LevelAmbient", GameObject.Find("DropdownLevel").GetComponent<Dropdown>().value);
        PlayerPrefs.SetInt("DropdownImages", GameObject.Find("DropdownImages").GetComponent<Dropdown>().value);

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
