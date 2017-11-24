using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;

public class ServerMatchmaking : Photon.PunBehaviour
{

    private GameObject currentPlayer;

    void Start()
    {
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    /////////////////////// Photon Methods  ///////////////////////

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {



        if (PhotonNetwork.countOfPlayers == 1)
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(0, 1.6f, 0), Quaternion.identity, 0);
        }
        else
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", new Vector3(2f, 1.6f, 0), Quaternion.identity, 0);
        }
        currentPlayer.GetComponent<PlayerController>().isControllable = true;
    }

    // This is called if there is no one playing or if all rooms are full, so create a new room
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}