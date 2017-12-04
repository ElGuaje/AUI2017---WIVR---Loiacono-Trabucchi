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
    private GameObject spriteLoader;

    private void Awake()
    {
        startTeleport = new UnityAction(StartTeleport);


    }

    // Use this for initialization
    void Start () {
        players = new GameObject[10];
        players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log("Fin qui okay");
        spriteLoader = GameObject.FindGameObjectWithTag("SpriteLoader");

        Debug.Log("Anche Fin qui okay");

        Sprite[] allSprites = spriteLoader.GetComponent<SpriteLoader>().allSprites;

        Debug.Log("Uccazz");

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
                int index = Random.Range(0, allSprites.Length);

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
                    GameObject memoryElement = PhotonNetwork.Instantiate("MemoryElement", new Vector3 (p.transform.position.x + direction.x*distance*sign, 
                        p.transform.position.y + direction.y* distance, p.transform.position.z + direction.z* distance), Quaternion.identity, 0);

                    memoryElement.GetComponent<SpriteRenderer>().sprite = allSprites[index];
                    memoryElement.GetComponent<Teleport>().spriteIndex = index;
                    memoryElement.GetComponent<Teleport>().myPlayerPosition = p.transform.position;
                    memoryElement.GetComponent<Teleport>().playerCube = playerID;
                    memoryElement.GetComponent<Teleport>().cubeNumber = i;

                    j++;
                }
            }
            cubes = GameObject.FindGameObjectsWithTag("MemoryElement");
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
            cubes = GameObject.FindGameObjectsWithTag("MemoryElement");
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
        Debug.Log("Deactivation Started");
        yield return new WaitForSeconds(3);
        if (!photonView.isMine)
        {
            cube1.GetComponent<Teleport>().photonView.RPC("Deactivate", PhotonTargets.All);
            cube2.GetComponent<Teleport>().photonView.RPC("Deactivate", PhotonTargets.All);
        }
        else
        {
            cube1.GetComponent<Teleport>().TeleportRandomly();
            cube2.GetComponent<Teleport>().TeleportRandomly();
        }
    }
}
