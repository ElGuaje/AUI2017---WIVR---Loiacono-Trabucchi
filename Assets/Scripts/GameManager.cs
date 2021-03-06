﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;

public class GameManager : Photon.MonoBehaviour {

    public GameObject[] memoryElements;
    public int elementsNumber;

    public GameObject[] players;

    public bool isDifferentObjects;
    public bool isMovingObjects;
    public bool isTimed;

    public GameObject timerPrefab;

    public float minheight = 3;

    private int playerViewID;
    private UnityAction startDeactivation;
    private UnityAction countElement;
    private SpriteLoader spriteLoader;
    private int elementsFound = 0;
    private Coroutine DeactivationCor = null;

    private void Awake()
    {
        startDeactivation = new UnityAction(StartDeactivation);
        countElement = new UnityAction(CountElement);
        elementsNumber = PlayerPrefs.GetInt("NumberOfImages");
        isDifferentObjects = PlayerPrefs.GetInt("DifferentObjects") != 0;
        isMovingObjects = PlayerPrefs.GetInt("MovingObjects") != 0;
        isTimed = PlayerPrefs.GetInt("Timer") != 0;
    }

    // Use this for initialization
    void Start ()
    {
        players = new GameObject[10];
        players = GameObject.FindGameObjectsWithTag("Player");
        spriteLoader = GameObject.FindGameObjectWithTag("SpriteLoader").GetComponent<SpriteLoader>();

        foreach (GameObject p in players)
        {
            if (p.gameObject.GetPhotonView().owner == photonView.owner)
                playerViewID = p.gameObject.GetPhotonView().viewID;       
        }

        if (photonView.isMine)
        {
            if (!isDifferentObjects)
            {
                int[] indexes = new int[elementsNumber];
                memoryElements = new GameObject[elementsNumber * 2];
                for (int i = 0; i < elementsNumber; i++)
                {
                    int j = 0;
                    int index = 0;
                    bool isRepeated = true;
                    while (isRepeated)
                    {
                        isRepeated = false;
                        index = UnityEngine.Random.Range(0, spriteLoader.allSprites.Length);
                        foreach (int l in indexes)
                        {
                            if (l == index)
                            {
                                isRepeated = true;
                                break;
                            }
                        }
                    }

                    indexes[i] = index;

                    foreach (GameObject p in players)
                    {
                        Vector3 direction = UnityEngine.Random.onUnitSphere;
                        int playerID = p.gameObject.GetPhotonView().viewID;
                        float distance = 4f;

                        if (direction.y < -0.1f)
                        {
                            direction.y = -direction.y;
                        }

                       
                        GameObject memoryElement = PhotonNetwork.Instantiate("MemoryElement", new Vector3(p.transform.position.x + direction.x * distance,
                            minheight + direction.y * distance, p.transform.position.z + direction.z * distance), Quaternion.identity, 0);
                        memoryElement.GetComponent<SpriteRenderer>().sprite = spriteLoader.allSprites[index];
                        memoryElement.GetComponent<Teleport>().spriteIndex = index;
                        memoryElement.GetComponent<Teleport>().realIndex = index;
                        memoryElement.GetComponent<Teleport>().myPlayerPosition = p.transform.position;
                        memoryElement.GetComponent<Teleport>().myPlayerPosition.y = minheight;
                        memoryElement.GetComponent<Teleport>().playerCube = playerID;
                        memoryElement.GetComponent<Teleport>().cubeNumber = i;

                        if (isMovingObjects)
                        {
                            memoryElement.GetComponent<SpriteMover>().isMovementActive = true;
                        }
                        else
                        {
                            memoryElement.GetComponent<SpriteMover>().isMovementActive = false;
                        }

                        j++;
                    }
                }
            }
            else
            {
                int[] indexes = new int[elementsNumber];
                memoryElements = new GameObject[elementsNumber * 2];
                for (int i = 0; i < elementsNumber; i++)
                {

                    int index = 0;
                    bool isRepeated = true;
                    while (isRepeated)
                    {
                        isRepeated = false;
                        index = UnityEngine.Random.Range(0, spriteLoader.allSprites.Length);
                        foreach (int l in indexes)
                        {
                            if (l == index || index%2 == 1)
                            {
                                isRepeated = true;
                                break;
                            }
                        }
                    }

                    indexes[i] = index;
                    int j = 0;
                    foreach (GameObject p in players)
                    {
                        
                        Vector3 direction = UnityEngine.Random.onUnitSphere;
                        int playerID = p.gameObject.GetPhotonView().viewID;
                        float distance = 4f;

                        if (direction.y < -0.1f)
                        {
                            direction.y = -direction.y;
                        }

                        GameObject memoryElement = PhotonNetwork.Instantiate("MemoryElement", new Vector3(p.transform.position.x + direction.x * distance,
                            minheight + direction.y * distance, p.transform.position.z + direction.z * distance), Quaternion.identity, 0);
                        memoryElement.GetComponent<SpriteRenderer>().sprite = spriteLoader.allSprites[index + j];
                        memoryElement.GetComponent<Teleport>().spriteIndex = index;
                        memoryElement.GetComponent<Teleport>().realIndex = index + j;
                        memoryElement.GetComponent<Teleport>().myPlayerPosition = p.transform.position;
                        memoryElement.GetComponent<Teleport>().myPlayerPosition.y = minheight;
                        memoryElement.GetComponent<Teleport>().playerCube = playerID;
                        memoryElement.GetComponent<Teleport>().cubeNumber = i;
                        j++;

                        if (isMovingObjects)
                        {
                            memoryElement.GetComponent<SpriteMover>().isMovementActive = true;
                        }
                        else
                        {
                            memoryElement.GetComponent<SpriteMover>().isMovementActive = false;
                        }
                    }
                }
            }
            memoryElements = GameObject.FindGameObjectsWithTag("MemoryElement");
        }

        if (!photonView.isMine)
            StartCoroutine("SearchImages");
        EventManager.StartListening("GazedAtEvent", startDeactivation);
        EventManager.StartListening("ElementFound", countElement);

        if (isTimed)
        {
            GameObject Timer = (GameObject)Instantiate(timerPrefab);
            Timer.transform.position = new Vector3(0, 3, 30);
        }

    }

