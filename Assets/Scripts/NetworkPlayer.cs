using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NetworkPlayer : Photon.MonoBehaviour
{

    public GameObject otherPlayerHead;
    public Camera playerCamera;
    public GameObject canvas;

    private string headName;
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
    private StreamWriter sw;
    private string myPath;

    void Start()
    {
        headName = otherPlayerHead.transform.GetChild(0).name;
        myPath = Application.persistentDataPath;
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
            stream.SendNext(otherPlayerHead.transform.GetChild(0).name);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
            string n = (string)stream.ReceiveNext();
            if (!n.Equals(headName))
            {
                headName = n;
                n = n.Substring(0, n.LastIndexOf("(Clone)"));
                ChangeThisAvatar(n);
            }
        }
    }

    private void ChangeThisAvatar(string s)
    {
        Debug.Log(s);
        GameObject l = Resources.Load<GameObject>("Camerinus/" + s);
        GameObject o = Instantiate(l, new Vector3(0, 0, 0), Quaternion.identity);
        Destroy(otherPlayerHead.transform.GetChild(0).gameObject);
        o.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 1.5f, this.gameObject.transform.position.z);
        o.transform.rotation = this.correctPlayerRot;
        o.transform.parent = otherPlayerHead.transform;
    }

    [PunRPC]
    public void ShowGameover()
    {
        SoundManager.Instance.Fanfare();
        GameObject o = Resources.Load<GameObject>("ButtonGoToMainMenu");
        GameObject l = Instantiate(o, new Vector3(3.5f, 3f, -8), Quaternion.identity);
        SaveGame();
    }

    [PunRPC]
    public void SaveGame()
    {
        sw = new StreamWriter(myPath + "/ScoreFile.txt", append: true);
        GameObject timer = GameObject.Find("Timer(Clone)");

        Debug.Log(PlayerPrefs.GetString("username") + " Time " + timer.GetComponentInChildren<Text>().text);
        sw.WriteLine(PlayerPrefs.GetString("username") + " Time " + timer.GetComponentInChildren<Text>().text);
        sw.Close();
    }


    private IEnumerator MoveFinalHead(GameObject o, Vector3 t)
    {
        yield return new WaitForSeconds(0.01f);
        float f = Time.time;
        while ((Time.time - f) < 6)
        {
            o.transform.position = Vector3.Lerp(o.transform.position, t, (Time.time - f)/3);
            yield return null;
        } 
    }
}