using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using System;

public class ServerMatchmaking : Photon.PunBehaviour
{

    private GameObject currentPlayer;

    public bool differentObjects = false;
    public bool movingObject = false;
    public bool isCamerinus = false;
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    /////////////////////// Photon Methods  ///////////////////////

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length == 1)
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(0, 3f, 0), Quaternion.identity, 0);
        }
        else
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(7f, 3f, 0), Quaternion.identity, 0);
            if (!isCamerinus)
            {
                StartGame();
            }
        }
        currentPlayer.GetComponent<PlayerController>().isControllable = true;
    }

    private void StartGame()
    {
        PhotonNetwork.room.IsVisible = false;
        if (differentObjects)
            PhotonNetwork.Instantiate("GameManager2", new Vector3(0, 0, 0), Quaternion.identity, 0);
        else if (movingObject)
            PhotonNetwork.Instantiate("GameManager3", new Vector3(0, 0, 0), Quaternion.identity, 0);
        else
            PhotonNetwork.Instantiate("GameManager", new Vector3(0, 0, 0), Quaternion.identity, 0);

    }

    // This is called if there is no one playing or if all rooms are full, so create a new room
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }

}