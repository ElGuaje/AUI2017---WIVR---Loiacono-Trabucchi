using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Photon.MonoBehaviour {

    public GameObject cube1;
    public GameObject cube2;
    private UnityAction startTeleport;

    private void Awake()
    {
        startTeleport = new UnityAction(StartTeleport);
        if (photonView.isMine)
        {
            cube1 = PhotonNetwork.Instantiate("Cube", new Vector3(1, 0f, 2), Quaternion.identity, 0);
            cube2 = PhotonNetwork.Instantiate("Cube", new Vector3(1, 0f, -2), Quaternion.identity, 0);
        }
    }

    // Use this for initialization
    void Start () {
        if (!photonView.isMine)
            StartCoroutine("SearchCubes");
        EventManager.StartListening("GazedAtEvent", startTeleport);
    }

    private void StartTeleport()
    {
        Debug.Log("Hello1");
		if (cube1.GetComponent<Teleport>().gazedAt && cube2.GetComponent<Teleport>().gazedAt)
        {
            Debug.Log("Hello2");
            StartCoroutine("TimerToTeleport");
        }
        else
        {
            StopCoroutine("TimerToTeleport");
        }

    }

    IEnumerator SearchCubes()
    {
        Debug.Log("StartedSearch");
        while (cube1 == null || cube2 == null)
        {
            GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
            cube1 = cubes[0];
            cube2 = cubes[1];
            yield return null;
        }
        Debug.Log("CubesFound");
    }

    IEnumerator TimerToTeleport()
    {
        yield return new WaitForSeconds(1);
        if (!photonView.isMine)
        {
            cube1.GetComponent<Teleport>().photonView.RPC("TeleportRandomly", PhotonTargets.Others);
            cube2.GetComponent<Teleport>().photonView.RPC("TeleportRandomly", PhotonTargets.Others);
        }
        else
        {
            cube1.GetComponent<Teleport>().TeleportRandomly();
            cube2.GetComponent<Teleport>().TeleportRandomly();
        }
    }
}
