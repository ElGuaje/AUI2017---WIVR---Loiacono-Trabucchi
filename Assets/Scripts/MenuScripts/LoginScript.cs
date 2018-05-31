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

    private IEnumerator DontFuckUp()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
        SceneManager.LoadScene("Main Menu");

    }
}
