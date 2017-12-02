using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Photon.MonoBehaviour {

    public GameObject[] cubes;
    public int cubeNumbers = 3;
    public GameObject[] players;

    private int playerViewID;
    private UnityAction startTeleport;

    private void Awake()
    {
        startTeleport = new UnityAction(StartTeleport);

    }

    // Use this for initialization
    void Start () {


        players = new GameObject[10];
        players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject p in players)
        {
            if (p.gameObject.GetPhotonView().owner == photonView.owner)
                playerViewID = p.gameObject.GetPhotonView().viewID;       
        }

        if (photonView.isMine)
        {
            cubes = new GameObject[cubeNumbers * 2];
            for (int i = 0; i < cubeNumbers; i++)
            {
                int j = 0;

                foreach (GameObject p in players)
                {
                    int sign;
                    Vector3 direction = Random.onUnitSphere;
                    int playerID= p.gameObject.GetPhotonView().viewID;
                    float distance = 2.5f;

                    if ((j % 2) == 0)
                    {
                        sign = 1;
                    }
                    else
                    {
                        sign = -1;
                    }
                    
                    direction.x = Mathf.Clamp(direction.x, 0.5f, 1f);
                    direction.y = Random.Range(0.5f, 1f);
                    Debug.Log(direction.y);
                    GameObject cube1 = PhotonNetwork.Instantiate("Cube", new Vector3 (p.transform.position.x + direction.x*distance*sign, p.transform.position.y + direction.y* distance,
                        p.transform.position.z + direction.z* distance), Quaternion.identity, 0);

                    cube1.GetComponent<Teleport>().myPlayerPosition = p.transform.position;
                    cube1.GetComponent<Teleport>().playerCube = playerID;
                    cube1.GetComponent<Teleport>().cubeNumber = i;
                    cube1.GetComponent<Teleport>().inactiveMaterial.color = new Vector4(0, 0, 0, 0.3f);

                    j++;
                }
            }
            cubes = GameObject.FindGameObjectsWithTag("Cube");
        }
        if (!photonView.isMine)
            StartCoroutine("SearchCubes");
        EventManager.StartListening("GazedAtEvent", startTeleport);
    }

    private void StartTeleport()
    {
        for (int i = 0; i < cubeNumbers*2; i++)
        {
            GameObject cube1 = cubes[i];
            for (int j = 0; j < cubeNumbers*2; j++)
            {
                if (i > j || i == j) continue;

                GameObject cube2 = cubes[j];
		        if (cube1.GetComponent<Teleport>().gazedAt && cube2.GetComponent<Teleport>().gazedAt &&
                    cube1.GetComponent<Teleport>().cubeNumber == cube2.GetComponent<Teleport>().cubeNumber)
                {
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
        yield return new WaitForSeconds(1);
        cubes = new GameObject[cubeNumbers * 2];

        while (cubes[(cubeNumbers*2-1)] == null)
        {
            cubes = GameObject.FindGameObjectsWithTag("Cube");
            yield return null;
        }

        foreach (GameObject cube in cubes)
        {
            Debug.Log(playerViewID + " " + cube.GetComponent<Teleport>().playerCube);
            if (playerViewID != cube.GetComponent<Teleport>().playerCube)
                cube.GetComponent<Teleport>().RequestOwnership(cube.GetComponent<Teleport>().playerCube);
        }

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
