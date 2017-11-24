using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : Photon.MonoBehaviour
{

    public GameObject otherPlayerHead;
    public Camera playerCamera;

    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this

    void Start()
    {

    }

    void Update()
    {

        // Check to see if this NetworkPlayer is the owned by the current instance
        if (!photonView.isMine)
        {
            // Lerping smooths the movement
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            otherPlayerHead.transform.rotation = Quaternion.Lerp(otherPlayerHead.transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(playerCamera.transform.rotation);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
        }
    }

}