    private void CountElement()
    {
        if (this.photonView.isMine)
        {
            elementsFound += 1;
            if (elementsFound == (elementsNumber * 2))
            {
                foreach (GameObject p in players)
                {
                    p.GetComponent<NetworkPlayer>().photonView.RPC("ShowGameover", PhotonTargets.All);
                    if (isTimed)
                    {
                        timerPrefab.GetComponentInChildren<TimerScript>().StopTimer();
                        p.GetComponent<NetworkPlayer>().photonView.RPC("SaveGame", PhotonTargets.All);
                    }
                }
            }
        }
    }

    private void StartDeactivation()
    {
        for (int i = 0; i < elementsNumber*2; i++)
        {
            GameObject memoryElement1 = memoryElements[i];

            if (!memoryElement1.activeInHierarchy) continue;

            for (int j = 0; j < elementsNumber*2; j++)
            {
                if (i > j || i == j) continue;

                GameObject memoryElement2 = memoryElements[j];
		        if (memoryElement1.GetComponent<Teleport>().gazedAt && memoryElement2.GetComponent<Teleport>().gazedAt &&
                    memoryElement1.GetComponent<Teleport>().spriteIndex == memoryElement2.GetComponent<Teleport>().spriteIndex
                    && memoryElement1.activeInHierarchy && memoryElement1.activeInHierarchy)
                {
                    memoryElement1.GetComponent<Teleport>().photonView.RPC("EnableHighlighting", PhotonTargets.All);
                    memoryElement2.GetComponent<Teleport>().photonView.RPC("EnableHighlighting", PhotonTargets.All);
                    memoryElement1.GetComponent<Teleport>().isDeactivating = true;
                    memoryElement2.GetComponent<Teleport>().isDeactivating = true;
                    DeactivationCor = StartCoroutine(DeactivateTimer(memoryElement1, memoryElement2));
                }
                else if (memoryElement1.GetComponent<Teleport>().isDeactivating && memoryElement2.GetComponent<Teleport>().isDeactivating)
                {
                    memoryElement1.GetComponent<Teleport>().photonView.RPC("DisableHighlighting", PhotonTargets.All);
                    memoryElement2.GetComponent<Teleport>().photonView.RPC("DisableHighlighting", PhotonTargets.All);
                    memoryElement1.GetComponent<Teleport>().isDeactivating = false;
                    memoryElement2.GetComponent<Teleport>().isDeactivating = false;
                    Debug.Log("I'm Stopping deac");
                    StopCoroutine(DeactivationCor);
                }
            }
        }


    }

    IEnumerator SearchImages()
    {
        yield return new WaitForSeconds(1);
        memoryElements = new GameObject[elementsNumber * 2];

        while (memoryElements[(elementsNumber*2-1)] == null)
        {
            memoryElements = GameObject.FindGameObjectsWithTag("MemoryElement");
            yield return null;
        }

        foreach (GameObject memoryElement in memoryElements)
        {
            Debug.Log(playerViewID + " " + memoryElement.GetComponent<Teleport>().playerCube);
            if (playerViewID != memoryElement.GetComponent<Teleport>().playerCube) 
                memoryElement.GetComponent<Teleport>().RequestOwnership(memoryElement.GetComponent<Teleport>().playerCube);

            if (isMovingObjects)
            {
                memoryElement.GetComponent<SpriteMover>().isMovementActive = true;
            }
            else
            {
                memoryElement.GetComponent<SpriteMover>().isMovementActive = false;
            }
        }

    }

    IEnumerator DeactivateTimer(GameObject memoryElement1, GameObject memoryElement2)
    {
        yield return new WaitForSeconds(3);
        memoryElement1.GetComponent<Teleport>().photonView.RPC("Deactivate", PhotonTargets.All);
        memoryElement2.GetComponent<Teleport>().photonView.RPC("Deactivate", PhotonTargets.All);

    }

}
