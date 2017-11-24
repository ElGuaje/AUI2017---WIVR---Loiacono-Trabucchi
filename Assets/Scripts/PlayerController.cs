using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public GameObject head;
    public GameObject playerCamera;

    // Used to check if is this user's player or an external player
    public bool isControllable;

    // Use this for initialization
    void Start()
    {
        if (isControllable)
        {
            playerCamera.SetActive(true);
            head.SetActive(false);
        }
        else
        {
            playerCamera.SetActive(false);
        }
    }
}