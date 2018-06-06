using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon;
using System;

public class ServerMatchmaking : Photon.PunBehaviour
{

    private GameObject currentPlayer;

    public Vector3 player1Position;
    public Vector3 player2Position;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    /////////////////////// Photon Methods  ///////////////////////

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinRoom(PlayerPrefs.GetInt("Timer").ToString() +
        PlayerPrefs.GetInt("MovingObjects").ToString() +
        PlayerPrefs.GetInt("DifferentObjects").ToString() +
        PlayerPrefs.GetInt("LevelAmbient").ToString() +
        PlayerPrefs.GetInt("DropdownImages").ToString());
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.playerList.Length == 1)
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", player1Position, Quaternion.identity, 0);
        }
        else
        {
            currentPlayer = PhotonNetwork.Instantiate("PhotonPlayer", player2Position, Quaternion.identity, 0);
            StartGame();
        }
        currentPlayer.GetComponent<PlayerController>().isControllable = true;
    }

    private void StartGame()
    {
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.Instantiate("GameManager", new Vector3(0, 0, 0), Quaternion.identity, 0);
    }

    // This is called if there is no one playing or if all rooms are full, so create a new room
    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(PlayerPrefs.GetInt("Timer").ToString() +
        PlayerPrefs.GetInt("MovingObjects").ToString() +
        PlayerPrefs.GetInt("DifferentObjects").ToString() +
        PlayerPrefs.GetInt("LevelAmbient").ToString() +
        PlayerPrefs.GetInt("DropdownImages").ToString());
    }

}