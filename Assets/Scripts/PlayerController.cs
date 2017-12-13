using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gvr;

public class PlayerController : MonoBehaviour
{

    public GameObject head;
    public GameObject playerCamera;
    public GameObject playerCanvas;

    private Camera cam;

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
            playerCanvas.SetActive(false);
            playerCamera.SetActive(false);
        }
    }

}