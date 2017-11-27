using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCube : Photon.MonoBehaviour
{

    public GameObject cube;
    private Vector3 correctCubePos;
    private Quaternion correctCubeRot = Quaternion.identity; // We lerp towards this
    private Material correctCubeMaterial;

    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = correctCubePos;
            cube.transform.rotation = correctCubeRot;
            //cube.GetComponent<MeshRenderer>().material = correctCubeMaterial;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            //stream.SendNext(cube.GetComponent<MeshRenderer>().material);
        }
        else if (stream.isReading)
        {
            // Network player, receive data
            this.correctCubePos = (Vector3)stream.ReceiveNext();
            this.correctCubeRot = (Quaternion)stream.ReceiveNext();
            //this.correctCubeMaterial = (Material)stream.ReceiveNext();
        }
    }
}
