using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Gvr;
using UnityEngine.EventSystems;
using System;

public class PlayerController : MonoBehaviour
{

    public GameObject head;
    public GameObject playerCamera;
    public GameObject playerCanvas;

    private UnityAction changeMyAvatar;
    private UnityAction changeYourAvatar;

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
            EventManager.StartListening("ChangeMyAvatar", changeMyAvatar);
            SelectInitialHead();
        }
        else
        {
            playerCanvas.SetActive(false);
            playerCamera.SetActive(false);
            EventManager.StartListening("ChangeYourAvatar", changeMyAvatar);
        }
    }

    private void SelectInitialHead()
    {
        GameObject l = Resources.Load<GameObject>(PlayerPrefs.GetString("ChosenAvatar"));
        GameObject o = Instantiate(l, new Vector3(0, 0, 0), playerCamera.transform.rotation);
        Destroy(head.transform.GetChild(0).gameObject);
        o.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.5f, this.gameObject.transform.position.z);
        if (!isControllable)
        {
            o.transform.rotation = head.transform.rotation;
        }
        o.transform.parent = head.transform;
    }

}