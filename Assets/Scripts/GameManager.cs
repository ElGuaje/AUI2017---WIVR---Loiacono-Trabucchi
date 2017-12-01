using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Photon.MonoBehaviour {

    public GameObject[] cubes;
    public int cubeNumbers = 3;

    private UnityAction startTeleport;

    private void Awake()
    {
        startTeleport = new UnityAction(StartTeleport);
        cubes = new GameObject[20];
    }

    // Use this for initialization
    void Start () {
        if (photonView.isMine)
        {
            GameObject[] players = new GameObject[10];
            players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(players.ToStringFull());
            for (int i = 0; i < cubeNumbers; i++)
            {
                Debug.Log("Creating...");

                foreach (GameObject p in players)
                {
                    Debug.Log("...A pair...");
                    int playerID= p.gameObject.GetPhotonView().viewID;
                    GameObject cube1 = PhotonNetwork.Instantiate("Cube", new Vector3(p.transform.position.x - 2, 2f, i * 2), Quaternion.identity, 0);
                    cube1.GetComponent<Teleport>().playerCube = playerID;
                    cube1.GetComponent<Teleport>().cubeNumber = i;
                    cube1.GetComponent<Teleport>().inactiveMaterial.color = new Vector4(i, i, i, 0.3f);
                }
            }
            cubes = GameObject.FindGameObjectsWithTag("Cube");
        }
        Debug.Log("... of Cubes");
        if (!photonView.isMine)
            StartCoroutine("SearchCubes");
        EventManager.StartListening("GazedAtEvent", startTeleport);
    }

    private void StartTeleport()
    {
        Debug.Log("We can Teleport?");
        for (int i = 0; i < cubeNumbers*2; i++)
        {
            GameObject cube1 = cubes[i];
            for (int j = 0; j < cubeNumbers*2; j++)
            {
                if (i > j || i == j) continue;

                GameObject cube2 = cubes[j];
                Debug.Log(cube1.GetComponent<Teleport>().cubeNumber + " " + cube2.GetComponent<Teleport>().cubeNumber);
		        if (cube1.GetComponent<Teleport>().gazedAt && cube2.GetComponent<Teleport>().gazedAt &&
                    cube1.GetComponent<Teleport>().cubeNumber == cube2.GetComponent<Teleport>().cubeNumber)
                {
                    Debug.Log("Yes I want");
                    StartCoroutine(TimerToTeleport(cube1, cube2));
                }
                else
                {
                    StopCoroutine(TimerToTeleport(cube1, cube2));
                }
            }
        }


    }

    IEnumerator SearchCubes()
    {
        Debug.Log("StartedSearch");
        while (cubes[cubeNumbers*2-1] == null)
        {
            cubes = GameObject.FindGameObjectsWithTag("Cube");
            yield return null;
        }
        Debug.Log("CubesFound");
    }

    IEnumerator TimerToTeleport(GameObject cube1, GameObject cube2)
    {
        yield return new WaitForSeconds(3);
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
